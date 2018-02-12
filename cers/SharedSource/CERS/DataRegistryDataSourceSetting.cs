using CERS.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CERS
{
	public class DataRegistryDataSourceSetting
	{
		#region Fields

		private static Dictionary<DataRegistryDataSourceType, List<IDataElementItem>> _Cache;
		private static object _Lock = new object();

		#endregion Fields

		#region Properties

		public string Acronym { get; set; }

		public CacheStrategy CacheStrategy { get; set; }

		public DataRegistryDataSourceType Type { get; set; }

		#endregion Properties

		#region Constructors

		public DataRegistryDataSourceSetting(DataRegistryDataSourceType type, string acronym, CacheStrategy cacheStrategy = CERS.CacheStrategy.None)
		{
			Type = type;
			Acronym = acronym;
			CacheStrategy = cacheStrategy;
		}

		#endregion Constructors

		#region GetDataElements Method

		public List<IDataElementItem> GetDataElements()
		{
			if ( string.IsNullOrWhiteSpace( Acronym ) )
			{
				throw new Exception( "The Acronym property is null which is required to get items for a given Data Source." );
			}

			List<IDataElementItem> results = null;
			if ( CacheStrategy == CERS.CacheStrategy.None )
			{
				results = new List<IDataElementItem>( FetchElements() );
			}
			else
			{
				InitCache();
				BuildCache();
				results = _Cache[ Type ];
			}
			return results;
		}

		private void InitCache()
		{
			//if the cache store is null, lets create it.
			if ( _Cache == null )
			{
				lock ( _Lock )
				{
					_Cache = new Dictionary<DataRegistryDataSourceType, List<IDataElementItem>>();
				}
			}
		}

		#endregion GetDataElements Method

		#region ClearCache Method

		public void ClearCache()
		{
			if ( _Cache != null )
			{
				lock ( _Lock )
				{
					if ( _Cache.ContainsKey( Type ) )
					{
						_Cache.Remove( Type );
					}
				}
			}
		}

		#endregion ClearCache Method

		#region BuildCache Method

		public void BuildCache()
		{
			if ( string.IsNullOrWhiteSpace( Acronym ) )
			{
				throw new Exception( "The Acronym property is null which is required to get items for a given Data Source." );
			}

			if ( CacheStrategy == CERS.CacheStrategy.None )
			{
				throw new Exception( "BuildCache cannot be invoked when the configured CacheStrategy is set to None. CDR Data Source: " + Acronym );
			}

			//make sure the Cache stored is initialized...
			InitCache();

			//make sure we don't already have this DataElementDataSourceType already in the cache.
			if ( !_Cache.ContainsKey( Type ) )
			{
				lock ( _Lock )
				{
					//add it.
					_Cache.Add( Type, new List<IDataElementItem>( FetchElements() ) );
				}
			}
		}

		#endregion BuildCache Method

		#region FromConfig Method

		public static DataRegistryDataSourceSetting FromConfig(CDRDataSourceConfigurationElement element)
		{
			return new DataRegistryDataSourceSetting( element.Key, element.Acroynm, element.CacheStrategy );
		}

		#endregion FromConfig Method

		public List<IDataElementItem> FetchElements()
		{
			if ( string.IsNullOrWhiteSpace( Acronym ) )
			{
				throw new Exception( "The Acronym property is null which is required to get items for a given Data Source." );
			}

			List<IDataElementItem> results = null;
			using ( ICERSRepositoryManager repo = ServiceLocator.GetRepositoryManager() )
			{
				results = repo.DataRegistry.DataElements.ItemSearch( Acronym ).ToList();
			}
			return results;
		}
	}
}