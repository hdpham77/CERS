using CERS.Model;
using CERS.Repository;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using UPF;
using UPF.Core;

namespace CERS
{
	public class CersRepositories : ICersRepositoryService, IDisposable
	{
		#region Fields

		private int _AccountID;
		private CERSEntities _DataModel;
		private bool _DataModelInitializedOutside = false;
		private bool _Disposed = false;
		private Dictionary<Type, object> _Repositories;

		#endregion Fields

		#region Constructors

		public CersRepositories()
			: this( Constants.DefaultAccountID )
		{
		}

		public CersRepositories( int accountID )
		{
			accountID = accountID;
		}

		public CersRepositories( CERSEntities dataModel )
		{
			Contract.Requires( dataModel != null );
			_DataModel = dataModel;
			_DataModelInitializedOutside = true;
		}

		#endregion Constructors

		#region Properties

		public virtual int AccountID
		{
			get { return _AccountID; }
		}

		public BPActivityRepository BPActivities
		{
			get
			{
				return GetRepository<BPActivityRepository>();
			}
		}

		public BPFacilityChemicalRepository BPFacilityChemicals
		{
			get { return GetRepository<BPFacilityChemicalRepository>(); }
		}

		public BPOwnerOperatorRepository BPOwnerOperators
		{
			get { return GetRepository<BPOwnerOperatorRepository>(); }
		}

		public CERSFacilityGeoPointRepository CERSFacilityGeoPoints
		{
			get { return GetRepository<CERSFacilityGeoPointRepository>(); }
		}

		public ChemicalRepository Chemicals
		{
			get { return GetRepository<ChemicalRepository>(); }
		}

		public ChemicalSynonymRepository ChemicalSynonyms
		{
			get { return GetRepository<ChemicalSynonymRepository>(); }
		}

		public CMEBatchRepository CMEBatches
		{
			get
			{
				return GetRepository<CMEBatchRepository>();
			}
		}

		public CMEProgramElementRepository CMEProgramElements
		{
			get
			{
				return GetRepository<CMEProgramElementRepository>();
			}
		}

		public ContactRespository Contacts
		{
			get
			{
				return GetRepository<ContactRespository>();
			}
		}

		public ContactStatisticRepository ContactStatistics
		{
			get
			{
				return GetRepository<ContactStatisticRepository>();
			}
		}

		public CountyRepository Counties
		{
			get
			{
				return GetRepository<CountyRepository>();
			}
		}

		public CountryRepository Countries
		{
			get
			{
				return GetRepository<CountryRepository>();
			}
		}

		public DeferredProcessingRepository DeferredProcessing
		{
			get
			{
				return GetRepository<DeferredProcessingRepository>();
			}
		}

		public DocumentRepository Documents
		{
			get
			{
				return GetRepository<DocumentRepository>();
			}
		}

		public EDTEndpointRepository EDTEndpoints
		{
			get
			{
				return GetRepository<EDTEndpointRepository>();
			}
		}

		public EDTQueueRepository EDTQueue
		{
			get
			{
				return GetRepository<EDTQueueRepository>();
			}
		}

		public EDTTransactionActivityLogRepository EDTTransactionActivityLogs
		{
			get
			{
				return GetRepository<EDTTransactionActivityLogRepository>();
			}
		}

		public EDTTransactionMessageRepository EDTTransactionMessages
		{
			get
			{
				return GetRepository<EDTTransactionMessageRepository>();
			}
		}

		public EDTTransactionRepository EDTTransactions
		{
			get
			{
				return GetRepository<EDTTransactionRepository>();
			}
		}

		public EmailQueueProcessReportRepository EmailQueueProcessReports
		{
			get
			{
				return GetRepository<EmailQueueProcessReportRepository>();
			}
		}

		public EmailRepository Emails
		{
			get
			{
				return GetRepository<EmailRepository>();
			}
		}

		public EmailTemplateRepository EmailTemplates
		{
			get
			{
				return GetRepository<EmailTemplateRepository>();
			}
		}

