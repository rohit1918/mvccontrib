using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;
using MvcContrib.Rest.Routing.Attributes;
using MvcContrib.Rest.Routing.Builders;
using MvcContrib.Rest.Routing.Descriptors;
using MvcContrib.Rest.Routing.Ext;
using MvcContrib.Rest.Routing.Services;

namespace MvcContrib.Rest.Routing
{
	public class RestfulRoutingBuilder : IRestfulRuleContainer, IMvcRouteBuilder
	{
		private readonly IDictionary<string, List<ControllerRoutingDescriptor>> _registeredControllerScopes =
			new Dictionary<string, List<ControllerRoutingDescriptor>>();

		private readonly IDictionary<Type, List<string>> _registeredScopesForController =
			new Dictionary<Type, List<string>>();

		private readonly IRestfulRuleConverter _ruleConverter;
		private readonly List<IRestfulRuleSetDescriptor> _ruleSets = new List<IRestfulRuleSetDescriptor>();
		private readonly IRestfulRuleSorter _ruleSorter;
		private readonly List<IRestfulRuleSetDescriptor> _unRegisteredRuleSets = new List<IRestfulRuleSetDescriptor>();

		public RestfulRoutingBuilder()
			: this(null, null) { }

		public RestfulRoutingBuilder(IRestfulRuleSorter sorter, IRestfulRuleConverter ruleConverter)
		{
			_ruleSorter = sorter ?? new RestfulRuleSorter();
			_ruleConverter = ruleConverter ?? new RestfulRuleConverter(this, new RestfulFragmentConverter());
		}

		protected IRestfulRuleSorter RuleSorter
		{
			get { return _ruleSorter; }
		}

		protected IRestfulRuleConverter RuleConverter
		{
			get { return _ruleConverter; }
		}

		protected IList<IRestfulRuleSetDescriptor> ListOfRuleSets
		{
			get { return _ruleSets; }
		}

		#region IMonorailRouteBuilder Members

		public void BuildRoutes(IRestfulRuleContainer ruleContainer, RouteCollection routingRuleContainer)
		{
			RuleSorter.Sort(ruleContainer.RuleSets).ForEach(
				rule => routingRuleContainer.Add(RuleConverter.ToRestfulRoutingRule(rule)));
		}

		#endregion

		#region IRestfulRuleContainer Members

		public virtual IEnumerable<IRestfulRuleSetDescriptor> RuleSets
		{
			get { return ListOfRuleSets.AsEnumerable(); }
		}

		public virtual void Register(IRestfulRuleSetDescriptor ruleSet)
		{
			if (_unRegisteredRuleSets.Contains(ruleSet))
			{
				_unRegisteredRuleSets.Remove(ruleSet);
			}
			RegisterControllersInScope(ruleSet);
			RegisterScopeInControllers(ruleSet);
			ListOfRuleSets.Add(ruleSet);
		}

		public virtual void Register(IEnumerable<IRestfulRuleSetDescriptor> ruleSets)
		{
			ruleSets.ForEach(ruleSet => Register(ruleSet));
		}

		public bool IsControllerRegisteredInDifferentScope(string controllerName, string scope)
		{
			ControllerRoutingDescriptor controllerDescriptor = FindControllerRoutingDescriptorForScope(controllerName, scope);

			if (controllerDescriptor != null)
			{
				return
					_registeredScopesForController[controllerDescriptor.ControllerType].Exists(
						s => s.Equals(scope, StringComparison.OrdinalIgnoreCase));
			}

			foreach (var scopes in _registeredControllerScopes)
			{
				if (scopes.Key.Equals(scope, StringComparison.OrdinalIgnoreCase))
				{
					continue;
				}
				if (scopes.Value.Exists(controller => controller.Name.Equals(controllerName, StringComparison.OrdinalIgnoreCase)))
				{
					return true;
				}
			}
			return false;
		}

		public IRestfulRuleSetDescriptor GetOrCreateRuleSetFor(Type controllerType)
		{
			IRestfulRuleSetDescriptor ruleSet = GetRuleSetForControllerType(controllerType);
			if (ruleSet != null)
			{
				return ruleSet;
			}
			ruleSet = new RestfulRuleSetDescriptor();
			ruleSet.AddController(controllerType);
			_unRegisteredRuleSets.Add(ruleSet);
			return ruleSet;
		}

