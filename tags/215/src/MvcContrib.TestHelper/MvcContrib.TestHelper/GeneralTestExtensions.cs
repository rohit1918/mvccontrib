using System;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace MvcContrib.TestHelper
{
    /// <summary>
    /// Contains basic extension methods to simplify testing.
    /// </summary>
    public static class GeneralTestExtensions
    {
        /// <summary>
        /// Asserts that the object is of type T.  Also returns T to enable method chaining.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="actual"></param>
        /// <returns></returns>
        public static T ShouldBe<T>(this object actual)
        {
            Assert.That(actual, Is.InstanceOfType(typeof(T)));
            return (T)actual;
        }

        /// <summary>
        /// Asserts that the object is the expected value.
        /// </summary>
        /// <param name="actual"></param>
        /// <param name="expected"></param>
        public static void ShouldBe(this object actual, object expected)
        {
            Assert.That(actual, Is.EqualTo(expected));
        }

        /// <summary>
        /// Compares the two strings (case-insensitive).
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        public static void AssertSameStringAs(this string left, string right)
        {
            Assert.That(string.Equals(left, right, StringComparison.InvariantCultureIgnoreCase),
                string.Format("Expected {0} but was {1}", right, left));
        }
    }
}
