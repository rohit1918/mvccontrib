using System;
using System.Configuration;
using System.Globalization;
using System.Security.Permissions;
using System.Web;

namespace MvcContrib.NHamlViewEngine.Configuration
{
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class NHamlViewEngineSection : ConfigurationSection
	{
		private const string ProductionAttribute = "production";
		private const string ViewsSection = "views";

		public static NHamlViewEngineSection Read()
		{
			return (NHamlViewEngineSection)ConfigurationManager.GetSection("nhamlViewEngine");
		}

		[ConfigurationProperty(ProductionAttribute)]
		public bool Production
		{
			get { return Convert.ToBoolean(this[ProductionAttribute], CultureInfo.CurrentCulture); }
		}

		[ConfigurationProperty(ViewsSection)]
		public ViewsConfiguration Views
		{
			get { return (ViewsConfiguration)this[ViewsSection]; }
		}
	}
}