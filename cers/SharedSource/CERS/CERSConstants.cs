using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CERS
{
	public class CERSConstants
	{
		public const int LeadRegulatorPercentageThreshold = 90;
		public const int UnifiedProgramRegulatorEDTIdentityKey = 9900;

		public static DateTime CERS2LaunchDate
		{
			get
			{
				return DateTime.Parse( "1/6/2012" );
			}
		}
	}
}