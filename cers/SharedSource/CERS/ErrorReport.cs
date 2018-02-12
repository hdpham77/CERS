using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CERS
{
	public class ErrorReport : IErrorReport
	{
		public int? AccountID { get; set; }

		public int? EnvironmentID { get; set; }

		public int? SystemPortalID { get; set; }

		public string Url { get; set; }

		public string ReferrerUrl { get; set; }

		public string UserAgent { get; set; }

		public string UserHostAddress { get; set; }

		public int? CERSID { get; set; }

		public int? RegulatorID { get; set; }

		public int? OrganizationID { get; set; }

		public DateTime OccurredOn { get; set; }

		public string TARTicketCode { get; set; }

		#region Exception Properties

		public string ExceptionMessage { get; set; }

		public string ExceptionDetail { get; set; }

		#endregion Exception Properties
	}
}