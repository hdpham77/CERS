using System;

namespace CERS
{
    public enum AccountStatusViewMode
    {
        ActivationPending,
        AccountActivated,
        ResetRequestSent
    }

    public enum AddEntityContactWizardStep
    {
        EnterEmail,
        ExistingContact,
        NewContact,
        EditPermissions,
        Edit
    }

    public enum AddFacilityWizardAction
    {
        TakeOwnership = 1,
        RequestAccess = 2
    }

    public enum AddFacilityWizardResult
    {
        FacilityAddedToUsersExistingOrganization = 1,
        FacilityAddedToNewOrganization = 2,
        FacilityAddedToUsersExistingOrganizationAdditional = 3,
        FacilityAddedToNewOrganizationAdditional = 4,
        FacilityAlreadyExistsForExistingOrganization = 5,
        FacilityTransferRequest = 6,
        FacilityTransferRequestAborted = 7,
        FacilityNotAddedCancelledByUser = 8,
        OrganizationAccessRequestFacilityAlreadyExists = 9
    }

    public enum AddFacilityWizardStep
    {
        ProvideAddress = 1,
        ProvideFacilityName = 2,
        NewFacilityNewOrganization = 3,
        NewFacilityAddedConfirmation = 4,
        ChooseExistingFacility = 5,
        FacilityExistsAlreadyBelongsToUsersOrganization = 6,
        NewAdditionalFacilitySharesAddressWithAnotherFacility = 7,
        FacilityExistsRequestAccessOrTransfer = 8
    }

    public enum BuiltInPermissionGroup
    {
        SysAdmins = 1,
        HelpAdmins = 2,
        SecurityAdmins = 3,
        UPViewers = 4,
        UPEditors = 5,
        StatewideViewersUnrestricted = 6,
        UPAViewers = 7,
        UPAApprovers = 8,
        UPAInspectors = 9,
        UPAAdmins = 10,
        OrgViewers = 11,
        OrgEditors = 12,
        OrgApprovers = 13,
        OrgAdmins = 14,
        UPAOrgNotifications = 15,
        UPADataUploaders = 21,
        StatewideViewersRestricted = 33,
        CERSDataRegistryHelpTextEditors = 34,
		UPAERAdmins = 35,
		UPAERViewers = 36
    }

    public enum CacheStrategy
    {
        OnDemand,
        Preload,
        None
    }

    public enum CallerContext
    {
        EDT = 1,
        UI = 2,
        Seeding = 3
    }

    public enum ChemicalNameType
    {
        ChemicalName = 1,
        CommonName = 2,
        Synonym = 3
    }

    public enum CircleIcon
    {
        Green,

        // Yellow,
        Red,

        Grey,
        Blue,
        Orange
    }

    public enum CMEDataStatus
    {
        Draft = 1,
        Approved = 2,
        Deleted = 3
    }

    public enum CMEProgramElementID
    {
        HazardousMaterialsReleaseResponsePlan = 1,
        CaliforniaAccidentalReleasePrevention = 2,
        UndergroundStorageTank = 3,
        AbovegroundStorageTank = 4,
        HazardousWasteGenerator = 5,
        HazardousWasteRCRALargeQuantityGenerator = 6,
        HazardousWasteRecycler = 7,
        PermitByRule = 8,
        ConditionallyAuthorized = 9,
        ConditionallyExempt = 10,
        HouseholdHazardousWaste = 11
    }

    public enum CMEResourceType
    {
        Inspection,
        Enforcement,
        Violation,
        EnforcementViolation
    }

    public enum CMEViolationContext
    {
        Summary = 1,
        Detailed = 2
    }

    public enum ConfirmAddressType
    {
        UserEntered,
        Washed
    }

    public enum Context
    {
        Organization = 1,
        Regulator = 2,
        Facility = 3
    }

    public enum CreateOrganizationReason
    {
        AddFacility = 1,
        TransferFacility = 2
    }

    public enum DataRegistryDataSourceType
    {
        UPDD,
        System,
        Supplemental
    }

    public enum DebugOutputLayout
    {
        RowPropertyList,
        VerticalPropertyList
    }

    public enum DeferredJobStatus
    {
        Queued = 1,
        InProgress = 2,
        Success = 3,
        Failed = 4,
        Hold = 5,
    }

    public enum DeferredJobType
    {
        RegulatorHMIDownload = 1,
        OrganizationHMIDownload = 2,
    }

    public enum DeferredProcessingBatchType
    {
        OwnerOperator = 1,
        SingleFacilityHMI = 2,
        MultiFacilityHMI = 3,
    }

    public enum DeferredProcessingItemStatus
    {
        Queued = 1,
        InProgress = 2,
        Success = 3,
        Failed = 4,
        Cancelled = 5,
    }

    public enum DeferredProcessingUploadType
    {
        Replace = 1,
        Append = 2
    }

    public enum DeltaChangeSignificance
    {
        NoChange = 1,
        Change = 2,
        SignificantChange = 3,
        VerySignificantChange = 4
    }

    public enum DocumentContext
    {
        Regulator,
        Facility,
        Organization,
        Enforcement,
        Inspection,
        Violation,
        HelpItem
    }

    public enum DocumentFormat
    {
        PDF = 1,
        DOC = 2,
        XLS = 3,
        JPEG = 4,
        DOCX = 5,
        XLSX = 6
    }

