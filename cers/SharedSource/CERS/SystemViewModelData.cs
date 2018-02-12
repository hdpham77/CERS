using CERS.Model;
using CERS.Security;
using CERS.ViewModels;
using CERS.ViewModels.AccountManagement;
using CERS.ViewModels.Contacts;
using CERS.ViewModels.Facilities;
using CERS.ViewModels.Infrastructure.Security;
using CERS.ViewModels.Organizations;
using CERS.ViewModels.Regulators;
using CERS.ViewModels.SubmittalElements;
using CERS.ViewModels.SubmittalElementTemplates;
using CERS.ViewModels.Violations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using UPF;
using UPF.Core;
using UPF.Linq;

namespace CERS
{
	public class SystemViewModelData
	{
		#region Private Properties

		private int _EvaluationYear = 2003;
		private int _UpdateNumber = 15; //TODO: This value should come from a configuration File.
		//TODO: This value should come from a configuration File.

		#endregion Private Properties

		#region Public Properties

		public ICERSRepositoryManager Repository { get; protected set; }

		public ICERSSystemServiceManager Services { get; protected set; }

		#endregion Public Properties

		#region Construction

		public SystemViewModelData( ICERSRepositoryManager repository, ICERSSystemServiceManager services )
		{
			Repository = repository;
			Services = services;
		}

		#endregion Construction

		#region Organization

		public virtual OrganizationContactViewModel BuildUpOrganizationContactViewModel( int organizationID, int? contactID = null )
		{
			OrganizationContactViewModel result = new OrganizationContactViewModel() { Entity = new OrganizationContact() };
			result.Organization = Repository.Organizations.GetByID( organizationID );

			if ( contactID == null )
			{
				result.Entities = Repository.OrganizationContacts.Search( organizationID );
			}
			else
			{
				result.Entity = Repository.OrganizationContacts.GetByID( contactID.Value );
			}

			return result;
		}

		public virtual OrganizationDocumentViewModel BuildUpOrganizationDocumentViewModel( int organizationID, int? documentID = null, bool loadDictionaryData = false, Context? context = null, Qualifier? qualifier = null )
		{
			OrganizationDocumentViewModel result = new OrganizationDocumentViewModel() { Entity = new OrganizationDocument() };
			result.Organization = Repository.Organizations.GetByID( organizationID );

			if ( documentID == null )
			{
				result.Entities = Repository.OrganizationDocuments.GetByOrganization( organizationID );
			}
			else
			{
				result.Entity = Repository.OrganizationDocuments.GetByID( documentID.Value );
			}

			if ( loadDictionaryData )
			{
				result.Formats = Repository.SystemLookupTables.GetValues( SystemLookupTable.DocumentFormat );
				result.Types = Repository.SystemLookupTables.GetDocumentTypes( context, qualifier );

				//get all the regulators.
				result.Regulators = Services.Security.GetRegulators().ToList();
			}

			return result;
		}

		public virtual OrganizationViewModel BuildUpOrganizationViewModel( int? organizationID = null, string name = null, int? edtIdentityKey = null )
		{
			IEnumerable<ISystemLookupEntity> origins = Repository.SystemLookupTables.GetValues( SystemLookupTable.OrganizationOrigin );
			var viewModel = new OrganizationViewModel() { Entity = new Organization(), OrganizationStatuses = Repository.SystemLookupTables.GetValues( SystemLookupTable.OrganizationStatus ), Origins = origins };

			if ( organizationID == null )
			{
				viewModel.Entities = Repository.Organizations.Search( name, edtIdentityKey );
				viewModel.Name = name;
				if ( !string.IsNullOrWhiteSpace( name ) )
				{
					viewModel.Searched = true;
				}
			}
			else
			{
				viewModel.Entity = Repository.Organizations.GetByID( organizationID.Value );
			}

			return viewModel;
		}

		public virtual OrganizationViewModel BuildUpOrganizationViewModel( OrganizationViewModel viewModel )
		{
			return BuildUpOrganizationViewModel( name: viewModel.Name );
		}

		#endregion Organization

		#region Facility

		public virtual FacilityViewModel BuildUpFacilityViewModel( int? cersID = null, IEnumerable<Facility> facilities = null )
		{
			var viewModel = new FacilityViewModel() { Entity = new Facility() };
			if ( cersID.HasValue )
			{
				viewModel.Entity = Repository.Facilities.GetByID( cersID.Value );
			}

			if ( facilities != null )
			{
				viewModel.Entities = facilities;
			}
			return viewModel;
		}

		#endregion Facility

		#region Regulator

		public virtual RegulatorDocumentViewModel BuildRegulatorDocumentViewModel( int regulatorID, bool loadDictionaryData = false, int? regulatorDocumentID = null )
		{
			RegulatorDocumentViewModel viewModel = new RegulatorDocumentViewModel()
			{
				Entity = ( regulatorDocumentID.HasValue ) ? Repository.RegulatorDocuments.GetByID( regulatorDocumentID.Value ) : new RegulatorDocument(),
				Regulator = Repository.Regulators.GetByID( regulatorID ),
			};

			if ( regulatorDocumentID == null )
			{
				viewModel.Entity.IsPublic = true;  //this is the default value for new regulator documents
				viewModel.Entities = Repository.RegulatorDocuments.GetByRegulator( regulatorID );
			}
			if ( loadDictionaryData )
			{
				viewModel.RegulatorDocumentTypes = Repository.SystemLookupTables.GetValues( SystemLookupTable.RegulatorDocumentType );
				viewModel.Formats = Repository.SystemLookupTables.GetValues( SystemLookupTable.DocumentFormat );
				viewModel.EvaluationYears = GetEvaluationYears();
				viewModel.UpdateNumbers = GetUpdateNumbers();
			}

			return viewModel;
		}

