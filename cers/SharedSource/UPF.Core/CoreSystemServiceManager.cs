using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UPF.Core.SystemServices;

namespace UPF.Core
{
	public class CoreSystemServiceManager : CachedTypeInstanceContainer, ICoreSystemServiceManager
	{
		private CoreSystemServiceManager( ICoreRepositoryManager repositoryManager )
		{
			Repository = repositoryManager;
		}

		public ActivityLogService ActivityLog
		{
			get { return GetService<ActivityLogService>(); }
		}

		public AuthenticationService Authentication
		{
			get { return GetService<AuthenticationService>(); }
		}

		public PortalService Portals
		{
			get { return GetService<PortalService>(); }
		}

		public ICoreRepositoryManager Repository
		{
			get;
			protected set;
		}

		public UrlService Urls
		{
			get { return GetService<UrlService>(); }
		}

		/// <summary>
		/// This is to make it harder for someone to call the constructor as they need to use the <see cref="ServiceLocator.GetSystemServiceManager"/> method to get
		/// an instance of this class.
		/// </summary>
		/// <param name="repositoryManager"></param>
		/// <returns></returns>
		public static CoreSystemServiceManager Create( ICoreRepositoryManager repositoryManager )
		{
			return new CoreSystemServiceManager( repositoryManager );
		}

		public virtual TService GetService<TService>() where TService : class, ICoreSystemService
		{
			TService service = GetObject<TService>( this );

			return service;
		}
	}
}