using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UPF
{
	public class CachedTypeInstanceContainer
	{
		private CachedTypeInstanceCollection _CachedObjects;

		protected CachedTypeInstanceCollection CachedObjects
		{
			get
			{
				if ( _CachedObjects == null )
				{
					_CachedObjects = new CachedTypeInstanceCollection();
				}
				return _CachedObjects;
			}
		}

		protected virtual TObject GetObject<TObject>()
		{
			TObject obj = default( TObject );
			Type type = typeof( TObject );
			if ( CachedObjects.ContainsKey( type ) )
			{
				obj = (TObject) CachedObjects[type];// as TObject;
			}
			else
			{
				obj = (TObject) Activator.CreateInstance( type );// as TObject;
				CachedObjects.Add( type, obj );
			}

			return obj;
		}

		protected virtual TObject GetObject<TObject>( params object[] arguments )
		{
			TObject obj = default( TObject );
			Type type = typeof( TObject );
			if ( CachedObjects.ContainsKey( type ) )
			{
				obj = (TObject) CachedObjects[type];// as TObject;
			}
			else
			{
				obj = (TObject) Activator.CreateInstance( type, arguments );// as TObject;
				CachedObjects.Add( type, obj );
			}

			return obj;
		}
	}
}