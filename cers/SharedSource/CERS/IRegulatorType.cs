using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace CERS
{
	public interface IRegulatorType : ISystemLookupEntity
	{
		bool DataFilteringEnabled { get; set; }

		[Display( Name = "Regulator Type" )]
		string Display { get; }

		[Display( Name = "Regulator Type" )]
		string DisplayInverted { get; }

		[Display( Name = "Regulator Type" )]
		RegulatorType? Type { get; set; }

		[Display( Name = "Regulator Type" )]
		string TypeCode { get; set; }
	}
}