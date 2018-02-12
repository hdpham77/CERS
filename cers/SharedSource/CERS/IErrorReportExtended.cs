using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using CERS.Security;

namespace CERS
{
	public interface IErrorReportExtended : IErrorReport
	{
		string FacilityName { get; set; }

		string RegulatorName { get; set; }

		string EnvironmentName { get; set; }

		string SystemPortalName { get; set; }

		string AccountFullName { get; set; }

		string AccountEmail { get; set; }

		string AccountUserName { get; set; }

		string OrganizationName { get; set; }

		string OrganizationHeadquarters { get; set; }

		PermissionRoleMatrixCollection Permissions { get; set; }

		NameValueCollection FormVariables { get; set; }

		NameValueCollection QueryStringVariables { get; set; }

		NameValueCollection ServerVariables { get; set; }
	}
}