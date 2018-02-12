using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace UPF
{
	public class EntityPropertyDifferenceCollection<T> : Collection<EntityPropertyDifference<T>>
	{
		public EntityPropertyDifference<T> this[string propertyName]
		{
			get
			{
				return this.SingleOrDefault( p => p.PropertyName == propertyName );
			}
		}

		public void Add( T first, T second, PropertyInfo property, object firstValue, object secondValue )
		{
			EntityPropertyDifference<T> item = new EntityPropertyDifference<T>();
			item.First = first;
			item.Second = second;
			item.Property = property;
			item.FirstValue = firstValue;
			item.SecondValue = secondValue;
			this.Add( item );
		}

		public List<Triple<string, object, object>> GetSummary()
		{
			List<Triple<string, object, object>> results = new List<Triple<string, object, object>>();
			foreach ( var item in this )
			{
				results.Add( new Triple<string, object, object>( item.PropertyName, item.FirstValue, item.SecondValue ) );
			}
			return results;
		}
	}
}