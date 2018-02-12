using CERS.Compositions;
using CERS.Model;
using CERS.ViewModels;
using CERS.ViewModels.Chemicals;
using CERS.ViewModels.Contacts;
using CERS.ViewModels.Enforcements;
using CERS.ViewModels.Enhancements;
using CERS.ViewModels.Facilities;
using CERS.ViewModels.GuidanceMessages;
using CERS.ViewModels.Infrastructure.Security;
using CERS.ViewModels.Inspections;
using CERS.ViewModels.Organizations;
using CERS.ViewModels.Regulators;
using CERS.ViewModels.SubmittalElements;
using CERS.ViewModels.SubmittalElementTemplates;
using CERS.ViewModels.SystemAdmin;
using CERS.ViewModels.UndergroundStorageTanks;
using CERS.ViewModels.Violations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UPF;
using UPF.Core.Model;

namespace CERS
{
	public static class GridViewTransformationExtensionMethods
	{
		public static string FormatRegulatorTypes(this PermissionGroup permGroup)
		{
			string result = "Not Applicable";
			string temp = permGroup.RegulatorTypes.Where( p => !p.Voided ).Select( p => p.LUTRegulatorType.TypeCode ).ToDelimitedString( ", " );
			if ( !string.IsNullOrWhiteSpace( temp ) )
			{
				result = temp;
			}
			return result;
		}

		public static IEnumerable<EventTypeGridViewModel> ToGridView(this IEnumerable<EventType> eventTypes)
		{
			var results = from et in eventTypes
						  select new EventTypeGridViewModel
						  {
							  ID = et.ID,
							  Name = et.Name,
							  Description = et.Description,
							  ActionRequired = et.ActionRequired,
							  DefaultDeliveryMechanism = ( et.DefaultDeliveryMechanism != null ? et.DefaultDeliveryMechanism.Name : "N/A" ),
							  DefaultEmailTemplate = ( et.DefaultEmailTemplate != null ? et.DefaultEmailTemplate.Name : "N/A" ),
							  Expires = et.Expires,
							  Enabled = et.Enabled,
							  DefaultExpirationWindowInDays = et.DefaultExpirationWindowInDays,
							  DefaultOrganizationPriority = ( et.DefaultOrganizationPriority != null ? et.DefaultOrganizationPriority.Name : "N/A" ),
							  DefaultRegulatorPriority = ( et.DefaultRegulatorPriority != null ? et.DefaultRegulatorPriority.Name : "N/A" ),
							  NotificationMessageTemplate = ( et.DefaultNotificationTemplate != null ? et.DefaultNotificationTemplate.Title : "N/A" )
						  };

			return results;
		}

		public static IEnumerable<EventGridViewModel> ToGridView(this IEnumerable<EventItem> eventItems, string baseImageContentUrl = "")
		{
			var results = from ei in eventItems
						  select new EventGridViewModel
						  {
							  BaseImageContentUrl = baseImageContentUrl,
							  EventID = ei.ID,
							  EventType = ei.EventType,
							  Message = ei.Message,
							  OccurredOn = ei.OccurredOn,
							  Priority = ei.Priority,
							  PriorityName = ei.PriorityName,
							  CanProcess = ei.CanProcess,
							  ActionRequest = ei.ActionRequest,
							  Completed = ei.Completed,
							  ContactEmail = ei.ContactEmail,
							  ContactName = ei.ContactName,
							  TargetEntityName = ei.TargetEntityName,
							  TargetEntityID = ei.TargetEntityID,
							  Context = ei.Context,
							  TicketCode = ei.TicketCode
						  };

			return results;
		}

		public static IEnumerable<EventGridViewModel> ToGridView(this IEnumerable<Event> notifications)
		{
			var results = from e in notifications
						  select new EventGridViewModel
						  {
							  EventID = e.ID,
						  };
			return results;
		}

		public static IEnumerable<RoleGridViewModel> ToGridView(this IEnumerable<LUTPermissionRole> roles)
		{
			var results = from r in roles
						  select new RoleGridViewModel
						  {
							  ID = r.ID,
							  Name = r.Name,
							  Description = r.Description
						  };
			return results;
		}

		#region CME Grid View Methods

		public static IEnumerable<EnforcementGridViewModel> ToGridView(this IEnumerable<Enforcement> enforcements)
		{
			var results = ( from e in enforcements
							select new EnforcementGridViewModel
							{
								CERSID = e.CERSID,
								FacilityName = e.Facility.Name,
								ID = e.ID,
								RegulatorID = e.RegulatorID,
								RegulatorNameShort = e.Regulator.NameShort,
								CMEDataStatus = e.Status.Name,
								OccurredOn = e.OccurredOn,
								Type = e.Type,
								FormalType = e.FormalType,
                                ViolationCount = e.EnforcementViolations.Where( ev => !ev.Voided && !ev.Violation.Voided && !ev.Violation.Inspection.Voided ).Count(),
                                Comment = e.Comment ?? string.Empty
							} ).ToList();

			//translate properties with DataRegistryMetadata attribute and codes in CDR into descriptions instead of values.
			results.TranslateDataRegistryProperties();
			return results;
		}

		public static IEnumerable<InspectionGridViewModel> ToGridView(this IEnumerable<Inspection> inspections)
		{
			var results = ( from i in inspections
							select new InspectionGridViewModel
							{
								CERSID = i.CERSID,
								FacilityName = i.Facility.Name,
								ID = i.ID,
								RegulatorID = i.RegulatorID,
								RegulatorNameShort = i.Regulator.NameShort,
								CMEDataStatus = i.Status.Name,
								CMEProgramElement = i.CMEProgramElement.Acronym,
								OccurredOn = i.OccurredOn,
								Type = i.Type,
								ViolationCount = i.ClassIViolationCount + i.ClassIIViolationCount + i.MinorViolationCount,
								ViolationsRTCOn = i.ViolationsRTCOn
							} ).ToList();

			//expand field values to use Data Dictionary values.
			results.TranslateDataRegistryProperties();

			return results;
		}

		public static IEnumerable<ViolationGridViewModel> ToGridView(this IEnumerable<Violation> violations)
		{
			var results = ( from v in violations
							select new ViolationGridViewModel
							{
								ID = v.ID,
								InspectionID = v.InspectionID,
								InspectionOccurredOn = v.Inspection.OccurredOn,
								CERSID = v.Inspection.CERSID,
								RegulatorID = v.RegulatorID,
								RegulatorNameShort = v.Regulator.NameShort,
								FacilityName = v.Inspection.Facility.Name,
								OccurredOn = v.OccurredOn,
								Class = v.Class,
                                CMEProgramElement = v.Inspection.CMEProgramElement.Acronym,
								Program = v.ViolationType.ViolationCategory.ViolationProgramElement.ProgramElement.Acronym,
								ViolationTypeNumber = v.ViolationType.ViolationTypeNumber,
								ViolationType = v.ViolationType.Name,
								ScheduledRTCOn = v.ScheduledRTCOn,
								ActualRTCOn = v.ActualRTCOn,
								ActualRTCQualifier = v.ActualRTCQualifier,
								Comment = v.Comment,
								ViolationTypeProgramElement = v.ViolationType.ViolationCategory.ViolationProgramElement.Name,
								ViolationTypeCategory = v.ViolationType.ViolationCategory.Name,
								ViolationTypeName = v.ViolationType.Name,
								ViolationTypeDescription = v.ViolationType.Description,
								ViolationTypeBeginDate = v.ViolationType.BeginDate,
								ViolationTypeEndDate = v.ViolationType.EndDate,
								ViolationTypeComments = v.ViolationType.Comments
							} ).ToList();

			//expand field values to use Data Dictionary values.
			results.TranslateDataRegistryProperties();

			return results;
		}

