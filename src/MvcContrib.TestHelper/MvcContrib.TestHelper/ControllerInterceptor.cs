using System.Reflection;
using System.Web.Mvc;
using Castle.Core.Interceptor;

namespace MvcTestingFramework
{
    public partial class TestControllerBuilder //remember c++ "friend" classes?
    {
        #region Nested type: ControllerInterceptor

        private class ControllerInterceptor : IInterceptor
        {
            private TestControllerBuilder _parentHandler;

            public ControllerInterceptor(TestControllerBuilder parentHandler)
            {
                _parentHandler = parentHandler;
            }

            #region IInterceptor Members

            public void Intercept(IInvocation invocation)
            {
                if(invocation.Method.Name == "RenderView" && invocation.Arguments.Length == 3)
                {
                    string viewName = (string)invocation.Arguments[0];
                    string masterName = (string)invocation.Arguments[1];
                    object viewData = (object)invocation.Arguments[2];
                    _parentHandler.RenderViewData = new RenderViewData
                    {
                        ViewName = viewName,
                        MasterName = masterName,
                        ViewData = viewData
                    }
                    ;
                    return;
                }
                if(invocation.Method.Name == "RedirectToAction" && invocation.Arguments.Length == 1)
                {
                    HandleRedirectToAction(invocation.Arguments[0]);
                    return;
                }
                /*
                if(invocation.Method.GetCustomAttributes(typeof(ControllerActionAttribute), false).Length > 0)
                {
                    //Could add code here to build the RouteData of the controller (by assigning _parentHandler.RouteData)
                    //based on the method call and arguments in combination with the defined route table
                }
                */
                invocation.Proceed();
            }

            #endregion

            protected void HandleRedirectToAction(object values)
            {
                //crazy reflection stuff because RedirectData and RedirectDataWithController are marked as internal. (Rage!)
                PropertyInfo[] props = values.GetType().GetProperties();
                string actionName = "", controllerName = "";
                foreach(PropertyInfo p in props)
                {
                    if(p.Name == "Action")
                    {
                        actionName = (string)p.GetValue(values, null);
                    }
                    else if(p.Name == "Controller")
                    {
                        controllerName = (string)p.GetValue(values, null);
                    }
                }
                _parentHandler.RedirectToActionData = new RedirectToActionData
                {
                    ActionName = actionName,
                    ControllerName = controllerName
                }
                ;
            }
        }

        #endregion
    }
}