    public enum DocumentSource
    {
        Exempt = 1,
        ProvidedToRegulator = 2,
        ProvidedInOtherSubmittalElement = 3,
        PublicInternetURL = 4,
        UploadDocuments = 5,
        StoredAtFacility = 6
    }

    public enum DocumentStatus
    {
        Exists = 1,
        Missing = 2,
        Deleted = 3,
        Purged = 4
    }

    public enum DocumentType
    {
        Evaluation_Initial = 1,
        Evaluation_Update = 2,
        Evaluation_FinalClosing = 3,
        Evaluation_Correspondence = 4,
        Evaluation_InitialFinalClosing = 5,
        Evaluation_FinalClosingWithDeficiencies = 6,
        General_Site_Map = 7,
        Annotated_Site_Map = 8,
        Emergency_Response_Plan = 9,
        Employee_Training_Plan = 10,
        Plot_Plan = 11,
        Prior_Enforcement_History = 12,
        Written_Estimate_of_Closure_Costs = 13,
        Financial_Assurance_Closure_Mechanism = 14,
        Tank_and_Container_Certifications = 15,
        Notification_of_Local_Agency_or_Agencies = 16,
        Notification_of_Property_Owner = 17,
        Documentation_of_Known_Market = 18,
        Hazardous_Waste_Tank_Closure_Certification = 19,
        Locally_Required_Document_Upload = 20,
        UST_Monitoring_Site_Plan = 21,
        UST_Certification_of_Financial_Responsibility = 22,
        UST_Response_Plan = 23,
        Written_agreement_between_UST_Owner_and_UST_Operator = 24,
        Letter_from_the_Chief_Financial_Officer = 25,
        Owner_Statement_of_Designated_UST_Operator_Understanding_of_and_Compliance_with_UST_Requirements = 26,
        Other = 27,
        APSA_Document = 28,
        Organization_Request_Letter = 29,
        RMR_Document = 30,
        Miscellaneous_State_Required_Document = 31
    }

    public enum EDTAuthenticationRequestResult
    {
        AuthenticatedAuthorized = 1,
        AuthenticatedNotAuthorizedForRegulator = 2,
        AuthenticationFailure_UnknownUsername = 3,
        AuthenticationFailure_IncorrectPassword = 4,
        AuthenticationFailure_AccountDisabled = 5,
        AuthenticationFailure_AccountNotActivated = 6,
        AuthenticationFailure_FailureUnknown = 7,
        RegulatorNotAuthorizedForEDT = 8,
        MissingAuthorizationHeader = 9
    }

    public enum EDTEndpoint
    {
        DataDictionaryQuery = 1,
        ChemicalLibraryQuery = 2,
        ViolationLibraryQuery = 3,
        RegulatorFacilitySubmittalQuery = 4,
        RegulatorFacilityQuery = 5,
        RegulatorFacilityCreate = 6,
        RegulatorFacilitySubmittalActionNotification = 7,
        RegulatorCMEQuery = 8,
        RegulatorCMESubmit = 9,
        RegulatorFacilityMetadataSubmit = 10,
        RegulatorFacilityInformationSubmit = 11,
        RegulatorFacilitySubmittalSubmit = 12,
        RegulatorAuthenticationTest = 13,
        RegulatorFacilitySubmittalDocumentQuery = 14,
        RegulatorOrganizationQuery = 15,
        RegulatorActionItemQuery = 16,
        RegulatorFacilityTransferQuery = 17,
        EndpointMetadata = 18
    }

    public enum EDTEndpointCategory
    {
        RegulatorLibraryServices = 1,
        RegulatorQueryServices = 2,
        CUPACMESubmissionServices = 3,
        CUPARegulatorFacilitySubmittalReviewServices = 4,
        CUPAFacilityManagementServices = 5,
        CUPAFacilitySubmittalSubmissionServices = 6
    }

    public enum EDTEndpointStatus
    {
        Active = 1,
        Inactive = 2,
        InDevelopment = 3,
        InTesting = 4,
        Invisible = 5
    }

    public enum EDTExceptionType
    {
        Authentication,
        ContentType,
        RequestMethod
    }

    public enum EDTQueuePriority
    {
        Low = 1,
        Medium = 2,
        High = 3
    }

    public enum EDTQueueProcessStatus
    {
        Pending = 1,
        InProgress = 2,
        Processed = 3
    }

    public enum EDTTransactionMessageType
    {
        Error = 1,
        Warning = 2,
        Advisory = 3,
        Required = 4
    }

    public enum EDTTransactionStatus
    {
        Queued = 1,
        Accepted = 2,
        InProcess = 3,
        Rejected = 4,
        ErrorProcessing = 5
    }

    public enum EDTTransactionXmlDirection
    {
        Inbound = 1,
        Outbound = 2
    }

    public enum EmailDeliveryMode
    {
        Queued,
        Realtime
    }

    public enum EmailPriority
    {
        Low = 3,
        Normal = 2,
        High = 1
    }

    public enum EmailStatus
    {
        Queued = 1,
        InProgress = 2,
        Sent = 3,
        SendFailed = 4,
        ManualImmediate = 5
    }

    public enum EnhancementStatus
    {
        Proposed = 1,
        Scheduled = 2,
        Deferred = 3,
        Completed = 6,
        NotAccepted = 7,
        Approved = 8,
        CERS3Proposed = 9,
        CERS3Approved = 10,
        FutureCERSImplementation=14

    }

