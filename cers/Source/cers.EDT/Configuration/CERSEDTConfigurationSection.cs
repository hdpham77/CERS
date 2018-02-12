using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace CERS.EDT.Configuration
{
	public class CERSEDTConfigurationSection : ConfigurationSection
	{
		private static ConfigurationPropertyCollection _Properties;

		public CERSEDTConfigurationSection()
		{
			_Properties = new ConfigurationPropertyCollection();
			_Properties.Add(_Serialization);
		}

		private ConfigurationProperty _Serialization = new ConfigurationProperty("serialization", typeof(SerializationConfigurationElement), new SerializationConfigurationElement(), ConfigurationPropertyOptions.None);

		public SerializationConfigurationElement Serialization
		{
			get { return this["serialization"] as SerializationConfigurationElement; }
			set { this["serialization"] = value; }
		}

		/// <summary>
		/// Overrides <see cref="ConfigurationElement.Properties"/>.
		/// </summary>
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return _Properties;
			}
		}

		/// <summary>
		/// Gets an instance of the entire configuration section and all sub element instances.
		/// </summary>
		/// <returns>A CoreConfiguration XML configuration object.</returns>
		public static CERSEDTConfigurationSection Current
		{
			get
			{
				return ConfigurationManager.GetSection("cers.edt") as CERSEDTConfigurationSection;
			}
		}
	}
}