		public virtual RegulatorDocumentViewModel BuildRegulatorDocumentViewModel( bool loadDictionaryData = true )
		{
			RegulatorDocumentViewModel viewModel = new RegulatorDocumentViewModel();

			if ( loadDictionaryData )
			{
				viewModel.RegulatorDocumentTypes = Repository.SystemLookupTables.GetValues( SystemLookupTable.RegulatorDocumentType );
				viewModel.Formats = Repository.SystemLookupTables.GetValues( SystemLookupTable.DocumentFormat );
				viewModel.EvaluationYears = GetEvaluationYears();
				viewModel.RegulatorTypes = Repository.SystemLookupTables.GetValues( SystemLookupTable.RegulatorType );
				viewModel.UpdateNumbers = GetUpdateNumbers();
			}

			return viewModel;
		}

		public virtual RegulatorLocalViewModel BuildRegulatorLocalViewModel( int regulatorID, int? regulatorLocalID = null, bool? loadInfoLink = true )
		{
			RegulatorLocalViewModel viewModel = new RegulatorLocalViewModel()
			{
				Entity = ( regulatorLocalID.HasValue ) ? Repository.RegulatorLocals.GetByID( regulatorLocalID.Value ) : new RegulatorSubmittalElementLocalRequirement(),
				Regulator = Repository.Regulators.GetByID( regulatorID ),
				SubmittalElements = Repository.SubmittalElements.GetAll()
			};

			return viewModel;
		}

		public virtual RegulatorViewModel BuildRegulatorViewModel( int? regulatorId = null, bool loadDictionaryData = false, RegulatorViewModel viewModel = null )
		{
			if ( viewModel == null )
			{
				viewModel = new RegulatorViewModel();
			}

			viewModel.Entity = ( regulatorId.HasValue ) ? Repository.Regulators.GetByID( regulatorId.Value ) : new Regulator();

			if ( loadDictionaryData )
			{
				viewModel.RegulatorTypes = Repository.SystemLookupTables.RegulatorTypes;
				viewModel.Counties = Repository.SystemLookupTables.GetValues( SystemLookupTable.County );
			}

			return viewModel;
		}

		public virtual RegulatorZipSubmittalViewModel BuildRegulatorZipSubmittalViewModel( int regulatorID, bool loadDictionaryData = false, int? submittalElementID = null, int? regulatorSubmittalZipCodeID = null )
		{
			Regulator regulator = Repository.Regulators.GetByID( regulatorID );
			RegulatorZipCodeSubmittalElement regZipSub = new RegulatorZipCodeSubmittalElement();

			if ( submittalElementID.HasValue )
			{
				regZipSub = Repository.RegulatorZipCodeSubmittalElements.Search( regulatorID, submittalElementID.Value ).First();
			}

			RegulatorZipSubmittalViewModel viewModel = new RegulatorZipSubmittalViewModel()
			{
				Entity = regZipSub,
				Regulator = regulator,
				RegulatorID = regulatorID,
				CountyID = regulator.CountyID.Value,
				SubmittalElementID = ( submittalElementID.HasValue ) ? submittalElementID.Value : 0
			};

			if ( loadDictionaryData )
			{
				viewModel.ZipCodes = Repository.ZipCodes.Search( countyID: regulator.CountyID );
				viewModel.Counties = Repository.SystemLookupTables.GetValues( SystemLookupTable.County );
				viewModel.SubmittalElements = Repository.SubmittalElements.GetAll().ToList();
				viewModel.AssociatedZipCodes = Repository.Regulators.GetZipCodes( regulatorID, submittalElementID );
			}

			return viewModel;
		}

		#endregion Regulator

		#region Permissions

		public virtual PermissionGroupViewModel BuildUpPermissionGroupViewModel( int? pgID = null, string name = null, bool getRolesById = false, bool getPermissionGroups = false, bool getPermissionRoles = false, bool getContexts = false, bool getRegulatorTypes = false, bool getOwners = false, bool includeGroups = true )
		{
			PermissionGroupViewModel pgViewModel = new PermissionGroupViewModel() { Entity = new PermissionGroup() };
			if ( ( pgID == null || getOwners ) && includeGroups )
			{
				pgViewModel.Entities = Repository.PermissionGroups.Search( name );
			}

			if ( pgID != null )
			{
				pgViewModel.Entity = Repository.PermissionGroups.GetByID( pgID.Value );
			}

			if ( getRegulatorTypes )
			{
				pgViewModel.RegulatorTypes = Repository.SystemLookupTables.RegulatorTypes;
			}

			if ( getContexts )
			{
				pgViewModel.Contexts = Repository.SystemLookupTables.GetValues( SystemLookupTable.Context );
			}

			if ( getRolesById == true )
			{
				pgViewModel.AssociatedRoles = Repository.PermissionGroupRoles.GetRolesByPermissionGroupID( pgID.Value ).ToGridView();
			}

			if ( getPermissionGroups == true )
			{
				pgViewModel.PermissionGroups = Repository.PermissionGroups.Search();
			}

			if ( getPermissionRoles == true )
			{
				pgViewModel.PermissionRoles = Repository.SystemLookupTables.GetValues( SystemLookupTable.PermissionRole ).OfType<LUTPermissionRole>();
			}
			return pgViewModel;
		}

		public virtual PermissionGroupViewModel BuildUpPermissionGroupViewModel( PermissionGroupViewModel pgViewModel )
		{
			return this.BuildUpPermissionGroupViewModel( pgViewModel.Entity.ID, pgViewModel.Entity.Name );
		}

		#endregion Permissions

		#region Submittal Element Templates

		public virtual SubmittalElementTemplateViewModel BuildUpSubmittalElementTemplateViewModel( int? id = null, string name = null )
		{
			SubmittalElementTemplateViewModel setVM = new SubmittalElementTemplateViewModel() { Entity = new SubmittalElementTemplate() };
			if ( id == null || id.Value == 0 )
			{
				setVM.Entities = Repository.SubmittalElementTemplates.Search( name: name );
			}
			else
			{
				setVM.Entity = Repository.SubmittalElementTemplates.GetByID( id.Value );
			}
			return setVM;
		}

		#region Submittal Element Template Resources