		public EnforcementHistoryRepository EnforcementHistories
		{
			get
			{
				return GetRepository<EnforcementHistoryRepository>();
			}
		}

		public EnforcementRepository Enforcements
		{
			get
			{
				return GetRepository<EnforcementRepository>();
			}
		}

		public EnforcementViolationRepository EnforcementViolations
		{
			get
			{
				return GetRepository<EnforcementViolationRepository>();
			}
		}

		public EnhancementCommentRepository EnhancementComments
		{
			get
			{
				return GetRepository<EnhancementCommentRepository>();
			}
		}

		public EnhancementRepository Enhancements
		{
			get
			{
				return GetRepository<EnhancementRepository>();
			}
		}

		public ErrorRepository Errors
		{
			get
			{
				return GetRepository<ErrorRepository>();
			}
		}

		public EventRepository Events
		{
			get
			{
				return GetRepository<EventRepository>();
			}
		}

		public EventTypeRepository EventTypes
		{
			get
			{
				return GetRepository<EventTypeRepository>();
			}
		}

		public FacilityRepository Facilities
		{
			get
			{
				return GetRepository<FacilityRepository>();
			}
		}

		public FacilityRegulatorSubmittalElementRepository FacilityRegulatorSubmittalElements
		{
			get
			{
				return GetRepository<FacilityRegulatorSubmittalElementRepository>();
			}
		}

		public FacilitySubmittalEDTTransactionRepository FacilitySubmittalEDTTransactions
		{
			get
			{
				return GetRepository<FacilitySubmittalEDTTransactionRepository>();
			}
		}

		public FacilitySubmittalElementDocumentRepository FacilitySubmittalElementDocuments
		{
			get
			{
				return GetRepository<FacilitySubmittalElementDocumentRepository>();
			}
		}

		public FacilitySubmittalElementResourceDocumentRepository FacilitySubmittalElementResourceDocuments
		{
			get
			{
				return GetRepository<FacilitySubmittalElementResourceDocumentRepository>();
			}
		}

		public FacilitySubmittalElementResourceRepository FacilitySubmittalElementResources
		{
			get
			{
				return GetRepository<FacilitySubmittalElementResourceRepository>();
			}
		}

		public FacilitySubmittalElementRepository FacilitySubmittalElements
		{
			get
			{
				return GetRepository<FacilitySubmittalElementRepository>();
			}
		}

		public FacilitySubmittalElementXmlRepository FacilitySubmittalElementXmls
		{
			get
			{
				return GetRepository<FacilitySubmittalElementXmlRepository>();
			}
		}

		public FacilitySubmittalRepository FacilitySubmittals
		{
			get
			{
				return GetRepository<FacilitySubmittalRepository>();
			}
		}

		public GuidanceMessageRepository GuidanceMessages
		{
			get
			{
				return GetRepository<GuidanceMessageRepository>();
			}
		}

		public GuidanceMessageTemplateRepository GuidanceMessageTemplates
		{
			get
			{
				return GetRepository<GuidanceMessageTemplateRepository>();
			}
		}

		public HelpItemRepository HelpItems
		{
			get
			{
				return GetRepository<HelpItemRepository>();
			}
		}

		public HWTPFacilityRepository HWTPFacilities
		{
			get
			{
				return GetRepository<HWTPFacilityRepository>();
			}
		}

		public HWTPFinancialAssuranceRepository HWTPFinancialAssurances
		{
			get
			{
				return GetRepository<HWTPFinancialAssuranceRepository>();
			}
		}

		public HWTPUnitRepository HWTPUnits
		{
			get
			{
				return GetRepository<HWTPUnitRepository>();
			}
		}

		public ImpersonatingSignInRepository ImpersonatingSignIns
		{
			get
			{
				return GetRepository<ImpersonatingSignInRepository>();
			}
		}

		public InspectionHistoryRepository InspectionHistories
		{
			get
			{
				return GetRepository<InspectionHistoryRepository>();
			}
		}

