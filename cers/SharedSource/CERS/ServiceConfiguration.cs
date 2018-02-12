using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CERS.Configuration;

namespace CERS
{
	public class ServiceConfiguration
	{
		public ServiceType Type { get; set; }

		public string BaseUri { get; set; }

		public static T GetConfiguration<T>(ServiceType type) where T : ServiceConfiguration, new()
		{
			var current = CERSConfigurationSection.Current;
			var serviceConfigElement = current.ServiceExtensions.Services[type];

			T config = new T();
			config.Type = serviceConfigElement.Key;
			config.BaseUri = serviceConfigElement.BaseUri;
			return config;
		}
	}
}