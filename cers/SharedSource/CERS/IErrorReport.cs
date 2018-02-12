using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CERS
{
	public interface IErrorReport
	{
		int? SystemPortalID { get; set; }

		int? EnvironmentID { get; set; }

		int? AccountID { get; set; }

		DateTime OccurredOn { get; set; }

		string Url { get; set; }

		string ReferrerUrl { get; set; }

		int? CERSID { get; set; }

		int? OrganizationID { get; set; }

		int? RegulatorID { get; set; }

		string TARTicketCode { get; set; }

		string UserAgent { get; set; }

		string UserHostAddress { get; set; }

		string ExceptionMessage { get; set; }

		string ExceptionDetail { get; set; }
	}
}