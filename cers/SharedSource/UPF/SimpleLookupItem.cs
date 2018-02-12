using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UPF
{
	public class SimpleLookupItem<TKey, TValue, TTag> : SimpleLookupItem<TKey, TValue>
	{
		public SimpleLookupItem()
		{
		}

		public SimpleLookupItem(TKey key, TValue value)
			: base(key, value)
		{
		}

		public SimpleLookupItem(TKey key, TValue value, TTag tag)
			: base(key, value)
		{
			Tag = tag;
		}

		/// <summary>
		/// Gets or sets the Tag property value.
		/// </summary>
		public TTag Tag { get; set; }
	}

	/// <summary>
	/// Provides a simple Key/Value generic class. Helpful for use in custom UI binding scenarios.
	/// </summary>
	/// <typeparam name="TKey">The type of the Key.</typeparam>
	/// <typeparam name="TValue">The type of the Value.</typeparam>
	public class SimpleLookupItem<TKey, TValue>
	{
		public SimpleLookupItem()
		{
		}

		public SimpleLookupItem(TKey key, TValue value)
		{
			Key = key;
			Value = value;
		}

		/// <summary>
		/// Gets or sets the Key property value.
		/// </summary>
		public TKey Key { get; set; }

		/// <summary>
		/// Gets or sets the Value property value.
		/// </summary>
		public TValue Value { get; set; }
	}
}