    public enum ErrorKeyValueQualifier
    {
        Form,
        QueryString,
        ServerVariable
    }

    public enum EventCategory
    {
        CERSAccountManagement = 1,
        OrganizationEvents = 2,
        RegulatorEvents = 3,
        Listserv = 4,
        OrganizationAccessRequests = 5,
        RegulatorAccessRequests = 6,
        General = 7
    }

    public enum EventDeliveryMechanism
    {
        UINotificationOnly = 1,
        UINotificationwithEmailNotification = 2,
        EmailNotificationOnly = 3
    }

    public enum EventParameterCode
    {
        FirstName,
        LastName,
        Email,
        TargetOrganizationID,
        TargetCERSID,
        Title,
        Phone
    }

    public enum EventPriority
    {
        Urgent = 1,
        Normal = 2,
        Low = 3,
    }

    public enum EventTypeCode
    {
        CERSAccountActivation = 1,
        CERSAccountPasswordReset = 2,
        CERSBusinessAccountInvitation = 3,
        CERSRegulatorAccountInvitation = 4,
        RegulatorUserAdded = 5,
        RegulatorUserAccessRequest = 6,
        OrganizationUserAdded = 7,
        OrganizationUserAccessRequestToLeadUser = 8,
        FacilityAddedNormal = 9,
        UserAuthorizedFacilityTransfer = 10,
        RegulatorAuthorizedFacilityTransfer = 11,
        FacilitySubmittalNotification = 12,
        FacilityInformationSubmittalAttentionNeeded = 13,
        OrganizationAdded = 14,
        FacilitySubmittalElementChangedtoNotApplicableByBusiness = 15,
        OrganizationUserAccessRequestAccepted = 16,
        OrganizationUserAccessRequestRejected = 17,
        RegulatorUserAccessRequestAccepted = 18,
        RegulatorUserAccessRequestRejected = 19,
        CERSAccountUsernameReminder = 20,
        OrganizationUserAccessRequestToRegulator = 21,
        OrganizationUserAccessRequestToHelpCenterMultijurisdictional = 22,
        NewOrganizationWithNewFacilityAdded = 23,
        FacilityDeleteRequest = 24,
        FacilityMergeRequest = 25,
        FacilityTransferRequest = 26,
        FacilityTransferredByRegulator = 27,
        FacilityDeletedByRegulator = 28,
        FacilitiesMergedByRegulator = 29,
        FacilityDeleteRequestAccepted = 30,
        FacilityDeleteRequestRejected = 31,
        FacilityMergeRequestAccepted = 32,
        FacilityMergeRequestRejected = 33,
        FacilityTransferRequestAccepted = 34,
        FacilityTransferRequestRejected = 35,
        FacilityRegulatorsChanged = 36,
        FacilitySubmittalElementChangedtoNotApplicableByRegulator = 37,
        OrganizationUserAccessRequestToHelpCenterNoBizRegUsersToServiceRequest = 38,
        HelpTicketToHelpCenter = 39,
        HelpTicketToRequestor = 40,
        SubmittalUnderReviewByRegulator = 41,
        SubmittalSetToNotApplicable = 42,
        SubmittalNotAcceptedByRegulator = 43,
        AcceptedSubmittalNowUnderReview = 44,
        PreviouslyAcceptedSubmittalNowNotAccepted = 45,
        PreviouslyNotAcceptedSubmittalNowAccepted = 46,
        PreviouslyNotApplicableSubmittalNowUnderReview = 47,
        PreviouslyNotApplicableSubmittalNowAccepted = 48,
        PreviouslyNotApplicableSubmittalNowNotAccepted = 49,
        SubmittalAcceptedByRegulator = 50,
        FacilityAddedToSecondaryCUPA = 51,
        BusinessUserChangedBusinessName = 52,
        BusinessUserChangedBusinessHeadquarters = 53,
        SubmittalCommentsModifiedByRegulator = 54,
        PreviouslyNotAcceptedSubmittalNowUnderReview = 55,
        NewEDTCreatedFacilityDuplicatesExistingFacilityAddress = 57,
        RemoteFacilityIndicatorChangedtoYesbyRegulator = 58,
        RemoteFacilityIndicatorChangedtoNobyRegulator = 59,
        FacilitySQGIndicatorChangedtoYesbyRegulator = 60,
        FacilitySQGIndicatorChangedtoNobyRegulator = 61,
        CERSAccountPasswordResetCompleted = 62,
        FacilityOwnerOperatorReportingNoRegulatedActivities = 63,
        UserPermissionChanged = 64,
		RegulatorEmergencyResponderUserAccessRequest = 65

    }

    public enum FacilityFieldToUpdate
    {
        RemoteFacility = 1,
        SQGFacility = 2,
        LocalFacilityGrouping = 3,
    }

    public enum FacilityOperationMessageType
    {
        Warning,
        Error
    }

    public enum FacilitySubmittalHistoryGroup
    {
        All = 0,
        Current = 1,
        Archived = 2,
    }

    public enum FileSizeCompareOperator
    {
        GreaterThan,
        Equal,
        LessThan
    }

    public enum GuidanceLevel
    {
        Required = 1,
        Warning = 2,
        Advisory = 3
    }

