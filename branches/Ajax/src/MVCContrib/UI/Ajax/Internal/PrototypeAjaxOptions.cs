using System.Text;
using System.Web.Mvc.Ajax;
using MvcContrib.UI.Ajax.JavaScriptElements;

namespace MvcContrib.UI.Ajax.Internal
{
	/// <summary>
	/// Wraps an AjaxOptions object and converts it into Prototype-specific javascript.
	/// </summary>
	public class PrototypeAjaxOptionsWrapper : AjaxOptionsWrapper, IJavaScriptElement
	{
		/// <summary>
		/// Creates a new instance of the PrototypeAjaxOptionsWrapper class.
		/// </summary>
		/// <param name="options">The AjaxOptions object to work with</param>
		public PrototypeAjaxOptionsWrapper(System.Web.Mvc.Ajax.AjaxOptions options) : base(options)
		{
		}

		private IJavaScriptElement BuildOptions()
		{
			var options = new JavaScriptDictionary();

			//The LoadingElementId property should be ignored if onCreate/onComplete are explicitly specified. 
			if(!string.IsNullOrEmpty(_options.LoadingElementId) 
				&& string.IsNullOrEmpty(_options.OnBegin) 
				&& string.IsNullOrEmpty(_options.OnComplete))
			{
				options["onCreate"] = new AnonymousFunction(string.Format("$('{0}').show();", _options.LoadingElementId));
				options["onComplete"] = new AnonymousFunction(string.Format("$('{0}').hide();", _options.LoadingElementId));
			}
			else
			{
				options["onCreate"] = new JavaScriptLiteral(_options.OnBegin);
				options["onComplete"] = new JavaScriptLiteral(_options.OnComplete);				
			}

			options["onSuccess"] = new JavaScriptLiteral(_options.OnSuccess);
			options["onFailure"] = new JavaScriptLiteral(_options.OnFailure);
			options["method"] = _options.HttpMethod;

			if(_options.InsertionMode == InsertionMode.InsertAfter)
			{
				options["insertion"] = new JavaScriptLiteral("Insertion.After");
			}
			else if(_options.InsertionMode == InsertionMode.InsertBefore)
			{
				options["insertion"] = new JavaScriptLiteral("Insertion.Before");
			}

			return options;
		}

		public string ToJavaScript()
		{
			FunctionCall function;
			
			bool useUpdater = !string.IsNullOrEmpty(_options.UpdateTargetId);

			if(useUpdater)
			{
				function = new FunctionCall("new Ajax.Updater", _options.UpdateTargetId, _options.Url, BuildOptions());
			}
			else
			{
				function = new FunctionCall("new Ajax.Request", _options.Url, BuildOptions());
			}

			if(!string.IsNullOrEmpty(_options.Confirm))
			{
				string confirm = "if(confirm('{0}')) {{ {1} }}";
				return string.Format(confirm, _options.Confirm, function.ToJavaScript());
			}

			return function.ToJavaScript();
		}
	}
}