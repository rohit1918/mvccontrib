using System.Web.Mvc;

namespace MvcContrib.UI.Html
{
	public abstract class BaseHelper
	{
		private IDataBinder _binder = new DefaultDataBinder();

		public ViewContext ViewContext { get; set; }

		public IDataBinder Binder
		{
			get { return _binder; }
			set { _binder = value; }
		}
	}
}
