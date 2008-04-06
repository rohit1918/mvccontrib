using System;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using MvcContrib.Rest.Routing.Attributes;
using MvcContrib.Rest.Routing.Builders;
using MvcContrib.Rest.Routing.Ext;

namespace MvcContrib.Rest.Routing.Services
{
	public class AttributeBuilder
	{
		public void BuildAndRegister(RestfulRuleSetBuilder builder, Type controllerType)
		{
			if (!typeof(IController).IsAssignableFrom(controllerType))
			{
				throw new ArgumentException("controllerType must be a IController", "controllerType");
			}
			if (controllerType.IsAbstract)
			{
				throw new ArgumentException("controllerType cannot be an abstract class, it must be a concrete class",
				                            "controllerType");
			}
			if (controllerType.IsInterface)
			{
				throw new ArgumentException("controller type cannot be an interface, it must be a concrete class",
				                            "controllerType");
			}

			if (controllerType.AttributeExists<SimplyRestfulRouteAttribute>())
			{
				BuildRoute(builder, controllerType);
				BuildExtraListings(builder, controllerType);
				builder.Register();
			}
		}

		private void BuildExtraListings(RestfulRuleSetBuilder builder, Type controllerType)
		{
			foreach (MethodInfo methodInfo in controllerType.GetMethods(BindingFlags.Public | BindingFlags.Instance))
			{
				object[] methodInfoAttributes = methodInfo.GetCustomAttributes(false);
				if (methodInfoAttributes.Any(attrib => attrib is ExtraListingActionAttribute))
				{
					methodInfoAttributes = Sort(methodInfoAttributes);
					ExtraListingRuleBuilder<RestfulRuleSetBuilder, RestfulRuleSetBuilder> extraBuilder =
						builder.WithExtraListingRoute();
					foreach (object obj in methodInfoAttributes)
					{
						if (obj is OptionalParameterAttribute)
						{
							var attrib = (OptionalParameterAttribute) obj;
							extraBuilder.ToOptionalParameter(attrib.Name, attrib.DefaultValue,
							                                 ((IRestfulParameterAttribute) attrib).AcceptedValuesAndAliases);
						}

						if (obj is RequiredParameterAttribute)
						{
							var attrib = (RequiredParameterAttribute) obj;
							extraBuilder.ToRequiredParameter(attrib.Name,
							                                 ((IRestfulParameterAttribute) attrib).AcceptedValuesAndAliases);
						}

						if (obj is ExtraListingActionAttribute)
						{
							var attrib = (ExtraListingActionAttribute) obj;
							if (string.IsNullOrEmpty(attrib.Alias))
							{
								extraBuilder.ToAction(methodInfo.Name);
							}
							else
							{
								extraBuilder.ToAction(methodInfo.Name, attrib.Alias);
							}
						}
					}
					extraBuilder.Register();
				}
			}
		}

		private void BuildRoute(RestfulRuleSetBuilder builder, Type controllerType)
		{
			foreach (object obj in Sort(controllerType.GetCustomAttributes(false)))
			{
				if (obj is OptionalParameterAttribute)
				{
					var attrib = (OptionalParameterAttribute) obj;
					builder.ToOptionalParameter(attrib.Name, attrib.DefaultValue,
					                            ((IRestfulParameterAttribute) attrib).AcceptedValuesAndAliases);
				}

				if (obj is RequiredParameterAttribute)
				{
					var attrib = (RequiredParameterAttribute) obj;
					builder.ToRequiredParameter(attrib.Name, ((IRestfulParameterAttribute) attrib).AcceptedValuesAndAliases);
				}
				if (obj is RestfulParentAttribute)
				{
					var attrib = (RestfulParentAttribute) obj;
					builder.FromRestfulParent(attrib.ParentControllers[0]);
				}
				if (obj is RestfulIdAttribute)
				{
					var attrib = (RestfulIdAttribute) obj;
					IdBuilder<RestfulRuleSetBuilder, RestfulRuleSetBuilder> idBuilder = builder.UsingId();
					if (!string.IsNullOrEmpty(attrib.Pattern))
					{
						idBuilder.RestrictedToPattern(attrib.Pattern);
					}
					else
					{
						switch (attrib.PatternType)
						{
							case PatternType.Int32:
								idBuilder.RestrictedToPositiveInt32();
								break;
							case PatternType.Int64:
								idBuilder.RestrictedToPositiveInt64();
								break;
							case PatternType.Guid:
								idBuilder.RestrictedToGuid();
								break;
							case PatternType.Custom:
								idBuilder.RestrictedToPattern(".+");
								break;
							default:
								throw new ArgumentOutOfRangeException();
						}
					}
				}
				if (obj is RestfulControllerAttribute)
				{
					builder.ToController();
				}
				if (obj is RestfulListingAttribute)
				{
					builder.ToListing();
				}
				if (obj is RestfulEntityAttribute)
				{
					builder.ToEntity();
				}
			}
		}

		private object[] Sort(object[] attributes)
		{
			return (from attrib in attributes
			        orderby
			        	(attrib is IRestfulNodePositionAttribute ? ((IRestfulNodePositionAttribute) attrib).NodePosition : -1)
			        select attrib).ToArray();
		}
	}
}