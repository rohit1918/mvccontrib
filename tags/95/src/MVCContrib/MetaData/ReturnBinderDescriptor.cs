using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvcContrib.MetaData
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
	/// Holds information about the returnbinder.
	/// </summary>
	/// <remarks>
	/// This is useful since you don't need to do reflection all the time to get the binder
	/// Modified from Monorail project at http://svn.castleproject.org:8080/svn/castle/trunk/MonoRail
	/// </remarks>
	public class ReturnBinderDescriptor
	{
		private readonly Type returnType;
		private readonly IReturnBinder returnTypeBinder;

		/// <summary>
		/// Initializes a new instance of the <see cref="ReturnBinderDescriptor"/> class.
		/// </summary>
		/// <param name="returnType">Type of the return.</param>
		/// <param name="returnTypeBinder">The return type binder.</param>
		public ReturnBinderDescriptor(Type returnType, IReturnBinder returnTypeBinder)
		{
			this.returnType = returnType;
			this.returnTypeBinder = returnTypeBinder;
		}

		/// <summary>
		/// Gets the type of the return.
		/// </summary>
		/// <value>The type of the return.</value>
		public Type ReturnType
		{
			get { return returnType; }
		}

		/// <summary>
		/// Gets or sets the return type binder.
		/// </summary>
		/// <value>The return type binder.</value>
		public IReturnBinder ReturnTypeBinder
		{
			get { return returnTypeBinder; }
		}
	}
}
