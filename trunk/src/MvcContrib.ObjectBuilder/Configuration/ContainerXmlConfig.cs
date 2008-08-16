using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Microsoft.Practices.ObjectBuilder;
using ConstructorReflectionStrategy=
	Microsoft.Practices.CompositeWeb.ObjectBuilder.Strategies.ConstructorReflectionStrategy;
using CreationStrategy=Microsoft.Practices.CompositeWeb.ObjectBuilder.Strategies.CreationStrategy;
using MethodReflectionStrategy=Microsoft.Practices.CompositeWeb.ObjectBuilder.Strategies.MethodReflectionStrategy;
using PropertyReflectionStrategy=Microsoft.Practices.CompositeWeb.ObjectBuilder.Strategies.PropertyReflectionStrategy;

namespace MvcContrib.ObjectBuilder.Configuration
{
    public partial class ContainerXmlConfig : IBuilderConfigurator<BuilderStage>
    {
        // Fields

        private IBuilder<BuilderStage> builder;
        private ContainerXmlConfigElement config;

        // Lifetime

        public ContainerXmlConfig(bool enableReflection)
        {
            config = new ContainerXmlConfigElement {EnableReflection = enableReflection};
        }

        public ContainerXmlConfig(string xml)
        {
            config = ParseXmlConfiguration(xml);
        }

        // Methods

        public void ApplyConfiguration(IBuilder<BuilderStage> builder)
        {
            this.builder = builder;

            ProcessStrategies();

            if(config.Mappings != null)
                foreach(var mapping in config.Mappings)
                    ProcessMapping(mapping);

            if(config.BuildRules != null)
            {
                foreach(var buildRule in config.BuildRules)
                {
                    Type buildType = Type.GetType(buildRule.Type);
                    Debug.Assert(buildType != null);
                    if(buildType == null)
                        throw new ArgumentNullException("Type");

                    ProcessConstructor(buildRule, buildType);
                    ProcessProperties(buildRule, buildType);
                    ProcessMethods(buildRule, buildType);
                    ProcessSingleton(buildRule, buildType);
                }
            }
        }

        private MethodPolicy GetMethodPolicy(Type typeToBuild, string idToBuild)
        {
            var policy = builder.Policies.Get<IMethodPolicy>(typeToBuild, idToBuild) as MethodPolicy;

            if(policy == null)
            {
                policy = new MethodPolicy();
                builder.Policies.Set<IMethodPolicy>(policy, typeToBuild, idToBuild);
            }

            return policy;
        }

        private static IParameter GetParameterFromConfigParam(object param)
        {
            if(param is ValueElement)
                return ValueElementToParameter(param as ValueElement);
            else if(param is RefElement)
                return RefElementToParameter(param as RefElement);

            Debug.Assert(false);
            return null;
        }

        private static ContainerXmlConfigElement ParseXmlConfiguration(string xml)
        {
            var ser = new XmlSerializer(typeof(ContainerXmlConfigElement));
            var stringReader = new StringReader(xml);
            XmlSchema schema =
                XmlSchema.Read(
                    Assembly.GetExecutingAssembly().GetManifestResourceStream(
                        "MvcContrib.ObjectBuilder.Configuration.ContainerXmlConfigElement.xsd"), null);
            var settings = new XmlReaderSettings {ValidationType = ValidationType.Schema};
        	settings.Schemas.Add(schema);
            XmlReader reader = XmlReader.Create(stringReader, settings);
            var configData = (ContainerXmlConfigElement)ser.Deserialize(reader);
            return configData;
        }

        private void ProcessStrategies()
        {
            builder.Strategies.AddNew<TypeMappingStrategy>(BuilderStage.PreCreation);
            builder.Strategies.AddNew<SingletonStrategy>(BuilderStage.PreCreation);

            if(config.EnableReflection)
            {
                builder.Strategies.AddNew<ConstructorReflectionStrategy>(BuilderStage.PreCreation);
                builder.Strategies.AddNew<PropertyReflectionStrategy>(BuilderStage.PreCreation);
                builder.Strategies.AddNew<MethodReflectionStrategy>(BuilderStage.PreCreation);
            }

            builder.Strategies.AddNew<CreationStrategy>(BuilderStage.Creation);
            builder.Strategies.AddNew<PropertySetterStrategy>(BuilderStage.Initialization);
            builder.Strategies.AddNew<MethodExecutionStrategy>(BuilderStage.Initialization);

            builder.Strategies.AddNew<BuilderAwareStrategy>(BuilderStage.PostInitialization);

            builder.Policies.SetDefault<ICreationPolicy>(new DefaultCreationPolicy());
        }


        private void ProcessConstructor(BuildRuleElement buildRule, Type buildType)
        {
            if(buildRule.Constructor == null)
                return;

            var policy = new ConstructorPolicy();

            foreach(var param in buildRule.Constructor.Items)
                policy.AddParameter(GetParameterFromConfigParam(param));

            builder.Policies.Set<ICreationPolicy>(policy, buildType, null);
        }

        private void ProcessMapping(MappingElement mapping)
        {
            Type fromType = Type.GetType(mapping.FromType);
            Type toType = Type.GetType(mapping.ToType);

            if (fromType == null) throw new InvalidOperationException("FromType is required");
            if (toType == null) throw new InvalidOperationException("ToType is required");

            builder.Policies.Set<ITypeMappingPolicy>(new TypeMappingPolicy(toType, null), fromType, null);
        }

        private void ProcessMethods(BuildRuleElement buildRule, Type buildType)
        {
            if(buildRule.Method == null)
                return;

            MethodPolicy policy = GetMethodPolicy(buildType, null);

            foreach(var method in buildRule.Method)
            {
                var parameters = new List<IParameter>();

                foreach(var param in method.Items)
                    parameters.Add(GetParameterFromConfigParam(param));

                policy.Methods.Add(method.Name, new MethodCallInfo(method.Name, parameters));
            }
        }

        private void ProcessProperties(BuildRuleElement buildRule, Type buildType)
        {
            if(buildRule.Property == null)
                return;

            var policy = new PropertySetterPolicy();

            foreach(var prop in buildRule.Property)
                policy.Properties.Add(prop.Name,
                                      new PropertySetterInfo(prop.Name, GetParameterFromConfigParam(prop.Item)));

            builder.Policies.Set<IPropertySetterPolicy>(policy, buildType, null);
        }

        private void ProcessSingleton(BuildRuleElement buildRule, Type buildType)
        {
            if(buildRule.Mode == ModeElement.Singleton)
                builder.Policies.Set<ISingletonPolicy>(new SingletonPolicy(true), buildType, null);
        }

        private static IParameter RefElementToParameter(RefElement refParam)
        {
            Type paramType = Type.GetType(refParam.Type);
            Debug.Assert(paramType != null);
            return new CreationParameter(paramType, null);
        }

        private static IParameter ValueElementToParameter(ValueElement valueParam)
        {
            Type paramType = Type.GetType(valueParam.Type);
            Debug.Assert(paramType != null);
            return new ValueParameter(paramType, Convert.ChangeType(valueParam.Value, paramType));
        }
    }
}
