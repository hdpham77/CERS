using System;
using System.Collections.Generic;

using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace UPF
{
	public class SimpleLookupItemCollection<TKey, TValue> : Collection<SimpleLookupItem<TKey, TValue>>
	{
		public SimpleLookupItemCollection()
		{
		}

		public SimpleLookupItemCollection(IEnumerable<SimpleLookupItem<TKey, TValue>> items)
		{
			AddRange(items);
		}

		public SimpleLookupItem<TKey, TValue> Add(TKey key, TValue value)
		{
			SimpleLookupItem<TKey, TValue> item = new SimpleLookupItem<TKey, TValue>(key, value);
			this.Add(item);
			return item;
		}

		public bool ContainsKey(TKey key)
		{
			return (this.SingleOrDefault(p => p.Key.Equals(key)) != null);
		}

		public void AddRange(IEnumerable<SimpleLookupItem<TKey, TValue>> items)
		{
			if (items != null)
			{
				foreach (var item in items)
				{
					this.Add(item);
				}
			}
		}
	}
}