		public virtual SubmittalElementTemplateResourceViewModel BuildUpSubmittalElementTemplateResourceViewModel( int? id = null, int submittalElementTemplateID = 0, bool getSubmittalElements = false, bool getParentResources = false, bool getResourceTypes = false )
		{
			SubmittalElementTemplateResourceViewModel setrVM = new SubmittalElementTemplateResourceViewModel();
			if ( id == null || id == 0 )
			{
				setrVM.Entities = Repository.SubmittalElementTemplateResources.Search();
			}
			else
			{
				setrVM.Entity = Repository.SubmittalElementTemplateResources.GetByID( id.Value );
			}

			if ( submittalElementTemplateID != 0 )
			{
				setrVM.SubmittalElementTemplate = Repository.SubmittalElementTemplates.GetByID( submittalElementTemplateID );
			}

			if ( getSubmittalElements == true )
			{
				setrVM.SubmittalElements = Repository.SubmittalElements.GetAll();
			}

			if ( getParentResources == true )
			{
				setrVM.ParentResources = Repository.SubmittalElementTemplateResources.Search();
			}

			if ( getResourceTypes == true )
			{
				setrVM.ResourceTypes = Repository.SystemLookupTables.GetValues( SystemLookupTable.ResourceType );
			}

			return setrVM;
		}

		#endregion Submittal Element Template Resources

		#endregion Submittal Element Templates

		#region Build Up DeferredJobViewModel Method

		public virtual DeferredJobViewModel BuildUpDeferredJobDocumentRetrievalViewModel( int deferredJobID )
		{
			var deferredJob = Repository.DeferredJobs.GetByID( deferredJobID );
			DeferredJobViewModel viewModel = new DeferredJobViewModel()
			{
				Entity = deferredJob,
			};

			if ( deferredJob != null )
			{
				XElement xmlElements = XElement.Parse( deferredJob.Parameters );
				if ( (DeferredJobType)viewModel.Entity.DeferredJobTypeID == DeferredJobType.RegulatorHMIDownload )
				{
					int regulatorID;
					int.TryParse( xmlElements.Element( "RegulatorID" ).Value, out regulatorID );
					viewModel.RegulatorID = regulatorID;
				}
				else if ( (DeferredJobType)viewModel.Entity.DeferredJobTypeID == DeferredJobType.OrganizationHMIDownload )
				{
					int organizationID;
					int.TryParse( xmlElements.Element( "OrganizationID" ).Value, out organizationID );
					viewModel.OrganizationID = organizationID;
				}

				var account = Repository.Contacts.GetByAccount( deferredJob.CreatedByID );
				viewModel.RequesterName = account != null ? account.FullName : string.Empty;
			}

			return viewModel;
		}

		#endregion Build Up DeferredJobViewModel Method

		#region Build Up PrintJobViewModel Method

		public virtual PrintJobViewModel BuildUpPrintJobDocumentRetrievalViewModel( int printJobID )
		{
			var printJob = Repository.PrintJobs.GetByID( printJobID );
			PrintJobViewModel viewModel = new PrintJobViewModel()
			{
				Entity = printJob,
			};
			if ( printJob != null )
			{
				var facilitySubmittal = Repository.FacilitySubmittals.GetByID( printJob.FacilitySubmittalID.Value );
				var account = Repository.Contacts.GetByAccount( printJob.CreatedByID );

				viewModel.RequesterName = account != null ? account.FullName : string.Empty;
				viewModel.FacilitySubmittalSubmittedDate = facilitySubmittal != null ? facilitySubmittal.SubmittedOn : DateTime.Now;
			}

			return viewModel;
		}

		#endregion Build Up PrintJobViewModel Method

		#region BuildUpSubmittalStartViewModel Method

		public virtual SubmittalStartViewModel BuildUpSubmittalStartViewModel( int? cersID = null )
		{
			var viewModel = new SubmittalStartViewModel();
			var currentSubmittalElements = Repository.FacilityRegulatorSubmittalElements.GetCurrentFacilitySubmittalElements( cersID.Value );
			viewModel.CurrentSubmittalElementViewModelCollection = currentSubmittalElements.GetCurrentFacilitySubmittalElementViewModels().ToList();

			if ( cersID.HasValue )
			{
				viewModel.Facility = Repository.Facilities.GetByID( cersID.Value );
				viewModel.Regulators = Repository.FacilityRegulatorSubmittalElements.Search( CERSID: cersID.Value ).Select( s => s.Regulator ).Distinct();
			}

			viewModel.ReadyToSubmitCollection = viewModel.CurrentSubmittalElementViewModelCollection.Where( c => c.CurrentSubmittalElement.SEStatusID == (int)SubmittalElementStatus.Draft ).Where( c => c.CurrentSubmittalElement.IsReadyToSubmit() ).ToList();
			viewModel.ReviewNeededCollection = viewModel.CurrentSubmittalElementViewModelCollection.Where( c => c.CurrentSubmittalElement.SEStatusID == (int)SubmittalElementStatus.Draft ).Where( c => !c.CurrentSubmittalElement.IsReadyToSubmit() ).ToList();

			return viewModel;
		}

		#endregion BuildUpSubmittalStartViewModel Method

		#region BuildUpSubmittalFinishedViewModel

		public virtual SubmittalFinishedViewModel BuildUpSubmittalFinishedViewModel( int CERSID, int FSID )
		{
			var viewModel = new SubmittalFinishedViewModel();
			viewModel.Facility = Repository.Facilities.GetByID( CERSID );
			viewModel.Organization = viewModel.Facility.Organization;
			viewModel.Entity = Repository.FacilitySubmittals.GetByID( FSID );
			viewModel.FacilitySubmittalElements = Repository.FacilitySubmittalElements.Search( facilitySubmittalID: FSID ).ToList();

			return viewModel;
		}

		#endregion BuildUpSubmittalFinishedViewModel

		#region BuildUpFacilitySubmittalElementViewModel Method

		public virtual CERS.ViewModels.Facilities.FacilitySubmittalElementViewModel BuildUpFacilitySubmittalElementViewModel( int CERSID, int FSEID )
		{
			return new CERS.ViewModels.Facilities.FacilitySubmittalElementViewModel()
			{
				Entity = Repository.FacilitySubmittalElements.GetByID( FSEID ),
				CERSID = CERSID
			};
		}

