using System;
using System.Web.Mvc;

namespace MvcContrib.Castle
{
	public class HtmlExtensionDuck : ExtensionDuck
	{
		public static readonly Type[] HTML_EXTENSION_TYPES =
			new Type[]
				{
					//typeof(ButtonsAndLinkExtensions),
                    //typeof(CheckBoxExtension),
					typeof(FormExtensions), 
                    //typeof(ImageExtensions), 
                    typeof(LinkExtensions), 
                    //typeof(RadioListExtension),
					//typeof(SelectExtension),
                    typeof(TextInputExtensions), typeof(UserControlExtensions)
				};

		public HtmlExtensionDuck(ViewContext viewContext, IViewDataContainer container)
			: this(new NVelocityHtmlHelper(viewContext, container))
		{
		}

		public HtmlExtensionDuck(HtmlHelper htmlHelper)
			: this(htmlHelper, HTML_EXTENSION_TYPES)
		{
		}

		public HtmlExtensionDuck(HtmlHelper htmlHelper, params Type[] extentionTypes)
			: base(htmlHelper, extentionTypes)
		{
		}
	}
}