		public InspectionRepository Inspections
		{
			get
			{
				return GetRepository<InspectionRepository>();
			}
		}

		public MigrationIssueRepository MigrationIssues
		{
			get
			{
				return GetRepository<MigrationIssueRepository>();
			}
		}

		public NewsItemRepository NewsItems
		{
			get
			{
				return GetRepository<NewsItemRepository>();
			}
		}

		public NotificationRepository Notifications
		{
			get
			{
				return GetRepository<NotificationRepository>();
			}
		}

		public NotificationTemplateRepository NotificationTemplates
		{
			get
			{
				return GetRepository<NotificationTemplateRepository>();
			}
		}

		public OrganizationContactPermissionGroupRepository OrganizationContactPermissionGroups
		{
			get
			{
				return GetRepository<OrganizationContactPermissionGroupRepository>();
			}
		}

		public OrganizationContactRepository OrganizationContacts
		{
			get
			{
				return GetRepository<OrganizationContactRepository>();
			}
		}

		public OrganizationDocumentRepository OrganizationDocuments
		{
			get
			{
				return GetRepository<OrganizationDocumentRepository>();
			}
		}

		public OrganizationRepository Organizations
		{
			get
			{
				return GetRepository<OrganizationRepository>();
			}
		}

		public PermissionGroupRegulatorTypeRepository PermissionGroupRegulatorTypes
		{
			get
			{
				return GetRepository<PermissionGroupRegulatorTypeRepository>();
			}
		}

		public PermissionGroupRoleRepository PermissionGroupRoles
		{
			get
			{
				return GetRepository<PermissionGroupRoleRepository>();
			}
		}

		public PermissionGroupRepository PermissionGroups
		{
			get
			{
				return GetRepository<PermissionGroupRepository>();
			}
		}

		public PlaceRepository Places
		{
			get
			{
				return GetRepository<PlaceRepository>();
			}
		}

		public RegulatorContactPermissionGroupRepository RegulatorContactPermissionGroups
		{
			get
			{
				return GetRepository<RegulatorContactPermissionGroupRepository>();
			}
		}

		public RegulatorContactRepository RegulatorContacts
		{
			get
			{
				return GetRepository<RegulatorContactRepository>();
			}
		}

		public RegulatorDocumentRepository RegulatorDocuments
		{
			get
			{
				return GetRepository<RegulatorDocumentRepository>();
			}
		}

		public RegulatorEDTTransactionRepository RegulatorEDTTransactions
		{
			get
			{
				return GetRepository<RegulatorEDTTransactionRepository>();
			}
		}

		public RegulatorSubmittalElementLocalRequirementsRepository RegulatorLocals
		{
			get
			{
				return GetRepository<RegulatorSubmittalElementLocalRequirementsRepository>();
			}
		}

		public RegulatorRelationshipRepository RegulatorRelationships
		{
			get
			{
				return GetRepository<RegulatorRelationshipRepository>();
			}
		}

		public RegulatorRepository Regulators
		{
			get
			{
				return GetRepository<RegulatorRepository>();
			}
		}

		public RegulatorZipCodeSubmittalElementRepository RegulatorZipCodeSubmittalElements
		{
			get
			{
				return GetRepository<RegulatorZipCodeSubmittalElementRepository>();
			}
		}

		public RMActivitiesReportRepository RMActivityReports
		{
			get
			{
				return GetRepository<RMActivitiesReportRepository>();
			}
		}

		public RMOffsiteGeneratorIdentificationRepository RMOffsiteGeneratorIdentifications
		{
			get
			{
				return GetRepository<RMOffsiteGeneratorIdentificationRepository>();
			}
		}

		public RMRecyclableMaterialRepository RMRecyclableMaterials
		{
			get
			{
				return GetRepository<RMRecyclableMaterialRepository>();
			}
		}

		public RWConsolidationSiteRepository RWConsolidationSite
		{
			get
			{
				return GetRepository<RWConsolidationSiteRepository>();
			}
		}

