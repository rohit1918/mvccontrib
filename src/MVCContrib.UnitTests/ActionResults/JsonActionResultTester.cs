using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MvcContrib.ActionResults;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Rhino.Mocks;
using System.IO;
using System.Web.Script.Serialization;
using System.Runtime.Serialization;
using System.Text;
using System.Runtime.Serialization.Json;

namespace MvcContrib.UnitTests.ActionResults
{
	[TestFixture]
	public class JsonResultTester
	{
		private MockRepository _mocks;
		private ControllerContext _controllerContext;

		[SetUp]
		public void SetUp()
		{
			_mocks = new MockRepository();
			_controllerContext = new ControllerContext(_mocks.DynamicHttpContextBase(), new RouteData(), _mocks.DynamicMock<IController>());
			_mocks.ReplayAll();
		}

		[Test]
		public void ObjectToSerialize_should_return_the_object_to_serialize()
		{
			var result = new JsonResult(new Person {Id = 1, Name = "Bob"});
			Assert.That(result.ObjectToSerialize, Is.InstanceOfType(typeof(Person)));
			Assert.That(((Person)result.ObjectToSerialize).Name, Is.EqualTo("Bob"));
		}

		[Test]
		public void Should_set_content_type()
		{
			JsonResult result = new JsonResult(new int[] {2, 3, 4});
			result.ExecuteResult(_controllerContext);
			Assert.AreEqual("application/json", _controllerContext.HttpContext.Response.ContentType);
		}

		[Test]
		public void Should_serialise_json()
		{
			JsonResult result = new JsonResult(new Person {Id = 5, Name = "Jeremy"});
			result.ExecuteResult(_controllerContext);

#pragma warning disable 0618
			//http://weblogs.asp.net/scottgu/archive/2007/10/01/tip-trick-building-a-tojson-extension-method-using-net-3-5.aspx#4301973
			JavaScriptSerializer serializer = new JavaScriptSerializer();
#pragma warning restore 0618

			Person p = serializer.Deserialize<Person>(_controllerContext.HttpContext.Response.Output.ToString());

			Assert.That(p.Name, Is.EqualTo("Jeremy"));
			Assert.That(p.Id, Is.EqualTo(5));
		}

		[Test]
		public void Should_use_datacontract()
		{
			JsonResult result = new JsonResult(new PersonWithDataContract {Id = 9, Name = "Chris"});
			result.ExecuteResult(_controllerContext);
			
			DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(PersonWithDataContract));
			_controllerContext.HttpContext.Response.OutputStream.Seek(0, SeekOrigin.Begin);
			PersonWithDataContract p = serializer.ReadObject(_controllerContext.HttpContext.Response.OutputStream) as PersonWithDataContract;
			
			Assert.That(p.Name, Is.EqualTo("Chris"));
			Assert.That(p.Id, Is.EqualTo(9));
		}

		public class Person
		{
			public string Name { get; set; }
			public int Id { get; set; }
		}

		[DataContract(Name="Person")]
		public class PersonWithDataContract
		{
			[DataMember(Name="FullName")] public string Name { get; set; }
			[DataMember] public int Id { get; set; }
		}
	}
}
