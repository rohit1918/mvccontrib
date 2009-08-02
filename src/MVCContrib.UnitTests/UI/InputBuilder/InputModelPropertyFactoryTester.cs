using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Web.Mvc;
using MvcContrib.UI.InputBuilder;
using MvcContrib.UI.InputBuilder.Attributes;
using NUnit.Framework;

namespace UnitTests
{
    [TestFixture]
    public class InputModelPropertyFactoryTester
    {
        [Test]
        public void The_factory_should_call_the_conventions()
        {
            //arrange+            
            ConfigureFactoryConventions();

            var model = new Model() { String = "foo" };            
            var factory = new InputModelPropertyFactory<Model>(CreateHelper(model));

            PropertyInfo property = model.GetType().GetProperty("String");

            //act
            var inputModelProperty = factory.Create(property);
            
            //assert
            Assert.AreEqual(inputModelProperty.Type,typeof(String));
            Assert.AreEqual(inputModelProperty.Name, "name");
            Assert.AreEqual(inputModelProperty.PartialName, "String");
            Assert.AreEqual(inputModelProperty.HasExample(), true);
            Assert.AreEqual(inputModelProperty.Example, "example");
            Assert.AreEqual(inputModelProperty.PropertyIsRequired, true);
            
            

        }

        private void ConfigureFactoryConventions()
        {
            InputModelPropertyFactory<Model>.ExampleForPropertyConvention = (prop) => "example";
            InputModelPropertyFactory<Model>.LabelForPropertyConvention = (prop) => "label";
            InputModelPropertyFactory<Model>.ModelIsInvalidConvention = (prop, helper) => false;
            InputModelPropertyFactory<Model>.PartialNameConvention = (prop) => "String";
            InputModelPropertyFactory<Model>.ModelPropertyBuilder = (prop, value) => new ModelProperty<String>();
            InputModelPropertyFactory<Model>.PropertyIsRequiredConvention = (prop) => true;
            InputModelPropertyFactory<Model>.PropertyNameConvention = (prop) => "name";
            InputModelPropertyFactory<Model>.ValueFromModelPropertyConvention = (prop, obj) => "value";
        }

        public static HtmlHelper<T> CreateHelper<T>(T model) where T : class
        {
            var context = new ViewContext();
            context.ViewData = new ViewDataDictionary();
            context.ViewData.Model = model;
            return new HtmlHelper<T>(context, new ViewDataContainer(context.ViewData));
        }
    }

    public class Model
    {
        public string String { get; set; }

        [Label("label")]
        [Example("example")]
        public Foo Enum { get; set; }

        [PartialView("theview")]
        public string UiHintProperty { get; set; }

        public DateTime timestamp { get; set; }

        [DataType(System.ComponentModel.DataAnnotations.DataType.EmailAddress)]
        public string DataType { get; set; }
    }

    public enum Foo
    {
        Bar,
        OhYeah
    }

    public class ViewDataContainer : IViewDataContainer
    {
        public ViewDataContainer(ViewDataDictionary viewData)
        {
            ViewData = viewData;
        }

        public ViewDataDictionary ViewData { get; set; }
    }

}