		#endregion

		protected void RegisterControllersInScope(IRestfulRuleSetDescriptor ruleSet)
		{
			if (_registeredControllerScopes.ContainsKey(ruleSet.Scope))
			{
				_registeredControllerScopes[ruleSet.Scope].AddRange(ruleSet.Controllers);
			}
			else
			{
				_registeredControllerScopes[ruleSet.Scope] = new List<ControllerRoutingDescriptor>(ruleSet.Controllers);
			}
		}

		protected void RegisterScopeInControllers(IRestfulRuleSetDescriptor ruleSet)
		{
			foreach (ControllerRoutingDescriptor descriptor in ruleSet.Controllers)
			{
				if (_registeredScopesForController.ContainsKey(descriptor.ControllerType))
				{
					List<string> scopes = _registeredScopesForController[descriptor.ControllerType];
					if (!scopes.Contains(ruleSet.Scope))
					{
						scopes.Add(ruleSet.Scope);
					}
				}
				else
				{
					_registeredScopesForController[descriptor.ControllerType] = new List<string>(new[] { ruleSet.Scope });
				}
			}
		}

		public ControllerRoutingDescriptor FindControllerRoutingDescriptorForScope(string name, string scope)
		{
			List<ControllerRoutingDescriptor> descriptors;
			if (_registeredControllerScopes.TryGetValue(scope, out descriptors))
			{
				return
					(from descriptor in descriptors
					 where descriptor.Name.Equals(name, StringComparison.OrdinalIgnoreCase)
					 select descriptor).FirstOrDefault();
			}
			return null;
		}

		private IRestfulRuleSetDescriptor GetRuleSetForControllerType(Type controllerType)
		{
			foreach (IRestfulRuleSetDescriptor ruleSet in _ruleSets)
			{
				foreach (ControllerRoutingDescriptor controller in ruleSet.Controllers)
				{
					if (controller.ControllerType == controllerType)
					{
						return ruleSet;
					}
				}
			}
			foreach (IRestfulRuleSetDescriptor ruleSet in _unRegisteredRuleSets)
			{
				foreach (ControllerRoutingDescriptor controller in ruleSet.Controllers)
				{
					if (controller.ControllerType == controllerType)
					{
						return ruleSet;
					}
				}
			}
			return null;
		}

		public virtual void BuildRoutes(RouteCollection routingRuleContainer)
		{
			for (int i = _unRegisteredRuleSets.Count - 1; i >= 0; i--)
			{
				new RestfulRuleSetBuilder(this, _unRegisteredRuleSets[i]).Register();
			}
			_unRegisteredRuleSets.Clear();
			BuildRoutes(this, routingRuleContainer);
		}

		public RestfulRuleSetBuilder ForAnyController()
		{
			return new RestfulRuleSetBuilder(this);
		}

		public RestfulRuleSetBuilder ForController<TController>()
		{
			return ForController(typeof(TController));
		}

		private RestfulRuleSetBuilder ForController(Type controllerType)
		{
			IRestfulRuleSetDescriptor ruleSet = GetRuleSetForControllerType(controllerType);
			if (ruleSet != null)
			{
				return new RestfulRuleSetBuilder(this, ruleSet);
			}

			var builder = new RestfulRuleSetBuilder(this);
			builder.AddControllerToRuleSet(controllerType);
			return builder;
		}

		public void RegisterController(Type controllerType)
		{
			new AttributeBuilder().BuildAndRegister(ForController(controllerType), controllerType);
		}

		public void RegisterController<TController>()
		{
			RegisterController(typeof(TController));
		}

		public void RegisterControllers(Assembly assembly)
		{
			if (assembly == null)
			{
				throw new ArgumentNullException("assembly");
			}
			
			IEnumerable<Type> restfulControllers = 
				from type in assembly.GetTypes()
				where !type.IsAbstract && !type.IsInterface 
					&& typeof(IController).IsAssignableFrom(type) 
					&& type.GetCustomAttributes(typeof(SimplyRestfulRouteAttribute), true).Length > 0
				select type;

			foreach (Type type in restfulControllers)
			{
				RegisterController(type);
			}
		}
	}
}