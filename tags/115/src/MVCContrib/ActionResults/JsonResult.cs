using System.Web.Mvc;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using System.Web.Script.Serialization;

namespace MvcContrib.ActionResults
{
	/// <summary>
	/// Action result that serializes the specified object into JSON and outputs it to the response stream.
	/// <example>
	/// <![CDATA[
	/// public JsonResult AsJson() {
	///		List<Person> people = _peopleService.GetPeople();
	///		return new JsonResult(people);
	/// }
	/// ]]>
	/// </example>
	/// </summary>
	public class JsonResult : ActionResult
	{
		private object _objectToSerialize;

		/// <summary>
		/// Creates a new instance of the JsonResult class.
		/// </summary>
		/// <param name="objectToSerialize">The object to serialize to JSON.</param>
		public JsonResult(object objectToSerialize)
		{
			_objectToSerialize = objectToSerialize;
		}

		/// <summary>
		/// The object to be serialized to XML.
		/// </summary>
		public object ObjectToSerialize
		{
			get { return _objectToSerialize; }
		}

		/// <summary>
		/// Serialises the object that was passed into the constructor to XML and writes the corresponding XML to the result stream.
		/// </summary>
		/// <param name="context">The controller context for the current request.</param>
		public override void ExecuteResult(ControllerContext context)
		{
			if (_objectToSerialize != null)
			{
				context.HttpContext.Response.ContentType = "application/json";

				object[] attribs = _objectToSerialize.GetType().GetCustomAttributes(typeof(DataContractAttribute), true);
				if (attribs != null && attribs.Length > 0)
				{
					DataContractJsonSerializer serializer = new DataContractJsonSerializer(_objectToSerialize.GetType());
					serializer.WriteObject(context.HttpContext.Response.OutputStream, _objectToSerialize);
				}
				else
				{
#pragma warning disable 0618
					//Obsolete way according to MS, but preferred by many since you don't have to do anything
					//scottgu has to convince someone to remove the obsolete attribute :)
					//http://weblogs.asp.net/scottgu/archive/2007/10/01/tip-trick-building-a-tojson-extension-method-using-net-3-5.aspx#4301973
					JavaScriptSerializer serializer = new JavaScriptSerializer();
					context.HttpContext.Response.Output.Write(serializer.Serialize(_objectToSerialize));
#pragma warning restore 0618
				}

			}
		}
	}
}
