using System;
using System.Collections.Generic;
using Microsoft.Practices.CompositeWeb.ObjectBuilder;
using Microsoft.Practices.ObjectBuilder;

namespace MvcContrib.ObjectBuilder
{
	public class DependencyContainer : IDisposable, IDependencyContainer
	{
		private IBuilder<BuilderStage> builder;
		private LifetimeContainer lifetime;
		private Locator locator;

		// Lifetime

		/// <summary>
		/// Initializes a new instance of the <see cref="DependencyContainer"/> class
		/// that supports attribute-based reflection.
		/// </summary>
		public DependencyContainer()
			: this(true)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DependencyContainer"/> class.
		/// </summary>
		/// <param name="enableReflection">Set to <c>true</c> to enable support for
		/// attribute-based reflection</param>
		public DependencyContainer(bool enableReflection)
			: this(new ContainerXmlConfig(enableReflection))
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DependencyContainer"/> class.
		/// </summary>
		/// <param name="xml">The XML configuration for the container</param>
		public DependencyContainer(string xml)
			: this(new ContainerXmlConfig(xml))
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DependencyContainer"/> class.
		/// </summary>
		/// <param name="configurator">The container configurator.</param>
		public DependencyContainer(IBuilderConfigurator<BuilderStage> configurator)
		{
			builder = new WCSFBuilderBase<BuilderStage>(configurator);
			locator = new Locator();
			lifetime = new LifetimeContainer();
			locator.Add(typeof(ILifetimeContainer), lifetime);
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing,
		/// or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			if(lifetime != null)
			{
				List<object> items = new List<object>();
				items.AddRange(lifetime);

				foreach(object item in items)
					builder.TearDown(locator, item);

				lifetime.Dispose();
				lifetime = null;
				locator = null;
				builder = null;
			}
		}

		// Methods

		public object Inject(object @object)
		{
			if(@object != null)
			{
				return Inject(@object, @object.GetType());
			}
			else
			{
				return null;
			}
		}

		public object Inject<TToBuild>(object @object)
		{
			if(@object != null)
			{
				return Inject(@object, typeof(TToBuild));
			}
			else
			{
				return null;
			}
		}

		public object Inject(object @object, Type itemType)
		{
			if(@object != null)
			{
				string temporaryId = Guid.NewGuid().ToString();
				PolicyList policies = new PolicyList(new PolicyList[0]);
				policies.Set<ISingletonPolicy>(new SingletonPolicy(false), itemType, temporaryId);
				policies.Set<ICreationPolicy>(new DefaultCreationPolicy(), itemType, temporaryId);
				policies.Set<IPropertySetterPolicy>(new PropertySetterPolicy(), itemType, temporaryId);
				return builder.BuildUp(locator, itemType, temporaryId, @object, new PolicyList[] {policies});
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// Finds all the singletons in the container that implement the given type.
		/// </summary>
		/// <returns>An enumeration of the matching items</returns>
		public IEnumerable<T> FindSingletons<T>()
		{
			foreach(object obj in lifetime)
				if(obj is T)
					yield return (T)obj;
		}

		/// <summary>
		/// Gets an object of the given type from the container.
		/// </summary>
		/// <returns>The object</returns>
		public TBuild Get<TBuild>()
		{
			return builder.BuildUp<TBuild>(locator, null, null);
		}

		/// <summary>
		/// Gets an object of the given type from the container.
		/// </summary>
		/// <returns>The object</returns>
		public object Get(Type tBuild)
		{
			return builder.BuildUp(locator, tBuild, null, null);
		}

		/// <summary>
		/// Registers a singleton instance in the container.
		/// </summary>
		/// <typeparam name="TBuild">The type of the singleton</typeparam>
		/// <param name="item">The item instance to be registered as the singleton</param>
		public void RegisterInstance<TBuild>(TBuild item)
		{
			RegisterSingleton<TBuild>();
			builder.BuildUp<TBuild>(locator, null, item);
			Inject<TBuild>(item);
		}

		/// <summary>
		/// Registers the given type as a singleton in the container.
		/// </summary>
		/// <typeparam name="TBuild">The type to be made a singleton</typeparam>
		public void RegisterSingleton<TBuild>()
		{
			builder.Policies.Set<ISingletonPolicy>(new SingletonPolicy(true), typeof(TBuild), null);
		}

		/// <summary>
		/// Registers a type mapping in the container.
		/// </summary>
		/// <typeparam name="TRequested">The type that is requested by the user</typeparam>
		/// <typeparam name="TToBuild">The type to be built instead</typeparam>
		public void RegisterTypeMapping<TRequested, TToBuild>()
		{
			RegisterTypeMapping(typeof(TRequested), typeof(TToBuild));
		}

		/// <summary>
		/// Registers a type mapping in the container.
		/// </summary>
		/// <typeparam name="TRequested">The type that is requested by the user</typeparam>
		/// <typeparam name="TToBuild">The type to be built instead</typeparam>
		public void RegisterTypeMapping(Type requestedType, Type typeToBuild)
		{
			builder.Policies.Set<ITypeMappingPolicy>(new TypeMappingPolicy(typeToBuild, null), requestedType, null);
		}
	}
}