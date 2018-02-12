using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CERS
{
	[AttributeUsage(AttributeTargets.Class)]
	public class FacilitySubmittalElementResourceValidatorAttribute : Attribute
	{
		public ResourceType Type { get; set; }

		public FacilitySubmittalElementResourceValidatorAttribute(ResourceType type)
		{
			Type = type;
		}
	}
}