		#endregion BuildUpFacilitySubmittalElementViewModel Method

		#region BuildUpFacilitySubmittalViewModel Method

		public virtual CERS.ViewModels.Facilities.FacilitySubmittalViewModel BuildUpFacilitySubmittalViewModel( int FSID, int? organizationId = null )
		{
			var fses = Repository.FacilitySubmittalElements.Search( facilitySubmittalID: FSID );

			var fiFSE = fses.SingleOrDefault( p => p.SubmittalElementID == (int)SubmittalElementType.FacilityInformation && !p.Voided );
			Regulator regulator = null;

			if ( fiFSE != null )
			{
				regulator = fiFSE.OwningRegulator;
			}

			var viewModel = new FacilitySubmittalViewModel()
			{
				FacilitySubmittalElements = fses,
				Entity = Repository.FacilitySubmittals.GetByID( FSID ),
				Regulator = regulator
			};

			if ( organizationId.HasValue )
			{
				viewModel.Organization = Repository.Organizations.GetByID( organizationId.Value );
			}
			return viewModel;
		}

		#endregion BuildUpFacilitySubmittalViewModel Method

		#region Violation Library

		public virtual ViolationCitationViewModel BuildUpViolationCitationViewModel( int? violationCitationID = null, bool loadDictionaryData = false )
		{
			ViolationCitationViewModel viewModel = new ViolationCitationViewModel()
			{
				Entity = ( violationCitationID.HasValue ) ? Repository.ViolationCitations.GetByID( violationCitationID.Value ) : new ViolationCitation()
			};

			if ( loadDictionaryData )
			{
				// Populate Lists used for building Drop-Down Search Menus:
				viewModel.ViolationSources = Repository.ViolationSources.Search();
			}

			return viewModel;
		}

		public virtual ViolationTypeViewModel BuildUpViolationTypeViewModel( int? violationTypeID = null, bool loadDictionaryData = false )
		{
			ViolationTypeViewModel viewModel = new ViolationTypeViewModel()
			{
				Entity = ( violationTypeID.HasValue ) ? Repository.ViolationTypes.GetByID( violationTypeID.Value ) : new ViolationType()
			};

			if ( loadDictionaryData )
			{
				// Populate Lists used for building Drop-Down Search Menus:
				viewModel.ViolationProgramElements = Repository.ViolationProgramElements.Search().OrderBy( vpe => vpe.Name );
				viewModel.ViolationCategories = Repository.ViolationCategories.Search().OrderBy( vc => vc.Name );
				viewModel.ViolationSources = Repository.ViolationSources.Search().OrderBy( vs => vs.Name );
			}

			return viewModel;
		}

		#endregion Violation Library

		#region GetIDList

		public static List<int> GetIDList( params EventTypeCode[] items )
		{
			List<int> results = new List<int>();
			if ( items != null )
			{
				foreach ( var item in items )
				{
					results.Add( (int)item );
				}
			}
			return results;
		}

		#endregion GetIDList

		#region BuildAccountProfileActionViewModel Method

		public TModel BuildAccountProfileActionViewModel<TModel>( TModel viewModel = null, int? accountID = null ) where TModel : AccountProfileActionViewModel, new()
		{
			if ( viewModel == null )
			{
				viewModel = new TModel();
			}

			if ( accountID != null )
			{
				var account = Repository.CoreData.Accounts.GetByID( accountID.Value );
				if ( account != null )
				{
					var contact = Repository.Contacts.EnsureExists( account );
					if ( contact != null )
					{
						viewModel.FullName = account.DisplayName;
						viewModel.FirstName = contact.FirstName;
						viewModel.LastName = contact.LastName;
						viewModel.Email = contact.Email;
						viewModel.ConfirmEmail = contact.Email;
						viewModel.PasswordProtectionPhrase = account.PasswordProtectionPhrase;
						viewModel.Username = account.UserName;
						viewModel.AccountID = accountID.Value;
					}
				}
			}

			return viewModel;
		}

		#endregion BuildAccountProfileActionViewModel Method

		#region BuildAccountRegisterViewModel Method

		public AccountRegisterViewModel BuildAccountRegisterViewModel( AccountRegisterViewModel viewModel = null )
		{
			viewModel = BuildAccountProfileActionViewModel( viewModel );

			return viewModel;
		}

		#endregion BuildAccountRegisterViewModel Method

		#region BuildMyAccountViewModel Method

		public MyAccountViewModel BuildMyAccountViewModel( int accountID, MyAccountViewModel viewModel = null )
		{
			if ( viewModel == null )
			{
				viewModel = new MyAccountViewModel();
			}

			var coreRepo = Repository.CoreData;

			viewModel.Account = coreRepo.Accounts.GetByID( accountID );
			viewModel.Contact = Repository.Contacts.EnsureExists( viewModel.Account );

			viewModel.SignInHistory = Repository.Contacts.GetSignInHistory( viewModel.Contact, success: true );

			var sqa = viewModel.Account.AccountSecurityQuestionAnswers.SingleOrDefault( p => !p.Voided );
			if ( sqa != null )
			{
				viewModel.SecurityQuestionID = sqa.QuestionID;
				viewModel.SecurityQuestion = sqa.SecurityQuestion.Name;
				viewModel.SecurityQuestionAnswer = sqa.Answer;
			}

			//var

			//lets add all the stuff we need.
			viewModel.SecurityQuestions.AddRange( coreRepo.SecurityQuestions.Search( allowSelection: true ) );

			viewModel.ContactRelationships = Repository.Contacts.GetContactRelationships( viewModel.Contact );

			return viewModel;
		}

		#endregion BuildMyAccountViewModel Method

		#region BuildOrganizationContactsGridView

