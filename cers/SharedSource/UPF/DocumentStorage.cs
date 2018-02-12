using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using UPF.Configuration;
using UPF.Providers;

namespace UPF
{
	public static class DocumentStorage
	{
		#region Fields

		private static bool _Initialized;
		private static bool _InitializedDefaultProvider;
		private static Exception _InitializeException;
		private static object _Lock;
		private static DocumentStorageProvider _Provider;
		private static DocumentStorageProviderCollection _Providers;

		#endregion Fields

		#region Properties

		public static DocumentStorageProvider Provider
		{
			get
			{
				Initialize();
				if ( _Provider == null )
				{
					throw new InvalidOperationException( "Default Storage Provider not found or configured." );
				}
				return _Provider;
			}
		}

		public static DocumentStorageProviderCollection Providers
		{
			get
			{
				Initialize();
				return _Providers;
			}
		}

		#endregion Properties

		#region Static Constructor

		static DocumentStorage()
		{
			_Lock = new object();
			_Initialized = false;
			_InitializedDefaultProvider = false;
		}

		#endregion Static Constructor

		#region Initialize Method

		private static void Initialize()
		{
			if ( !_Initialized || !_InitializedDefaultProvider )
			{
				if ( _InitializeException != null )
				{
					throw _InitializeException;
				}

				lock ( _Lock )
				{
					if ( !_Initialized || !_InitializedDefaultProvider )
					{
						if ( _InitializeException != null )
						{
							throw _InitializeException;
						}

						bool initializeGeneralSettings = !_Initialized;
						bool initializeDefaultProvider = !_InitializedDefaultProvider;

						if ( initializeDefaultProvider || initializeGeneralSettings )
						{
							bool generalSettingsInitialized;
							bool defaultProviderInitialized = false;

							try
							{
								//initialize provider.
								UPFConfigurationSection cersConfig = UPFConfigurationSection.Current;
								DocumentStorageConfigurationElement docStorage = cersConfig.DocumentStorage;
								generalSettingsInitialized = InitializeSettings( initializeGeneralSettings, docStorage );
								defaultProviderInitialized = InitializeDefaultProvider( initializeDefaultProvider, docStorage );
							}
							catch ( Exception ex )
							{
								_InitializeException = ex;
								throw;
							}

							_Initialized = generalSettingsInitialized;
							_InitializedDefaultProvider = defaultProviderInitialized;
						}
					}
				}
			}
		}

		#endregion Initialize Method

		#region InitializeSettings Method

		private static bool InitializeSettings( bool initializeGeneralSettings, DocumentStorageConfigurationElement settings )
		{
			bool result = false;
			if ( initializeGeneralSettings )
			{
				_Providers = new DocumentStorageProviderCollection();
				foreach ( DocumentStorageProviderConfigurationElement providerConfiguration in settings.Providers )
				{
					Type providerType = Type.GetType( providerConfiguration.Type, true, true );
					if ( !typeof( DocumentStorageProvider ).IsAssignableFrom( providerType ) )
					{
						throw new ArgumentException( "Provider must implement type " + typeof( DocumentStorageProvider ).Name );
					}

					DocumentStorageProvider provider = (DocumentStorageProvider) Activator.CreateInstance( providerType );
					NameValueCollection parameters = providerConfiguration.Parameters;
					provider.Initialize( providerConfiguration.Name, parameters );
					_Providers.Add( provider );
				}
			}
			return result;
		}

		#endregion InitializeSettings Method

		#region InitializeDefaultProvider Method

		private static bool InitializeDefaultProvider( bool initializeDefaultProvider, DocumentStorageConfigurationElement settings )
		{
			bool result = false;
			if ( initializeDefaultProvider )
			{
				if ( string.IsNullOrWhiteSpace( settings.DefaultProvider ) || _Providers.Count < 1 )
				{
					throw new Exception( "Default DocumentStorageProvider not specified." );
				}

				_Provider = _Providers[settings.DefaultProvider];
				if ( _Provider == null )
				{
					throw new ConfigurationErrorsException( "Default DocumentStorageProvider not found", settings.ElementInformation.Properties["defaultProvider"].Source, settings.ElementInformation.Properties["defaultProvider"].LineNumber );
				}
				result = true;
			}
			return result;
		}

		#endregion InitializeDefaultProvider Method

		#region Exists Method

		/// <summary>
		/// Gets a value indicating whether a file at the specified <paramref name="location"/> exists.
		/// </summary>
		/// <param name="location">The <see cref="String"/> containing the path of the file to check existance for.</param>
		/// <returns></returns>
		public static bool Exists( string location )
		{
			return Provider.Exists( location );
		}

		#endregion Exists Method

		#region GetBytes Method

