using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvcContrib.UI.Tags.Validators
{
	public interface IValidator
	{
		string ReferenceId { get; set; }
		string ErrorMessage { get; set; }
		string ValidationGroup { get; set; }
		string ValidationFunction { get; }
	}
}
