using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UPF.Core
{
	public enum AuthenticationStatus : int
	{
		Success = 1,
		Failure_AccountNotApproved = 2,
		Failure_AccountDisabled = 4,
		Failure_AccountNotExist = 8,
		Failure_IncorrectPassword = 16,
		Failure_NoPassword = 32,
		Failure_Unknown = 512,
		Missing_AuthorizationHeader = 1024,
		Failure_RegulatorNotAuthorizedForEDT = 1536
	}

	public enum CoreLookupTable
	{
		ActivityLogEventLevel,
		SystemPortalEnvironment,
		SystemPortalType
	}

	public enum PasswordValidationResult
	{
		NotProvided,
		ConfirmNotProvided,
		NoMatch,
		Match
	}

	public enum SystemPortalType
	{
		CERSAdminUI = 1,
		CERSRegulatorUI = 2,
		CERSBusinessUI = 3,
		CERSReportUI = 4,
		EDTServices = 5,
		CERSPublicUI = 6,
		CERSServices = 7,
		CERSPluginManager = 8,
		CERSSwitchboard = 9
	}
}