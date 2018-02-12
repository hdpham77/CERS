using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace CERS.EDT.Configuration
{
	public class SerializationConfigurationElement : ConfigurationElement
	{
		[ConfigurationProperty("regulatorFacilitySubmittalQuery")]
		public RegulatorFacilitySubmittalQuerySerializationConfigurationElement RegulatorFacilitySubmittalQuery
		{
			get
			{
				var result = (RegulatorFacilitySubmittalQuerySerializationConfigurationElement)this["regulatorFacilitySubmittalQuery"];
				if (result == null)
				{
					result = new RegulatorFacilitySubmittalQuerySerializationConfigurationElement();
				}
				return result;
			}
			set { this["regulatorFacilitySubmittalQuery"] = value; }
		}
	}
}