		public virtual IEnumerable<EntityContactGridView> BuildOrganizationContactGridView( Organization organization, int? permissionGroupID = null )
		{
			organization.CheckNull( "org" );
			List<EntityContactGridView> result = new List<EntityContactGridView>();

			var currentPortalInfo = Services.CoreServices.Portals.Current;

			//get all contacts for this Org.
			var dataModel = Repository.DataModel;
			var coreDataModel = Repository.CoreData.DataModel;

			//get core organization contact data.
			var orgContacts = ( from oc in dataModel.OrganizationContacts
								join c in dataModel.Contacts on oc.ContactID equals c.ID
								where !oc.Voided && !c.Voided && oc.OrganizationID == organization.ID
								select new EntityContactGridView
								{
									ContactID = oc.ContactID,
									EntityContactID = oc.ID,
									EntityID = oc.OrganizationID,
									AccountID = c.AccountID,
									FirstName = c.FirstName,
									LastName = c.LastName,
									Email = c.Email,
									Phone = oc.Phone,
									Title = oc.Title,
									FullName = c.FullName
								} ).ToList();

			//get a distinct list of Contact Primary Keys (ID's)
			var cIDList = orgContacts.Select( p => p.ContactID ).Distinct().ToList();

			//now we need to look at sign in history.
			var signInHistory = ( from cs in dataModel.ContactStatistics
								  where cIDList.Contains( cs.ContactID )
								  select new
								  {
									  cs.ContactID,
									  cs.LastSignIn,
									  cs.PreviousSignIn,
									  cs.SystemPortalID,
									  cs.UALastShown
								  } ).ToList();

			//get a distinct list of OrganizationContact Primary Keys (ID's)
			var ocIDList = orgContacts.Select( p => p.EntityContactID ).Distinct().ToList();

			//gets grab all the permissions available for all these users. Note that OrganizationContacts
			//may or may *NOT* have permissions...so this list may be short.
			var permissions = ( from pg in dataModel.OrganizationContactPermissionGroups
								join g in dataModel.PermissionGroups on pg.GroupID equals g.ID
								where !pg.Voided && !g.Voided && ocIDList.Contains( pg.OrganizationContactID )
								select new
								{
									EntityContactID = pg.OrganizationContactID,
									GroupID = g.ID,
									GroupName = g.DisplayName
								} ).ToList();

			//grab a list of all the NON null accounts.
			var accountIDList = orgContacts.Where( p => p.AccountID != null ).Select( p => p.AccountID.Value ).ToList();

			//get the account data we need.
			var accounts = ( from a in coreDataModel.Accounts
							 where !a.Voided && accountIDList.Contains( a.ID )
							 select new
							 {
								 a.ID,
								 a.UserName,
								 a.LockedOut,
								 a.ServiceAccount,
								 a.Disabled,
								 a.Approved
							 }
							).ToList();

			//in some cases we are dealing with events that are linked by ContactID (Invitations), and in some cases we are dealing with events
			//that are linked via AccountID (Activations)

			//gets get "invitiation" events for all these people
			var invitationTypeIDs = GetIDList( EventTypeCode.CERSBusinessAccountInvitation, EventTypeCode.CERSRegulatorAccountInvitation );
			var invitationEvents = ( from e in dataModel.Events
									 where e.ContactID != null &&
									 invitationTypeIDs.Contains( e.TypeID ) &&
									 cIDList.Contains( e.ContactID.Value ) &&
									 !e.Voided &&
									 !e.Completed
									 select e ).ToList();

			//get all activation events for all these people.
			int activationEventTypeID = (int)EventTypeCode.CERSAccountActivation;
			var activationEvents = ( from e in dataModel.Events
									 where e.AccountID != null &&
									 e.TypeID == activationEventTypeID &&
									 accountIDList.Contains( e.AccountID.Value ) &&
									 !e.Voided &&
									 !e.Completed
									 select e ).ToList();

			//bring all the events together.
			var events = invitationEvents.Union( activationEvents ).ToList();

			//lets bring in all the other data.
			var merged = ( from oc in orgContacts
						   select oc.Set( c =>
						   {
							   //set up security groups if we have them.
							   var groups = ( from p in permissions where p.EntityContactID == oc.EntityContactID select p ).ToList();
							   oc.Groups = ( from p in groups select p.GroupName ).ToList();
							   oc.GroupIDs = ( from p in groups select p.GroupID ).ToList();

							   //format the phone number.
							   oc.Phone = oc.Phone.FormatPhoneNumber();

							   //bring in the account if we have it.
							   if ( oc.AccountID != null )
							   {
								   var account = accounts.SingleOrDefault( p => p.ID == oc.AccountID.Value );
								   if ( account != null )
								   {
									   c.Activated = account.Approved;
									   c.LockedOut = account.LockedOut;
									   c.Disabled = account.Disabled;
									   c.Username = account.UserName;
									   c.ServiceAccount = account.ServiceAccount;
								   }
								   if ( !c.Activated )
								   {
									   //lets see if we have an activation event gathered from above.
									   c.PendingEvent = events.SingleOrDefault( p => p.AccountID == c.AccountID && p.TypeID == (int)EventTypeCode.CERSAccountActivation );
								   }
							   }
							   else
							   {
								   //lets see if we have an invitiation still hanging around.
								   c.PendingEvent = events.SingleOrDefault( p => p.ContactID == oc.ContactID && ( p.TypeID == (int)EventTypeCode.CERSBusinessAccountInvitation || p.TypeID == (int)EventTypeCode.CERSRegulatorAccountInvitation ) && !p.Completed && !p.Abandoned );
							   }

							   var siHistory = signInHistory.Where( p => p.ContactID == oc.ContactID );
							   if ( siHistory.Count() > 0 )
							   {
								   var item = siHistory.OrderByDescending( p => p.LastSignIn ).FirstOrDefault();
								   if ( item != null )
								   {
									   oc.LastSignIn = item.LastSignIn;
								   }

								   //var portal = siHistory.SingleOrDefault(p => p.SystemPortalID == currentPortalInfo.ID);
								   //if (portal != null)
								   //{
								   //    oc.LastSignIn = portal.LastSignIn;
								   //}
							   }
						   } ) ).ToList();

			if ( permissionGroupID != null )
			{
				merged = ( from m in merged where m.GroupIDs.Contains( permissionGroupID.Value ) select m ).ToList();
			}

			result.AddRange( merged );

			return result;
		}

