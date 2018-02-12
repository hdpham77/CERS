using CERS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UPF;

namespace CERS.Windows
{
	public class WindowBase : UPF.Windows.WindowBase
	{
		private ICERSRepositoryManager _Repository;
		private ICERSSystemServiceManager _Services;

		public ICERSRepositoryManager Repository
		{
			get
			{
				if ( _Repository == null )
				{
					_Repository = ServiceLocator.GetRepositoryManager();
				}
				return _Repository;
			}
		}

		public ICERSSystemServiceManager Services
		{
			get
			{
				if ( _Services == null )
				{
					_Services = ServiceLocator.GetSystemServiceManager( Repository );
				}
				return _Services;
			}
		}
	}
}