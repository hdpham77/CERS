using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace UPF
{
	public class ObjectPropertyCache<T>
	{
		private List<PropertyInfo> _PropertyCache;

		public ObjectPropertyCache()
		{
			Entity = default( T );
		}

		public ObjectPropertyCache( T entity )
		{
			Entity = entity;
		}

		public T Entity { get; set; }

		public List<PropertyInfo> PropertyCache
		{
			get
			{
				if ( _PropertyCache == null )
				{
					BuildPropertyCache();
				}
				return _PropertyCache;
			}
		}

		protected void BuildPropertyCache()
		{
			PropertyInfo[] properties = typeof( T ).GetProperties( BindingFlags.Instance | BindingFlags.Public );
			_PropertyCache = new List<PropertyInfo>();
			foreach ( PropertyInfo property in properties )
			{
				if ( !ShouldIgnoreProperty( property ) )
				{
					_PropertyCache.Add( property );
				}
			}
		}

		protected virtual bool ShouldIgnoreProperty( PropertyInfo info )
		{
			bool result = false;
			return result;
		}
	}
}