		#endregion BuildOrganizationContactsGridView

		#region BuildRegulatorContactGridView Method

		public virtual IEnumerable<EntityContactGridView> BuildRegulatorContactGridView( Regulator regulator, int? permissionGroupID = null )
		{
			regulator.CheckNull( "regulator" );
			List<EntityContactGridView> result = new List<EntityContactGridView>();

			var currentPortalInfo = Services.CoreServices.Portals.Current;

			//get all contacts for this Org.
			var dataModel = Repository.DataModel;
			var coreDataModel = Repository.CoreData.DataModel;

			//get core organization contact data.
			var regContacts = ( from oc in dataModel.RegulatorContacts
								join c in dataModel.Contacts on oc.ContactID equals c.ID
								where !oc.Voided && !c.Voided && oc.RegulatorID == regulator.ID
								select new EntityContactGridView
								{
									ContactID = oc.ContactID,
									EntityContactID = oc.ID,
									EntityID = oc.RegulatorID,
									AccountID = c.AccountID,
									FirstName = c.FirstName,
									LastName = c.LastName,
									Email = c.Email,
									Phone = oc.Phone,
									Title = oc.Title,
									FullName = c.FullName
								} ).ToList();

			//get a distinct list of Contact Primary Keys (ID's)
			var cIDList = regContacts.Select( p => p.ContactID ).Distinct().ToList();

			//now we need to look at sign in history.
			var signInHistory = ( from cs in dataModel.ContactStatistics
								  where cIDList.Contains( cs.ContactID )
								  select new
								  {
									  cs.ContactID,
									  cs.LastSignIn,
									  cs.PreviousSignIn,
									  cs.SystemPortalID,
									  cs.UALastShown
								  } ).ToList();

			//get a distinct list of OrganizationContact Primary Keys (ID's)
			var ocIDList = regContacts.Select( p => p.EntityContactID ).Distinct().ToList();

			//gets grab all the permissions available for all these users. Note that OrganizationContacts
			//may or may *NOT* have permissions...so this list may be short.
			var permissions = ( from pg in dataModel.RegulatorContactPermissionGroups
								join g in dataModel.PermissionGroups on pg.GroupID equals g.ID
								where !pg.Voided && !g.Voided && ocIDList.Contains( pg.RegulatorContactID ) && !g.EDT
								select new
								{
									EntityContactID = pg.RegulatorContactID,
									GroupID = g.ID,
									GroupName = g.DisplayName
								} ).ToList();

			//grab a list of all the NON null accounts.
			var accountIDList = regContacts.Where( p => p.AccountID != null ).Select( p => p.AccountID.Value ).ToList();

			//get the account data we need.
			var accounts = ( from a in coreDataModel.Accounts
							 where !a.Voided && accountIDList.Contains( a.ID )
							 select new
							 {
								 a.ID,
								 a.UserName,
								 a.LockedOut,
								 a.ServiceAccount,
								 a.Disabled,
								 a.Approved
							 }
							).ToList();

			//in some cases we are dealing with events that are linked by ContactID (Invitations), and in some cases we are dealing with events
			//that are linked via AccountID (Activations)

			//gets get "invitiation" events for all these people
			var invitationTypeIDs = GetIDList( EventTypeCode.CERSBusinessAccountInvitation, EventTypeCode.CERSRegulatorAccountInvitation );
			var invitationEvents = ( from e in dataModel.Events
									 where e.ContactID != null &&
									 invitationTypeIDs.Contains( e.TypeID ) &&
									 cIDList.Contains( e.ContactID.Value ) &&
									 !e.Voided &&
									 !e.Completed
									 select e ).ToList();

			//get all activation events for all these people.
			int activationEventTypeID = (int)EventTypeCode.CERSAccountActivation;
			var activationEvents = ( from e in dataModel.Events
									 where e.AccountID != null &&
									 e.TypeID == activationEventTypeID &&
									 accountIDList.Contains( e.AccountID.Value ) &&
									 !e.Voided &&
									 !e.Completed
									 select e ).ToList();

			//bring all the events together.
			var events = invitationEvents.Union( activationEvents ).ToList();

			//lets bring in all the other data.
			var merged = ( from oc in regContacts
						   select oc.Set( c =>
						   {
							   //set up security groups if we have them.
							   var groups = ( from p in permissions where p.EntityContactID == oc.EntityContactID select p ).ToList();
							   oc.Groups = ( from p in groups select p.GroupName ).ToList();
							   oc.GroupIDs = ( from p in groups select p.GroupID ).ToList();

							   //format the phone number.
							   oc.Phone = oc.Phone.FormatPhoneNumber();

							   //bring in the account if we have it.
							   if ( oc.AccountID != null )
							   {
								   var account = accounts.SingleOrDefault( p => p.ID == oc.AccountID.Value );
								   if ( account != null )
								   {
									   c.Activated = account.Approved;
									   c.LockedOut = account.LockedOut;
									   c.Disabled = account.Disabled;
									   c.Username = account.UserName;
									   c.ServiceAccount = account.ServiceAccount;
								   }

								   if ( !c.Activated )
								   {
									   //lets see if we have an activation event gathered from above.
									   c.PendingEvent = events.SingleOrDefault( p => p.AccountID == c.AccountID && p.TypeID == (int)EventTypeCode.CERSAccountActivation );
								   }
							   }
							   else
							   {
								   //lets see if we have an invitiation still hanging around.
								   c.PendingEvent = events.SingleOrDefault( p => p.ContactID == oc.ContactID && ( p.TypeID == (int)EventTypeCode.CERSBusinessAccountInvitation || p.TypeID == (int)EventTypeCode.CERSRegulatorAccountInvitation ) && !p.Completed && !p.Abandoned );
							   }

							   var siHistory = signInHistory.Where( p => p.ContactID == oc.ContactID );
							   if ( siHistory.Count() > 0 )
							   {
								   var item = siHistory.OrderByDescending( p => p.LastSignIn ).FirstOrDefault();
								   if ( item != null )
								   {
									   oc.LastSignIn = item.LastSignIn;
								   }

								   //var portal = siHistory.SingleOrDefault(p => p.SystemPortalID == currentPortalInfo.ID);
								   //if (portal != null)
								   //{
								   //    oc.LastSignIn = portal.LastSignIn;
								   //}
							   }
						   } ) ).ToList();

			if ( permissionGroupID != null )
			{
				merged = ( from m in merged where m.GroupIDs.Contains( permissionGroupID.Value ) select m ).ToList();
			}

			result.AddRange( merged );

			return result;
		}

