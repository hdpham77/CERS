using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UPF.Core.Model;

namespace UPF.Core
{
	public class CoreServiceLocator
	{
		public static ICoreRepositoryManager GetRepositoryManager( int contextAccountID = Constants.DefaultAccountID )
		{
			return CoreRepositoryManager.Create( contextAccountID );
		}

		public static ICoreSystemServiceManager GetServiceManager( ICoreRepositoryManager coreRepositoryManager )
		{
			return CoreSystemServiceManager.Create( coreRepositoryManager );
		}
	}
}