		public static IEnumerable<ViolationTypeGridViewModel> ToGridView(this IEnumerable<ViolationType> violationTypes)
		{
			var results = ( from vt in violationTypes
							select new ViolationTypeGridViewModel
							{
								ID = vt.ID,
								ViolationTypeNumber = vt.ViolationTypeNumber,
								ViolationProgram = vt.ViolationCategory.ViolationProgramElement.Name,
								ViolationCategory = vt.ViolationCategory.Name,
								Name = vt.Name,
                                BeginDate = vt.BeginDate,
                                EndDate = vt.EndDate,

								// Truncate to 200 characters for grid display (some descriptions are longer than 5,000 characters)
								Description = ( vt.Description != null && vt.Description.Length > 200 ) ? vt.Description.Substring( 0, 200 ) + "..." : vt.Description
							} ).ToList();

			//translate properties with DataRegistryMetadata attribute and codes in CDR into descriptions instead of values.
			results.TranslateDataRegistryProperties();
			return results;
		}

		public static IEnumerable<ViolationCitationGridViewModel> ToGridView(this IEnumerable<ViolationCitation> violationCitations)
		{
			var results = ( from vc in violationCitations
							select new ViolationCitationGridViewModel
							{
								ID = vc.ID,
								ViolationTypeID = vc.ViolationTypeID,
								ViolationSource = vc.ViolationSource.Name,
								Chapter = vc.Chapter,
								Section = vc.Section,
								Name = vc.Name,
								Description = vc.Description
							} ).ToList();

			//translate properties with DataRegistryMetadata attribute and codes in CDR into descriptions instead of values.
			results.TranslateDataRegistryProperties();
			return results;
		}

		#endregion CME Grid View Methods

		public static IEnumerable<RegulatorGridViewModel> ToGridView(this IEnumerable<Regulator> regulators)
		{
			var results = from r in regulators
						  select new RegulatorGridViewModel
						  {
							  ID = r.ID,
							  Name = r.Name,
							  NameShort = r.NameShort,
							  NameAbbreviation = r.NameAbbreviation,
							  Street = r.Address1,
							  City = r.City,
							  State = r.State,
							  ZipCode = r.ZipCode,
							  AllowsSubmissions = ( ( r.AllowSubmission ) ? "Yes" : "No" ),
							  AllowFacilityRequests = ( ( r.AllowFacilityRequest ? "Yes" : "No" ) ),
							  Fax = r.Fax.FormatPhoneNumber(),
							  Phone = r.Phone.FormatPhoneNumber(),
							  Type = r.Type.TypeCode,
							  CountyName = ( ( r.CountyID != null ) ? r.County.Name : "" ),
							  PublicContactEmail = r.PublicContactEmail,
							  CERSNotificationEmail = r.CERSNotificationEmail,
							  PublicContactUrl = r.PublicContactUrl,
							  WebSite = r.WebSite
						  };

			return results;
		}

		public static IEnumerable<OrganizationDocumentGridViewModel> ToGridView(this IEnumerable<OrganizationDocument> organizationDocuments)
		{
			var results = from od in organizationDocuments
						  select new OrganizationDocumentGridViewModel
						  {
							  ID = od.ID,
							  DocumentID = od.DocumentID,
							  FileName = od.FileName,
							  FileSize = od.FileSize,
							  Format = od.Format.Name,
							  Name = od.Name,
							  OrganizationID = od.OrganizationID,
							  Type = od.Type.Name,
							  DocumentDate = od.DocumentDate,
						  };

			return results;
		}

		public static IEnumerable<RegulatorContactGridViewModel> ToGridView(this IEnumerable<RegulatorContact> regulatorContacts)
		{
			var results = from rc in regulatorContacts
						  select new RegulatorContactGridViewModel
						  {
							  ContactID = rc.Contact.ID,
							  RegulatorID = rc.Regulator.ID,
							  RegulatorContactID = rc.ID,
							  FirstName = rc.Contact.FirstName,
							  LastName = rc.Contact.LastName,
							  FullName = rc.Contact.FullName,
							  Email = rc.Contact.Email,
							  Phone = rc.Phone.FormatPhoneNumber(),
							  Title = rc.Title,
						  };

			return results;
		}

		public static IEnumerable<RegulatorDocumentGridViewModel> ToGridView(this IEnumerable<RegulatorDocument> regulatorDocuments)
		{
			var results = from rd in regulatorDocuments
						  select new RegulatorDocumentGridViewModel
						  {
							  ID = rd.ID,
							  RegulatorID = rd.RegulatorID,
							  RegulatorName = rd.Regulator.Name,
							  FileName = rd.FileName,
							  FormatID = rd.FormatID,
							  FormatName = rd.Format.Name,
							  TypeID = rd.TypeID,
							  TypeName = rd.TypeID.ToString(),
							  DocumentID = rd.DocumentID,
							  Name = rd.Name,
							  Description = rd.Description,
							  CreatedOn = rd.CreatedOn
						  };

			return results;
		}

		public static IEnumerable<OrganizationContactGridViewModel> ToGridView(this IEnumerable<OrganizationContact> organizationContacts)
		{
			var results = from oc in organizationContacts
						  select new OrganizationContactGridViewModel
						  {
							  OrganizationID = oc.OrganizationID,
							  ContactID = oc.ContactID,
							  ID = oc.ID,
							  FirstName = oc.Contact.FirstName,
							  LastName = oc.Contact.LastName,
							  Email = oc.Contact.Email,
							  Phone = oc.Phone.FormatPhoneNumber(),
							  Title = oc.Title,
							  FullName = oc.Contact.FullName
						  };

			return results;
		}

		public static IEnumerable<OrganizationGridViewModel> ToGridView(this IEnumerable<OrganizationView> organizations)
		{
			var results = from o in organizations
						  select new OrganizationGridViewModel
						  {
							  ID = o.ID,
							  Name = o.Name,
							  HeadQuarters = o.Headquarters,
							  FacilityCount = o.FacilityCount,
							  UserCount = o.ContactCount,
							  CreatedOnDate = o.CreatedOn//.ToShortDateString()

							  //Street = o.Facilities.Count(p => !p.Voided) == 1 ? o.Facilities.First().Street : null,
							  //City = o.Facilities.Count(p => !p.Voided) == 1 ? o.Facilities.First().City : null,
							  //ZipCode = o.Facilities.Count(p => !p.Voided) == 1 ? o.Facilities.First().ZipCode : null
						  };
			return results;
		}

