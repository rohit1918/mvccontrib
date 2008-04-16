using System.Web.Mvc;

namespace MvcContrib.UI.Html
{
	public abstract class BaseHelper
	{
		private ViewContext _viewContext;
		private IDataBinder _binder = new DefaultDataBinder();

		public ViewContext ViewContext
		{
			get { return _viewContext; }
			set { _viewContext = value; }
		}

		public IDataBinder Binder
		{
			get { return _binder; }
			set { _binder = value; }
		}
	}
}