    public enum GuidanceMessageCode
    {
        MissingForm = 1001,
        RequiredFieldMissingValue = 1002,
        MissingDocument = 1003,
        MissingResource = 1004,
        DocumentSourceExemptRequiresComment = 1005,
        DocumentSourceProvidedInOtherSubmittalElementRequiresSubmittalElement = 1006,
        DocumentSourceProvidedToRegulatorRequiresDateProvidedToRegulator = 1007,
        DocumentSourcePublicInternetUrlRequiresLinkUrl = 1008,
        DocumentSourceStoredAtFacilityRequiresCERSID = 1009,
        DocumentSourceAttachmentRequiresDocument = 1010,
        MissingHWTankClosureMetadata = 1011,
        ValueOutsideAcceptableRange = 1012,
        EntityValidationError = 1013,
        ResourceEntityMinMaxRange = 1014,
        ResourceEntityMinimum = 1015,
        ResourceEntityMaximum = 1016,
        FieldValueDoesNotValidateAgainstRegularExpression = 1017,
        MinimallyRequiredFieldDoesNotHaveValue = 1018,
        EnforcementOccurredOnIsInvalid = 1019,
        MinimallyRequiredFieldInvalidValue = 1020,
        FacilityNameChange = 1021,
        FacilityAddressChange = 1022,
        FacilityEPAIDChange = 1023,
        HazardousWasteGeneratorWithoutEPAID = 1024,
        RCRALargeQuantityGeneratorWithoutEPAID = 1025,
        RemoteWasteConsolidationSiteWithoutEPAID = 1026,
        HMIValueOutsideAcceptableRange = 1027,
        HMIMinimallyRequiredFieldDoesNotHaveValue = 1028,
        HMIFieldValueDoesNotValidateAgainstRegularExpression = 1029,
        USTTankInfoFormMissingMonitoringPlan = 1030,
        USTConditionalDocumentRequirementUSTCertificationOfFinancialResponsibility = 1031,
        USTConditionalDocumentRequirementUSTOwnerOperatorWrittenAgreement = 1032,
        USTConditionalDocumentRequirementUSTLetterFromTheChiefFinancialOfficer = 1033,
        USTConditionalFormRequirementUSTCertificationsOfInstallationModification = 1034,
        MissingDocumentChoice = 1035,
        MissingTieredPermittingUnitFormOrNumberOfCECLUnits = 1036,
        HMIAverageDailyAmountCannotExceedMaxDailyAmount = 1037,
        NoWasteAndTreatmentCombinationValueProvided = 1038,
        MissingTieredPermittingPlotPlan = 1039,
        PriorEnforcementHistoryMayBeRequired = 1040,
        TankAndContainerCertificationsMayBeRequired = 1041,
        MissingNotificationOfLocalAgencies = 1042,
        NotificationOfPropertyOwnerMayBeRequired = 1043,
        EstimateOfClosureCostsDocumentMayBeRequired = 1044,
        FinancialAssuranceClosureMechanismDocumentMayBeRequired = 1045,
        FinancialAssuranceCertificationRequired = 1046,
        DuplicateTieredPermittingUnitIDFound = 1047,
        DuplicateUSTTankIDFound = 1048,
        EDTInvalidRegulatorCode = 1049,
        EDTInvalidViolationTypeNumber = 1050,
        EDTMatchingRecordNotFoundForRegulatorKey = 1051,
        EDTRegulatorActionDateTimeMustBeLater = 1052,
        RegulatorNotAuthorizedToSubmitData = 1053,
        InvalidCERSID = 1054,
        DateMustOccurAfterSpecifiedDate = 1055,
        DateMustNotBeInTheFuture = 1056,
        ViolationMustIncludeScheduledRTCOrActualRTCDate = 1057,
        ViolationScheduledRTCDateMustOccurOnOrAfterViolationDate = 1058,
        ViolationActualRTCDateMustOccurOnOrAfterViolationDate = 1059,
        ViolationActualRTCQualifierMustAccompanyActualRTCDate = 1060,
        ViolationActualRTCDateMustAccompanyActualRTCQualifier = 1061,
        ViolationsMustOccurOnOrBeforeLinkedEnforcementDate = 1062,
        ApprovedChildrenMustNotBeLinkedToDeletedParents = 1063,
        InspectionAndLinkedEnforcementCERSIDsMustMatch = 1064,
        PreviouslyDeletedRecordSetToApproved = 1065,
        XMLSchemaValidationError = 1066,
        RCRALQGInspectedFacilityMissingEPAID = 1067,
        UserNotAuthorizedToSubmitDataOnBehalfOfRegulator = 1068,
        EHSMaterialsShouldBeReportedInPounds = 1069,
        FacilityIDChange = 1070,
        HWTPFinancialAssuranceEstimateCost = 1071,
        InvalidBOE = 1072,
        DateMustBeBetweenDateRange = 1073,
        NextDueDateMustBeGreaterOrEqualToSubmittedOn = 1074,
        NextDueDateOutOfRange = 1075,
        USTCertificationsOfInstallationModificationProjectTypeMustBeSelected = 1076,
        FacilityEPAIDTooShort = 1077,
        InventoryContainsTradeSecretMaterials = 1078,
        EDTInvalidViolationTypeNumberForProgramElement = 1079,
        USTFinancialResponsibilityMechanismRequired = 1080
    }

    public enum GuidanceMessageScope
    {
        Entity = 1,
        Resource = 2,
        SubmittalElement = 3
    }

    public enum GuidanceMessageTemplatePriority
    {
        Low = 3,
        Normal = 2,
        High = 1
    }