		public static IEnumerable<OrganizationGridViewModel> ToGridView(this IEnumerable<Organization> organizations)
		{
			var results = from o in organizations
						  select new OrganizationGridViewModel
						  {
							  ID = o.ID,
							  Name = o.Name,
							  HeadQuarters = o.Headquarters,
							  FacilityCount = o.Facilities.Count( p => !p.Voided ),
							  UserCount = o.Contacts.Count( p => !p.Voided ),
							  CreatedOnDate = o.CreatedOn,//.ToShortDateString(),
							  Street = o.Facilities.Count( p => !p.Voided ) == 1 ? o.Facilities.First().Street : null,
							  City = o.Facilities.Count( p => !p.Voided ) == 1 ? o.Facilities.First().City : null,
							  ZipCode = o.Facilities.Count( p => !p.Voided ) == 1 ? o.Facilities.First().ZipCode : null,
							  Status = o.Status.Name,
							  OriginName = o.Origin.Name
						  };
			return results;
		}

		public static IEnumerable<AccountGridViewModel> ToGridView(this IEnumerable<Account> accounts)
		{
			var results = from acct in accounts
						  select new AccountGridViewModel
						  {
							  ID = acct.ID,
							  UserName = acct.UserName,
							  FirstName = acct.FirstName,
							  LastName = acct.LastName,
							  Email = acct.Email,
							  Approved = acct.Approved
						  };
			return results;
		}

		public static IEnumerable<PermissionGroupGridViewModel> ToGridView(this IEnumerable<PermissionGroup> permissionGroups)
		{
			var results = from permGroup in permissionGroups

						  select new PermissionGroupGridViewModel
						  {
							  ID = permGroup.ID,
							  Description = permGroup.Description,
							  DisplayName = permGroup.DisplayName,
							  Name = permGroup.Name,
							  Owner = ( permGroup.Owner != null ? permGroup.Owner.Name : "" ),
							  Context = ( permGroup.Context != null ? permGroup.Context.Name : "" ),
							  Roles = permGroup.Roles.Where( p => !p.Voided ).Select( p => p.Role.Name ).ToDelimitedString( ", " ),
							  RegulatorTypes = permGroup.FormatRegulatorTypes(), // permGroup.RegulatorTypes.Where(p => !p.Voided).Select(p => p.LUTRegulatorType.TypeCode).ToDelimitedString(", "),
							  EDT = permGroup.EDT
						  };
			return results;
		}

		public static IEnumerable<RegulatorZipSubmittalGridViewModel> ToGridView(this IEnumerable<RegulatorZipCodeSubmittalElement> regulatorZipSubmittals)
		{
			var results = from regZips in regulatorZipSubmittals
						  select new RegulatorZipSubmittalGridViewModel
						  {
							  RegulatorZipElementID = regZips.ID,
							  RegulatorName = regZips.Regulator.Name,
							  RegulatorID = regZips.Regulator.ID,
							  County = regZips.ZipCode.PrimaryCountyID.ToString(),
							  ZipCode = regZips.ZipCodeID.ToString(),
							  SubmittalElementName = regZips.SubmittalElement.Name,
							  SubmittalElementID = regZips.SubmittalElement.ID,
							  ProgramName = regZips.SubmittalElement.ProgramElement.Name
						  };

			return results;
		}

		public static IEnumerable<RegulatorZipCodeSubmittalElementMappingSummaryGridViewModel> ToGridView(this IEnumerable<RegulatorZipCodeSubmittalElementMappingSummary> regulatorZipCodeSubmittalElementMappingSummaries)
		{
			var results = from regZipCodeSEMappingSummary in regulatorZipCodeSubmittalElementMappingSummaries
						  select new RegulatorZipCodeSubmittalElementMappingSummaryGridViewModel
						  {
							  ZipCodeID = regZipCodeSEMappingSummary.ZipCodeID,
							  County = regZipCodeSEMappingSummary.County,
							  PrimaryRegulatorNameAbbreviation = regZipCodeSEMappingSummary.PrimaryRegulatorNameAbbreviation,
							  SecondaryRegulatorNameAbbreviation = regZipCodeSEMappingSummary.SecondaryRegulatorNameAbbreviation,
							  FacilityRegulatorNameAbbreviation = regZipCodeSEMappingSummary.FacilityRegulatorNameAbbreviation,
							  InventoryRegulatorNameAbbreviation = regZipCodeSEMappingSummary.InventoryRegulatorNameAbbreviation,
							  PlansRegulatorNameAbbreviation = regZipCodeSEMappingSummary.PlansRegulatorNameAbbreviation,
							  USTRegulatorNameAbbreviation = regZipCodeSEMappingSummary.USTRegulatorNameAbbreviation,
							  TPRegulatorNameAbbreviation = regZipCodeSEMappingSummary.TPRegulatorNameAbbreviation,
							  RMRRegulatorNameAbbreviation = regZipCodeSEMappingSummary.RMRRegulatorNameAbbreviation,
							  RemoteRegulatorNameAbbreviation = regZipCodeSEMappingSummary.RemoteRegulatorNameAbbreviation,
							  TankRegulatorNameAbbreviation = regZipCodeSEMappingSummary.TankRegulatorNameAbbreviation,
							  APSARegulatorNameAbbreviation = regZipCodeSEMappingSummary.APSARegulatorNameAbbreviation,
							  CalARPRegulatorNameAbbreviation = regZipCodeSEMappingSummary.CalARPRegulatorNameAbbreviation,
							  PrimaryRegulatorName = regZipCodeSEMappingSummary.PrimaryRegulatorName,
							  SecondaryRegulatorName = regZipCodeSEMappingSummary.SecondaryRegulatorName,
							  FacilityRegulatorName = regZipCodeSEMappingSummary.FacilityRegulatorName,
							  InventoryRegulatorName = regZipCodeSEMappingSummary.InventoryRegulatorName,
							  PlansRegulatorName = regZipCodeSEMappingSummary.PlansRegulatorName,
							  USTRegulatorName = regZipCodeSEMappingSummary.USTRegulatorName,
							  TPRegulatorName = regZipCodeSEMappingSummary.TPRegulatorName,
							  RMRRegulatorName = regZipCodeSEMappingSummary.RMRRegulatorName,
							  RemoteRegulatorName = regZipCodeSEMappingSummary.RemoteRegulatorName,
							  TankRegulatorName = regZipCodeSEMappingSummary.TankRegulatorName,
							  APSARegulatorName = regZipCodeSEMappingSummary.APSARegulatorName,
							  CalARPRegulatorName = regZipCodeSEMappingSummary.CalARPRegulatorName
						  };

			return results;
		}

		public static IEnumerable<RegulatorLocalGridViewModel> ToGridView(this IEnumerable<RegulatorSubmittalElementLocalRequirement> regulatorLocals)
		{
			var results = from regLocal in regulatorLocals
						  select new RegulatorLocalGridViewModel
						  {
							  ID = regLocal.ID,
							  RegulatorID = regLocal.RegulatorID,
							  SubmittalElementID = regLocal.SubmittalElementID,
							  LocalText = regLocal.RequirementsText,
							  SubmittalName = regLocal.SubmittalElement.Name,
							  ProgramElementName = regLocal.SubmittalElement.ProgramElement.Name,
							  RegulatorName = regLocal.Regulator.Name,
							  IsLocalDocumentRequired = regLocal.IsLocalDocumentRequired
						  };

			return results;
		}

