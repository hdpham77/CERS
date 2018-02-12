using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CERS
{
	[AttributeUsage(AttributeTargets.Class)]
	public class DDModelValidatorAttribute : Attribute
	{
		public Type Type { get; set; }

		public DDModelValidatorAttribute()
		{
		}

		public DDModelValidatorAttribute(Type type)
		{
			Type = type;
		}
	}
}