    public enum HelpItemDisplayFormat
    {
        Link,
        Bullet
    }

    public enum HelpItemStatus
    {
        Active = 1,
        InActive = 2
    }

    public enum HMISReportType
    {
        Waste = 1,
        NonWaste = 2
    }

    public enum IconSize
    {
        /// <summary>
        /// 16
        /// </summary>
        Small = 16,

        /// <summary>
        /// 24
        /// </summary>
        Medium = 24,

        /// <summary>
        /// 32
        /// </summary>
        Large = 32,

        /// <summary>
        /// 48
        /// </summary>
        XLarge = 48,

        /// <summary>
        /// 64
        /// </summary>
        XXLarge = 64
    }

    public enum LastSubmittalElementDelta
    {
        None = 1,
        Changed = 10,
        New = 20
    }

    public enum NewCERSIDJustification
    {
        LocallyDiscoveredFacility = 1,
        SubstantialOperationalChangeAtFacility = 2,
        FacilitySubdivided = 3,
        LocalRegulatorNeeded = 4
    }

    public enum NotificationPriorityLevelIcon
    {
        Urgent,
        SeverelyUrgent,
        Normal,
        Low,
    }

    public enum OrganizationMetadataPart
    {
        All,
        Facility,
        Contact,
        LastSubmittalDate
    }

    public enum OrganizationOrigin
    {
        EDT = 1,
        Business = 2,
        CalEPA = 3,
        Letter = 4,
        Seeding = 5,
        CUPA = 6
    }

    public enum OrganizationStatus
    {
        Active = 1,
        InActive = 2
    }

    public enum OrganizationType
    {
        Normal = 1,
        InvactiveFacilityContainer = 2
    }

    public enum PermissionRole
    {
        SystemAdmin = 1,
        HelpAdmin = 2,
        SecurityAdmin = 3,
        UPViewer = 4,
        UPEditor = 5,
        StatewideViewerUnrestricted = 6,
        UPAViewer = 7,
        UPAApprover = 8,
        UPAInspector = 9,
        UPAAdmin = 10,
        OrgViewer = 11,
        OrgEditor = 12,
        OrgApprover = 13,
        OrgAdmin = 14,
        UPAOrgARNotification = 15,
        UPANotifications = 16,
        AccessRequestNotifications = 17,
        FacilityRequestNotifications = 18,
        FacilityNotifications = 19,
        HelpRequestNotifications = 20,
        UPADataUploader = 21,
        UPAEDTAdmin = 22,
        EDTFacilitySubmittalQuery = 23,
        ViolationLibraryAdmin = 24,
        ChemicalLibraryAdmin = 25,
        EDTFacilityCreate = 26,
        EDTFacilityQuery = 27,
        EDTFacilitySubmittalActionNotification = 28,
        EDTCMESubmittalSubmit = 29,
        EDTCMESubmittalQuery = 30,
        EDTFacilityMetadataSubmit = 31,
        EDTFacilityInformationSubmittalSubmit = 32,
        EDTLibraryServices = 33,
        EDTFacilitySubmittalSubmit = 34,
        EnhancementAdmin = 36,
        EDTOrganizationQuery = 37,
        EDTActionItemQuery = 38,
        EDTFacilityTransferQuery = 39,
        StatewideViewerRestricted = 40,
        DataRegistryHelpTextEditor = 41,
		UPAERAdmin = 42,
		UPAERViewer = 43
    }

    public enum PrintJobStatus
    {
        Queued = 1,
        InProgress = 2,
        Success = 3,
        Failed = 4,
        Hold = 5,
        Cancelled = 6
    }

    public enum Qualifier
    {
        RegulatorEvaluationDocuments = 1,
        RegulatorDocuments = 2,
        OrganizationDocuments = 3,
        FacilityDocuments = 4,
        FacilitySubmittalDocuments = 5
    }

    public enum RegulatorDocumentType
    {
        Evaluation_Initial = 1,
        Evaluation_Update = 2,
        Evaluation_Final_Closing = 3,
        Evaluation_Correspondence = 4,
        Evaluation_Initial_Final_Closing = 5,
        Evaluation_Final_Closing_with_Deficiencies = 6,
        Enforcement_Summary = 7
    }

    public enum RegulatorFacilityCreateMode
    {
        Commit,
        Trial
    }

    public enum RegulatorStatus
    {
        Active = 1,
        InActive = 2
    }

    public enum RegulatorType
    {
        CUPA = 1,
        PA = 2,
        UPSA = 3,
        UPOA = 4,
        FA = 5
    }

    public enum ReportingRequirement
    {
        NotApplicable = 1,
        Applicable = 2,
        AlwaysApplicable = 3
    }

    public enum ResetPasswordWizardStep
    {
        Verify = 1,
        Input = 2,
        Confirm = 3,
        InvalidTicket = 4
    }

    public enum ResourceType
    {
        BusinessActivities = 1,
        BusinessOwnerOperatorIdentification = 2,
        HazardousMaterialInventory = 3,

        /// <summary>
        /// This is obsolete, DO NOT USE.
        /// </summary>
        [Obsolete( "Do NOT Use ResourceType.GeneralSiteMap_Document" )]
        GeneralSiteMap_Document = 4,

        /// <summary>
        /// This one is the current SiteMap type...
        /// </summary>
        AnnotatedSiteMapOfficialUseOnly_Document = 5,

