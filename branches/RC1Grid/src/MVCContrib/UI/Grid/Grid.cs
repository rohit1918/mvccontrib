using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MvcContrib.UI.Grid
{
	///<summary>
	/// A basic table based grid renderer
	///</summary>
	public class Grid
	{
		/// <summary>
		/// The writer to output the results to.
		/// </summary>
		protected TextWriter Writer { get; set; }

		/// <summary>
		/// Renders text to the output stream
		/// </summary>
		/// <param name="text">The text to render</param>
		protected virtual void Write(string text)
		{
			Writer.Write(text);
		}

		public virtual void Render()
		{
			
		}



	}

	public interface IGridData : IEnumerator
	{
		bool HasData();
		

	}

	public interface IGridColumn
	{
		bool CanRenderColumn();
		bool CanRenderCell(object value);


	}
}
