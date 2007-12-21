﻿using System;
using MvcContrib.Attributes;
using NUnit.Framework;

namespace MVCContrib.UnitTests.MetaData
{
	[TestFixture]
	public class RescueAttributeTester
	{
		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void ViewIsRequired()
		{
			RescueAttribute rescue = new RescueAttribute(string.Empty);
		}
	}
}
