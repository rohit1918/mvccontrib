// Copyright 2004-2007 Castle Project - http://www.castleproject.org/
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

// MODIFICATIONS HAVE BEEN MADE TO THIS FILE

namespace MvcContrib.BrailViewEngine
{
	using System;
	using System.Collections;
	using System.IO;
	using System.Web;
	using System.Collections.Generic;
	using System.Text;
	using System.Web.Mvc;

	/// <summary>
	///Base class for all the view scripts, this is the class that is responsible for
	/// support all the behind the scenes magic such as variable to PropertyBag trasnlation, 
	/// resources usage, etc. 
	/// </summary>
	public abstract class BrailBase : IView
	{
		protected IController __controller;
		protected ViewContext __viewContext;

		/// <summary>
		/// Reference to the DSL service
		/// </summary>
		private DslProvider _dsl;

		/// <summary>
		/// This is used by layout scripts only, for outputing the child's content
		/// </summary>
		protected TextWriter childOutput;

//		protected IEngineContext context;
		public string LastVariableAccessed;
		private TextWriter outputStream;

		/// <summary>
		/// usually used by the layout to refer to its view, or a subview to its parent
		/// </summary>
		protected BrailBase parent;

		private Hashtable properties = new Hashtable(StringComparer.InvariantCultureIgnoreCase);

		/// <summary>
		/// used to hold the ComponentParams from the view, so their views/sections could access them
		/// </summary>
//		private IList viewComponentsParameters;

		protected BooViewEngine viewEngine;

		/// <summary>
		/// Initializes a new instance of the <see cref="BrailBase"/> class.
		/// </summary>
		/// <param name="viewEngine">The view engine.</param>
		/// <param name="output">The output.</param>
		/// <param name="context">The context.</param>
		/// <param name="__controller">The controller.</param>
		/// <param name="__controllerContext">The __controller context.</param>
		public BrailBase(BooViewEngine viewEngine, TextWriter output)
//		(BooViewEngine viewEngine, TextWriter output, IEngineContext context, IController __controller, IControllerContext __controllerContext)
		{
			this.viewEngine = viewEngine;
			outputStream = output;
		}

		private BrailBase _layout;
		public BrailBase Layout
		{
			get { return _layout; }
			set { _layout = value; }
		}

		/// <summary>
		///The path of the script, this is filled by AddBrailBaseClassStep
		/// and is used for sub views 
		/// </summary>
		public virtual string ScriptDirectory
		{
			get { return viewEngine.ViewRootDir; }
		}

		/// <summary>
		/// Gets the view engine.
		/// </summary>
		/// <value>The view engine.</value>
		public BooViewEngine ViewEngine
		{
			get { return viewEngine; }
		}

		/// <summary>
		/// Gets the DSL provider
		/// </summary>
		/// <value>Reference to the current DSL Provider</value>
		public DslProvider Dsl
		{
			get
			{
				BrailBase view = this;
				if (null == view._dsl)
				{
					view._dsl = new DslProvider(view);
				}

				return view._dsl;
				//while (view.parent != null)
				//{
				//    view = view.parent;
				//}

				//if (view._dsl == null)
				//{
				//    view._dsl = new DslProvider(view);
				//}

				//return view._dsl;
			}
		}

		/// <summary>
		/// Gets the flash.
		/// </summary>
		/// <value>The flash.</value>
		public TempDataDictionary Flash
		{
			get { return __viewContext.TempData; }
		}

		/// <summary>
		/// Gets the output stream.
		/// </summary>
		/// <value>The output stream.</value>
		public TextWriter OutputStream
		{
			get { return outputStream; }
		}

		/// <summary>
		/// Gets or sets the child output.
		/// </summary>
		/// <value>The child output.</value>
		public TextWriter ChildOutput
		{
			get { return childOutput; }
			set { childOutput = value; }
		}