        EmergencyResponseContingencyPlan_Document = 6,
        EmployeeTrainingPlan_Document = 7,
        OnsiteHazardousWasteTreatmentNotificationFacility = 8,
        OnsiteHazardousWasteTreatmentNotificationUnit = 9,
        TieredPermittingUnitCertificationofFinancialAssurance = 10,
        RecyclableMaterialsActivities = 11,
        RecyclableMaterialsMaterial = 12,
        RemoteWasteConsolidationSiteAnnualNotification = 13,
        OnsiteHazardousWasteTreatmentPlotPlanMap_Document = 14,
        TieredPermittingUnitPriorEnforcementHistory_Document = 15,
        OnsiteHazardousWasteTreatmentWrittenEstimateofClosureCosts_Document = 16,
        OnsiteHazardousWasteTreatmentFinancialAssuranceClosureMechanism_Document = 17,
        TieredPermittingUnitTankandContainerCertification_Document = 18,
        TieredPermittingUnitNotificationofLocalAgencyorAgencies_Document = 19,
        TieredPermittingUnitNotificationofPropertyOwner_Document = 20,
        RecyclableMaterialsKnownMarket_Document = 21,
        HazardousWasteTankClosureCertificate_Document = 22,
        FacilityInformationLocallyRequired_Document = 23,
        USTOperatingPermitApplicationFacilityInformation = 24,
        USTOperatingPermitApplicationTankInformation = 25,
        USTCertificationofInstallationModification = 26,
        USTMonitoringPlan = 27,
        USTMonitoringSitePlan_Document = 28,
        USTCertificationofFinancialResponsibility_Document = 29,
        USTResponsePlan_Document = 30,
        USTOwnerandUSTOperatorWrittenAgreement_Document = 31,
        USTLetterfromtheChiefFinancialOfficer_Document = 32,
        OwnerStatementofDesignatedUSTOperatorCompliance_Document = 33,
        FacilityInformationLocalFields = 34,
        HazardousMaterialInventoryLocallyRequired_Document = 35,
        EmergencyResponseAndTrainingPlansLocallyRequired_Document = 36,
        USTLocallyRequired_Document = 37,
        OnsiteHazardousWasteTreatmentNotificationLocallyRequired_Document = 38,
        RecyclableMaterialsReportLocallyRequired_Document = 39,
        RemoteWasteConsolidationAnnualNotificationLocallyRequired_Document = 40,
        HazardousWasteTankClosureCertificationLocallyRequired_Document = 41,
        AbovegroundPetroleumStorageTanksLocallyRequired_Document = 42,
        CaliforniaAccidentalReleaseProgramLocallyRequired_Document = 43,
        Inspection = 44,
        Enforcement = 45,
        Violation = 46,
        RecyclableMaterialsOffsiteGeneratorIdentification = 47,
        RecyclableMaterialsReportDocumentation_Document = 48,
        AbovegroundPetroleumStorageActDocumentation_Document = 49,
        EnforcementViolation = 50,
        FacilityInformationMiscellaneousStateRequired_Document = 51,
        HMIMiscellaneousStateRequired_Document = 52,
        ERTPMiscellaneousStateRequired_Document = 53,
        USTMiscellaneousStateRequired_Document = 54,
        TieredPermittingMiscellaneousStateRequired_Document = 55,
        RecyclableMaterialsReportMiscellaneousStateRequired_Document = 56,
        RemoteWasteConsolidationMiscellaneousStateRequired_Document = 57,
        HazardousWasteTankClosureCertificationMiscellaneousStateRequired_Document = 58,
        APSAMiscellaneousStateRequired_Document = 59,
        CalARPMiscellaneousStateRequired_Document = 60
    }

    //public enum ResetPasswordWizardStep
    //{
    //	AnswerSecurityQuestions,
    //	ResetPassword
    //}
    public enum ServiceType
    {
        Address
    }

    public enum ShieldIcon
    {
        Green,
        Yellow,
        Red,
        Grey,
        Blue
    }

    public enum SignInWizardStep
    {
        EnterUserName,
        EnterPassword,
        SignInDisabled,
        NotActivated,
        AccountLockedOut,
        AccountDisabled
    }

    public enum StringSearchOption
    {
        StartsWith = 1,
        Contains = 2,
        EndsWith = 3,
        ExactMatch = 4,
    }

    // Status of Submittal Delta Results
    public enum SubmittalDeltaStatus
    {
        InitialSubmittal = 1,
        NoChanges = 2,
        HasChanges = 3,
        HasSignificantChanges = 4,
        HasVerySignificantChanges = 5,
        Pending = 6,
        NotCalculated = 7
    }

    public enum SubmittalElementStatus
    {
        Draft = 1,
        Submitted = 2,
        UnderReview = 3,
        Accepted = 4,
        NotAccepted = 5,
        NotApplicable = 6,
        NotSubmitted = 7
    }

    public enum SubmittalElementType
    {
        FacilityInformation = 1,
        HazardousMaterialsInventory = 2,
        EmergencyResponseandTrainingPlans = 3,
        UndergroundStorageTanks = 4,
        OnsiteHazardousWasteTreatmentNotification = 5,  //aka Tiered Permitting
        RecyclableMaterialsReport = 6,
        RemoteWasteConsolidationSiteAnnualNotification = 7,
        HazardousWasteTankClosureCertification = 8,
        AbovegroundPetroleumStorageTanks = 9,
        CaliforniaAccidentalReleaseProgram = 10,
    }

