using CERS.Configuration;
using CERS.Model;
using CERS.Repository;
using CERS.Repository.DataRegistry;
using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Text;
using UPF;
using UPF.Core;

namespace CERS
{
	public class CERSRepositoryManager : CachedTypeInstanceContainer, ICERSRepositoryManager, IDisposable
	{
		private bool _AutoDisposeDataModel = true;

		private IAccount _ContextAccount;
		private int _ContextAccountID;
		private ICoreRepositoryManager _CoreData;
		private CERSEntities _DataModel;
		private bool _Disposed;

		private CERSRepositoryManager( int contextAccountID = Constants.DefaultAccountID, CERSEntities dataModel = null )
		{
			ContextAccountID = contextAccountID;

			if ( dataModel != null )
			{
				_DataModel = dataModel;
				_AutoDisposeDataModel = false;
			}
			ApplyDataModelConfiguration();
		}

		~CERSRepositoryManager()
		{
			Dispose( false );
		}

		public AddFacilityWizardStateRepository AddFacilityWizardStates
		{
			get { return GetRepository<AddFacilityWizardStateRepository>(); }
		}

		public BPActivityRepository BPActivities
		{
			get { return GetRepository<BPActivityRepository>(); }
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

		public CERSStatisticsRepository CERSStatistics
		{
			get { return GetRepository<CERSStatisticsRepository>(); }
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
			get { return GetRepository<CMEBatchRepository>(); }
		}

		public CMEProgramElementRepository CMEProgramElements
		{
			get { return GetRepository<CMEProgramElementRepository>(); }
		}

		public int? CommandTimeout
		{
			get
			{
				return DataModel.CommandTimeout;
			}
			set
			{
				DataModel.CommandTimeout = value;
			}
		}

		public ContactRespository Contacts
		{
			get { return GetRepository<ContactRespository>(); }
		}

		public ContactStatisticRepository ContactStatistics
		{
			get { return GetRepository<ContactStatisticRepository>(); }
		}

		public IAccount ContextAccount
		{
			get
			{
				if ( _ContextAccount == null )
				{
					_ContextAccount = CoreData.Accounts.GetByID( ContextAccountID );
				}
				else
				{
					if ( ContextAccountID != _ContextAccount.ID )
					{
						_ContextAccount = CoreData.Accounts.GetByID( ContextAccountID );
					}
				}
				return _ContextAccount;
			}
		}

		public int ContextAccountID
		{
			get { return _ContextAccountID; }
			protected set { _ContextAccountID = value; }
		}

		public ICoreRepositoryManager CoreData
		{
			get
			{
				if ( _CoreData == null )
				{
					_CoreData = CoreServiceLocator.GetRepositoryManager( ContextAccountID );
				}
				return _CoreData;
			}
		}

		public CountyRepository Counties
		{
			get { return GetRepository<CountyRepository>(); }
		}

		public CountryRepository Countries
		{
			get { return GetRepository<CountryRepository>(); }
		}

		public Model.CERSEntities DataModel
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

		public IDataRegistryRepository DataRegistry { get { return GetRepository<DataRegistryRepository>(); } }

		public DeferredJobRepository DeferredJobs
		{
			get { return GetRepository<DeferredJobRepository>(); }
		}

		public DeferredProcessingRepository DeferredProcessingUpload
		{
			get { return GetRepository<DeferredProcessingRepository>(); }
		}

		public DocumentRepository Documents
		{
			get { return GetRepository<DocumentRepository>(); }
		}

		public EDTAuthenticationRequestRepository EDTAuthenticationRequests
		{
			get { return GetRepository<EDTAuthenticationRequestRepository>(); }
		}

		public EDTEndpointRepository EDTEndpoints
		{
			get { return GetRepository<EDTEndpointRepository>(); }
		}

		public EDTTransactionActivityLogRepository EDTTransactionActivityLogs
		{
			get { return GetRepository<EDTTransactionActivityLogRepository>(); }
		}

		public EDTTransactionMessageRepository EDTTransactionMessages
		{
			get { return GetRepository<EDTTransactionMessageRepository>(); }
		}

		public EDTTransactionQueryArgumentRepository EDTTransactionQueryArguments
		{
			get { return GetRepository<EDTTransactionQueryArgumentRepository>(); }
		}

		public EDTTransactionRepository EDTTransactions
		{
			get { return GetRepository<EDTTransactionRepository>(); }
		}

		public EDTTransactionXmlRepository EDTTransactionXmls
		{
			get { return GetRepository<EDTTransactionXmlRepository>(); }
		}

		public EmailQueueProcessReportRepository EmailQueueProcessReports
		{
			get { return GetRepository<EmailQueueProcessReportRepository>(); }
		}

		public EmailRepository Emails
		{
			get { return GetRepository<EmailRepository>(); }
		}

		public EmailTemplateRepository EmailTemplates
		{
			get { return GetRepository<EmailTemplateRepository>(); }
		}

		public EnforcementHistoryRepository EnforcementHistories
		{
			get { return GetRepository<EnforcementHistoryRepository>(); }
		}

		public EnforcementRepository Enforcements
		{
			get { return GetRepository<EnforcementRepository>(); }
		}

		public EnforcementViolationRepository EnforcementViolations
		{
			get { return GetRepository<EnforcementViolationRepository>(); }
		}

		public EnhancementCommentRepository EnhancementComments
		{
			get { return GetRepository<EnhancementCommentRepository>(); }
		}

		public EnhancementRepository Enhancements
		{
			get { return GetRepository<EnhancementRepository>(); }
		}

		public ErrorRepository Errors
		{
			get { return GetRepository<ErrorRepository>(); }
		}

		public EventRepository Events
		{
			get { return GetRepository<EventRepository>(); }
		}

		public EventTypeRepository EventTypes
		{
			get { return GetRepository<EventTypeRepository>(); }
		}

		public FacilityRepository Facilities
		{
			get { return GetRepository<FacilityRepository>(); }
		}

		public FacilityEDTTransactionRepository FacilityEDTTransactions
		{
			get { return GetRepository<FacilityEDTTransactionRepository>(); }
		}

		public FacilityRegulatorSubmittalElementEDTTransactionRepository FacilityRegulatorSubmittalElementEDTTransactions
		{
			get { return GetRepository<FacilityRegulatorSubmittalElementEDTTransactionRepository>(); }
		}

		public FacilityRegulatorSubmittalElementRepository FacilityRegulatorSubmittalElements
		{
			get { return GetRepository<FacilityRegulatorSubmittalElementRepository>(); }
		}

		public FacilitySubmittalEDTTransactionRepository FacilitySubmittalEDTTransactions
		{
			get { return GetRepository<FacilitySubmittalEDTTransactionRepository>(); }
		}

		public FacilitySubmittalElementDocumentRepository FacilitySubmittalElementDocuments
		{
			get { return GetRepository<FacilitySubmittalElementDocumentRepository>(); }
		}

		public FacilitySubmittalElementEDTTransactionRepository FacilitySubmittalElementEDTTransactions
		{
			get { return GetRepository<FacilitySubmittalElementEDTTransactionRepository>(); }
		}

		public FacilitySubmittalElementResourceDocumentRepository FacilitySubmittalElementResourceDocuments
		{
			get { return GetRepository<FacilitySubmittalElementResourceDocumentRepository>(); }
		}

		public FacilitySubmittalElementResourceRepository FacilitySubmittalElementResources
		{
			get { return GetRepository<FacilitySubmittalElementResourceRepository>(); }
		}

		public FacilitySubmittalElementRepository FacilitySubmittalElements
		{
			get { return GetRepository<FacilitySubmittalElementRepository>(); }
		}

		public FacilitySubmittalElementXmlRepository FacilitySubmittalElementXmls
		{
			get { return GetRepository<FacilitySubmittalElementXmlRepository>(); }
		}

		public FacilitySubmittalRepository FacilitySubmittals
		{
			get { return GetRepository<FacilitySubmittalRepository>(); }
		}

		public FacilityTransferHistoryRepository FacilityTransferHistories
		{
			get { return GetRepository<FacilityTransferHistoryRepository>(); }
		}

		public GuidanceMessageRepository GuidanceMessages
		{
			get { return GetRepository<GuidanceMessageRepository>(); }
		}

		public GuidanceMessageTemplateRepository GuidanceMessageTemplates
		{
			get { return GetRepository<GuidanceMessageTemplateRepository>(); }
		}

		public HelpItemRepository HelpItems
		{
			get { return GetRepository<HelpItemRepository>(); }
		}

		public HWTPFacilityRepository HWTPFacilities
		{
			get { return GetRepository<HWTPFacilityRepository>(); }
		}

		public HWTPFinancialAssuranceRepository HWTPFinancialAssurances
		{
			get { return GetRepository<HWTPFinancialAssuranceRepository>(); }
		}

		public HWTPUnitRepository HWTPUnits
		{
			get { return GetRepository<HWTPUnitRepository>(); }
		}

		public ImpersonatingSignInRepository ImpersonatingSignIns
		{
			get { return GetRepository<ImpersonatingSignInRepository>(); }
		}

		public CERS.Repository.Infrastructure.InfrastructureRepository Infrastructure { get { return GetRepository<CERS.Repository.Infrastructure.InfrastructureRepository>(); } }

		public InspectionHistoryRepository InspectionHistories
		{
			get { return GetRepository<InspectionHistoryRepository>(); }
		}

		public InspectionRepository Inspections
		{
			get { return GetRepository<InspectionRepository>(); }
		}

		public InventorySearchRepository InventorySearch
		{
			get { return GetRepository<InventorySearchRepository>(); }
		}

		public InventorySummaryRepository InventorySummaries
		{
			get { return GetRepository<InventorySummaryRepository>(); }
		}

		public NewsItemRepository NewsItems
		{
			get { return GetRepository<NewsItemRepository>(); }
		}

		public NotificationRepository Notifications
		{
			get { return GetRepository<NotificationRepository>(); }
		}

		public NotificationTemplateRepository NotificationTemplates
		{
			get { return GetRepository<NotificationTemplateRepository>(); }
		}

		public OrganizationContactPermissionGroupRepository OrganizationContactPermissionGroups
		{
			get { return GetRepository<OrganizationContactPermissionGroupRepository>(); }
		}

		public OrganizationContactRepository OrganizationContacts
		{
			get { return GetRepository<OrganizationContactRepository>(); }
		}

		public OrganizationDocumentRepository OrganizationDocuments
		{
			get { return GetRepository<OrganizationDocumentRepository>(); }
		}

		public OrganizationRepository Organizations
		{
			get { return GetRepository<OrganizationRepository>(); }
		}

		public PermissionGroupRegulatorTypeRepository PermissionGroupRegulatorTypes
		{
			get { return GetRepository<PermissionGroupRegulatorTypeRepository>(); }
		}

		public PermissionGroupRoleRepository PermissionGroupRoles
		{
			get { return GetRepository<PermissionGroupRoleRepository>(); }
		}

		public PermissionGroupRepository PermissionGroups
		{
			get { return GetRepository<PermissionGroupRepository>(); }
		}

		public PlaceRepository Places
		{
			get { return GetRepository<PlaceRepository>(); }
		}

		public PrintJobRepository PrintJobs
		{
			get { return GetRepository<PrintJobRepository>(); }
		}

		public PrintJobUnMergeableDocumentRepository PrintJobUnMergeableDocuments
		{
			get { return GetRepository<PrintJobUnMergeableDocumentRepository>(); }
		}

		public RegulatorContactPermissionGroupRepository RegulatorContactPermissionGroups
		{
			get { return GetRepository<RegulatorContactPermissionGroupRepository>(); }
		}

		public RegulatorContactRepository RegulatorContacts
		{
			get { return GetRepository<RegulatorContactRepository>(); }
		}

		public RegulatorDocumentRepository RegulatorDocuments
		{
			get { return GetRepository<RegulatorDocumentRepository>(); }
		}

		public RegulatorEDTTransactionRepository RegulatorEDTTransactions
		{
			get { return GetRepository<RegulatorEDTTransactionRepository>(); }
		}

		public RegulatorSubmittalElementLocalRequirementsRepository RegulatorLocals
		{
			get { return GetRepository<RegulatorSubmittalElementLocalRequirementsRepository>(); }
		}

		public RegulatorRelationshipRepository RegulatorRelationships
		{
			get { return GetRepository<RegulatorRelationshipRepository>(); }
		}

		public RegulatorRepository Regulators
		{
			get { return GetRepository<RegulatorRepository>(); }
		}

		public RegulatorZipCodeSubmittalElementRepository RegulatorZipCodeSubmittalElements
		{
			get { return GetRepository<RegulatorZipCodeSubmittalElementRepository>(); }
		}

		public RMActivitiesReportRepository RMActivityReports
		{
			get { return GetRepository<RMActivitiesReportRepository>(); }
		}

		public RMOffsiteGeneratorIdentificationRepository RMOffsiteGeneratorIdentifications
		{
			get { return GetRepository<RMOffsiteGeneratorIdentificationRepository>(); }
		}

		public RMRecyclableMaterialRepository RMRecyclableMaterials
		{
			get { return GetRepository<RMRecyclableMaterialRepository>(); }
		}

		public RWConsolidationSiteRepository RWConsolidationSite
		{
			get { return GetRepository<RWConsolidationSiteRepository>(); }
		}

		public SettingRepository Settings
		{
			get { return GetRepository<SettingRepository>(); }
		}

		public StateRepository States
		{
			get { return GetRepository<StateRepository>(); }
		}

		public SubmittalDeltaResultRepository SubmittalDeltaResults
		{
			get { return GetRepository<SubmittalDeltaResultRepository>(); }
		}

		public SubmittalElementRepository SubmittalElements
		{
			get { return GetRepository<SubmittalElementRepository>(); }
		}

		public SubmittalElementTemplateResourceRepository SubmittalElementTemplateResources
		{
			get { return GetRepository<SubmittalElementTemplateResourceRepository>(); }
		}

		public SubmittalElementTemplateRepository SubmittalElementTemplates
		{
			get { return GetRepository<SubmittalElementTemplateRepository>(); }
		}

		public SystemLookupTableRepository SystemLookupTables
		{
			get { return GetRepository<SystemLookupTableRepository>(); }
		}

		public USTFacilityInfoRepository USTFacilityInfos
		{
			get { return GetRepository<USTFacilityInfoRepository>(); }
		}

		public USTInstallModCertRepository USTInstallModCerts
		{
			get { return GetRepository<USTInstallModCertRepository>(); }
		}

		public USTMonitoringPlanRepository USTMonitoringPlans
		{
			get { return GetRepository<USTMonitoringPlanRepository>(); }
		}

		public USTTankInfoRepository USTTankInfos
		{
			get { return GetRepository<USTTankInfoRepository>(); }
		}

		public ViolationCategoryRepository ViolationCategories
		{
			get { return GetRepository<ViolationCategoryRepository>(); }
		}

		public ViolationCitationRepository ViolationCitations
		{
			get { return GetRepository<ViolationCitationRepository>(); }
		}

		public ViolationHistoryRepository ViolationHistories
		{
			get { return GetRepository<ViolationHistoryRepository>(); }
		}

		public ViolationProgramElementRepository ViolationProgramElements
		{
			get { return GetRepository<ViolationProgramElementRepository>(); }
		}

		public ViolationRepository Violations
		{
			get { return GetRepository<ViolationRepository>(); }
		}

		public ViolationSourceRepository ViolationSources
		{
			get { return GetRepository<ViolationSourceRepository>(); }
		}

		public ViolationTypeRepository ViolationTypes
		{
			get { return GetRepository<ViolationTypeRepository>(); }
		}

		public XmlSchemaRepository XmlSchemas
		{
			get { return GetRepository<XmlSchemaRepository>(); }
		}

		public ZipCodeRepository ZipCodes
		{
			get { return GetRepository<ZipCodeRepository>(); }
		}

		/// <summary>
		/// This is to make it harder for someone to call the constructor as they need to use the
		/// <see cref="ServiceLocator.GetRepositoryManager" /> method to get an instance of this class.
		/// </summary>
		/// <param name="repositoryManager"></param>
		/// <returns></returns>
		public static CERSRepositoryManager Create( int contextAccountID = Constants.DefaultAccountID, CERSEntities dataModel = null )
		{
			return new CERSRepositoryManager( contextAccountID, dataModel );
		}

		public virtual void ApplyDataModelConfiguration()
		{
			CERSConfigurationSection config = CERSConfigurationSection.Current;
			if ( config != null && config.DataModel != null )
			{
				if ( config.DataModel.CommandTimeout != null )
				{
					CommandTimeout = config.DataModel.CommandTimeout;
				}
			}
		}

		public void ClearObjectContextState()
		{
			IEnumerable<object> collection = from e in DataModel.ObjectStateManager.GetObjectStateEntries( System.Data.EntityState.Modified | System.Data.EntityState.Deleted )
											 select e.Entity;

			DataModel.Refresh( RefreshMode.StoreWins, collection );

			IEnumerable<object> AddedCollection = from e in DataModel.ObjectStateManager.GetObjectStateEntries( System.Data.EntityState.Added )
												  select e.Entity;

			foreach ( object addedEntity in AddedCollection )
			{
				DataModel.Detach( addedEntity );
			}
		}

		public void Dispose()
		{
			Dispose( true );
			GC.SuppressFinalize( this );
		}

		public virtual TRepository GetRepository<TRepository>() where TRepository : class, ICERSRepository
		{
			return GetObject<TRepository>( this );
		}

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

		protected virtual void DisposeResources()
		{
			if ( _DataModel != null && _AutoDisposeDataModel )
			{
				_DataModel.Dispose();
			}

			if ( _CoreData != null )
			{
				_CoreData.Dispose();
			}
		}
	}
}