		/// <summary>
		/// Gets the properties.
		/// </summary>
		/// <value>The properties.</value>
		public IDictionary Properties
		{
			get { return properties; }
		}

		/// <summary>
		/// Runs this instance, this is generated by the script
		/// </summary>
		public abstract void Run();

		/// <summary>
		/// Output the subview to the client, this is either a relative path "SubView" which
		/// is relative to the current /script/ or an "absolute" path "/home/menu" which is
		/// actually relative to ViewDirRoot
		/// </summary>
		/// <param name="subviewName"></param>
		public string OutputSubView(string subviewName)
		{
			return OutputSubView(subviewName, new Hashtable());
		}

		/// <summary>
		/// Similiar to the OutputSubView(string) function, but with a bunch of parameters that are used
		/// just for this subview. This parameters are /not/ inheritable.
		/// </summary>
		/// <returns>An empty string, just to make it possible to use inline ${OutputSubView("foo")}</returns>
		public string OutputSubView(string subviewName, IDictionary parameters)
		{
			OutputSubView(subviewName, outputStream, parameters);
			return string.Empty;
		}

		/// <summary>
		/// Outputs the sub view to the writer
		/// </summary>
		/// <param name="subviewName">Name of the subview.</param>
		/// <param name="writer">The writer.</param>
		/// <param name="parameters">The parameters.</param>
		public void OutputSubView(string subviewName, TextWriter writer, IDictionary parameters)
		{
			string subViewFileName = GetSubViewFilename(subviewName);
			BrailBase subView = viewEngine.GetCompiledScriptInstance(subViewFileName, writer);
			subView.SetParent(this);
			foreach(DictionaryEntry entry in parameters)
			{
				subView.properties[entry.Key] = entry.Value;
			}
			subView.RenderView(__viewContext);
			foreach(DictionaryEntry entry in subView.Properties)
			{
				if (subView.Properties.Contains(entry.Key + ".@bubbleUp") == false)
					continue;
				properties[entry.Key] = entry.Value;
				properties[entry.Key + ".@bubbleUp"] = true;
			}
		}

		/// <summary>
		/// Get the sub view file name, if the subview starts with a '/' 
		/// then the filename is considered relative to ViewDirRoot
		/// otherwise, it's relative to the current script directory
		/// </summary>
		/// <param name="subviewName"></param>
		/// <returns></returns>
		public string GetSubViewFilename(string subviewName)
		{
			//absolute path from Views directory
			if (subviewName[0] == '/')
				return subviewName.Substring(1) + viewEngine.ViewFileExtension;
			return Path.Combine(ScriptDirectory, subviewName) + viewEngine.ViewFileExtension;
		}

		/// <summary>
		/// this is called by ReplaceUnknownWithParameters step to create a more dynamic experiance
		/// any uknown identifier will be translate into a call for GetParameter('identifier name').
		/// This mean that when an uknonwn identifier is in the script, it will only be found on runtime.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public object GetParameter(string name)
		{
			ParameterSearch search = GetParameterInternal(name);
			if (search.Found == false)
				throw new Exception("Parameter '" + name + "' was not found!");
			return search.Value;
		}

		/// <summary>
		/// this is called by ReplaceUnknownWithParameters step to create a more dynamic experiance
		/// any uknown identifier with the prefix of ? will be translated into a call for 
		/// TryGetParameter('identifier name without the ? prefix').
		/// This method will return null if the value it not found.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public object TryGetParameter(string name)
		{
			ParameterSearch search = GetParameterInternal(name);
			return new IgnoreNull(search.Value);
		}

