using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UPF.Core.Model;
using UPF.Core.Repository;

namespace UPF.Core
{
	public interface ICoreRepositoryManager : IDisposable
	{
		AccountAuthenticationAttemptRepository AccountAuthenticationAttempts { get; }

		AccountAuthenticationStatisticRepository AccountAuthenticationStatistics { get; }

		AccountRepository Accounts { get; }

		AccountSecurityQuestionAnswerRepository AccountSecurityQuestions { get; }

		ActivityLogRepository ActivityLog { get; }

		IAccount ContextAccount { get; }

		int ContextAccountID { get; }

		CoreEntities DataModel { get; }

		LookupTableRepository LookupTables { get; }

		SecurityQuestionRepository SecurityQuestions { get; }

		SettingRepository Settings { get; }

		SystemPortalRepository SystemPortals { get; }

		UrlLinkRepository UrlLinks { get; }
	}
}