		public static IEnumerable<FacilityGridViewModel> ToGridView(this IEnumerable<Facility> facilities)
		{
			var results = from facility in facilities
						  select new FacilityGridViewModel
						  {
							  CERSID = facility.CERSID,
							  FacilityID = facility.FacilityID,
							  Name = facility.Name,
							  OrganizationID = facility.OrganizationID,
							  OrganizationName = facility.Organization.Name,
							  OrganizationHeadquarters = facility.Organization.Headquarters,
							  Street = facility.Street,
							  City = facility.City,
							  ZipCode = facility.ZipCode,
							  WashedCity = facility.WashedCity,
							  WashedStreet = facility.WashedStreet,
							  WashedZipCode = facility.WashedZipCode,
							  LastSubmittalSubmittedOnDate = facility.GetLastSubmittalSubmittedOn() != null ? facility.GetLastSubmittalSubmittedOn().SubmittedDateTime : null
						  };
			return results;
		}

		public static IEnumerable<FacilityGridViewModel> ToGridView(this IEnumerable<FacilityView> facilityViews)
		{
			var results = from facilityView in facilityViews
						  select new FacilityGridViewModel
						  {
							  CERSID = facilityView.CERSID,
							  Name = facilityView.Name,
							  OrganizationID = facilityView.OrganizationID,
							  OrganizationName = facilityView.OrganizationName,
							  OrganizationHeadquarters = facilityView.OrganizationHeadquarters,
							  Street = facilityView.Street,
							  City = facilityView.City,
							  ZipCode = facilityView.ZipCode,
							  WashedCity = facilityView.WashedCity,
							  WashedStreet = facilityView.WashedStreet,
							  WashedZipCode = facilityView.WashedZipCode,
							  LastSubmittalSubmittedOnDate = facilityView.LastFacilityInfoSubmittalSubmittedOn
						  };
			return results;
		}

		public static IEnumerable<WashedAddressGridViewModel> ToGridView(this IEnumerable<WashedAddress> facilityAddresses)
		{
			var results = from washedAddress in facilityAddresses
						  select new WashedAddressGridViewModel
						  {
							  WashedCity = washedAddress.WashedCity,
							  WashedStreet = washedAddress.WashedStreet,
							  WashedZipCode = washedAddress.WashedZipCode,
							  FacilityName = washedAddress.FacilityName,
							  OrganizationName = washedAddress.OrgaizationName,
							  CERSID = washedAddress.CERSID,
							  OrganizationHeadquarters = washedAddress.OrganizationHeadquarters,
							  LastSubmittalSubmittedOnDate = washedAddress.LastSubmittalSubmittedOnDate
						  };
			return results;
		}

		public static IEnumerable<ContactGridViewModel> ToGridView(this IEnumerable<Contact> contacts)
		{
			var results = from contact in contacts
						  select new ContactGridViewModel
						  {
							  ID = contact.ID,
							  FullName = contact.FullName,
							  Email = contact.Email,
							  OrganizationContacts = contact.OrganizationContacts,
							  RegulatorContacts = contact.RegulatorContacts
						  };
			return results;
		}

		//    return results;
		//}
		public static IEnumerable<SubmittalElementTemplateGridViewModel> ToGridView(this IEnumerable<SubmittalElementTemplate> submittalElementTemplates)
		{
			var results = from submitElementTemplate in submittalElementTemplates
						  select new SubmittalElementTemplateGridViewModel
						  {
							  ID = submitElementTemplate.ID,
							  Name = submitElementTemplate.Name,
							  Active = submitElementTemplate.Active,
							  ChangeReason = submitElementTemplate.ChangeReason
						  };
			return results;
		}

		//public static IEnumerable<RegulatorSubmittalElementLocalInfoLinkGridViewModel> ToGridView(this IEnumerable<RegulatorSubmittalElementLocalInfoLink> regLocalLinks)
		//{
		//    var results = from regLocalLink in regLocalLinks
		//                  select new RegulatorSubmittalElementLocalInfoLinkGridViewModel
		//                  {
		//                      ID = regLocalLink.ID,
		//                      RegulatorSubmittalLocalInfoID = regLocalLink.RegulatorSubmittalElementLocalInfoID,
		//                      RegulatorSubmittalElement = regLocalLink.RegulatorSubmittalElementLocal.SubmittalElement.Name,
		//                      Caption = regLocalLink.Caption,
		//                      URL = regLocalLink.URL,
		//                      Description = regLocalLink.Description,
		//                      Order = regLocalLink.Order
		//                  };
		public static IEnumerable<SubmittalElementTemplateResourceGridViewModel> ToGridView(this IEnumerable<SubmittalElementTemplateResource> submittalElementTemplateResources)
		{
			var results = from submittalElementTemplateResource in submittalElementTemplateResources
						  select new SubmittalElementTemplateResourceGridViewModel
						  {
							  ID = submittalElementTemplateResource.ID,
							  SubmittalElementID = submittalElementTemplateResource.SubmittalElementID,
							  SubmittalElementName = submittalElementTemplateResource.SubmittalElement.Name,
							  TemplateID = submittalElementTemplateResource.TemplateID,
							  TemplateName = submittalElementTemplateResource.SubmittalElementTemplate.Name,
							  ParentResourceID = submittalElementTemplateResource.ParentResourceID,
							  ParentResourceName = ( ( submittalElementTemplateResource.Parent == null ) ? string.Empty : submittalElementTemplateResource.Parent.Type.Name ),
							  Order = submittalElementTemplateResource.Order,
							  TypeID = submittalElementTemplateResource.TypeID,
							  TypeName = submittalElementTemplateResource.Type.Name
						  };
			return results;
		}

		public static IEnumerable<FacilitySubmittalElementResourceDocumentGridViewModel> ToGridView(this IEnumerable<FacilitySubmittalElementResourceDocument> facilitySubmittalElementResourceDocuments)
		{
			var results = from fserd in facilitySubmittalElementResourceDocuments
						  select new FacilitySubmittalElementResourceDocumentGridViewModel
						  {
							  ID = fserd.ID,
							  SourceID = fserd.SourceID,
							  Source = fserd.Source.Name,
							  LinkUrl = fserd.LinkUrl,
							  DateProvidedToRegulator = fserd.DateProvidedToRegulator,
							  StoredAtFacilityCERSID = fserd.StoredAtFacilityCERSID,
							  StoredAtFacilityName = fserd.StoredAtFacility.Name,
							  ProvidedInSubmittalElementID = fserd.ProvidedInSubmittalElementID,
							  ProvidedInSubmittalElementName = fserd.ProvidedInSubmittalElement.Name,
							  Comments = fserd.Comments
						  };
			return results;
		}

		public static IEnumerable<FacilitySubmittalElementDocumentGridViewModel> ToGridView(this IEnumerable<FacilitySubmittalElementDocument> facilitySubmittalElementDocuments)
		{
			var results = from fsed in facilitySubmittalElementDocuments
						  select new FacilitySubmittalElementDocumentGridViewModel
						  {
							  ID = fsed.ID,
							  FacilitySubmittalElementResourceDocumentID = fsed.FacilitySubmittalElementResourceDocumentID,
							  DocumentID = fsed.DocumentID,
							  TypeID = fsed.Type.ID,
							  TypeName = fsed.Type.Name,
							  FormatID = fsed.FormatID,
							  FormatName = fsed.Format.Name,
							  Name = fsed.Name,
							  FileName = fsed.FileName,
							  FileSize = fsed.FileSize,
							  DocumentDate = fsed.DocumentDate,
							  Description = fsed.Description
						  };
			return results;
		}

