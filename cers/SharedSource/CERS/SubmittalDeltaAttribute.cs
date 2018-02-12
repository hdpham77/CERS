using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CERS
{
	[AttributeUsage( AttributeTargets.Field |
		AttributeTargets.Property )]
	internal class SubmittalDeltaAttribute : System.Attribute
	{
		private bool compare;

		public SubmittalDeltaAttribute( bool compare )
		{
			this.compare = compare;
		}

		public bool Compare
		{
			get
			{
				return compare;
			}
		}
	}
}