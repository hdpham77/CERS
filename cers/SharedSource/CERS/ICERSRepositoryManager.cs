using CERS.Model;
using CERS.Repository;
using CERS.Repository.DataRegistry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UPF;
using UPF.Core;

namespace CERS
{
	public interface ICERSRepositoryManager : IDisposable
	{
		AddFacilityWizardStateRepository AddFacilityWizardStates { get; }

		BPActivityRepository BPActivities { get; }

		BPFacilityChemicalRepository BPFacilityChemicals { get; }

		BPOwnerOperatorRepository BPOwnerOperators { get; }

		CERSFacilityGeoPointRepository CERSFacilityGeoPoints { get; }

        CERSStatisticsRepository CERSStatistics { get; }

		ChemicalRepository Chemicals { get; }

		ChemicalSynonymRepository ChemicalSynonyms { get; }

		CMEBatchRepository CMEBatches { get; }

		CMEProgramElementRepository CMEProgramElements { get; }

		int? CommandTimeout
		{
			get;
			set;
		}

		ContactRespository Contacts { get; }

		ContactStatisticRepository ContactStatistics { get; }

		IAccount ContextAccount { get; }

		int ContextAccountID { get; }

		ICoreRepositoryManager CoreData { get; }

		CountyRepository Counties { get; }

		CountryRepository Countries { get; }

		CERSEntities DataModel { get; }

		IDataRegistryRepository DataRegistry { get; }

		DeferredJobRepository DeferredJobs { get; }

		DeferredProcessingRepository DeferredProcessingUpload { get; }

		DocumentRepository Documents { get; }

		EDTAuthenticationRequestRepository EDTAuthenticationRequests { get; }

		EDTEndpointRepository EDTEndpoints { get; }

		EDTTransactionActivityLogRepository EDTTransactionActivityLogs { get; }

		EDTTransactionMessageRepository EDTTransactionMessages { get; }

		EDTTransactionQueryArgumentRepository EDTTransactionQueryArguments { get; }

		EDTTransactionRepository EDTTransactions { get; }

		EDTTransactionXmlRepository EDTTransactionXmls { get; }

		EmailQueueProcessReportRepository EmailQueueProcessReports { get; }

		EmailRepository Emails { get; }

		EmailTemplateRepository EmailTemplates { get; }

		EnforcementHistoryRepository EnforcementHistories { get; }

		EnforcementRepository Enforcements { get; }

		EnforcementViolationRepository EnforcementViolations { get; }

		EnhancementCommentRepository EnhancementComments { get; }

		EnhancementRepository Enhancements { get; }

		ErrorRepository Errors { get; }

		EventRepository Events { get; }

		EventTypeRepository EventTypes { get; }

		FacilityRepository Facilities { get; }

		FacilityEDTTransactionRepository FacilityEDTTransactions { get; }

		FacilityRegulatorSubmittalElementEDTTransactionRepository FacilityRegulatorSubmittalElementEDTTransactions { get; }

		FacilityRegulatorSubmittalElementRepository FacilityRegulatorSubmittalElements { get; }

		FacilitySubmittalEDTTransactionRepository FacilitySubmittalEDTTransactions { get; }

		FacilitySubmittalElementDocumentRepository FacilitySubmittalElementDocuments { get; }

		FacilitySubmittalElementEDTTransactionRepository FacilitySubmittalElementEDTTransactions { get; }

		FacilitySubmittalElementResourceDocumentRepository FacilitySubmittalElementResourceDocuments { get; }

		FacilitySubmittalElementResourceRepository FacilitySubmittalElementResources { get; }

		FacilitySubmittalElementRepository FacilitySubmittalElements { get; }

		FacilitySubmittalElementXmlRepository FacilitySubmittalElementXmls { get; }

		FacilitySubmittalRepository FacilitySubmittals { get; }

		FacilityTransferHistoryRepository FacilityTransferHistories { get; }