		public static IEnumerable<FacilitySubmittalElementGridViewModel> ToGridView(this IEnumerable<FacilitySubmittalElement> facilitySubmittalElements)
		{
			var results = from fse in facilitySubmittalElements
						  select new FacilitySubmittalElementGridViewModel
						  {
							  ID = fse.ID,
							  CERSID = fse.CERSID,
							  SubmittalElementID = fse.SubmittalElementID,
							  FacilitySubmittalID = fse.FacilitySubmittalID,
							  StatusID = fse.StatusID,
							  TemplateID = fse.TemplateID,
							  OwningRegulatorID = fse.OwningRegulatorID,
							  SubmittalElementName = fse.SubmittalElement.Name,
							  StatusName = fse.Status.Name,
							  TemplateName = fse.SubmittalElementTemplate.Name,
							  OwningRegulatorName = fse.OwningRegulator.Name,
							  SubmittedDateTime = fse.SubmittedDateTime,
							  LastDueDateOnSubmittalDate = fse.LastDueDateOnSubmittalDate,
							  SubmittedByFirstName = fse.SubmittedByFirstName,
							  SubmittedByLastName = fse.SubmittedByLastName,
							  SubmittedByEmail = fse.SubmittedByEmail,
							  SubmittedByID = fse.SubmittedByID,
							  SubmittalActionAgentName = fse.SubmittalActionAgentName,
							  SubmittalActionAgentEmail = fse.SubmittalActionAgentEmail,
							  SubmittalActionAgentID = fse.SubmittalActionAgentID,
							  SubmittalActionRegulatorID = fse.SubmittalActionRegulatorID,
							  Comments = fse.SubmittalActionComments
						  };
			return results;
		}

		public static IEnumerable<FacilitySubmittalElementResourceGridViewModel> ToGridView(this IEnumerable<FacilitySubmittalElementResource> facilitySubmittalElementResources)
		{
			var results = from fser in facilitySubmittalElementResources
						  select new FacilitySubmittalElementResourceGridViewModel
						  {
							  ID = fser.ID,
							  FacilitySubmittalElementID = fser.FacilitySubmittalElementID,
							  TemplateResourceID = fser.TemplateResourceID,
							  ParentResourceID = fser.ParentResourceID,
							  ResourceTypeID = fser.ResourceTypeID,
							  TemplateResourceName = fser.SubmittalElementTemplateResource.Type.Name,
							  ParentResourceName = fser.Parent.Type.Name,
							  ResourceTypeName = fser.Type.Name,
							  Order = fser.Order,
							  IsStarted = fser.IsStarted,
							  IsDocument = fser.IsDocument,
							  IsRequired = fser.IsRequired,
							  DocumentSourceID = fser.DocumentSourceID,
							  DocumentSourceName = fser.LUTDocumentSource.Name,
							  MinRequiredFieldsSubmitted = fser.MinRequiredFieldsSubmitted,
							  RequiredGuidanceMessageCount = fser.RequiredGuidanceMessageCount,
							  WarningGuidanceMessageCount = fser.WarningGuidanceMessageCount,
							  AdvisoryGuidanceMessageCount = fser.AdvisoryGuidanceMessageCount,
							  ResourceEntityCount = fser.ResourceEntityCount,
							  DisplayName = fser.DisplayName
						  };
			return results;
		}

		public static IEnumerable<GuidanceMessageGridViewModel> ToGridView(this IEnumerable<GuidanceMessage> guidanceMessages)
		{
			var results = from messages in guidanceMessages
						  select new GuidanceMessageGridViewModel
						  {
							  ID = messages.ID,
							  Message = messages.Message,
							  Level = (CERS.GuidanceLevel)messages.LevelID,
							  LevelID = messages.LevelID,

							  // Retrieve Guidance Level from LUTGuidanceLevel if linked, otherwise pull string representation from Enumeration:
							  LevelName = messages.LUTGuidanceLevel != null ? messages.LUTGuidanceLevel.Name : ( (CERS.GuidanceLevel)messages.LevelID ).ToString(),
							  EntityName = messages.GetEntityName()
						  };
			return results;
		}

		public static IEnumerable<GuidanceMessageTemplateGridViewModel> ToGridView(this IEnumerable<GuidanceMessageTemplate> guidanceMessageTemplates)
		{
			var results = from guidanceMessageTemplate in guidanceMessageTemplates
						  select new GuidanceMessageTemplateGridViewModel
						  {
							  ID = guidanceMessageTemplate.ID
						  };
			return results;
		}

		public static IEnumerable<SubmittalElementRegulatorGridViewModel> ToGridView(this IEnumerable<FacilitySubmittalElementRegulator> facilitySubmittalElementRegulators)
		{
			//Group the submittal elements by CERSID and Submittal Date NOT DateTime.
			var groupedByCERSIDandDate = from s in facilitySubmittalElementRegulators
										 group s by new { s.FacilitySubmittalElement.CERSID, s.FacilitySubmittalElement.SubmittedDateTime.Value.Date }
											 into grp
											 select new
											 {
												 grp.Key.CERSID,
												 grp.Key.Date,
												 FacilityInfoStatus = grp.Where( f => f.FacilitySubmittalElement.SubmittalElementID == (int)CERS.SubmittalElementType.FacilityInformation ).FirstOrDefault(),
												 HazMatInvStatus = grp.Where( f => f.FacilitySubmittalElement.SubmittalElementID == (int)CERS.SubmittalElementType.HazardousMaterialsInventory ).FirstOrDefault(),
												 ERPlansStatus = grp.Where( f => f.FacilitySubmittalElement.SubmittalElementID == (int)CERS.SubmittalElementType.EmergencyResponseandTrainingPlans ).FirstOrDefault(),
												 USTStatus = grp.Where( f => f.FacilitySubmittalElement.SubmittalElementID == (int)CERS.SubmittalElementType.UndergroundStorageTanks ).FirstOrDefault(),
												 HWTreatmentStatus = grp.Where( f => f.FacilitySubmittalElement.SubmittalElementID == (int)CERS.SubmittalElementType.OnsiteHazardousWasteTreatmentNotification ).FirstOrDefault(),
												 HWRecycleStatus = grp.Where( f => f.FacilitySubmittalElement.SubmittalElementID == (int)CERS.SubmittalElementType.RecyclableMaterialsReport ).FirstOrDefault(),
												 HWRWStatus = grp.Where( f => f.FacilitySubmittalElement.SubmittalElementID == (int)CERS.SubmittalElementType.RemoteWasteConsolidationSiteAnnualNotification ).FirstOrDefault(),
												 HWTClosureStatus = grp.Where( f => f.FacilitySubmittalElement.SubmittalElementID == (int)CERS.SubmittalElementType.HazardousWasteTankClosureCertification ).FirstOrDefault(),
												 ASTStatus = grp.Where( f => f.FacilitySubmittalElement.SubmittalElementID == (int)CERS.SubmittalElementType.AbovegroundPetroleumStorageTanks ).FirstOrDefault(),
												 CalARPStatus = grp.Where( f => f.FacilitySubmittalElement.SubmittalElementID == (int)CERS.SubmittalElementType.CaliforniaAccidentalReleaseProgram ).FirstOrDefault()
											 };

			var results = from s in groupedByCERSIDandDate
						  select new SubmittalElementRegulatorGridViewModel
						  {
							  CERSID = s.CERSID,
							  SubmittalDate = s.Date,
							  FacilityInfoStatus = ( s.FacilityInfoStatus != null ) ? s.FacilityInfoStatus.FacilitySubmittalElement.Status.Name : null,
							  HazMatInvStatus = ( s.HazMatInvStatus != null ) ? s.HazMatInvStatus.FacilitySubmittalElement.Status.Name : null,
							  ERPlansStatus = ( s.ERPlansStatus != null ) ? s.ERPlansStatus.FacilitySubmittalElement.Status.Name : null,
							  USTStatus = ( s.USTStatus != null ) ? s.USTStatus.FacilitySubmittalElement.Status.Name : null,
							  HWTreatmentStatus = ( s.HWTreatmentStatus != null ) ? s.HWTreatmentStatus.FacilitySubmittalElement.Status.Name : null,
							  HWRecycleStatus = ( s.HWRecycleStatus != null ) ? s.HWRecycleStatus.FacilitySubmittalElement.Status.Name : null,
							  HWRWStatus = ( s.HWRWStatus != null ) ? s.HWRWStatus.FacilitySubmittalElement.Status.Name : null,
							  HWTClosureStatus = ( s.HWTClosureStatus != null ) ? s.HWTClosureStatus.FacilitySubmittalElement.Status.Name : null,
							  ASTStatus = ( s.ASTStatus != null ) ? s.ASTStatus.FacilitySubmittalElement.Status.Name : null,
							  CalARPStatus = ( s.CalARPStatus != null ) ? s.CalARPStatus.FacilitySubmittalElement.Status.Name : null
						  };

			return results;
		}

