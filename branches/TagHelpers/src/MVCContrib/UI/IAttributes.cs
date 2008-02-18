using System;
using System.Collections;
using System.Collections.Generic;
namespace MvcContrib.UI
{
	public interface IAttributes: IDictionary<string, string>, IDictionary
	{
		new int Count { get; }
		new System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<string, string>> GetEnumerator();
		int GetEstLength();
	}
}