		GuidanceMessageRepository GuidanceMessages { get; }

		GuidanceMessageTemplateRepository GuidanceMessageTemplates { get; }

		HelpItemRepository HelpItems { get; }

		HWTPFacilityRepository HWTPFacilities { get; }

		HWTPFinancialAssuranceRepository HWTPFinancialAssurances { get; }

		HWTPUnitRepository HWTPUnits { get; }

		ImpersonatingSignInRepository ImpersonatingSignIns { get; }

		CERS.Repository.Infrastructure.InfrastructureRepository Infrastructure { get; }

		InspectionHistoryRepository InspectionHistories { get; }

		InspectionRepository Inspections { get; }

		InventorySummaryRepository InventorySummaries { get; }

        InventorySearchRepository InventorySearch { get; }

		NewsItemRepository NewsItems { get; }

		NotificationRepository Notifications { get; }

		NotificationTemplateRepository NotificationTemplates { get; }

		OrganizationContactPermissionGroupRepository OrganizationContactPermissionGroups { get; }

		OrganizationContactRepository OrganizationContacts { get; }

		OrganizationDocumentRepository OrganizationDocuments { get; }

		OrganizationRepository Organizations { get; }

		PermissionGroupRegulatorTypeRepository PermissionGroupRegulatorTypes { get; }

		PermissionGroupRoleRepository PermissionGroupRoles { get; }

		PermissionGroupRepository PermissionGroups { get; }

		PlaceRepository Places { get; }

		PrintJobRepository PrintJobs { get; }

		PrintJobUnMergeableDocumentRepository PrintJobUnMergeableDocuments { get; }

		RegulatorContactPermissionGroupRepository RegulatorContactPermissionGroups { get; }

		RegulatorContactRepository RegulatorContacts { get; }

		RegulatorDocumentRepository RegulatorDocuments { get; }

		RegulatorEDTTransactionRepository RegulatorEDTTransactions { get; }

		RegulatorSubmittalElementLocalRequirementsRepository RegulatorLocals { get; }

		RegulatorRelationshipRepository RegulatorRelationships { get; }

		RegulatorRepository Regulators { get; }

		RegulatorZipCodeSubmittalElementRepository RegulatorZipCodeSubmittalElements { get; }

		RMActivitiesReportRepository RMActivityReports { get; }

		RMOffsiteGeneratorIdentificationRepository RMOffsiteGeneratorIdentifications { get; }

		RMRecyclableMaterialRepository RMRecyclableMaterials { get; }

		RWConsolidationSiteRepository RWConsolidationSite { get; }

		SettingRepository Settings { get; }

		StateRepository States { get; }

		SubmittalDeltaResultRepository SubmittalDeltaResults { get; }

		SubmittalElementRepository SubmittalElements { get; }

		SubmittalElementTemplateResourceRepository SubmittalElementTemplateResources { get; }

		SubmittalElementTemplateRepository SubmittalElementTemplates { get; }

		SystemLookupTableRepository SystemLookupTables { get; }

		USTFacilityInfoRepository USTFacilityInfos { get; }

		USTInstallModCertRepository USTInstallModCerts { get; }

		USTMonitoringPlanRepository USTMonitoringPlans { get; }

		USTTankInfoRepository USTTankInfos { get; }

		ViolationCategoryRepository ViolationCategories { get; }

		ViolationCitationRepository ViolationCitations { get; }

		ViolationHistoryRepository ViolationHistories { get; }

		ViolationProgramElementRepository ViolationProgramElements { get; }

		ViolationRepository Violations { get; }

		ViolationSourceRepository ViolationSources { get; }

		ViolationTypeRepository ViolationTypes { get; }

		XmlSchemaRepository XmlSchemas { get; }

		ZipCodeRepository ZipCodes { get; }

		void ClearObjectContextState();

		TRepository GetRepository<TRepository>() where TRepository : class, ICERSRepository;
	}
}