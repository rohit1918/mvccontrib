using System;
using System.Collections.Specialized;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MvcContrib.ControllerFactories;
using Rhino.Mocks;

namespace MvcContrib.TestHelper
{
	/// <summary>
	/// This is primary class used to create controllers.
	/// After initializing, call InitializeController to create a controller with proper environment elements.
	/// Exposed properties such as Form, QueryString, and HttpContext allow access to text environment.
	/// RenderViewData and RedirectToActionData record those methods
	/// </summary>
	public class TestControllerBuilder
	{
		protected MockRepository _mocks;
		protected TempDataDictionary _tempData;

		/// <summary>
		/// Initializes a new instance of the <see cref="TestControllerBuilder"/> class.
		/// </summary>
		public TestControllerBuilder()
		{
			AppRelativeCurrentExecutionFilePath = "~/";
			ApplicationPath = "/";
			PathInfo = "";
			RouteData = new RouteData();
			_mocks = new MockRepository();
			Session = new MockSession();
			IControllerFactory = new IoCControllerFactory();
			SetupHttpContext();
		}

		/// <summary>
		/// Gets the IControllerFactory that will be used to create controllers with CreateController, by default IoCControllerFactory
		/// </summary>
		/// <value>The IControllerFactory</value>
		public IControllerFactory IControllerFactory { get; protected set; }

		/// <summary>
		/// Gets the HttpContext that built controllers will have set internally when created with InitializeController
		/// </summary>
		/// <value>The HTTPContext</value>
		public HttpContextBase HttpContext { get; protected set; }

		/// <summary>
		/// Gets the Form data that built controllers will have set internally when created with InitializeController
		/// </summary>
		/// <value>The NameValueCollection Form</value>
		public NameValueCollection Form { get; protected set; }

		/// <summary>
		/// Gets the QueryString that built controllers will have set internally when created with InitializeController
		/// </summary>
		/// <value>The NameValueCollection QueryString</value>
		public NameValueCollection QueryString { get; protected set; }

		/// <summary>
		/// Gets the Session that built controllers will have set internally when created with InitializeController
		/// </summary>
		/// <value>The IHttpSessionState Session</value>
		public HttpSessionStateBase Session { get; protected set; }

		/// <summary>
		/// Gets the TempDataDictionary that built controllers will have set internally when created with InitializeController
		/// </summary>
		/// <value>The TempDataDictionary</value>
		public TempDataDictionary TempDataDictionary { get; protected set; }

		/// <summary>
		/// Gets or sets the RouteData that built controllers will have set internally when created with InitializeController
		/// </summary>
		/// <value>The RouteData</value>
		public RouteData RouteData { get; set; }

		/// <summary>
		/// Gets or sets the AppRelativeCurrentExecutionFilePath that built controllers will have set internally when created with InitializeController
		/// </summary>
		/// <value>The RouteData</value>
		public string AppRelativeCurrentExecutionFilePath { get; set; }

		/// <summary>
		/// Gets or sets the AppRelativeCurrentExecutionFilePath string that built controllers will have set internally when created with InitializeController
		/// </summary>
		/// <value>The ApplicationPath string</value>
		public string ApplicationPath { get; set; }

		/// <summary>
		/// Gets or sets the PathInfo string that built controllers will have set internally when created with InitializeController
		/// </summary>
		/// <value>The PathInfo string</value>
		public string PathInfo { get; set; }

		protected void SetupHttpContext()
		{
			HttpContext = _mocks.DynamicMock<HttpContextBase>();
			var request = _mocks.DynamicMock<HttpRequestBase>();
			var response = _mocks.DynamicMock<HttpResponseBase>();
			var server = _mocks.DynamicMock<HttpServerUtilityBase>();

			SetupResult.For(HttpContext.Request).Return(request);
			SetupResult.For(HttpContext.Response).Return(response);
			SetupResult.For(HttpContext.Session).Return(Session);
			SetupResult.For(HttpContext.Server).Return(server);

			QueryString = new NameValueCollection();
			SetupResult.For(request.QueryString).Return(QueryString);

			Form = new NameValueCollection();
			SetupResult.For(request.Form).Return(Form);

			Func<NameValueCollection> paramsFunc = () => new NameValueCollection {QueryString, Form};
			SetupResult.For(request.Params).Do(paramsFunc);

			SetupResult.For(request.AppRelativeCurrentExecutionFilePath).Return(AppRelativeCurrentExecutionFilePath);
			SetupResult.For(request.ApplicationPath).Return(ApplicationPath);
			SetupResult.For(request.PathInfo).Return(PathInfo);
			SetupResult.For(HttpContext.User).PropertyBehavior();

			_mocks.Replay(HttpContext);
			_mocks.Replay(request);
			_mocks.Replay(response);

			//TempDataDictionary = new TempDataDictionary(HttpContext);
            TempDataDictionary = new TempDataDictionary();
		}

		/// <summary>
		/// Creates the controller with proper environment variables setup. 
		/// </summary>
		/// <param name="controller">The controller to initialize</param>
		public void InitializeController(Controller controller)
		{
			var controllerContext = new ControllerContext(HttpContext, RouteData, controller);
			controller.ControllerContext = controllerContext;
			controller.TempData = TempDataDictionary;
		}


		/// <summary>
		/// Creates the controller with proper environment variables setup. 
		/// </summary>
		/// <typeparam name="T">The type of controller to create</typeparam>
		/// <param name="constructorArgs">Arguments to pass to the constructor for the controller</param>
		/// <returns>A new controller of the specified type</returns>
		public T CreateController<T>(params object[] constructorArgs) where T : Controller
		{
			var controller = (Controller)Activator.CreateInstance(typeof(T), constructorArgs);
			InitializeController(controller);
			return controller as T;
		}

		/// <summary>
		/// Creates the controller with proper environment variables setup, using IoC for arguments
		/// </summary>
		/// <typeparam name="T">The type of controller to create</typeparam>
		/// <returns>A new controller of the specified type</returns>
		public T CreateIoCController<T>() where T : Controller
		{
			var controller = (Controller)IControllerFactory.CreateController(null, typeof(T).Name);
			InitializeController(controller);
			return controller as T;
		}
	}
}