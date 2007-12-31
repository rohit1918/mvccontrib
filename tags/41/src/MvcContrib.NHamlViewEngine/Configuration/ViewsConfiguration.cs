using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvcContrib.NHamlViewEngine.Configuration
{
	public class ViewsConfiguration : ConfigurationElement
	{
		private const string AssembliesElement = "assemblies";
		private const string NamespacesElement = "namespaces";

		[ConfigurationProperty(AssembliesElement)]
		[ConfigurationCollection(typeof(AssembliesConfigurationCollection))]
		public AssembliesConfigurationCollection Assemblies
		{
			get { return (AssembliesConfigurationCollection)base[AssembliesElement]; }
		}

		[ConfigurationProperty(NamespacesElement)]
		[ConfigurationCollection(typeof(NamespacesConfigurationCollection))]
		public NamespacesConfigurationCollection Namespaces
		{
			get { return (NamespacesConfigurationCollection)base[NamespacesElement]; }
		}
	}

	public class AssembliesConfigurationCollection : BaseConfigurationCollection<AssemblyConfigurationElement>
	{
	}

	public class NamespacesConfigurationCollection : BaseConfigurationCollection<NamespaceConfigurationElement>
	{
	}

	public class AssemblyConfigurationElement : ViewsCollectionElement
	{
		private const string AssemblyElement = "assembly";

		public AssemblyConfigurationElement()
		{ 
		}

		public AssemblyConfigurationElement(string name)
		{
			this.Name = name;
		}

		public override string Key
		{
			get { return Name; }
		}

		[ConfigurationProperty(AssemblyElement, IsRequired = true, IsKey = true)]
		public string Name
		{
			get { return (string)this[AssemblyElement]; }
			set { this[AssemblyElement] = value; }
		}
	}

	public class NamespaceConfigurationElement : ViewsCollectionElement
	{
		private const string NamespaceElement = "namespace";

		public NamespaceConfigurationElement()
		{ 
		}

		public NamespaceConfigurationElement(string name)
		{
			this.Name = name;
		}

		public override string Key
		{
			get { return Name; }
		}

		[ConfigurationProperty(NamespaceElement, IsRequired = true, IsKey = true)]
		public string Name
		{
			get { return (string)this[NamespaceElement]; }
			set { this[NamespaceElement] = value; }
		}
	}

	public class BaseConfigurationCollection<T> : ConfigurationElementCollection
		where T : ViewsCollectionElement, new()
	{
		public override ConfigurationElementCollectionType CollectionType
		{
			get { return ConfigurationElementCollectionType.AddRemoveClearMap; }
		}

		protected override ConfigurationElement CreateNewElement()
		{
			return new T();
		}

		protected override Object GetElementKey(ConfigurationElement element)
		{
			return ((T)element).Key;
		}

		public T this[int index]
		{
			get
			{
				return (T)BaseGet(index);
			}
			set
			{
				if (BaseGet(index) != null)
					BaseRemoveAt(index);
				BaseAdd(index, value);
			}
		}

		public new T this[string name]
		{
			get { return (T)BaseGet(name); }
		}

		public int IndexOf(T element)
		{
			return BaseIndexOf(element);
		}

		public void Add(T element)
		{
			BaseAdd(element);
		}
		protected override void BaseAdd(ConfigurationElement element)
		{
			BaseAdd(element, false);
		}

		public void Remove(T element)
		{
			if (BaseIndexOf(element) >= 0)
				BaseRemove(element.Key);
		}

		public void RemoveAt(int index)
		{
			BaseRemoveAt(index);
		}

		public void Remove(string name)
		{
			BaseRemove(name);
		}

		public void Clear()
		{
			BaseClear();
		}
	}

	public abstract class ViewsCollectionElement : ConfigurationElement
	{
		public abstract string Key
		{
			get;
		}
	}
}
