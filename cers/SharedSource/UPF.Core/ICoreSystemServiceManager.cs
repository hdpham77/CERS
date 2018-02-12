using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UPF.Core.SystemServices;

namespace UPF.Core
{
	public interface ICoreSystemServiceManager
	{
		ActivityLogService ActivityLog { get; }

		AuthenticationService Authentication { get; }

		PortalService Portals { get; }

		ICoreRepositoryManager Repository { get; }

		UrlService Urls { get; }

		TService GetService<TService>() where TService : class, ICoreSystemService;
	}
}