    public enum SubmittalType
    {
        UI = 1,
        EDTPassthrough = 2,
        EDTProxy = 3
    }

    public enum SurveyQuestionStatus
    {
        Active = 1,
        Archived = 2
    }

    public enum SurveyQuestionType
    {
        MultipleChoiceSingleNumericScale = 1,
        Narrative = 2
    }

    public enum SurveyResponseReviewStatus
    {
        Unreviewed = 1,
        ReviewedPendingResponse = 2,
        ReviewedResponseCompleted = 3,
        ReviewedDoNotRespond = 4
    }

    public enum SurveyStatus
    {
        Active = 1,
        Archived = 2
    }

    public enum SystemLookupTable
    {
        County,
        DocumentFormat,
        DocumentType,
        RegulatorDocumentType,
        DocumentStorageProvider,
        EmailStatus,
        EnforcementType,
        InspectionType,
        PermissionRole,
        RegulatorType,
        SubmittalElementResource,
        SubmittalElementStatus,
        GuidanceLevel,
        ResourceType,
        Context,
        Qualifier,
        ReportingRequirement,
        Prop65Type,
        DocumentSource,
        GeoCollectionMethod,
        GeoReferenceDatum,
        GeoReferencePoint,
        EventDeliveryMechanism,
        EventPriority,
        EventCategory,
        Country,
        OrganizationStatus,
        OrganizationOrigin,
        EventTypeAppliesTo,
        EventTypeEmailsTo,
        EventTypeStatus,
        EDTTransactionMessageType,
        CMEDataStatus,
        CMEProgramElement,
        HelpItemStatus,
        HelpItemType,
        HelpItemCategory,
        HelpPortalType,
        EDTEndpoint,
        EDTEndpointParameter,
        EDTEndpointStatus,
        EDTEndpointCategory,
        OrganizationType,
        EDTTransactionXmlDirection,
        EDTAuthenticationRequestResult,
        EDTTransactionStatus,
        SubmittalType,
        AddFacilityWizardResult,
        AddFacilityWizardStep,
        SurveyQuestionType,
        SurveyQuestionStatus,
        SurveyStatus,
        SurveyResponseReviewStatus,

        DeferredProcessingBatchType,
        DeferredProcessingItemStatus,
        RegulatorStatus,
        EmailPriority,
        SubmittalDeltaStatus,
        DocumentStatus,
        DataSubmittalDeltaSignificance,
        DataType,
        DataTypeGeneral,
        DataUIElementType,
        XmlSchemaVersionStatus,
        XmlSchema
    }

    public enum TableName
    {
        BPActivities,
        BPFacilityChemical,
        BPOwnerOperator,
        CERSFacilityGeoPoint,
        CERSFacilityGeoPointHistory,
        Chemical,
        ChemicalProp65Type,
        ChemicalSynonym,
        CMEBatch,
        CMEProgramElement,
        Contact,
        ContactOrganizationAccessRequest,
        ContactRegulatorAccessRequest,
        ContactRole,
        Document,
        EDTExternalTransactionData,
        EDTQueue,
        EDTTransaction,
        EDTTransactionActivityLog,
        EDTTransactionMessage,
        Email,
        EmailRecipient,
        Enforcement,
        EnforcementGuidance,
        EnforcementHistory,
        EnforcementViolation,
        EnforcementViolationHistory,
        Event,
        EventEmail,
        EventOrganizationContact,
        EventParameter,
        EventRegulatorContact,
        Facility,
        FacilityInformationLocalField,
        FacilityRegulatorSubmittalElement,
        FacilitySubmittal,
        FacilitySubmittalEDTTransaction,
        FacilitySubmittalElement,
        FacilitySubmittalElementDocument,
        FacilitySubmittalElementGuidance,
        FacilitySubmittalElementResource,
        FacilitySubmittalElementResourceDocument,
        FacilitySubmittalElementResourceGuidance,
        GuidanceMessageTemplate,
        HWTankClosure,
        HWTPFacility,
        HWTPFinancialAssurance,
        HWTPUnit,
        HWTPUnitCA,
        HWTPUnitCEL,
        HWTPUnitCESQT,
        HWTPUnitCESW,
        HWTPUnitPBR,
        Inspection,
        InspectionGuidance,
        InspectionHistory,
        LUTAccessRequestStatus,
        LUTCMEDataStatus,
        LUTContactRoleType,
        LUTContext,
        LUTDocumentFormat,
        LUTDocumentSource,
        LUTDocumentStorageProvider,
        LUTDocumentType,
        LUTEDTDataFlow,
        LUTEDTQueuePriority,
        LUTEDTQueueProcessStatus,
        LUTEDTTransactionMessageType,
        LUTEDTTransactionStatus,
        LUTEmailEntityType,
        LUTEmailStatus,
        LUTEmailType,
        LUTEventType,
        LUTGeoCollectionMethod,
        LUTGeoReferenceDatum,
        LUTGeoReferencePoint,
        LUTGuidanceLevel,
        LUTPermissionRole,
        LUTProp65Type,
        LUTQualifier,
        LUTRegulatorType,
        LUTReportingRequirement,
        LUTResourceType,
        LUTSubmittalElementStatus,
        MigrationIssue,
        NewsItem,
        Organization,
        OrganizationContact,
        OrganizationContactPermissionGroup,
        OrganizationContactRole,
        OrganizationDocument,
        PermissionGroup,
        PermissionGroupRole,
        Place,
        PlaceZipCode,
        ProgramElement,
        Regulator,
        RegulatorContact,
        RegulatorContactPermissionGroup,
        RegulatorContactRole,
        RegulatorDocument,
        RegulatorEDTTransaction,
        RegulatorRelationship,
        RegulatorSubmittalElementLocal,
        RegulatorSubmittalElementLocalInfoLink,
        RegulatorZipCodeSubmittalElement,
        RMActivitiesReport,
        RMRecyclableMaterial,
        RWConsolidationSite,
        Setting,
        SubmittalElement,
        SubmittalElementTemplate,
        SubmittalElementTemplateResource,
        sysdiagrams,
        USTFacilityInfo,
        USTInstallModCert,
        USTMonitoringPlan,
        USTTankInfo,
        Violation,
        ViolationCategory,
        ViolationCitation,
        ViolationGuidance,
        ViolationHistory,
        ViolationProgramElement,
        ViolationSource,
        ViolationType,
        XmlSchema,
        ZipCode,
        RMOffsiteGeneratorIdentification
    }