		/// <summary>
		/// Gets the parameter - implements the logic for searching parameters.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <returns></returns>
		private ParameterSearch GetParameterInternal(string name)
		{
			LastVariableAccessed = name;
			//temporary syntax to turn @variable to varaible, imitating :symbol in ruby
			if (name.StartsWith("@"))
				return new ParameterSearch(name.Substring(1), true);
//			if (viewComponentsParameters != null)
//			{
//				foreach(IDictionary viewComponentProperties in viewComponentsParameters)
//				{
//					if (viewComponentProperties.Contains(name))
//						return new ParameterSearch(viewComponentProperties[name], true);
//				}
//			}
			if (properties.Contains(name))
				return new ParameterSearch(properties[name], true);
			if (parent != null)
				return parent.GetParameterInternal(name);
			return new ParameterSearch(null, false);
		}

		/// <summary>
		/// Sets the parent.
		/// </summary>
		/// <param name="myParent">My parent.</param>
		public void SetParent(BrailBase myParent)
		{
			parent = myParent;
		}

		/// <summary>
		/// Allows to check that a parameter was defined
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public bool IsDefined(string name)
		{
			ParameterSearch search = GetParameterInternal(name);
			return search.Found;
		}

//		/// <summary>
//		/// This is required because we may want to replace the output stream and get the correct
//		/// behavior from components call RenderText() or RenderSection()
//		/// </summary>
//		public IDisposable SetOutputStream(TextWriter newOutputStream)
//		{
//			ReturnOutputStreamToInitialWriter disposable = new ReturnOutputStreamToInitialWriter(OutputStream, this);
//			outputStream = newOutputStream;
//			return disposable;
//		}

		/// <summary>
		/// Will output the given value as escaped HTML
		/// </summary>
		/// <param name="toOutput"></param>
		public void OutputEscaped(object toOutput)
		{
			if (toOutput == null)
				return;
			string str = toOutput.ToString();
			OutputStream.Write(HttpUtility.HtmlEncode(str));
		}

		/// <summary>
		/// Note that this will overwrite any existing property.
		/// </summary>
		public void AddProperty(string name, object item)
		{
			properties[name] = item;
		}

//		/// <summary>
//		/// Adds the view component newProperties.
//		/// This will be included in the parameters searching, note that this override
//		/// the current parameters if there are clashing.
//		/// The search order is LIFO
//		/// </summary>
//		/// <param name="newProperties">The newProperties.</param>
//		public void AddViewComponentProperties(IDictionary newProperties)
//		{
//			if (viewComponentsParameters == null)
//				viewComponentsParameters = new ArrayList();
//			viewComponentsParameters.Insert(0, newProperties);
//		}
//
//		/// <summary>
//		/// Removes the view component properties, so they will no longer be visible to the views.
//		/// </summary>
//		/// <param name="propertiesToRemove">The properties to remove.</param>
//		public void RemoveViewComponentProperties(IDictionary propertiesToRemove)
//		{
//			if (viewComponentsParameters == null)
//				return;
//			viewComponentsParameters.Remove(propertiesToRemove);
//		}
//
//		public void RenderComponent(string componentName)
//		{
//			RenderComponent(componentName, new Hashtable());
//		}
//
//		public void RenderComponent(string componentName, IDictionary parameters)
//		{
//			BrailViewComponentContext componentContext =
//				new BrailViewComponentContext(this, null, componentName, OutputStream,
//				                              new Hashtable(StringComparer.InvariantCultureIgnoreCase));
//			AddViewComponentProperties(componentContext.ComponentParameters);
//			IViewComponentFactory componentFactory = (IViewComponentFactory) context.GetService(typeof(IViewComponentFactory));
//			ViewComponent component = componentFactory.Create(componentName);
//			component.Init(context, componentContext);
//			component.Render();
//			if (componentContext.ViewToRender != null)
//			{
//				OutputSubView("/" + componentContext.ViewToRender, componentContext.ComponentParameters);
//			}
//			RemoveViewComponentProperties(componentContext.ComponentParameters);
//		}

