using System.Web.Mvc;

namespace MvcContrib.UI.Html
{
	public abstract class BaseHelper
	{
#pragma warning disable 618,612
		private IDataBinder _binder = new DefaultDataBinder();
#pragma warning restore 618,612

		public ViewContext ViewContext { get; set; }

		public IDataBinder Binder
		{
			get { return _binder; }
			set { _binder = value; }
		}
	}
}
