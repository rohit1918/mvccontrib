using System.Xml;

namespace MvcContrib.XsltViewEngine
{
	public static class XsltViewEngineExtensionMethods
	{

		public static XmlElement CreateNewNode(this XmlDocument xmldoc, string sNode, string sText, params string[] sAttributes)
	{
		XmlElement xmlelem;
		XmlText xmltext;
		XmlAttribute xmlatt;

		xmlelem = xmldoc.CreateElement("", sNode, "");
		xmltext = xmldoc.CreateTextNode(sText);
		xmlelem.AppendChild(xmltext);

		for (int i = 0; i < sAttributes.Length; i++)
		{
			if (i % 2 == 0)
			{
				xmlatt = xmldoc.CreateAttribute("", sAttributes[i], "");
				xmlelem.SetAttributeNode(xmlatt);

				if ((i + 1) < sAttributes.Length)
				{ xmlatt.Value = sAttributes[i + 1]; }

			}

		}
		return xmlelem;
	}
	}
}