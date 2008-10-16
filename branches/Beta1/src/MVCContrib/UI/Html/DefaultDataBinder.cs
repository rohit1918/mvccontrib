using System;
using System.Web.Mvc;

namespace MvcContrib.UI.Html
{
	public class DefaultDataBinder : IDataBinder
	{
		public object NestedRootInstance { get; set; }
        
		public object ExtractValue(string target, ViewContext context)
		{
			if (target == null)
				return null;

			var viewData = context.ViewData;

			if(NestedRootInstance != null)
			{
				viewData = new ViewDataDictionary(NestedRootInstance);
			}

			return viewData.Eval(target);
		}

		public IDisposable NestedBindingScope(object rootDataItem)
		{
			return new DefaultBindingScope(this, rootDataItem);
		}

		public class DefaultBindingScope : IDisposable
		{
			private readonly DefaultDataBinder _binder;
			private readonly object _originalRootInstance;

			public DefaultBindingScope(DefaultDataBinder binder, object newRootInstance)
			{
				_binder = binder;
				_originalRootInstance = binder.NestedRootInstance;
				binder.NestedRootInstance = newRootInstance;
			}

			public void Dispose()
			{
				_binder.NestedRootInstance = _originalRootInstance;
			}
		}
	}
}
