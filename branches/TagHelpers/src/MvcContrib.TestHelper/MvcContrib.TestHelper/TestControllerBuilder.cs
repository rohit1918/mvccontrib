using System.Collections.Specialized;
using System.Web;
using System.Web.Mvc;
using Castle.DynamicProxy;
using Rhino.Mocks;

namespace MvcTestingFramework
{
    public partial class TestControllerBuilder
    {
        protected MockRepository _mocks;
        protected TempDataDictionary _tempData;

        public TestControllerBuilder()
        {
            AppRelativeCurrentExecutionFilePath = "~/";
            ApplicationPath = "/";
            PathInfo = "";
            RouteData = new RouteData();
            _mocks = new MockRepository();
            Session = new MockSession();
            SetupHttpContext();
        }

        public IHttpContext HttpContext
        {
            get;
            protected set;
        }

        public RedirectToActionData RedirectToActionData
        {
            get;
            protected set;
        }

        public RenderViewData RenderViewData
        {
            get;
            protected set;
        }

        public NameValueCollection Form
        {
            get;
            protected set;
        }

        public NameValueCollection QueryString
        {
            get;
            protected set;
        }

        public IHttpSessionState Session
        {
            get;
            protected set;
        }

        public TempDataDictionary TempDataDictionary
        {
            get;
            protected set;
        }

        public RouteData RouteData
        {
            get;
            set;
        }

        public string AppRelativeCurrentExecutionFilePath
        {
            get;
            set;
        }

        public string ApplicationPath
        {
            get;
            set;
        }

        public string PathInfo
        {
            get;
            set;
        }

        protected void SetupHttpContext()
        {
            HttpContext = _mocks.DynamicMock<IHttpContext>();
            IHttpRequest request = _mocks.DynamicMock<IHttpRequest>();
            IHttpResponse response = _mocks.DynamicMock<IHttpResponse>();
            IHttpServerUtility server = _mocks.DynamicMock<IHttpServerUtility>();

            SetupResult.For(HttpContext.Request).Return(request);
            SetupResult.For(HttpContext.Response).Return(response);
            SetupResult.For(HttpContext.Session).Return(Session);
            SetupResult.For(HttpContext.Server).Return(server);

            QueryString = new NameValueCollection();
            SetupResult.For(request.QueryString).Return(QueryString);

            Form = new NameValueCollection();
            SetupResult.For(request.Form).Return(Form);

            SetupResult.For(request.AppRelativeCurrentExecutionFilePath).Return(AppRelativeCurrentExecutionFilePath);
            SetupResult.For(request.ApplicationPath).Return(ApplicationPath);
            SetupResult.For(request.PathInfo).Return(PathInfo);

            _mocks.Replay(HttpContext);
            _mocks.Replay(request);
            _mocks.Replay(response);

            TempDataDictionary = new TempDataDictionary(HttpContext);
        }

        public T CreateController<T>() where T : Controller
        {
            ProxyGenerator generator = new ProxyGenerator(); //Castle DynamicProxy
            Controller controller = (Controller)generator.CreateClassProxy(typeof(T), new ControllerInterceptor(this));

            ControllerContext controllerContext = new ControllerContext(HttpContext, RouteData, controller);
            controller.ControllerContext = controllerContext;
                //normally set in the internal function controller.Execute();

            typeof(T).GetProperty("TempData").SetValue(controller, TempDataDictionary, null);
                //also set in controller.Execute();

            return controller as T;
        }
    }
}