		public static IEnumerable<USTMonitoringPlanGridViewModel> ToGridView(this IEnumerable<USTMonitoringPlan> ustMonitoringPlans)
		{
			var results = ( from ustMonitoringPlan in ustMonitoringPlans
							select new USTMonitoringPlanGridViewModel
							{
								FSERID = ustMonitoringPlan.FacilitySubmittalElementResourceID,
								USTMonitoringPlanID = ustMonitoringPlan.ID,
								TankIDNumber = ( ustMonitoringPlan.USTTankInfo != null ? ustMonitoringPlan.USTTankInfo.TankIDNumber : "" ),
								TankUse = ( ustMonitoringPlan.USTTankInfo != null ? ustMonitoringPlan.USTTankInfo.TankUse : "" ),
								TankContents = ( ustMonitoringPlan.USTTankInfo != null ? ustMonitoringPlan.USTTankInfo.TankContents : "" ),
								TankCapacityInGallons = ( ustMonitoringPlan.USTTankInfo != null ? ustMonitoringPlan.USTTankInfo.TankCapacityInGallons : null )
							} ).ToList();

			// Expand field values to use Data Dictionary values.
			results.TranslateDataRegistryProperties();

			return results;
		}

		public static IEnumerable<USTTankInfoGridViewModel> ToGridView(this IEnumerable<USTTankInfo> ustTankInfos)
		{
			var results = ( from ustTankInfo in ustTankInfos
							select new USTTankInfoGridViewModel
							{
								FSERID = ustTankInfo.FacilitySubmittalElementResourceID,
								USTTankInfoID = ustTankInfo.ID,
								TankIDNumber = ( ustTankInfo != null ? ustTankInfo.TankIDNumber : "" ),
								TankUse = ( ustTankInfo != null ? ustTankInfo.TankUse : "" ),
								TankContents = ( ustTankInfo != null ? ustTankInfo.TankContents : "" ),
								TankCapacityInGallons = ( ustTankInfo != null ? ustTankInfo.TankCapacityInGallons : null )
							} ).ToList();

			// Expand field values to use Data Dictionary values.
			results.TranslateDataRegistryProperties();

			return results;
		}

        public static IEnumerable<EnhancementGridViewModel> ToGridView( this IEnumerable<CERS.Model.Enhancement> enhancements )
		{
            var results = ( from enhancement in enhancements
                            select new EnhancementGridViewModel
                            {
                                ID = enhancement.ID,
                                Status = enhancement.Status.Name,
                                Name = enhancement.Name,
                                Portal = enhancement.Portal.Name,
                                ImplementationTargetDate = enhancement.ImplementationTargetDate,
                                Assignee = enhancement.AssigneeID.HasValue ? enhancement.Assignee.Name : string.Empty,
                                Priority = enhancement.PriorityCalEPA != null ? enhancement.PriorityCalEPA.Name : "-",
                                EstimatedEffort = enhancement.EstimatedEffort,
                                UpdatedOn = enhancement.UpdatedOn,
                                NumberOfComments = enhancement.EnhancementUserComments.Count( c => !c.Voided )
                            }
                            ).ToList<EnhancementGridViewModel>();

			// Expand field values to use Data Dictionary values.
			results.TranslateDataRegistryProperties();

			return results;
		}

		public static IEnumerable<EnhancementCommentGridViewModel> ToGridView(this IEnumerable<CERS.Model.EnhancementUserComment> comments, ICERSRepositoryManager repository)
		{
			var results = ( from comment in comments
							select new EnhancementCommentGridViewModel
							{
								ID = comment.ID,
								EnhancementID = comment.EnhancementID,
								User = repository.Enhancements.AccountName( comment.AccountID ),
								Regulator = comment.Regulator == null ? String.Empty : comment.Regulator.NameAbbreviation,
								Organization = comment.Organization == null ? String.Empty : comment.Organization.Name,
								UserPriority = comment.UserPriority == null ? String.Empty : comment.UserPriority.Name,
								UserLikes = comment.UserLikes,
								Comments = Strings.Truncate( comment.Comments, 60, true ),
								UpdatedOn = comment.UpdatedOn,
								AccountID = comment.AccountID
							} ).ToList<EnhancementCommentGridViewModel>();

			// Expand field values to use Data Dictionary values.
			results.TranslateDataRegistryProperties();

			return results;
		}

		public static IEnumerable<RegulatorZipSubmittalGridViewModel> ToGroupByRegSubGridView(this IEnumerable<RegulatorZipCodeSubmittalElement> regulatorZipSubmittals)
		{
			var results = from regZips in regulatorZipSubmittals
						  group regZips by new { regZips.Regulator, regZips.SubmittalElement } into regSubGroup
						  select new RegulatorZipSubmittalGridViewModel
						  {
							  RegulatorName = regSubGroup.Key.Regulator.Name,
							  RegulatorID = regSubGroup.Key.Regulator.ID,
							  ZipCodeCount = regSubGroup.Count(),
							  SubmittalElementName = regSubGroup.Key.SubmittalElement.Name,
							  SubmittalElementID = regSubGroup.Key.SubmittalElement.ID,
							  ProgramName = regSubGroup.Key.SubmittalElement.ProgramElement.Name
						  };

			return results;
		}

