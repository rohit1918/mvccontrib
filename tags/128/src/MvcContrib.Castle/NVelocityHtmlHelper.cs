using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;

namespace MvcContrib.Castle
{
	public class NVelocityHtmlHelper : HtmlHelper
	{
		public NVelocityHtmlHelper(ViewContext viewContext, IViewDataContainer container)
			: base(viewContext, container)
		{
		}

        //public string Submit(string buttonText)
        //{
        //    return base.b
        //    return this.SubmitButton(string.Empty, buttonText);
        //}

        //public string Submit(string buttonText, IDictionary htmlAttributes)
        //{
        //    string id = RemoveKey(htmlAttributes, "id", string.Empty);
        //    return this.SubmitButton(id, buttonText, (object)new DescriptableDictionary(htmlAttributes));
        //}

		public string TextBox(string htmlName, IDictionary htmlAttributes)
		{
			return TextBox(htmlName, string.Empty, htmlAttributes);
		}

		public string TextBox(string htmlName, string value, IDictionary htmlAttributes)
		{
			return this.TextBox(htmlName, value, MakeGeneric(htmlAttributes));
		}

        //public string Mailto(string emailAddress, string linkText, IDictionary htmlAttributes)
        //{
        //    string subject = RemoveKey(htmlAttributes, "subject", string.Empty);
        //    string body = RemoveKey(htmlAttributes, "body", string.Empty);
        //    string cc = RemoveKey(htmlAttributes, "cc", string.Empty);
        //    string bcc = RemoveKey(htmlAttributes, "bcc", string.Empty);

        //    return base.
        //    return this.Mailto(emailAddress, linkText, subject, body, cc, bcc, MakeGeneric(htmlAttributes));
        //}

		private static T RemoveKey<T>(IDictionary hash, string key, T defaultValue)
		{
			object value = hash[key];
			if(value != null)
			{
				hash.Remove(key);
				TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));
				if(converter.CanConvertFrom(value.GetType()))
				{
					return (T)converter.ConvertFrom(value);
				}
			}

			return defaultValue;
		}

		//TODO: Make MvcContrib.Hash do this.
		private static IDictionary<string, object> MakeGeneric(IDictionary source)
		{
			Dictionary<string, object> toReturn = new Dictionary<string, object>();
			foreach(DictionaryEntry entry in source)
			{
				toReturn.Add(entry.Key.ToString(), entry.Value);
			}
			return toReturn;
		}
	}
}
