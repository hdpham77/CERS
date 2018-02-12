using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CERS
{
	public interface IAddress
	{
		string Street { get; set; }

		string City { get; set; }

		string State { get; set; }

		string ZipCode { get; set; }
	}
}