		#region Hazardous Material Inventory Grid View Methods

		public static IEnumerable<BPFacilityChemicalGridViewModel> ToGridView(this IEnumerable<BPFacilityChemical> bpFacilityChemicals)
		{
			var tempResults = ( from bpFacilityChemical in bpFacilityChemicals
								select new BPFacilityChemicalGridViewModel
								{
									ID = bpFacilityChemical.ID,
									ChemicalName = bpFacilityChemical.ChemicalName,
									CASNumber = bpFacilityChemical.CASNumber,
									Location = bpFacilityChemical.ChemicalLocation,

									//MaximumDailyAmount = bpFacilityChemical.MaximumDailyAmount.ToString(),
									Units = bpFacilityChemical.Units,
									CERSID = bpFacilityChemical.FacilitySubmittalElementResource.FacilitySubmittalElement.CERSID,
									OrgID = bpFacilityChemical.FacilitySubmittalElementResource.FacilitySubmittalElement.Facility.OrganizationID,
									FSEID = bpFacilityChemical.FacilitySubmittalElementResource.FacilitySubmittalElementID,
									FSERID = bpFacilityChemical.FacilitySubmittalElementResourceID
								} ).ToList();

			//translate properties with DataRegistryMetadata attribute and codes in CDR into descriptions instead of values.
			tempResults.TranslateDataRegistryProperties();

			// Regenerate BPFacilityChemicalGridViewModel to concatenate Amount and Units
			// NOTE: There likely is a better method of updating the existing List vs Recreating it:
			var results = ( from bpFacilityChemical in tempResults
							select new BPFacilityChemicalGridViewModel
							{
								ID = bpFacilityChemical.ID,
								ChemicalName = bpFacilityChemical.ChemicalName,
								CASNumber = bpFacilityChemical.CASNumber,
								Location = bpFacilityChemical.Location,
								MaximumDailyAmount = bpFacilityChemical.MaximumDailyAmount,

								//MaximumDailyAmountDisplay = bpFacilityChemical.MaximumDailyAmount + " " + bpFacilityChemical.Units,
								Units = bpFacilityChemical.Units,
								CERSID = bpFacilityChemical.CERSID,
								OrgID = bpFacilityChemical.OrgID,
								FSEID = bpFacilityChemical.FSEID,
								FSERID = bpFacilityChemical.FSERID
							} ).ToList();

			return results;
		}

		// To support Ajax Refresh of the ChemicalGridViewModel within the context of a single FacilitySubmittalElementResource
		// (adding inventory items to a Draft Submittal) we need store FSERID, FSEID, CERSID, and OrgID as part of the ChemicalGridViewModel.
		// Optional parameter "fser"
		public static IEnumerable<ChemicalGridViewModel> ToGridView(this IEnumerable<Chemical> chemicals, FacilitySubmittalElementResource fser = null)
		{
			var results = ( from chemical in chemicals
							select new ChemicalGridViewModel
							{
								ID = chemical.ID,
								CCLID = chemical.CCLID,
								CCLFQID = chemical.CCLFQID,
								ChemicalName = chemical.ChemicalName,
								CommonName = chemical.CommonName,
								CASNumber = chemical.CAS,
								USEPASRSNumber = chemical.USEPASRSNumber,
								STACode = chemical.STACode,
								PhysState = chemical.PhysState,
								HMType = chemical.HMType,

								//Mixture = chemical.Mixture,
								EHS = chemical.EHS,
								Firecode1 = chemical.FireCode1,
								Firecode2 = chemical.FireCode2,
								Firecode3 = chemical.FireCode3,
								Firecode4 = chemical.FireCode4,
								Firecode5 = chemical.FireCode5,
								Firecode6 = chemical.FireCode6,
								Firecode7 = chemical.FireCode7,
								Firecode8 = chemical.FireCode8,
								FHCFire = chemical.FHCFire,
								FHCReactive = chemical.FHCReactive,
								FHCPressureRelease = chemical.FHCPressureRelease,
								FHCAcuteHealth = chemical.FHCAcuteHealth,
								FHCChronicHealth = chemical.FHCChronicHealth,
                                //FHCHealthAcuteToxici
							    //FHCHealthAcuteToxicity = chemical.FHCHealthAcuteToxicity,
                                //FHCTest = chemical.fhctest,
                                DOTHazClassID = chemical.DOTHazClassID,
								RadioActive = chemical.RadioActive,
								//Prop65 = chemical.Prop65,
								//Prop65Type = chemical.Prop65Type,
								Component1Percent = chemical.Component1Percent,
								Component1Name = chemical.Component1Name,
								Component1EHS = chemical.Component1EHS,
								Component1CAS = chemical.Component1CAS,
								Component2Percent = chemical.Component2Percent,
								Component2Name = chemical.Component2Name,
								Component2EHS = chemical.Component2EHS,
								Component2CAS = chemical.Component2CAS,
								Component3Percent = chemical.Component3Percent,
								Component3Name = chemical.Component3Name,
								Component3EHS = chemical.Component3EHS,
								Component3CAS = chemical.Component3CAS,
								Component4Percent = chemical.Component4Percent,
								Component4Name = chemical.Component4Name,
								Component4EHS = chemical.Component4EHS,
								Component4CAS = chemical.Component4CAS,
								Component5Percent = chemical.Component5Percent,
								Component5Name = chemical.Component5Name,
								Component5EHS = chemical.Component5EHS,
								Component5CAS = chemical.Component5CAS,
								AdditionalMixtureComponent = chemical.AdditionalMixtureComponent,
								Comments = chemical.Comments,

								// "fser" is optional parameter used to support the Ajax Chemical Grid
								// when adding a new hazardous material inventory entry to a facility:
								OrgID = fser != null ? fser.FacilitySubmittalElement.Facility.OrganizationID : 0,
								CERSID = fser != null ? fser.FacilitySubmittalElement.CERSID : 0,
								FSEID = fser != null ? fser.FacilitySubmittalElementID : 0,
								FSERID = fser != null ? fser.ID : 0
							} ).ToList();

			//translate properties with DataRegistryMetadata attribute and codes in CDR into descriptions instead of values.
			results.TranslateDataRegistryProperties();

			return results;
		}

