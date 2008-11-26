using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MvcContrib.Pagination;
using MvcContrib.UI.Html.Grid;
using NUnit.Framework;

namespace MvcContrib.UnitTests
{
	[TestFixture]
	public class SortableTester
	{
		[Test, ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void Should_throw_if_page_number_is_less_than_1()
		{
			var strings = new List<string>();
			strings.AsSortable(0,0,null,false);
		}

		[Test]
		public void Can_create_sortable_from_queryable_and_grid_params()
		{
			var strings = new List<string>().AsQueryable();
			var gridParams = new GridParams();
			gridParams.PageSize = 44;
			gridParams.PageNumber = 4;
			gridParams.SortColumn = "bob";
			ISortable<string> sortable = strings.AsSortable(gridParams);
			Assert.IsNotNull(sortable);
			Assert.AreEqual(sortable.PageNumber, 4);
			Assert.AreEqual(sortable.PageSize, 44);
			Assert.AreEqual(sortable.SortColumn, "bob");
		}

		[Test]
		public void Can_create_sortable_from_queryable_and_arguments()
		{
			var strings = new List<string>().AsQueryable();
			ISortable<string> sortable = strings.AsSortable(4,44,"bob",false);
			Assert.IsNotNull(sortable);
			Assert.AreEqual(sortable.PageNumber, 4);
			Assert.AreEqual(sortable.PageSize, 44);
			Assert.AreEqual(sortable.SortColumn, "bob");
		}

		[Test]
		public void Can_create_sortable_from_enumerable_and_grid_params()
		{
			var strings = new List<string>();
			var gridParams = new GridParams();
			gridParams.PageSize = 44;
			gridParams.PageNumber = 4;
			gridParams.SortColumn = "bob";
			ISortable<string> sortable = strings.AsSortable(gridParams);
			Assert.IsNotNull(sortable);
			Assert.AreEqual(sortable.PageNumber, 4);
			Assert.AreEqual(sortable.PageSize, 44);
			Assert.AreEqual(sortable.SortColumn, "bob");
		}
	}
}
