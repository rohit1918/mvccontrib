using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Practices.CompositeWeb.ObjectBuilder;
using Microsoft.Practices.ObjectBuilder;
using MvcContrib.ObjectBuilder.Configuration;

namespace MvcContrib.ObjectBuilder
{
    public class DependencyContainer : IDisposable, IDependencyContainer
    {
        protected IBuilder<BuilderStage> builder;
        protected LifetimeContainer lifetime;
        protected Locator locator;

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
        public virtual void Dispose()
        {
            if(lifetime != null)
            {
                var items = new List<object>();
                items.AddRange(lifetime);

                foreach(var item in items)
                    builder.TearDown(locator, item);

                lifetime.Dispose();
                lifetime = null;
                locator = null;
                builder = null;
            }
        }

        // Methods

        virtual public object Inject(object @object)
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

        virtual public object Inject<TToBuild>(object @object)
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

        virtual public object Inject(object @object, Type itemType)
        {
            if(@object != null)
            {
                string temporaryId = Guid.NewGuid().ToString();
                var policies = new PolicyList(new PolicyList[0]);
                policies.Set<ISingletonPolicy>(new SingletonPolicy(false), itemType, temporaryId);
                policies.Set<ICreationPolicy>(new DefaultCreationPolicy(), itemType, temporaryId);
                policies.Set<IPropertySetterPolicy>(new PropertySetterPolicy(), itemType, temporaryId);
                return builder.BuildUp(locator, itemType, temporaryId, @object, new[] {policies});
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
        virtual public IEnumerable<T> FindSingletons<T>()
        {
            foreach(var obj in lifetime)
                if(obj is T)
                    yield return (T)obj;
        }

        /// <summary>
        /// Gets an object of the given type from the container.
        /// </summary>
        /// <returns>The object</returns>
        virtual public TBuild Get<TBuild>()
        {
            return builder.BuildUp<TBuild>(locator, null, null);
        }

        /// <summary>
        /// Gets an object of the given type from the container.
        /// </summary>
        /// <returns>The object</returns>
        virtual public object Get(Type tBuild)
        {
            return builder.BuildUp(locator, tBuild, null, null);
        }

        /// <summary>
        /// Registers a singleton instance in the container.
        /// </summary>
        /// <typeparam name="TBuild">The type of the singleton</typeparam>
        /// <param name="item">The item instance to be registered as the singleton</param>
        virtual public void RegisterInstance<TBuild>(TBuild item)
        {
            RegisterSingleton<TBuild>();
            builder.BuildUp<TBuild>(locator, null, item);
            Inject<TBuild>(item);
        }

        /// <summary>
        /// Registers the given type as a singleton in the container.
        /// </summary>
        /// <typeparam name="TBuild">The type to be made a singleton</typeparam>
        virtual public void RegisterSingleton<TBuild>()
        {
            RegisterSingleton(typeof(TBuild));
        }

        /// <summary>
        /// Registers the given type as a singleton in the container.
        /// </summary>
        /// <typeparam name="TBuild">The type to be made a singleton</typeparam>
        virtual public void RegisterSingleton(Type type)
        {
            builder.Policies.Set<ISingletonPolicy>(new SingletonPolicy(true), type, null);
        }
		
        /// <summary>
        /// Registers a type mapping in the container.
        /// </summary>
        /// <typeparam name="TRequested">The type that is requested by the user</typeparam>
        /// <typeparam name="TToBuild">The type to be built instead</typeparam>
        virtual public void RegisterTypeMapping<TRequested, TToBuild>()
        {
            RegisterTypeMapping(typeof(TRequested), typeof(TToBuild));
        }

        /// <summary>
        /// Registers a type mapping in the container.
        /// </summary>
        /// <typeparam name="TRequested">The type that is requested by the user</typeparam>
        /// <typeparam name="TToBuild">The type to be built instead</typeparam>
        virtual public void RegisterTypeMapping(Type requestedType, Type typeToBuild)
        {
            builder.Policies.Set<ITypeMappingPolicy>(new TypeMappingPolicy(typeToBuild, null), requestedType, null);
        }

        /// <summary>
        /// Registers a property setter.
        /// </summary>
        /// <typeparam name="T">The type</typeparam>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="value">The value.</param>
        virtual public void RegisterPropertySetter<T>(string propertyName, object value)
        {
            RegisterPropertySetter(typeof(T), propertyName, value);
        }

        /// <summary>
        /// Registers the property setter.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="value">The value.</param>
        virtual public void RegisterPropertySetter(Type type, string propertyName, object value)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }
            if  (propertyName == null) 
            {
                throw new ArgumentNullException("propertyName");
            }
            var policy = new PropertySetterPolicy();
            PropertyInfo pi = type.GetProperty(propertyName);
            if (pi == null)
                throw new ApplicationException("Property not found: " + propertyName);

            policy.Properties.Add(propertyName, new PropertySetterInfo(pi, new ValueParameter(pi.PropertyType, value)));

            builder.Policies.Set<IPropertySetterPolicy>(policy, type, null);
        }

        /// <summary>
        /// Registers a property setter.
        /// </summary>
        /// <typeparam name="TType">The type</typeparam>
        /// <typeparam name="TValue">The type of the value. An instance of this type will be retreived from the container</typeparam>
        /// <param name="propertyName">Name of the property.</param>
        virtual public void RegisterPropertySetter<TType, TValue>(string propertyName)
        {
            RegisterPropertySetter(typeof(TType), propertyName, typeof(TValue));
        }

        /// <summary>
        /// Registers the property setter.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="value">The type of the value. An instance of this type will be retreived from the container.</param>
        virtual public void RegisterPropertySetter(Type type, string propertyName, Type value)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }
            if (propertyName == null)
            {
                throw new ArgumentNullException("propertyName");
            }
            var policy = new PropertySetterPolicy();

            policy.Properties.Add(propertyName, new PropertySetterInfo(propertyName, new CreationParameter(value)));

            builder.Policies.Set<IPropertySetterPolicy>(policy, type, null);
        }

        /// <summary>
        /// Registers a method injection.
        /// </summary>
        /// <typeparam name="T">The type</typeparam>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="parameters">The parameters.</param>
        virtual public void RegisterMethodInjection<T>(string methodName, params IParameter[] parameters)
        {
            RegisterMethodInjection(typeof(T), methodName, parameters);
        }

        /// <summary>
        /// Registers a method injection.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="parameters">The parameters.</param>
        virtual public void RegisterMethodInjection(Type type, string methodName, params IParameter[] parameters)
        {
            if (methodName == null)
            {
                throw new ArgumentNullException("methodName");
            }
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            MethodPolicy policy = GetMethodPolicy(type, null);
            policy.Methods.Add(methodName, new MethodCallInfo(methodName, parameters));
        }

        private MethodPolicy GetMethodPolicy(Type typeToBuild, string idToBuild)
        {
            var policy = builder.Policies.Get<IMethodPolicy>(typeToBuild, idToBuild) as MethodPolicy;

            if (policy == null)
            {
                policy = new MethodPolicy();
                builder.Policies.Set<IMethodPolicy>(policy, typeToBuild, idToBuild);
            }

            return policy;
        }

    }
}
