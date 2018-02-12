using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CERS
{
	public interface IDataElementCode
	{
		string Code { get; set; }

		string Description { get; set; }

		string DescriptionDisplay { get; }

		bool Obsolete { get; set; }

		int? SortOrder { get; set; }
	}
}