		#endregion BuildRegulatorContactGridView Method

		#region BuildEntityContactGridViewModel Method(s)

		public virtual EntityContactGridViewModel BuildEntityContactGridViewModel( Organization organization, EntityContactGridViewModel viewModel = null )
		{
			if ( viewModel == null )
			{
				viewModel = new EntityContactGridViewModel();
			}
			viewModel.EntityID = organization.ID;
			viewModel.Context = Context.Organization;
			viewModel.GridView = BuildOrganizationContactGridView( organization, viewModel.PermissionGroupID ).OrderBy( p => p.LastName );
			viewModel.PermissionGroups = Services.Security.GetGroupsForOrganization( organization );
			return viewModel;
		}

		public virtual EntityContactGridViewModel BuildEntityContactGridViewModel( Regulator regulator, EntityContactGridViewModel viewModel = null )
		{
			if ( viewModel == null )
			{
				viewModel = new EntityContactGridViewModel();
			}
			viewModel.EntityID = regulator.ID;
			viewModel.Context = Context.Regulator;
			viewModel.GridView = BuildRegulatorContactGridView( regulator, viewModel.PermissionGroupID ).OrderBy( p => p.LastName );
			viewModel.PermissionGroups = Services.Security.GetGroupsForRegulator( regulator );
			return viewModel;
		}

		#endregion BuildEntityContactGridViewModel Method(s)

		#region BuildRegulatorContactViewModel Method

		public virtual EntityContactViewModel<Regulator, RegulatorContact> BuildRegulatorContactViewModel( int regulatorID, int? regulatorContactID = null, EntityContactViewModel<Regulator, RegulatorContact> viewModel = null )
		{
			if ( viewModel == null )
			{
				viewModel = new EntityContactViewModel<Regulator, RegulatorContact>();
			}
			viewModel.Entity = Repository.Regulators.GetByID( regulatorID );

			//load available permissions based on what regulator.
			viewModel.AvailablePermissionGroups = Services.Security.GetGroupsForRegulator( viewModel.Entity, edt: false );

			if ( regulatorContactID == null )
			{
				if ( !string.IsNullOrWhiteSpace( viewModel.Email ) )
				{
					//lets find a Contact with this email.
					viewModel.Contact = Repository.Contacts.GetByEmail( viewModel.Email );
					if ( viewModel.Contact != null )
					{
						//try and find an existing regulator account for this.
						viewModel.EntityContact = viewModel.Contact.RegulatorContacts.SingleOrDefault( p => p.RegulatorID == regulatorID && !p.Voided );

						//build up new entity contact if none already.
						if ( viewModel.EntityContact == null )
						{
							viewModel.EntityContact = new RegulatorContact();
						}

						//if we have an account associated with this contacts, let bring it in too.
						if ( viewModel.Contact.AccountID.HasValue )
						{
							viewModel.Account = Repository.CoreData.Accounts.GetByID( viewModel.Contact.AccountID.Value );
							viewModel.ContactStatistic = Repository.ContactStatistics.GetMostRecent( viewModel.Contact );
						}
					}
					else
					{
						//this is a brand new contact looks like.
						viewModel.Contact = new Contact();
						viewModel.Contact.Email = viewModel.Email;
						viewModel.EntityContact = new RegulatorContact();
					}
				}
				else
				{
					//we never got an email so lets start at step 1.
					viewModel.ViewPrefix = "AddPerson";
					viewModel.WizardStep = AddEntityContactWizardStep.EnterEmail;
				}
			}
			else
			{
				//we are on an entity contact .
				viewModel.EntityContact = Repository.RegulatorContacts.GetByID( regulatorContactID.Value );
				if ( viewModel.EntityContact != null )
				{
					//make sure the entity contact and the enity belong to each other.
					if ( viewModel.Entity.ID != viewModel.EntityContact.RegulatorID )
					{
						throw new Exception( "The RegulatorContact specified does not belong to the Regulator!" );
					}

					//gets get the contact of this regulator contact.
					viewModel.Contact = viewModel.EntityContact.Contact;

					//if we have an account, lets bring it in.
					if ( viewModel.Contact.AccountID.HasValue )
					{
						viewModel.Account = Repository.CoreData.Accounts.GetByID( viewModel.Contact.AccountID.Value );
						viewModel.ContactStatistic = Repository.ContactStatistics.GetMostRecent( viewModel.Contact );
					}
					else
					{
						//no account...lets figure out whats going on with this persons security.
						viewModel.InvitationEvent = Services.Events.GetExistingInvitation( viewModel.Contact );
						var pms = viewModel.EntityContact.RegulatorContactPermissionGroups.Where( p => !p.Voided && !p.PermissionGroup.EDT ).Select( p => p.PermissionGroup ).OrderBy( p => p.SortOrder ).ThenBy( p => p.DisplayName );
						viewModel.ActualPermissionGroups = pms.Select( p => p.ID ).ToList();
					}
				}
			}

			//do standard post setup.
			SetCommonData( viewModel, Context.Regulator );
			return viewModel;
		}

		#endregion BuildRegulatorContactViewModel Method

		#region BuildOrganizationContactViewModel Method

