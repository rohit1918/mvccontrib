using System;
using System.Linq;
using MvcContrib.FluentHtml;
using MvcContrib.FluentHtml.Behaviors;
using MvcContrib.UnitTests.FluentHtml.Helpers;
using NUnit.Framework;

namespace MvcContrib.UnitTests.FluentHtml
{
	public class ConventionViewModelContainerTestBase<T, TModel> where T : IViewModelContainer<TModel> where TModel : class
	{
		protected T target;

		[SetUp]
		public void SetUp()
		{
			target = (T)Activator.CreateInstance(typeof(T));
		}

		[Test]
		public virtual void includes_default_required_behavior()
		{
			target.MemberBehaviors.Where(x => x is DefaultRequiredMemberBehavior).ShouldCount(1);
		}

		[Test]
		public virtual void includes_default_maxlength_behavior()
		{
			target.MemberBehaviors.Where(x => x is DefaultMaxLengthMemberBehavior).ShouldCount(1);
		}
	}
}