		public SettingRepository Settings
		{
			get
			{
				return GetRepository<SettingRepository>();
			}
		}

		public StateRepository States
		{
			get
			{
				return GetRepository<StateRepository>();
			}
		}

		public SubmittalElementRepository SubmittalElements
		{
			get
			{
				return GetRepository<SubmittalElementRepository>();
			}
		}

		public SubmittalElementTemplateResourceRepository SubmittalElementTemplateResources
		{
			get
			{
				return GetRepository<SubmittalElementTemplateResourceRepository>();
			}
		}

		public SubmittalElementTemplateRepository SubmittalElementTemplates
		{
			get
			{
				return GetRepository<SubmittalElementTemplateRepository>();
			}
		}

		public SystemLookupTableRepository SystemLookupTables
		{
			get
			{
				return GetRepository<SystemLookupTableRepository>();
			}
		}

		public USTFacilityInfoRepository USTFacilityInfos
		{
			get
			{
				return GetRepository<USTFacilityInfoRepository>();
			}
		}

		public USTInstallModCertRepository USTInstallModCerts
		{
			get
			{
				return GetRepository<USTInstallModCertRepository>();
			}
		}

		public USTMonitoringPlanRepository USTMonitoringPlans
		{
			get
			{
				return GetRepository<USTMonitoringPlanRepository>();
			}
		}

		public USTTankInfoRepository USTTankInfos
		{
			get
			{
				return GetRepository<USTTankInfoRepository>();
			}
		}

		public ViolationCategoryRepository ViolationCategories
		{
			get
			{
				return GetRepository<ViolationCategoryRepository>();
			}
		}

		public ViolationCitationRepository ViolationCitations
		{
			get
			{
				return GetRepository<ViolationCitationRepository>();
			}
		}

		public ViolationHistoryRepository ViolationHistories
		{
			get
			{
				return GetRepository<ViolationHistoryRepository>();
			}
		}

		public ViolationProgramElementRepository ViolationProgramElements
		{
			get
			{
				return GetRepository<ViolationProgramElementRepository>();
			}
		}

		public ViolationRepository Violations
		{
			get
			{
				return GetRepository<ViolationRepository>();
			}
		}

		public ViolationSourceRepository ViolationSources
		{
			get
			{
				return GetRepository<ViolationSourceRepository>();
			}
		}

		public ViolationTypeRepository ViolationTypes
		{
			get
			{
				return GetRepository<ViolationTypeRepository>();
			}
		}

		public XmlSchemaRepository XmlSchemas
		{
			get
			{
				return GetRepository<XmlSchemaRepository>();
			}
		}

		public ZipCodeRepository ZipCodes
		{
			get
			{
				return GetRepository<ZipCodeRepository>();
			}
		}

		protected virtual CERSEntities DataModel
		{
			get
			{
				if ( _DataModel == null )
				{
					_DataModel = new CERSEntities();
				}
				return _DataModel;
			}
		}

		protected virtual Dictionary<Type, object> Repositories
		{
			get
			{
				if ( _Repositories == null )
				{
					_Repositories = new Dictionary<Type, object>();
				}
				return _Repositories;
			}
		}

		#endregion Properties

		#region Methods

		public virtual TRepository GetRepository<TRepository>() where TRepository : class, ICersRepository, new()
		{
			TRepository repo = default( TRepository );

			if ( Repositories.ContainsKey( typeof( TRepository ) ) )
			{
				repo = Repositories[typeof( TRepository )] as TRepository;
			}
			else
			{
				repo = new TRepository();
				repo.Initialize( DataModel, AccountID );
				Repositories.Add( typeof( TRepository ), repo );
			}

			return repo;
		}

		#endregion Methods

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
			if ( _DataModel != null && _DataModelInitializedOutside )
			{
				_DataModel.Dispose();
			}
		}

		#endregion DiposeResources Method

		#region Finalizer

		~CersRepositories()
		{
			Dispose( false );
		}

		#endregion Finalizer
	}
}