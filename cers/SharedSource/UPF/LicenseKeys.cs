using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UPF.Configuration;

namespace UPF
{
	public class LicenseKeys
	{
		private const string DefaultMelissaDataLicenseKey = "INDQ35SP7XSddnhX7e7+dO==hlIsOfgqTzdIG1Ir4Us73E==";
		private const string DefaultWinnovativeSuiteLicenseKey = "khwPHQ8OHQ0dBBMNHQ4MEwwPEwQEBAQ=";

		public static string GetLicenseKey( BuiltInLicenseKey name )
		{
			return GetLicenseKey( name.ToString() );
		}

		public static string GetLicenseKey( string name )
		{
			if ( string.IsNullOrWhiteSpace( name ) )
			{
				throw new ArgumentNullException( "name" );
			}

			//define result to be returned.
			string result = string.Empty;

			//if the name passed in matches the built in licensekey WinnovativeExcel, then default
			//the licensekey to the default constant in case it isn't present in the configuration.
			//if a key is provided in configuration, it will override this one.
			if ( name == BuiltInLicenseKey.WinnovativeSuite.ToString() )
			{
				result = DefaultWinnovativeSuiteLicenseKey;
			}
			else if ( name == BuiltInLicenseKey.MelissaData.ToString() )
			{
				result = DefaultMelissaDataLicenseKey;
			}

			var config = UPFConfigurationSection.Current;
			var keyConfig = config.LicenseKeys.GetByName( name );

			if ( keyConfig != null )
			{
				result = keyConfig.Key;
			}
			return result;
		}
	}
}