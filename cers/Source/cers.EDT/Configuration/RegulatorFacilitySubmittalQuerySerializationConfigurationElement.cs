using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace CERS.EDT.Configuration
{
	public class RegulatorFacilitySubmittalQuerySerializationConfigurationElement : ConfigurationElement
	{
		[ConfigurationProperty("useXmlCache", DefaultValue = false)]
		public bool UseXmlCache
		{
			get { return (bool)this["useXmlCache"]; }
			set { this["useXmlCache"] = value; }
		}
	}
}