		/*

		// To support Ajax Refresh of the ChemicalGridViewModel within the context of a single FacilitySubmittalElementResource
		// (adding inventory items to a Draft Submittal) we need store FSERID, FSEID, CERSID, and OrgID as part of the ChemicalGridViewModel.
		// Optional parameter "fser"
		public static IEnumerable<ChemicalLibraryEntryGridViewModel> ToGridView(this IEnumerable<ChemicalLibraryEntry> chemicalLibraryEntries, FacilitySubmittalElementResource fser = null)
		{
			var results = (from chemicalLibraryEntry in chemicalLibraryEntries
						   select new ChemicalLibraryEntryGridViewModel
						   {
							   ID = chemicalLibraryEntry.ID,
							   CCLID = chemicalLibraryEntry.CCLID,
							   CCLFQID = chemicalLibraryEntry.CCLFQID,
							   ChemicalName = chemicalLibraryEntry.ChemicalName,
							   CommonName = chemicalLibraryEntry.CommonName,
							   SynonymName = chemicalLibraryEntry.SynonymName,
							   CASNumber = chemicalLibraryEntry.CAS,
							   USEPASRSNumber = chemicalLibraryEntry.USEPASRSNumber,
							   STACode = chemicalLibraryEntry.STACode,
							   PhysState = chemicalLibraryEntry.PhysState,
							   HMType = chemicalLibraryEntry.HMType,

							   //Mixture = chemical.Mixture,
							   EHS = chemicalLibraryEntry.EHS,
							   Firecode1 = chemicalLibraryEntry.FireCode1.ToString(),
							   Firecode2 = chemicalLibraryEntry.FireCode2.ToString(),
							   Firecode3 = chemicalLibraryEntry.FireCode3.ToString(),
							   Firecode4 = chemicalLibraryEntry.FireCode4.ToString(),
							   Firecode5 = chemicalLibraryEntry.FireCode5.ToString(),
							   Firecode6 = chemicalLibraryEntry.FireCode6.ToString(),
							   Firecode7 = chemicalLibraryEntry.FireCode7.ToString(),
							   Firecode8 = chemicalLibraryEntry.FireCode8.ToString(),
							   FHCFire = chemicalLibraryEntry.FHCFire,
							   FHCReactive = chemicalLibraryEntry.FHCReactive,
							   FHCPressureRelease = chemicalLibraryEntry.FHCPressureRelease,
							   FHCAcuteHealth = chemicalLibraryEntry.FHCAcuteHealth,
							   FHCChronicHealth = chemicalLibraryEntry.FHCChronicHealth,
							   DOTHazClassID = chemicalLibraryEntry.DOTHazClassID.ToString(),
							   RadioActive = chemicalLibraryEntry.RadioActive,
							   //Prop65 = chemical.Prop65,
							   //Prop65Type = chemical.Prop65Type,
							   Component1Percent = chemicalLibraryEntry.Component1Percent.ToString(),
							   Component1Name = chemicalLibraryEntry.Component1Name,
							   Component1EHS = chemicalLibraryEntry.Component1EHS,
							   Component1CAS = chemicalLibraryEntry.Component1CAS,
							   Component2Percent = chemicalLibraryEntry.Component2Percent.ToString(),
							   Component2Name = chemicalLibraryEntry.Component2Name,
							   Component2EHS = chemicalLibraryEntry.Component2EHS,
							   Component2CAS = chemicalLibraryEntry.Component2CAS,
							   Component3Percent = chemicalLibraryEntry.Component3Percent.ToString(),
							   Component3Name = chemicalLibraryEntry.Component3Name,
							   Component3EHS = chemicalLibraryEntry.Component3EHS,
							   Component3CAS = chemicalLibraryEntry.Component3CAS,
							   Component4Percent = chemicalLibraryEntry.Component4Percent.ToString(),
							   Component4Name = chemicalLibraryEntry.Component4Name,
							   Component4EHS = chemicalLibraryEntry.Component4EHS,
							   Component4CAS = chemicalLibraryEntry.Component4CAS,
							   Component5Percent = chemicalLibraryEntry.Component5Percent.ToString(),
							   Component5Name = chemicalLibraryEntry.Component5Name,
							   Component5EHS = chemicalLibraryEntry.Component5EHS,
							   Component5CAS = chemicalLibraryEntry.Component5CAS,
							   AdditionalMixtureComponent = chemicalLibraryEntry.AdditionalMixtureComponent,
							   Comments = chemicalLibraryEntry.Comments,
							   IsSynonym = chemicalLibraryEntry.IsSynonym.Value ? "Y" : "N",

							   // "fser" is optional parameter used to support the Ajax Chemical Grid
							   // when adding a new hazardous material inventory entry to a facility:
							   OrgID = fser != null ? fser.FacilitySubmittalElement.Facility.OrganizationID : 0,
							   CERSID = fser != null ? fser.FacilitySubmittalElement.CERSID : 0,
							   FSEID = fser != null ? fser.FacilitySubmittalElementID : 0,
							   FSERID = fser != null ? fser.ID : 0
						   }).ToList();

			//translate properties with DataRegistryMetadata attribute and codes in CDR into descriptions instead of values.
			results.TranslateDataRegistryProperties();

			return results;
		}
		*/

		public static IEnumerable<ChemicalSynonymGridViewModel> ToGridView(this IEnumerable<ChemicalSynonym> chemicalSynonyms)
		{
			var results = ( from chemicalSynonym in chemicalSynonyms
							select new ChemicalSynonymGridViewModel
							{
								SynonymName = chemicalSynonym.Name
							} ).ToList();

			return results;
		}

		// To support Ajax Refresh of the ChemicalGridViewModel within the context of a single FacilitySubmittalElementResource
		// (adding inventory items to a Draft Submittal) we need store FSERID, FSEID, CERSID, and OrgID as part of the ChemicalGridViewModel.
		// Optional parameter "fser"
		public static IEnumerable<ChemicalLibrarySearchResultGridViewModel> ToGridView(this IEnumerable<ChemicalLibrarySearchResult> chemicalLibrarySearchResults, FacilitySubmittalElementResource fser = null)
		{
			var results = ( from chemicalLibrarySearchResult in chemicalLibrarySearchResults
							select new ChemicalLibrarySearchResultGridViewModel
							{
								ID = chemicalLibrarySearchResult.ChemicalID,
								CCLFQID = chemicalLibrarySearchResult.CCLFQID,
								ChemicalName = chemicalLibrarySearchResult.ChemicalName,
								//CommonSynonymName = chemicalLibrarySearchResult.Name,
								CASNumber = chemicalLibrarySearchResult.CAS,
								USEPASRSNumber = chemicalLibrarySearchResult.USEPASRSNumber,
								//IsSynonym = chemicalLibrarySearchResult.IsSynonym ? "Y" : "N",

								// "fser" is optional parameter used to support the Ajax Chemical Grid
								// when adding a new hazardous material inventory entry to a facility:
								OrgID = fser != null ? fser.FacilitySubmittalElement.Facility.OrganizationID : 0,
								CERSID = fser != null ? fser.FacilitySubmittalElement.CERSID : 0,
								FSEID = fser != null ? fser.FacilitySubmittalElementID : 0,
								FSERID = fser != null ? fser.ID : 0
							} ).ToList();

			//translate properties with DataRegistryMetadata attribute and codes in CDR into descriptions instead of values.
			results.TranslateDataRegistryProperties();

			return results;
		}

		#endregion Hazardous Material Inventory Grid View Methods

		public static IEnumerable<EnhancementMyCommentGridViewModel> ToMyCommentGridView(this IEnumerable<CERS.Model.Enhancement> enhancements, ICERSRepositoryManager repository)
		{
			var results = ( from enhancement in enhancements
							select new EnhancementMyCommentGridViewModel
							{
								ID = enhancement.ID,
								Name = enhancement.Name,
								Portal = enhancement.Portal.Name,
								Comments = repository.EnhancementComments.SearchLastComment( enhancement.ID, repository.ContextAccountID )
							} ).ToList<EnhancementMyCommentGridViewModel>();

			// Expand field values to use Data Dictionary values.
			results.TranslateDataRegistryProperties();

			return results;
		}
	}
}