		/// <summary>
		/// Initialize all the properties that a script may need
		/// One thing to note here is that resources are wrapped in ResourceToDuck wrapper
		/// to enable easy use by the script
		/// </summary>
		private void InitProperties(ViewContext viewContext)
		{
			properties = new Hashtable(StringComparer.InvariantCultureIgnoreCase);
			//properties.Add("dsl", new DslWrapper(this));
			properties.Add("Controller", viewContext.Controller);

			IHttpContext myContext = viewContext.HttpContext;

			properties.Add("Context", myContext);
			if (myContext != null)
			{
				properties.Add("request", myContext.Request);
				properties.Add("response", myContext.Response);
				properties.Add("session", myContext.Session);
			}

//			if (controllerContext.Resources != null)
//			{
//				foreach(string key in controllerContext.Resources.Keys)
//				{
//					properties.Add(key, new ResourceToDuck(controllerContext.Resources[key]));
//				}
//			}

			if (myContext != null && myContext.Request.QueryString != null)
			{
				foreach(string key in myContext.Request.QueryString.AllKeys)
				{
					if (key == null) continue;
					properties[key] = myContext.Request.QueryString[key];
				}
			}

			if (myContext != null && myContext.Request.Form != null)
			{
				foreach(string key in myContext.Request.Form.AllKeys)
				{
					if (key == null) continue;
					properties[key] = myContext.Request.Form[key];
				}
			}

			if (viewContext.TempData != null)
			{
				foreach (KeyValuePair<string, object> entry in viewContext.TempData)
				{
					properties[entry.Key] = entry.Value;
				}
			}

			IDictionary viewData = viewContext.ViewData as IDictionary;
			if (viewData != null)
			{
				foreach (DictionaryEntry entry in viewData)
				{
					properties[entry.Key] = entry.Value;
				}
			}
			else
			{
				properties["viewData"] = viewContext.ViewData;
			}

			properties["html"] = new HtmlHelper(viewContext);
			properties["url"] = new UrlHelper(viewContext);

//			if (controllerContext.Helpers != null)
//			{
//				foreach(DictionaryEntry entry in controllerContext.Helpers)
//				{
//					properties[entry.Key] = entry.Value;
//				}
//			}

			if (myContext != null)
			{
				properties["siteRoot"] = myContext.Request.ApplicationPath;
			}
		}

		#region Nested type: ParameterSearch

		private class ParameterSearch
		{
			private readonly bool found;
			private readonly object value;

			public ParameterSearch(object value, bool found)
			{
				this.found = found;
				this.value = value;
			}

			public bool Found
			{
				get { return found; }
			}

			public object Value
			{
				get { return value; }
			}
		}

		#endregion

//		#region Nested type: ReturnOutputStreamToInitialWriter
//
//		private class ReturnOutputStreamToInitialWriter : IDisposable
//		{
//			private TextWriter initialWriter;
//			private BrailBase parent;
//
//			public ReturnOutputStreamToInitialWriter(TextWriter initialWriter, BrailBase parent)
//			{
//				this.initialWriter = initialWriter;
//				this.parent = parent;
//			}
//
//			#region IDisposable Members
//
//			public void Dispose()
//			{
//				parent.outputStream = initialWriter;
//			}
//
//			#endregion
//		}
//
//		#endregion

		public void RenderView(ViewContext viewContext)
		{
			__controller = viewContext.Controller;
			__viewContext = viewContext;

			InitProperties(__viewContext);

			try
			{
				Run();
			}
			catch(Exception e)
			{
				HandleException("", this, e);
			}
			
			if (Layout != null)
			{
				Layout.SetParent(this);
				try
				{
					Layout.Run();
				}
				catch (Exception e)
				{
					HandleException("", Layout, e);
				}
			}
		}

		private void HandleException(string templateName, BrailBase view, Exception e)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("Exception on RenderView: ").AppendLine(templateName);
			sb.Append("Last accessed variable: ").Append(view.LastVariableAccessed);
			string msg = sb.ToString();
			sb.Append("Exception: ").AppendLine(e.ToString());
//			Log(msg);
			throw new Exception(msg, e);
		}
	}
}