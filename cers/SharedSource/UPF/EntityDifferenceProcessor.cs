using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace UPF
{
	public class EntityDifferenceProcessor<T>
	{
		public EntityDifferenceProcessor()
		{
		}

		public EntityPropertyDifferenceCollection<T> Process( T first, T second )
		{
			EntityPropertyDifferenceCollection<T> results = new EntityPropertyDifferenceCollection<T>();
			PropertyInfo[] properties = typeof( T ).GetProperties( BindingFlags.Instance | BindingFlags.Public );
			foreach ( var property in properties )
			{
				Compare( results, first, second, property );
			}

			return results;
		}

		protected virtual void Compare( EntityPropertyDifferenceCollection<T> results, T first, T second, PropertyInfo property )
		{
			object firstValue = property.GetValue( first, null );
			object secondValue = property.GetValue( second, null );
			if ( firstValue != secondValue )
			{
				results.Add( first, second, property, firstValue, secondValue );
			}
		}
	}
}