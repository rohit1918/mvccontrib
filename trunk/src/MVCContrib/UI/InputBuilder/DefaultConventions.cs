using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using MvcContrib.UI.InputBuilder.Attributes;

namespace MvcContrib.UI.InputBuilder
{
    public static class DefaultConventions
    {
        public static InputModelProperty ModelPropertyBuilder(PropertyInfo propertyInfo, object value)
        {
            if (propertyInfo.PropertyType.IsEnum)
            {

                var selectList = Enum.GetNames(propertyInfo.PropertyType).Select(
                    s => new SelectListItem() { Text = s, Value = s, Selected = s == value.ToString() }).ToArray();

                return new ModelProperty<IEnumerable<SelectListItem>> { Value = selectList };

            }
            if (propertyInfo.PropertyType==typeof(DateTime) )
            {
                return new ModelProperty<DateTime> {Value = (DateTime) value};
            }

            return new ModelProperty<object> { Value = value };
        }

        public static string PartialName(PropertyInfo propertyInfo)
        {
            if (propertyInfo.AttributeExists<UIHintAttribute>())
                return propertyInfo.GetAttribute<UIHintAttribute>().UIHint;

            if (propertyInfo.AttributeExists<DataTypeAttribute>())
                return propertyInfo.GetAttribute<DataTypeAttribute>().DataType.ToString();        
            
            if (propertyInfo.PropertyType.IsEnum)
                return typeof(Enum).Name;

            return propertyInfo.PropertyType.Name ;

        }
        
        
        public static string ExampleForProperty(PropertyInfo propertyInfo)
        {
            
            if (propertyInfo.AttributeExists<ExampleAttribute>())
            {
                return propertyInfo.GetAttribute<ExampleAttribute>().Example;
            }
            return string.Empty;
            
        }

        public static string LabelForProperty(PropertyInfo propertyInfo)
        {
            if (propertyInfo.AttributeExists<LabelAttribute>())
            {
                return propertyInfo.GetAttribute<LabelAttribute>().Label;                
            }
            return propertyInfo.Name.ToSeparatedWords();            
        }

        public static string PropertyName(PropertyInfo propertyInfo)
        {
            return propertyInfo.Name;
        }

        public static bool PropertyIsRequired(PropertyInfo propertyInfo)
        {
            return propertyInfo.AttributeExists<RequiredAttribute>();
        }

 
        public static bool AttributeExists<T>(this PropertyInfo propertyInfo) where T : class
        {
            var attribute = propertyInfo.GetCustomAttributes(typeof(T), false)
                                .FirstOrDefault() as T;
            if (attribute == null)
            {
                return false;
            }
            return true;            
        }
        
        public static T GetAttribute<T>(this PropertyInfo propertyInfo) where T : class
        {
            return propertyInfo.GetCustomAttributes(typeof(T), false).FirstOrDefault() as T;
        }
        public static Type PropertyType(PropertyInfo propertyInfo)
        {
            return propertyInfo.PropertyType;
        }

        public static bool ModelIsInvalid<T>(PropertyInfo propertyInfo,HtmlHelper<T> htmlHelper) where T : class
        {
            return htmlHelper.ViewData.ModelState.ContainsKey(propertyInfo.Name) &&
                   htmlHelper.ViewData.ModelState[propertyInfo.Name].Errors.Count > 0;
        }

        public static object ValueFromModelProperty(PropertyInfo propertyInfo,object model)
        {
            var value = propertyInfo.GetValue(model, new object[0]);
            if (value == null)
                return string.Empty;
            return value;
            
        }
    }
}