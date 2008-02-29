using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using MvcContrib.MetaData;

namespace MvcContrib.Attributes
{
	#region castle license headers
	// Copyright 2004-2008 Castle Project - http://www.castleproject.org/
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
	#endregion
	/// <summary>
	/// Base attribute for the return binders.
	/// </summary>
	/// <remarks>
	/// Modified from Monorail Project. It is more useful to have it as an attribute.
	/// </remarks>
	public abstract class AbstractReturnBinderAttribute:Attribute,IReturnBinder
	{
		public abstract void Bind(IController controller, ControllerContext controllerContext,
		                          Type returnType, object returnValue);
	}
}
