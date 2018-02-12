using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPF.Core
{
	public static class PortalManager
	{
		public static PortalConfiguration GetCurrentPortalConfiguration()
		{
			PortalConfiguration result = null;
			using ( ICoreRepositoryManager repo = CoreServiceLocator.GetRepositoryManager() )
			{
				ICoreSystemServiceManager services = CoreServiceLocator.GetServiceManager( repo );
				result = services.Portals.Current;
			}

			return result;
		}

		public static PortalConfiguration GetPortalConfigurationByID( int systemPortalID )
		{
			PortalConfiguration result = null;
			using ( ICoreRepositoryManager repo = CoreServiceLocator.GetRepositoryManager() )
			{
				ICoreSystemServiceManager services = CoreServiceLocator.GetServiceManager( repo );
				result = services.Portals.GetPortalConfiguration( systemPortalID );
			}

			return result;
		}

		public static PortalConfiguration GetPortalConfigurationByTypeAndEnvironment( SystemPortalType type, RuntimeEnvironment? environment = null )
		{
			PortalConfiguration result = null;

			using ( ICoreRepositoryManager repo = CoreServiceLocator.GetRepositoryManager() )
			{
				ICoreSystemServiceManager services = CoreServiceLocator.GetServiceManager( repo );
				result = services.Portals.GetPortalConfiguration( type, environment );
			}

			return result;
		}
	}
}