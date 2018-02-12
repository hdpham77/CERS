using CERS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UPF.Core;

namespace CERS
{
	public class ServiceLocator
	{
		public static ICERSRepositoryManager GetRepositoryManager( int contextAccountID = Constants.DefaultAccountID, CERSEntities dataModel = null )
		{
			return CERSRepositoryManager.Create( contextAccountID, dataModel );
		}

		public static ICERSSystemServiceManager GetSystemServiceManager( ICERSRepositoryManager repository )
		{
			return CERSSystemServiceManager.Create( repository );
		}
	}
}