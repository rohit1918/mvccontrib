using System.Collections;
using System.Collections.Specialized;

namespace MvcContrib.Rest.Routing.Ext
{
	public static class NameValueCollectionExtensions
	{
		public static NameValueCollection Merge(this NameValueCollection lhs, NameValueCollection rhs)
		{
			return lhs == null ? OverwritePairs(new NameValueCollection(), rhs) : OverwritePairs(lhs, rhs);
		}

		public static NameValueCollection MergePreserve(this NameValueCollection lhs, NameValueCollection rhs)
		{
			return lhs == null ? OverwritePairs(new NameValueCollection(), rhs) : PreserveExistingPairs(lhs, rhs);
		}

		public static NameValueCollection Merge(this NameValueCollection lhs, IDictionary rhs)
		{
			return lhs == null ? OverwritePairs(new NameValueCollection(), rhs) : OverwritePairs(lhs, rhs);
		}

		public static NameValueCollection MergePreserve(this NameValueCollection lhs, IDictionary rhs)
		{
			return lhs == null ? OverwritePairs(new NameValueCollection(), rhs) : PreserveExistingPairs(lhs, rhs);
		}

		private static NameValueCollection OverwritePairs(this NameValueCollection existing, NameValueCollection newValues)
		{
			foreach (var key in newValues.AllKeys)
			{
				existing[key] = newValues[key];
			}
			return existing;
		}

		private static NameValueCollection PreserveExistingPairs(this NameValueCollection existing, NameValueCollection newValues)
		{
			foreach (var key in newValues.AllKeys)
			{
				existing[key] = existing[key] ?? newValues[key];
			}
			return existing;
		}

		private static NameValueCollection OverwritePairs(NameValueCollection existing, IDictionary newValues)
		{
			foreach (DictionaryEntry pair in newValues)
			{
				existing[pair.Key.ToString()] = pair.Value.ToString();
			}
			return existing;
		}

		private static NameValueCollection PreserveExistingPairs(NameValueCollection existing, IDictionary newValues)
		{
			foreach (DictionaryEntry pair in newValues)
			{
				existing[pair.Key.ToString()] = existing[pair.Key.ToString()] ?? pair.Value.ToString();
			}
			return existing;
		}
	}
}