using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CERS
{
	[AttributeUsage(AttributeTargets.Class)]
	public class FacilitySubmittalElementValidatorAttribute : Attribute
	{
		public SubmittalElementType Type { get; set; }

		public FacilitySubmittalElementValidatorAttribute()
		{
			Type = SubmittalElementType.FacilityInformation;
		}

		public FacilitySubmittalElementValidatorAttribute(SubmittalElementType type)
		{
			Type = type;
		}
	}
}