using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CERS
{
	public class DataElementItemCode : IDataElementCode
	{
		public string Code { get; set; }

		public string Description { get; set; }

		[Display( Name = "Description" )]
		public string DescriptionDisplay
		{
			get
			{
				string result = Description;
				if ( Obsolete )
				{
					result += " (Obsolete)";
				}
				return result;
			}
		}

		public bool Obsolete { get; set; }

		public int? SortOrder { get; set; }

		public override string ToString()
		{
			return Code + " = " + DescriptionDisplay;
		}
	}
}