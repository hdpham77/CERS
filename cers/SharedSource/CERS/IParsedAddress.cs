using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CERS
{
	public interface IParsedAddress
	{
		string City { get; set; }

		string State { get; set; }

		string Zip5 { get; set; }

		string ZipPlus4 { get; set; }

		string Range { get; set; }

		string PreDirection { get; set; }

		string StreetName { get; set; }

		string Suffix { get; set; }

		string PostDirection { get; set; }

		string SuiteName { get; set; }

		string SuiteNumber { get; set; }

		string PrivateMailboxName { get; set; }

		string PrivateMailboxNumber { get; set; }

		string Garbage { get; set; }

		string ErrorCodes { get; set; }
	}
}