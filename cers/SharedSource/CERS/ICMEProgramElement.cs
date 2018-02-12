using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CERS
{
	public interface ICMEProgramElement : ISystemLookupEntity
	{
		string Acronym { get; set; }

		string Value { get; set; }
	}
}