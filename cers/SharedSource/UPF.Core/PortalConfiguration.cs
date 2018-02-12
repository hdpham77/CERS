using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UPF.Core
{
	[Serializable]
	public class PortalConfiguration
	{
		private PortalUrl _CurrentUrl;

		public bool AllowAuthentication { get; set; }

		public PortalUrl CurrentUrl
		{
			get
			{
				if ( _CurrentUrl == null )
				{
					_CurrentUrl = Urls.Current;
				}
				return _CurrentUrl;
			}
		}

		public string Description { get; set; }

		public RuntimeEnvironment Environment { get; set; }

		public string Identifier { get; set; }

		public bool IsCurrent { get; set; }

		public string Name { get; set; }

		public string SystemMaintenanceCustomMessage { get; set; }

		public DateTime? SystemMaintenanceEndsOn { get; set; }

		public int? SystemMaintenancePriorNotificationBeforeMaintenanceStartsInHours { get; set; }

		public bool SystemMaintenanceScheduled { get; set; }

		public DateTime? SystemMaintenanceStartsOn { get; set; }

		public int SystemPortalID { get; set; }

		public SystemPortalType Type { get; set; }

		public string Url
		{
			get
			{
				return CurrentUrl.Url;
			}
		}

		public PortalUrlCollection Urls { get; set; }

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public override string ToString()
		{
			return Identifier + "/" + Environment.ToString() + "/IsCurrent=" + IsCurrent;
		}
	}
}