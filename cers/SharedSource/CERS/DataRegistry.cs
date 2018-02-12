using CERS.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace CERS
{
	public static class DataRegistry
	{
		#region Fields

		private static List<IDataElementItem> _AllDataElements;
		private static object _AllDataElementsLock = new object();
		private static object _Lock = new object();
		private static List<DataRegistryDataSourceSetting> _Settings;

		#endregion Fields

		#region Constants

		private const string SupplementalAcronym_Default = "CERSSup";
		private const string SystemAcronym_Default = "CERSSys";
		private const string UPDDAcronym_Default = "Title27V310";

		#endregion Constants

		#region Properties

		public static List<DataRegistryDataSourceSetting> Settings
		{
			get
			{
				Initialize();
				return _Settings;
			}
		}

		#endregion Properties

		#region Methods

		public static IDataElementItem GetDataElement( DataRegistryMetadataAttribute attribute )
		{
			if ( attribute == null )
			{
				throw new ArgumentNullException( "attribute" );
			}

			return GetDataElements( attribute.DataSource, attribute.DataRegistryNumberString ).FirstOrDefault();
		}

		public static IDataElementItem GetDataElement( string identifier, DataRegistryDataSourceType type )
		{
			return GetDataElements( type, identifier ).FirstOrDefault();
		}

		public static List<IDataElementItem> GetDataElements( DataRegistryDataSourceType type, string identifier = null )
		{
			List<IDataElementItem> results = new List<IDataElementItem>();
			DataRegistryDataSourceSetting setting = GetSetting( type );
			if ( string.IsNullOrWhiteSpace( identifier ) )
			{
				results = setting.GetDataElements();
			}
			else
			{
				int decimalPos = identifier.IndexOf( "." );
				if ( decimalPos > -1 )
				{
					decimal cersRegistryID;
					if ( decimal.TryParse( identifier, out cersRegistryID ) )
					{
						results.AddRange( setting.GetDataElements().Where( p => p.CERSDataRegistryID == cersRegistryID ) );
					}
				}

				identifier = identifier.Trim().ToLower();

				results.AddRange( setting.GetDataElements().Where( p => p.DataElementIdentifier.Trim().ToLower() == identifier ) );
			}
			return results;
		}

		public static DataRegistryDataSourceSetting GetSetting( DataRegistryDataSourceType type )
		{
			return Settings.SingleOrDefault( p => p.Type == type );
		}

		public static void Initialize()
		{
			if ( _Settings == null )
			{
				//we have no settings, so lets go and get it from config and fill in whats not specifically defined in configuration.
				lock ( _Lock )
				{
					_Settings = new List<DataRegistryDataSourceSetting>();
					LoadConfiguration( _Settings );
					CompileSettings( _Settings );
				}
			}
		}

		private static void CompileSettings( List<DataRegistryDataSourceSetting> settings )
		{
			//the purpose is to make sure that we have a Setting defined for each of the registry data sources CERS needs to operate successfully.
			settings.AddIfNotExists( DataRegistryDataSourceType.UPDD, UPDDAcronym_Default, CacheStrategy.None );
			settings.AddIfNotExists( DataRegistryDataSourceType.System, SystemAcronym_Default, CacheStrategy.None );
			settings.AddIfNotExists( DataRegistryDataSourceType.Supplemental, SupplementalAcronym_Default, CacheStrategy.None );
		}

		private static void LoadConfiguration( List<DataRegistryDataSourceSetting> settings )
		{
			//lets get the config.
			var config = CERSConfigurationSection.Current;
			if ( config != null )
			{
				var cdrConfig = config.CDR;
				if ( cdrConfig != null )
				{
					var dataSourcesConfig = cdrConfig.DataSources;
					if ( dataSourcesConfig != null )
					{
						foreach ( CDRDataSourceConfigurationElement dsConfig in dataSourcesConfig )
						{
							DataRegistryDataSourceSetting setting = DataRegistryDataSourceSetting.FromConfig( dsConfig );

							if ( string.IsNullOrEmpty( setting.Acronym ) )
							{
								throw new ConfigurationErrorsException( "A CDR DataSource is missing an Acroynm value." );
							}

							if ( settings.Count( p => p.Type == setting.Type ) > 0 )
							{
								throw new ConfigurationErrorsException( "A CDR DataSource with Key " + dsConfig.Key.ToString() + " is duplicated in the configuration." );
							}

							settings.Add( setting );

							//check and see if this settings cache strategy is Preload, if so, then right now, go out and build the cache.
							if ( setting.CacheStrategy == CacheStrategy.Preload )
							{
								setting.BuildCache();
							}
						}
					}
				}
			}
		}

		#endregion Methods

		public static List<IDataElementItem> AllDataElementItems
		{
			get
			{
				if ( _AllDataElements == null )
				{
					lock ( _AllDataElementsLock )
					{
						_AllDataElements = new List<IDataElementItem>();
						foreach ( var setting in Settings )
						{
							_AllDataElements.AddRange( setting.GetDataElements() );
						}
					}
				}
				return _AllDataElements;
			}
		}

		public static IDataElementItem GetDataElement( decimal dataRegistryID )
		{
			Initialize();
			return AllDataElementItems.FirstOrDefault( p => p.CERSDataRegistryID == dataRegistryID );
		}

		public static void ReloadCache()
		{
			_Settings = null;
			_AllDataElements = null;
			DataRegistry.Initialize();
		}
	}
}