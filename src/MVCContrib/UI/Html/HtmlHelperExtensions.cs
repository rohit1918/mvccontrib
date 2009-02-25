using System;
using System.Web.Mvc;

namespace MvcContrib.UI.Html
{
	public static class HtmlHelperExtensions
	{

		[Obsolete("ValidationHelper is obsolete. Please consider using ModelState for validation instead.")]
		public static IValidationHelper Validation(this HtmlHelper helper)
		{
			return ValidationHelper.GetInstance(helper.ViewContext);
		}

        /// <summary>
        /// Normalizes a url in the form ~/path/to/resource.aspx.
        /// </summary>
        /// <param name="html"></param>
        /// <param name="relativeUrl"></param>
        /// <returns></returns>
        public static string ResolveUrl(this HtmlHelper html, string relativeUrl)
        {
            if (relativeUrl == null)
                return null;

            if (!relativeUrl.StartsWith("~"))
                return relativeUrl;

            var basePath = html.ViewContext.HttpContext.Request.ApplicationPath;
            string url = basePath + relativeUrl.Substring(1);
            return url.Replace("//", "/");
        }

        /// <summary>
        /// Renders a link tag referencing the stylesheet.  Assumes the CSS is in the /content/css directory unless a
        /// full relative URL is specified.
        /// </summary>
        /// <param name="html"></param>
        /// <param name="cssFile"></param>
        /// <returns></returns>
        public static string Stylesheet(this HtmlHelper html, string cssFile)
        {
            string cssPath = cssFile.Contains("~") ? cssFile : "~/content/css/" + cssFile;
            string url = ResolveUrl(html, cssPath);
            return string.Format("<link type=\"text/css\" rel=\"stylesheet\" href=\"{0}\" />\n", url);
        }

        /// <summary>
        /// Renders a script tag referencing the javascript file.  Assumes the file is in the /scripts directory
        /// unless a full relative URL is specified.
        /// </summary>
        /// <param name="html"></param>
        /// <param name="jsFile"></param>
        /// <returns></returns>
        public static string ScriptInclude(this HtmlHelper html, string jsFile)
        {
            string jsPath = jsFile.Contains("~") ? jsFile : "~/Scripts/" + jsFile;
            string url = ResolveUrl(html, jsPath);
            return string.Format("<script type=\"text/javascript\" src=\"{0}\" ></script>\n", url);
        }

        /// <summary>
        /// Renders a favicon link tag.  Points to ~/favicon.ico.
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string Favicon(this HtmlHelper html)
        {
            string path = ResolveUrl(html, "~/favicon.ico");
            return string.Format("<link rel=\"shortcut icon\" href=\"{0}\" />\n", path);
        }
	}
}