    public enum TextMacro
    {
        FirstName,
        LastName,
        Email,
        EmailLink,
        ExpirationDate,
        ActivationLink,
        CancelLink,
        FacilitySubmittalLink,
        Username,
        Content,
        BusinessPortalLink,
        Subject,
        AdditionalInformation,
        InviterFirstName,
        InviterLastName,
        RegulatorName,
        RegulatorCode,
        OrganizationName,
        OrganizationCode,
        OrganizationID,
        FacilityName,
        FacilityAddress,
        CERSID,
        PasswordResetLink,
        RequestDate,
        SignInLink,
        SubmittalElementName,
        BaseUrl,
        NotificationTitle,
        RegisterLink,
        RecipientFirstName,
        RecipientLastName,
        RecipientEmail,
        RecipientUsername,
        SubmittalDate,
        SubmittalElements,
        ActedUponByName,
        ActedUponOn,
        EventForwarderUrl,
        TargetOrgName,
        TargetFacilityName,
        TargetCERSID,
        Street,
        TargetStreet,
        City,
        TargetCity,
        ZipCode,
        TargetZipCode,
        TransferDate,
        PhoneNumber,
        Comments,
        HelpTicketCode,
        AccountID,
        UserAgent,
        HelpRequestCurrentUrl,
        EnvironmentName,
        PortalName
    }

    public enum TicketStatus
    {
        NotPresent,
        PresentNotFound,
        Active,
        Completed,
        Expired
    }

    public enum TieredPermittingUnitType
    {
        CESQT,
        CESW,
        CA,
        PBR,
        CEL,
    }

    public enum UIElementType
    {
        DropDownList = 1,
        RadioButtonList = 2,
        CheckBoxList = 3,
        CheckBox = 4,
        TextBox = 5,
        TextArea = 6,
        Label = 7,
        RichTextEditor = 8,
        DateTimePicker = 9,
        DatePicker = 10,
        TimePicker = 11,
        NumericTextBox = 12
    }

    public enum URLTarget
    {
        blank,
        self,
        parent,
        top
    }

    public enum USTReport
    {
        USTReport6SummaryByRegulator = 1,
        USTInspectionSummaryByRegulator = 2,
        USTSemiAnnualReport = 3,
        USTStatewideLeakPreventionReport = 4,
        USTFacilityOwnerTypeSummaryByRegulator = 5,
        USTFacilityFinancialResponsibilitySummaryByRegulator = 6,
    }

    public enum ValidationPattern
    {
        YesNo
    }

    public enum ViewPortMode
    {
        Full,
        Windowed
    }

    public enum ViolationProgramElementType
    {
        BusinessPlanProgram = 1,
        USTProgram = 2,
        HazardousWasteGenerator = 3,
        RCRALargeQuantityGenerator = 4,
        TieredPermittingProgram = 5,
        APSAProgram = 6,
        CalARPProgram = 7,
		HouseholdHazardousWaste = 8
    }

    public enum WashSource
    {
        Melissa = 1
    }

    public enum XmlSchema
    {
        RegulatorFacilitySubmittal = 1,
        RegulatorFacilitySubmittalQuery = 2,
        RegulatorFacilitySubmittalResponse = 3,
        RegulatorFacilitySubmittalActionNotification = 4,
        RegulatorCMESubmit = 5,
        RegulatorCMESubmitResponse = 6,
        DictionaryDataQuery = 7,
        ChemicalLibraryQuery = 8,
        ViolationLibraryQuery = 9,
        RegulatorCMEQuery = 10,
        RegulatorFacilityCreate = 11,
        RegulatorFacilityQuery = 12,
        RegulatorFacilityCreateResponse = 13,
        RegulatorFacilityMetadata = 14,
        RegulatorFacilityInformationSubmittal = 15,
        RegulatorFacilityMetadataResponse = 16,
        RegulatorOrganizationQuery = 17,
        RegulatorActionItemQuery = 18,
        RegulatorFacilityTransferQuery = 19,
        EndpointMetadata = 20
    }

    public enum XmlSchemaVersionStatus
    {
        Active = 1,
        InActive = 2,
        Deprecated = 3
    }

    public enum ZipCodeSearchType
    {
        RegulatorZipCodeSubmittalElements,
        ZipCode
    }

    /* Significance for differences when comparing submittals */
}