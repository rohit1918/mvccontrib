using System;
using System.Reflection;
using System.Web.Mvc;

namespace MvcContrib.UI.InputBuilder
{
    public class InputModelPropertyFactory<T> where T : class
    {
        #region ...
        private readonly HtmlHelper<T> _htmlHelper;

        public InputModelPropertyFactory(HtmlHelper<T> htmlHelper)
        {
            _htmlHelper = htmlHelper;
        }

        public static Func<PropertyInfo, string> ExampleForPropertyConvention = DefaultConventions.ExampleForProperty;

        public static Func<PropertyInfo, object, object> ValueFromModelPropertyConvention =
            DefaultConventions.ValueFromModelProperty;

        public static Func<PropertyInfo, string> LabelForPropertyConvention = DefaultConventions.LabelForProperty;

        public static Func<PropertyInfo, HtmlHelper<T>, bool> ModelIsInvalidConvention =
            DefaultConventions.ModelIsInvalid;


        public static Func<PropertyInfo, string> PropertyNameConvention = DefaultConventions.PropertyName;
        public static Func<PropertyInfo, Type> PropertyTypeConvention = DefaultConventions.PropertyType;
        public static Func<PropertyInfo, string> PartialNameConvention = DefaultConventions.PartialName;
        public static Func<PropertyInfo, object, InputModelProperty> ModelPropertyBuilder = DefaultConventions.ModelPropertyBuilder;
        #endregion

        public static Func<PropertyInfo, bool> PropertyIsRequiredConvention = DefaultConventions.PropertyIsRequired;

        public InputModelProperty Create(PropertyInfo propertyInfo)
        {
            object value = ValueFromModelPropertyConvention(propertyInfo, _htmlHelper.ViewData.Model);
            InputModelProperty model = ModelPropertyBuilder(propertyInfo, value);
            
            model.PropertyIsRequired = PropertyIsRequiredConvention(propertyInfo);
         
            #region ...
            model.PartialName = PartialNameConvention(propertyInfo);
            model.Name = PropertyNameConvention(propertyInfo);
            model.Label = LabelForPropertyConvention(propertyInfo);
            model.Example = ExampleForPropertyConvention(propertyInfo);
            model.HasValidationMessages = ModelIsInvalidConvention(propertyInfo, _htmlHelper);
            model.Type = PropertyTypeConvention(propertyInfo);
            #endregion

            return model;
        }
    }
}