		/// <summary>
		/// Gets a <see cref="byte"/> array containing the data for the specified file at <paramref name="location"/>.
		/// </summary>
		/// <param name="location">The <see cref="String"/> containing the path to the file to get.</param>
		/// <returns>A <see cref="byte"/> array.</returns>
		public static byte[] GetBytes( string location )
		{
			return Provider.GetBytes( location );
		}

		#endregion GetBytes Method

		#region GetStream Method

		/// <summary>
		/// Gets a <see cref="Stream"/> of a particular file for the specified <paramref name="location"/>.
		/// </summary>
		/// <param name="location">The <see cref="String"/> containing the path to the file to get.</param>
		/// <returns>A <see cref="Stream"/>.</returns>
		public static Stream GetStream( string location )
		{
			return Provider.GetStream( location );
		}

		#endregion GetStream Method

		#region Put Methods

		/// <summary>
		/// Writes the <paramref name="document"/> to the file system.
		/// </summary>
		/// <param name="document">The <see cref="byte"/> array containing the data to save to the file.</param>
		/// <param name="fileName">The <see cref="String"/> containing the name of the file that should be generated.</param>
		/// <param name="qualifier">The <see cref="String"/> containing a qualifier to be used in the storage path.</param>
		/// <param name="subQualifier">The <see cref="String"/> containing a sub qualifier to be used in the storage path.</param>
		/// <returns>A <see cref="DocumentStoragePutResult"/> object with path and status information.</returns>
		public static DocumentStoragePutResult Put( Stream document, string fileName, string qualifier, string subQualifier )
		{
			return Provider.Put( document, fileName, qualifier, subQualifier );
		}

		/// <summary>
		/// Writes the <paramref name="document"/> to the file system.
		/// </summary>
		/// <param name="document">The <see cref="byte"/> array containing the data to save to the file.</param>
		/// <param name="fileName">The <see cref="String"/> containing the name of the file that should be generated.</param>
		/// <param name="qualifier">The <see cref="String"/> containing a qualifier to be used in the storage path.</param>
		/// <param name="subQualifier">The <see cref="String"/> containing a sub qualifier to be used in the storage path.</param>
		/// <returns>A <see cref="DocumentStoragePutResult"/> object with path and status information.</returns>
		public static DocumentStoragePutResult Put( byte[] document, string fileName, string qualifier, string subQualifier )
		{
			return Provider.Put( document, fileName, qualifier, subQualifier );
		}

		/// <summary>
		/// Writes the <paramref name="document"/> to a file at the specified <paramref name="virtualLocation"/>. This overload is commonly used
		/// to overwrite an existing file with new content.
		/// </summary>
		/// <param name="document">The <see cref="Stream"/> containing the data for the document to store.</param>
		/// <param name="virtualLocation">The <see cref="String"/> containing the virtual or physical path to write the file to. Must include the file name.</param>
		public static void Put( Stream document, string virtualLocation )
		{
			Provider.Put( document, virtualLocation );
		}

		/// <summary>
		/// Writes the <paramref name="document"/> to a file at the specified <paramref name="virtualLocation"/>. This overload is commonly used
		/// to overwrite an existing file with new content.
		/// </summary>
		/// <param name="document">The <see cref="byte"/> array containing the data for the document to store.</param>
		/// <param name="virtualLocation">The <see cref="String"/> containing the virtual or physical path to write the file to. Must include the file name.</param>
		public static void Put( byte[] document, string virtualLocation )
		{
			Provider.Put( document, virtualLocation );
		}

		#endregion Put Methods

		#region GetProvider  Method

		/// <summary>
		/// Gets a <see cref="DocumentStorageProvider"/> by the specified <paramref name="providerID"/>.
		/// </summary>
		/// <param name="providerID">The <see cref="Int32"/> representing the numeric identifier of the provider.</param>
		/// <returns></returns>
		public static DocumentStorageProvider GetProvider( int providerID )
		{
			return Providers.SingleOrDefault( p => p.ProviderID == providerID );
		}

		#endregion GetProvider  Method

		#region Delete Method

		/// <summary>
		/// Deletes a specified file at the <paramref name="location"/>.
		/// </summary>
		/// <param name="location">The <see cref="String"/> containing the virtual or physical path to the file to delete.</param>
		public static void Delete( string location )
		{
			Provider.Delete( location );
		}

		#endregion Delete Method

        public static void RemoveEmptyDirectory( string location )
        {
            Provider.RemoveEmptyDirectory( location );
        }

		public static long? GetSize( string location )
		{
			return Provider.GetSize( location );
		}

		public static long? GetSize( string location, int providerID )
		{
			var provider = Providers.SingleOrDefault( p => p.ProviderID == providerID );
			return provider.GetSize( location );
		}
	}
}