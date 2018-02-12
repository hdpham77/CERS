using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UPF;
using UPF.Core.Model;
using UPF.Core.Repository;

namespace UPF.Core
{
	public class CoreRepositoryManager : CachedTypeInstanceContainer, ICoreRepositoryManager, IDisposable
	{
		#region Fields

		private IAccount _ContextAccount;
		private CoreEntities _DataModel;
		private bool _Disposed;

		#endregion Fields

		#region Properties

		public AccountAuthenticationAttemptRepository AccountAuthenticationAttempts
		{
			get
			{
				return GetRepository<AccountAuthenticationAttemptRepository>();
			}
		}

		public AccountAuthenticationStatisticRepository AccountAuthenticationStatistics
		{
			get
			{
				return GetRepository<AccountAuthenticationStatisticRepository>();
			}
		}

		public AccountRepository Accounts
		{
			get
			{
				return GetRepository<AccountRepository>();
			}
		}

		public AccountSecurityQuestionAnswerRepository AccountSecurityQuestions
		{
			get
			{
				return GetRepository<AccountSecurityQuestionAnswerRepository>();
			}
		}

		public ActivityLogRepository ActivityLog
		{
			get { return GetRepository<ActivityLogRepository>(); }
		}

		public IAccount ContextAccount
		{
			get
			{
				if ( _ContextAccount == null )
				{
					_ContextAccount = Accounts.GetByID( ContextAccountID );
				}
				else
				{
					if ( ContextAccountID != _ContextAccount.ID )
					{
						_ContextAccount = Accounts.GetByID( ContextAccountID );
					}
				}
				return _ContextAccount;
			}
		}

		public int ContextAccountID { get; protected set; }

		public CoreEntities DataModel
		{
			get
			{
				if ( _DataModel == null )
				{
					_DataModel = new CoreEntities();
				}
				return _DataModel;
			}
		}

		public LookupTableRepository LookupTables
		{
			get { return GetRepository<LookupTableRepository>(); }
		}

		public SecurityQuestionRepository SecurityQuestions
		{
			get
			{
				return GetRepository<SecurityQuestionRepository>();
			}
		}

		public SettingRepository Settings
		{
			get
			{
				return GetRepository<SettingRepository>();
			}
		}

		public SystemPortalRepository SystemPortals
		{
			get
			{
				return GetRepository<SystemPortalRepository>();
			}
		}

		public UrlLinkRepository UrlLinks
		{
			get { return GetRepository<UrlLinkRepository>(); }
		}

		#endregion Properties

		#region Constructors

		private CoreRepositoryManager( int contextAccountID = Constants.DefaultAccountID )
		{
			ContextAccountID = contextAccountID;
		}

		#endregion Constructors

		#region IDisposable Interface

		public void Dispose()
		{
			Dispose( true );
			GC.SuppressFinalize( this );
		}

		#endregion IDisposable Interface

		#region Dispose(disposing) Method

		protected virtual void Dispose( bool disposing )
		{
			if ( !_Disposed )
			{
				if ( disposing )
				{
					DisposeResources();
				}
				_Disposed = true;
			}
		}

		#endregion Dispose(disposing) Method

		#region DiposeResources Method

		protected virtual void DisposeResources()
		{
			if ( _DataModel != null )
			{
				_DataModel.Dispose();
			}
		}

		#endregion DiposeResources Method

		#region Finalizer

		~CoreRepositoryManager()
		{
			Dispose( false );
		}

		#endregion Finalizer

		public static ICoreRepositoryManager Create( int contextAccountID )
		{
			return new CoreRepositoryManager( contextAccountID );
		}

		public virtual TRepository GetRepository<TRepository>() where TRepository : class, ICoreRepository
		{
			return GetObject<TRepository>( this );
		}
	}
}