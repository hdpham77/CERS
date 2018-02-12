using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using UPF;
using UPF.Core.Cryptography;
using UPF.Core.Model;

namespace UPF.Core
{
	public static class AuthenticationManager
	{
		#region Authenticate Method

		public static AuthenticationResult Authenticate( string userName, string password, string hostID, string userAgent, out Account account, bool log = true, bool passwordIsHashed = false, int? systemPortalEnvironmentSettingUrlID = null )
		{
			AuthenticationResult result = null;

			using ( ICoreRepositoryManager repo = CoreServiceLocator.GetRepositoryManager() )
			{
				ICoreSystemServiceManager services = CoreServiceLocator.GetServiceManager( repo );
				result = services.Authentication.Authenticate( userName, password, hostID, userAgent, out account, log, passwordIsHashed, systemPortalEnvironmentSettingUrlID );
			}

			return result;
		}

		#endregion Authenticate Method
	}
}