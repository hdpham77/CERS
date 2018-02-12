using CERS.SystemServices;
using UPF;
using UPF.Core;

namespace CERS
{
	public class CERSSystemServiceManager : CachedTypeInstanceContainer, ICERSSystemServiceManager
	{
		private ICoreSystemServiceManager _CoreServices;
		private ICERSRepositoryManager _Repository;

		private CERSSystemServiceManager( ICERSRepositoryManager repositoryManager )
		{
			_Repository = repositoryManager;
		}

		public BusinessService BusinessLogic { get { return GetService<BusinessService>(); } }

		public virtual ICoreSystemServiceManager CoreServices
		{
			get
			{
				if ( _CoreServices == null )
				{
					_CoreServices = CoreServiceLocator.GetServiceManager( Repository.CoreData );
				}
				return _CoreServices;
			}
		}

        public DeferredJobService DeferredJob
        {
            get { return GetService<DeferredJobService>(); }
        }

		public EmailService Emails { get { return GetService<EmailService>(); } }

		public ErrorReportingService ErrorReporting
		{
			get { return GetService<ErrorReportingService>(); }
		}

		public EventService Events
		{
			get { return GetService<EventService>(); }
		}

		public ExcelService Excel
		{
			get { return GetService<ExcelService>(); }
		}

		public FacilityService Facilities
		{
			get { return GetService<FacilityService>(); }
		}

		public FacilitySubmittalModelEntityService FacilitySubmittalModelEntities
		{
			get { return GetService<FacilitySubmittalModelEntityService>(); }
		}

		public FacilitySubmittalService FacilitySubmittals
		{
			get { return GetService<FacilitySubmittalService>(); }
		}

		public GeoService Geo { get { return GetService<GeoService>(); } }

		public NotificationService Notification { get { return GetService<NotificationService>(); } }

		public PdfService PDF
		{
			get { return GetService<PdfService>(); }
		}

        public PrintJobService PrintJob
        {
            get { return GetService<PrintJobService>(); }
        }

        public JobDocumentCleanUpService JobDocumentCleanUp
        {
            get { return GetService<JobDocumentCleanUpService>(); }
        }
        
		public ReportService Reports { get { return GetService<ReportService>(); } }

		public virtual ICERSRepositoryManager Repository { get { return _Repository; } }

		public SecurityService Security
		{
			get { return GetService<SecurityService>(); }
		}

		public SubmittalDeltaService SubmittalDelta
		{
			get { return GetService<SubmittalDeltaService>(); }
		}

		public TextMacroProcessingService TextMacroProcessing
		{
			get { return GetService<TextMacroProcessingService>(); }
		}

		public ViewModelService ViewModels { get { return GetService<ViewModelService>(); } }

		public XmlService Xml
		{
			get { return GetService<XmlService>(); }
		}

		/// <summary>
  /// This is to make it harder for someone to call the constructor as they need to use the
  /// <see cref="ServiceLocator.GetSystemServiceManager"/> method to get an instance of this class.
  /// </summary>
  /// <param name="repositoryManager"></param>
  /// <returns></returns>
		public static CERSSystemServiceManager Create( ICERSRepositoryManager repositoryManager )
		{
			return new CERSSystemServiceManager( repositoryManager );
		}

		public virtual TService GetService<TService>() where TService : class, ICERSSystemService
		{
			TService service = GetObject<TService>( this );

			return service;
		}
	}
}