using CERS.Model;
using CERS.ViewModels.Enforcements;
using CERS.ViewModels.Inspections;
using CERS.ViewModels.Violations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CERS
{
	public class UPDDViewModelData
	{
		#region Public Properties

		public ICERSRepositoryManager DataRepository { get; protected set; }

		#endregion Public Properties

		#region Construction

		public UPDDViewModelData( ICERSRepositoryManager repository )
		{
			DataRepository = repository;
		}

		#endregion Construction

		#region Inspection

		public virtual InspectionViewModel BuildUpInspectionViewModel( int? inspectionID = null,
																	  int? CERSID = null,
																	  string name = "",
																	  string street = "",
																	  string city = "",
																	  string zipCode = "",
																	  int? cmeDataStatusID = null,
																	  int? regulatorID = null,
																	  string cmeProgramElement = "",
																	  DateTime? earliestOccurredOn = null,
																	  DateTime? latestOccurredOn = null,
																	  string type = "" )
		{
			InspectionViewModel inspectionViewModel = new InspectionViewModel();
			if ( inspectionID == null || inspectionID.Value == 0 )
			{
				inspectionViewModel.Entity = new Model.Inspection();
				inspectionViewModel.Entities = DataRepository.Inspections.Search( CERSID: CERSID,
																				 name: name,
																				 street: street,
																				 city: city,
																				 zipCode: zipCode,
																				 cmeDataStatusID: cmeDataStatusID,
																				 regulatorID: regulatorID,
																				 cmeProgramElement: cmeProgramElement,
																				 earliestOccurredOn: earliestOccurredOn,
																				 latestOccurredOn: latestOccurredOn,
																				 type: type );
			}
			else
			{
				// Populate the View Model's Entity with the Inspection Details:
				inspectionViewModel.Entity = DataRepository.Inspections.GetByID( inspectionID.Value );

				if ( inspectionViewModel.Entity.OccurredOn < DateTime.Parse( "10/1/2013" ) )
				{
					// Set CMEViolationContext (Summary or Detailed) based on the presence of
					// Violation Details:
					inspectionViewModel.CMEViolationContext = DataRepository.Violations.Search( inspectionID: inspectionID.Value ).Count() > 0
															  ? CMEViolationContext.Detailed
															  : CMEViolationContext.Summary;
				}
				else
				{
					inspectionViewModel.CMEViolationContext = CMEViolationContext.Detailed;
				}
			}
			inspectionViewModel.UpdateEmptyViolationActualRTCOn = null;
			inspectionViewModel.UpdateEmptyViolationActualRTCQualifier = "";

			return inspectionViewModel;
		}

		public virtual InspectionViewModel BuildUpInspectionViewModel( InspectionViewModel inspectionViewModel )
		{
			if ( inspectionViewModel != null )
			{
				inspectionViewModel = this.BuildUpInspectionViewModel( inspectionID: inspectionViewModel.ID,
																	  CERSID: inspectionViewModel.CERSID,
																	  name: inspectionViewModel.Name,
																	  street: inspectionViewModel.Street,
																	  city: inspectionViewModel.City,
																	  zipCode: inspectionViewModel.ZipCode,
																	  cmeDataStatusID: inspectionViewModel.CMEDataStatusID,
																	  regulatorID: inspectionViewModel.RegulatorID,
																	  cmeProgramElement: inspectionViewModel.CMEProgramElement,
																	  earliestOccurredOn: inspectionViewModel.EarliestOccurredOn,
																	  latestOccurredOn: inspectionViewModel.LatestOccurredOn,
																	  type: inspectionViewModel.Type );
			}
			return inspectionViewModel;
		}

		#endregion Inspection

		#region Enforcement

		public virtual EnforcementViewModel BuildUpEnforcementViewModel( int? enforcementID = null,
																		int? CERSID = null,
																		string facilityName = "",
																		string street = "",
																		string city = "",
																		string zipCode = "",
																		int? cmeDataStatusID = null,
																		int? regulatorID = null,
																		string type = null,
																		DateTime? earliestOccurredOn = null,
																		DateTime? latestOccurredOn = null,
																		string formalType = "",
																		bool onlyShowRedTagEnforcements = false )
		{
			EnforcementViewModel enforcementViewModel = new EnforcementViewModel();
			if ( enforcementID == null || enforcementID.Value == 0 )
			{
				enforcementViewModel.Entity = new Model.Enforcement();
				enforcementViewModel.Entities = DataRepository.Enforcements.Search( CERSID: CERSID,
																				   facilityName: facilityName,
																				   street: street,
																				   city: city,
																				   zipCode: zipCode,
																				   cmeDataStatusID: cmeDataStatusID,
																				   regulatorID: regulatorID,
																				   type: type,
																				   earliestOccurredOn: earliestOccurredOn,
																				   latestOccurredOn: latestOccurredOn,
																				   formalType: formalType,
																				   onlyShowRedTagEnforcements: onlyShowRedTagEnforcements );
				// If CERSID is specified, populate FacilityViolationCount property to determine if
				// any violations exist that could be linked to this Enforcement:
				if ( CERSID != null )
				{
					enforcementViewModel.FacilityViolationCount = DataRepository.Violations.Search( CERSID: CERSID ).Count();
				}
			}
			else
			{
				enforcementViewModel.Entity = DataRepository.Enforcements.GetByID( enforcementID.Value );
				enforcementViewModel.Violations = DataRepository.Violations.Search( enforcementID: enforcementID.Value );
				enforcementViewModel.FacilityViolationCount = DataRepository.Violations.Search( CERSID: enforcementViewModel.Entity.CERSID ).Count();
			}

			return enforcementViewModel;
		}

		public virtual EnforcementViewModel BuildUpEnforcementViewModel( EnforcementViewModel enforcementViewModel )
		{
			if ( enforcementViewModel != null )
			{
				enforcementViewModel = this.BuildUpEnforcementViewModel( enforcementID: enforcementViewModel.ID,
																		CERSID: enforcementViewModel.CERSID,
																		facilityName: enforcementViewModel.FacilityName,
																		street: enforcementViewModel.Street,
																		city: enforcementViewModel.City,
																		zipCode: enforcementViewModel.ZipCode,
																		cmeDataStatusID: enforcementViewModel.CMEDataStatusID,
																		regulatorID: enforcementViewModel.RegulatorID,
																		type: enforcementViewModel.Type,
																		earliestOccurredOn: enforcementViewModel.EarliestOccurredOn,
																		latestOccurredOn: enforcementViewModel.LatestOccurredOn,
																		formalType: enforcementViewModel.FormalType );
			}
			return enforcementViewModel;
		}

		#endregion Enforcement

		#region Violation

		public virtual ViolationViewModel BuildUpViolationViewModel( int? violationID = null,
																	int? inspectionID = null,
																	int? CERSID = null )
		{
			ViolationViewModel violationViewModel = new ViolationViewModel();
			if ( violationID == null || violationID.Value == 0 )
			{
				violationViewModel.Entity = new Model.Violation();
				violationViewModel.Entities = DataRepository.Violations.Search( violationID: violationID,
																			   inspectionID: inspectionID,
																			   CERSID: CERSID );
			}
			else
			{
				// Populate the View Model's Entity with the Violation Details
				violationViewModel.Entity = DataRepository.Violations.GetByID( violationID.Value );
			}

			return violationViewModel;
		}

		public virtual ViolationViewModel BuildUpViolationViewModel( ViolationViewModel violationViewModel )
		{
			if ( violationViewModel != null )
			{
				violationViewModel = this.BuildUpViolationViewModel( violationID: violationViewModel.ID,
																	inspectionID: violationViewModel.InspectionID,
																	CERSID: violationViewModel.CERSID );
			}
			return violationViewModel;
		}

		#endregion Violation
	}
}