		public virtual EntityContactViewModel<Organization, OrganizationContact> BuildOrganizationContactViewModel( int organizationID, int? organizationContactID = null, EntityContactViewModel<Organization, OrganizationContact> viewModel = null )
		{
			if ( viewModel == null )
			{
				viewModel = new EntityContactViewModel<Organization, OrganizationContact>();
			}
			viewModel.Entity = Repository.Organizations.GetByID( organizationID );

			//load available permissions based on what regulator.
			viewModel.AvailablePermissionGroups = Services.Security.GetGroupsForOrganization( viewModel.Entity );

			if ( organizationContactID == null )
			{
				if ( !string.IsNullOrWhiteSpace( viewModel.Email ) )
				{
					//clean up email.
					string email = viewModel.Email.Trim().ToLower();
					viewModel.Email = email;

					//lets find a Contact with this email.
					viewModel.Contact = Repository.Contacts.GetByEmail( email );
					if ( viewModel.Contact != null )
					{
						//try and find an existing organization account for this.
						viewModel.EntityContact = viewModel.Contact.OrganizationContacts.SingleOrDefault( p => p.OrganizationID == organizationID && !p.Voided );

						//build up new entity contact if none already.
						if ( viewModel.EntityContact == null )
						{
							viewModel.EntityContact = new OrganizationContact();
						}

						//if we have an account associated with this contacts, let bring it in too.
						if ( viewModel.Contact.AccountID.HasValue )
						{
							viewModel.Account = Repository.CoreData.Accounts.GetByID( viewModel.Contact.AccountID.Value );
							viewModel.ContactStatistic = Repository.ContactStatistics.GetMostRecent( viewModel.Contact );
						}
					}
					else
					{
						//this is a brand new contact looks like.
						viewModel.Contact = new Contact();
						viewModel.Contact.Email = viewModel.Email;
						viewModel.EntityContact = new OrganizationContact();
					}
				}
				else
				{
					//we never got an email so lets start at step 1.
					viewModel.ViewPrefix = "AddPerson";
					viewModel.WizardStep = AddEntityContactWizardStep.EnterEmail;
				}
			}
			else
			{
				//we are on an entity contact .
				viewModel.EntityContact = Repository.OrganizationContacts.GetByID( organizationContactID.Value );
				if ( viewModel.EntityContact != null )
				{
					//make sure the entity contact and the enity belong to each other.
					if ( viewModel.Entity.ID != viewModel.EntityContact.OrganizationID )
					{
						throw new Exception( "The OrganizationContact specified does not belong to the Organization!" );
					}

					//gets get the contact of this regulator contact.
					viewModel.Contact = viewModel.EntityContact.Contact;

					//if we have an account, lets bring it in.
					if ( viewModel.Contact.AccountID.HasValue )
					{
						viewModel.Account = Repository.CoreData.Accounts.GetByID( viewModel.Contact.AccountID.Value );
						viewModel.ContactStatistic = Repository.ContactStatistics.GetMostRecent( viewModel.Contact );
					}
					else
					{
						//no account...lets figure out whats going on with this persons security.
						viewModel.InvitationEvent = Services.Events.GetExistingInvitation( viewModel.Contact );
						var pms = viewModel.EntityContact.OrganizationContactPermissionGroups.Where( p => !p.Voided && !p.PermissionGroup.EDT ).Select( p => p.PermissionGroup ).OrderBy( p => p.SortOrder ).ThenBy( p => p.DisplayName );
						viewModel.ActualPermissionGroups = pms.Select( p => p.ID ).ToList();
					}
				}
			}

			SetCommonData( viewModel, Context.Organization );

			return viewModel;
		}

		#endregion BuildOrganizationContactViewModel Method

		#region Helpers

		protected Dictionary<string, string> GetEvaluationYears()
		{
			Dictionary<string, string> results = new Dictionary<string, string>();
			for ( int i = DateTime.Now.Year; i >= _EvaluationYear; i-- )
			{
				results.Add( i.ToString(), i.ToString() );
			}

			return results;
		}

		protected Dictionary<string, string> GetUpdateNumbers()
		{
			Dictionary<string, string> results = new Dictionary<string, string>();
			for ( int i = 1; i <= _UpdateNumber; i++ )
			{
				results.Add( i.ToString(), i.ToString() );
			}
			return results;
		}

		#region SetCommonData (EntityContactViewModel<TEntity, TEntityContact)

		private void SetCommonData<TEntity, TEntityContact>( EntityContactViewModel<TEntity, TEntityContact> viewModel, Context context = Context.Regulator )
			where TEntity : IEntityWithContacts<TEntityContact>
			where TEntityContact : class, IEntityContact
		{
			if ( viewModel.ContactID != null )
			{
				viewModel.ContactRelationships = Repository.Contacts.GetContactRelationships( viewModel.Contact );

				//make sure and remove the current entity so we don't get tripped up.
				viewModel.ContactRelationships.Remove( viewModel.Entity.ID, context );
				viewModel.IsContactShared = viewModel.ContactRelationships.Count > 0;
			}

			if ( viewModel.Account != null )
			{
				PermissionGroupMatrixCollection accountGroupMatrices = null;
				if ( context == Context.Regulator )
				{
					accountGroupMatrices = Services.Security.GetRegulatorGroupsForAccount( viewModel.Account );
				}
				else
				{
					accountGroupMatrices = Services.Security.GetOrganizationGroupsForAccount( viewModel.Account );
				}

				var matrices = accountGroupMatrices;
				viewModel.ActualPermissionGroups = matrices.GetGroupIDList( viewModel.Entity.ID, context );

				if ( viewModel.Contact != null )
				{
					//get the sign-in history for this account.

					viewModel.SignInHistory = Repository.Contacts.GetSignInHistory( viewModel.Contact, success: true );//.ToSingleContactGridView();
				}
			}

			viewModel.EasyEditNameEmailEnabled = !( viewModel.IsContactShared || viewModel.Account != null );

			viewModel.PortalType = Services.CoreServices.Portals.Current.Type;
			viewModel.Context = context;
		}

		#endregion SetCommonData (EntityContactViewModel<TEntity, TEntityContact)

		#endregion Helpers
	}
}