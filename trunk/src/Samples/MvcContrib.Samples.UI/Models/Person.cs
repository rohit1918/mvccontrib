using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace MvcContrib.Samples.UI.Models
{
	public class Person
	{
		[Range(0, 50)]
		public string Name { get; set; }
		public int Id { get; set; }
		public string Gender { get; set; }
		public DateTime DateOfBirth { get; set; }
		public IList<int> Roles { get; set; }
	}
}