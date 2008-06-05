using MvcContrib.NHamlViewEngine.Configuration;
using NUnit.Framework;

namespace MvcContrib.UnitTests.NHamlViewEngine
{
	[TestFixture, Category("NHamlViewEngine")]
	public class ConfigurationTester
	{
		[Test]
		public void Can_Read_Production_Setting_From_AppSettings()
		{
			var section = NHamlViewEngineSection.Read();

			Assert.IsNotNull(section);
			Assert.IsFalse(section.Production);
		}

		[Test]
		public void Can_Read_Views_Assemblies_Section_From_AppSettings()
		{
			var section = NHamlViewEngineSection.Read();
			Assert.IsNotNull(section);

			var assembly = section.Views.Assemblies[0];
			Assert.AreEqual(1, section.Views.Assemblies.Count);
			Assert.AreEqual("System, Version=2.0.0.0, Culture=neutral, PublicKeyToken=B77A5C561934E089", assembly.Name);
		}

		[Test]
		public void Can_Read_Views_Namespaces_Section_From_AppSettings()
		{
			var section = NHamlViewEngineSection.Read();
			Assert.IsNotNull(section);

			var _namespace = section.Views.Namespaces[0];
			Assert.AreEqual(1, section.Views.Namespaces.Count);
			Assert.AreEqual("System.Collections", _namespace.Name);
		}

		[Test]
		public void ForCoverage()
		{
			new AssemblyConfigurationElement("Name").Name = "Name";
			new NamespaceConfigurationElement("Name").Name = "Name";

			var ncc = new NamespacesConfigurationCollection();
			ncc.Add(new NamespaceConfigurationElement("Name"));
			var nce = ncc["Name"];
			ncc.IndexOf(nce);
			ncc.Remove(nce);
			ncc.Add(nce);
			ncc.RemoveAt(0);
			ncc.Add(nce);
			ncc.Remove("Name");
			ncc.Clear();
			ncc.Add(nce);
			ncc[0] = nce;
		}
	}
}