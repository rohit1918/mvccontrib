using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Microsoft.Web.Mvc;

namespace MvcContrib.Castle
{
	public class HtmlExtensionDuck : ExtensionDuck
	{
	    private static readonly List<Type> _extensionTypes = new List<Type>
	        {
	        		typeof(ButtonsAndLinkExtensions),                    
					typeof(FormExtensions), 
                    typeof(ImageExtensions), 
                    typeof(LinkExtensions),    
                    typeof(ViewExtensions)
			};

	    public HtmlExtensionDuck(ViewContext viewContext, IViewDataContainer container)
			: this(new NVelocityHtmlHelper(viewContext, container))
		{
		}

		public HtmlExtensionDuck(HtmlHelper htmlHelper)
			: this(htmlHelper, _extensionTypes.ToArray())
		{
		}

		public HtmlExtensionDuck(HtmlHelper htmlHelper, params Type[] extentionTypes)
			: base(htmlHelper, extentionTypes)
		{
		}

	    ///<summary>
	    /// Registers an extension type for evaluation later during duck typing interrogation.
	    /// 
	    /// Add your own extensions here in Application_Start for use in NVelocity views.
	    ///</summary>
	    ///<param name="type"></param>
	    public static void AddExtension(Type type)
	    {
	        if (!_extensionTypes.Contains(type))
	            _extensionTypes.Add(type);
	    }
	}
}
