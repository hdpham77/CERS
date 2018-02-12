using CERS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using UPF;
using UPF.Core;

namespace CERS
{
    public static class ModelEntityExtensionMethods
    {
        #region SetDDCommonFields Method

        public static void SetDDCommonFields( this IDDModelEntity entity, int contextAccountID = Constants.DefaultAccountID, bool creating = false, bool voided = false, string edtClientKey = null )
        {
            if ( creating )
            {
                entity.Key = Guid.NewGuid();
                entity.EDTClientKey = edtClientKey;
            }

            IModelEntity modelEntity = (IModelEntity)entity;
            modelEntity.SetCommonFields( contextAccountID, creating, voided );
        }

        #endregion SetDDCommonFields Method

        #region CME PreSave Methods

		public static void PreSave(this Inspection inspection, ICERSRepositoryManager repository, CallerContext callerContext = CallerContext.UI, bool voided = false, List<Inspection> cmePackageInspections = null )
        {
            if ( inspection.ID == 0 )
            {
				inspection.PreCreate(repository, callerContext, voided, cmePackageInspections);
            }
            else
            {
				inspection.PreUpdate(repository, callerContext, voided, cmePackageInspections);
            }
        }

		public static void PreSave(this Violation violation, ICERSRepositoryManager repository, CallerContext callerContext = CallerContext.UI, bool voided = false, List<Violation> cmePackageViolations = null )
        {
            if ( violation.ID == 0 )
            {
                violation.PreCreate( repository, callerContext, voided, cmePackageViolations );
            }

			//We will never need to pass in the cmePackageViolations parameter here because it is not necessary to call CalculateRCRASequence method for a Violation update
			//TODO: Explore more efficient way to handle PreSave for CME Packages (EDT/Upload). Possibly create a PreSave extension method for CMEPackage object.
            else
            {
                violation.PreUpdate( repository, callerContext, voided );
            }
        }

        public static void PreSave( this Enforcement enforcement, ICERSRepositoryManager repository, CallerContext callerContext = CallerContext.UI, bool voided = false, List<Enforcement> cmePackageEnforcements = null )
        {
            if ( enforcement.ID == 0 )
            {
                enforcement.PreCreate( repository, callerContext, voided, cmePackageEnforcements );
            }
            else
            {
                enforcement.PreUpdate( repository, callerContext, voided, cmePackageEnforcements );
            }
        }

        public static void PreSave( this EnforcementViolation enforcementViolation, ICERSRepositoryManager repository, CallerContext callerContext = CallerContext.UI, bool voided = false )
        {
            if ( enforcementViolation.ID == 0 )
            {
                enforcementViolation.PreCreate( repository, callerContext, voided );
            }
            else
            {
                enforcementViolation.PreUpdate( repository, callerContext, voided );
            }
        }

        #endregion CME PreSave Methods

        #region CME PreCreate Methods

		public static void PreCreate(this Inspection inspection, ICERSRepositoryManager repository, CallerContext callerContext = CallerContext.UI, bool voided = false, List<Inspection> cmePackageInspections = null )
        {
            // If CMEBatchID is not specified, create a CMEBatch record
            // to track this transaction:
            if ( inspection.CMEBatch == null )
            {
                CMEBatch cmeBatch = new CMEBatch();
                inspection.CMEBatch = cmeBatch;
            }
            if ( inspection.CMEBatchID == 0 )
            {
                inspection.CMEBatch.SetCommonFields( repository.ContextAccountID, creating:true );
            }
            inspection.CMEBatch.PopulateCMEEntityCounts();
            inspection.CMEBatch.CallerContextID = (int)callerContext;

            // Default RCRASequence to zero, proceed with RCRASequence calculation if this is a RCRA LQG Inspection
            int rcraSequence = 0;

            if ( inspection.CMEProgramElementID == (int)CMEProgramElementID.HazardousWasteRCRALargeQuantityGenerator )
            {
                // If Facility reference is null, bind Inspection to Facility to access other Inspections:
                if ( inspection.Facility == null )
                {
                    inspection.Facility = repository.Facilities.GetByID( inspection.CERSID );
                }

                // If a linked Facility is found, proceed with RCRASequence calculation:
                if ( inspection.Facility != null )
                {
					rcraSequence = CalculateRCRASequence(inspection, repository, cmePackageInspections);
                }
            }
            inspection.RCRASequence = rcraSequence;

            // If CallerContext is UI, set additional fields:
            if ( callerContext == CallerContext.UI )
            {
                // Any time an Inspection is created/updated from the UI, update the RegulatorActionDateTime
                inspection.RegulatorActionDateTime = DateTime.Now;

                // Set the EDTClientKey to a new GUID (EDTClientKey is not supplied through the UI)
                inspection.EDTClientKey = Guid.NewGuid().ToString();
            }

            inspection.SetDDCommonFields( repository.ContextAccountID, creating:true, voided:voided, edtClientKey:inspection.EDTClientKey );
        }

		public static void PreCreate(this Violation violation, ICERSRepositoryManager repository, CallerContext callerContext = CallerContext.UI, bool voided = false, List<Violation> cmePackageViolations = null )
        {
			// Default RCRASequence to zero, proceed with RCRASequence calculation below if this is Violation belongs to a RCRA LQG Inspection
			int rcraSequence = 0;

            // If Inspection reference is null, bind Violation to Inspection to access other Violations:
            if ( violation.Inspection == null )
            {
                violation.Inspection = repository.Inspections.GetByID( violation.InspectionID );
            }

			// Make sure Inspection Violation Counts are up-to-date and calculate RCRASequence if this is Violation belongs to a RCRA LQG Inspection:
            if ( violation.Inspection != null )
            {
				if ( violation.ViolationType != null &&
					violation.ViolationType.ViolationCategory.ViolationProgramElementID == (int)ViolationProgramElementType.RCRALargeQuantityGenerator )
				{
					rcraSequence = CalculateRCRASequence(violation, repository, cmePackageViolations);
				}

                // Retrieve Inspection Counts
                var classIViolationCount = violation.Inspection.Violations.Count( v => v.Class == "1" && v.CMEDataStatusID != (int)CMEDataStatus.Deleted && !v.Voided && v.GuidanceMessages.Count( g => g.LevelID == (int)GuidanceLevel.Required ) == 0 );
                var classIIViolationCount = violation.Inspection.Violations.Count( v => v.Class == "2" && v.CMEDataStatusID != (int)CMEDataStatus.Deleted && !v.Voided && v.GuidanceMessages.Count( g => g.LevelID == (int)GuidanceLevel.Required ) == 0 );
                var minorViolationCount = violation.Inspection.Violations.Count( v => v.Class == "9" && v.CMEDataStatusID != (int)CMEDataStatus.Deleted && !v.Voided && v.GuidanceMessages.Count( g => g.LevelID == (int)GuidanceLevel.Required ) == 0 );

                // If these do not match the Summary Counts on the Inspection, update the Inspection:
                if ( classIViolationCount != violation.Inspection.ClassIViolationCount ||
                    classIIViolationCount != violation.Inspection.ClassIIViolationCount ||
                    minorViolationCount != violation.Inspection.MinorViolationCount )
                {
                    violation.Inspection.ClassIViolationCount = classIViolationCount;
                    violation.Inspection.ClassIIViolationCount = classIIViolationCount;
                    violation.Inspection.MinorViolationCount = minorViolationCount;
                    violation.Inspection.SetDDCommonFields( repository.ContextAccountID, creating:violation.Inspection.ID == 0, voided:violation.Inspection.Voided, edtClientKey:violation.Inspection.EDTClientKey );
                }

                // Make sure CME Batch counts are up to date as well:
                if ( violation.Inspection.CMEBatch != null )
                {
                    violation.Inspection.CMEBatch.PopulateCMEEntityCounts();
                }
            }

			violation.RCRASequence = rcraSequence;

            // If CallerContext is UI, set additional fields:
            if ( callerContext == CallerContext.UI )
            {
                // Any time a Violation is created/updated from the UI, update the RegulatorActionDateTime
                violation.RegulatorActionDateTime = DateTime.Now;

                // Set the EDTClientKey to a new GUID (EDTClientKey is not supplied through the UI)
                violation.EDTClientKey = Guid.NewGuid().ToString();
            }

            violation.SetDDCommonFields( repository.ContextAccountID, creating:true, voided:voided, edtClientKey:violation.EDTClientKey );
        }

        public static void PreCreate( this Enforcement enforcement, ICERSRepositoryManager repository, CallerContext callerContext = CallerContext.UI, bool voided = false, List<Enforcement> cmePackageEnforcements = null )
        {
            // If CMEBatchID is not specified, create a CMEBatch record
            // to track this transaction:
            if ( enforcement.CMEBatch == null )
            {
                CMEBatch cmeBatch = new CMEBatch();
                enforcement.CMEBatch = cmeBatch;
            }
            if ( enforcement.CMEBatchID == 0 )
            {
                enforcement.CMEBatch.SetCommonFields( repository.ContextAccountID, creating:true );
            }
            enforcement.CMEBatch.PopulateCMEEntityCounts();
            enforcement.CMEBatch.CallerContextID = (int)callerContext;

            // If Facility reference is null, bind Enforcement to Facility to access other Enforcements:
            if ( enforcement.Facility == null )
            {
                enforcement.Facility = repository.Facilities.GetByID( enforcement.CERSID );
            }

			//Calculate RCRA Sequence Number
            enforcement.CalculateRCRASequence( repository, cmePackageEnforcements );
			
            // Set CERS Unique Key (Enforcement.Key) to new GUID:
            enforcement.Key = Guid.NewGuid();

            // If CallerContext is UI, set additional fields:
            if ( callerContext == CallerContext.UI )
            {
                // Any time an Enforcement is created/updated from the UI, update the RegulatorActionDateTime
                enforcement.RegulatorActionDateTime = DateTime.Now;

                // Set the EDTClientKey to a new GUID (EDTClientKey is not supplied through the UI)
                enforcement.EDTClientKey = Guid.NewGuid().ToString();
            }

            enforcement.SetDDCommonFields( repository.ContextAccountID, creating:true, voided:voided, edtClientKey:enforcement.EDTClientKey );
        }

        public static void PreCreate( this EnforcementViolation enforcementViolation, ICERSRepositoryManager repository, CallerContext callerContext = CallerContext.UI, bool voided = false )
        {
            // If CMEBatchID is not specified, create a CMEBatch record
            // to track this transaction:
            if ( enforcementViolation.CMEBatch == null )
            {
                CMEBatch cmeBatch = new CMEBatch();
                enforcementViolation.CMEBatch = cmeBatch;
            }
            if ( enforcementViolation.CMEBatchID == 0 )
            {
                enforcementViolation.CMEBatch.SetCommonFields( repository.ContextAccountID, creating:true );
            }
            enforcementViolation.CMEBatch.PopulateCMEEntityCounts();
            enforcementViolation.CMEBatch.CallerContextID = (int)callerContext;

            // Set CERS Unique Key (EnforcementViolation.Key) to new GUID:
            enforcementViolation.Key = Guid.NewGuid();

            // If CallerContext is UI, set additional fields:
            if ( callerContext == CallerContext.UI )
            {
                // Any time an EnforcementViolation is created/updated from the UI, update the RegulatorActionDateTime
                enforcementViolation.RegulatorActionDateTime = DateTime.Now;

                // Set the EDTClientKey to a new GUID (EDTClientKey is not supplied through the UI)
                enforcementViolation.EDTClientKey = Guid.NewGuid().ToString();
            }

            enforcementViolation.SetDDCommonFields( repository.ContextAccountID, creating:true, voided:voided, edtClientKey:enforcementViolation.EDTClientKey );
        }

        #endregion CME PreCreate Methods

        #region CME PreUpdate Methods

		public static void PreUpdate(this Inspection inspection, ICERSRepositoryManager repository, CallerContext callerContext = CallerContext.UI, bool voided = false, List<Inspection> cmePackageInspections = null)
        {
            // If CallerContext is UI, set additional fields:
            if ( callerContext == CallerContext.UI )
            {
                // Any time an Inspection is created/updated from the UI, update the RegulatorActionDateTime
                inspection.RegulatorActionDateTime = DateTime.Now;

                // If this Inspection is linked to an EDT Transaction through CMEBatch.EDTTransmissionID,
                // spawn a new non-EDT CMEBatch and link this Inspection to the new CMEBatch
                if ( inspection.CMEBatch.EDTTransmissionID.HasValue )
                {
                    CMEBatch cmeBatch = new CMEBatch();
                    inspection.CMEBatch = cmeBatch;
                    inspection.CMEBatch.PopulateCMEEntityCounts();
                    inspection.CMEBatch.CallerContextID = (int)callerContext;
                    inspection.CMEBatch.SetCommonFields( repository.ContextAccountID, creating:true );
                }
            }

            // If Violation child entities are linked under a different Regulator ID
            // (this would happen if the user updates the Inspection's Regulator), update
            // the Violation child to belong to the same Regulator as the Inspection:
            foreach ( var violation in inspection.Violations.Where( v => !v.Voided ) )
            {
                if ( inspection.RegulatorID != violation.RegulatorID )
                {
                    violation.RegulatorID = inspection.RegulatorID;
                    violation.SetCommonFields( repository.ContextAccountID );
                }
            }

            inspection.SetDDCommonFields( repository.ContextAccountID, creating:false, voided:voided );

			//Need to get the original value of the OccurredOn property for this entity before it was modified
			var stateEntry = repository.DataModel.ObjectStateManager.GetObjectStateEntry(inspection);
			DateTime originalOccurredOn = (DateTime)stateEntry.OriginalValues["OccurredOn"];

			//Call Calculate RCRA sequence number method if this is a RCRA LQG inspection and the composite key has changed. Composite key consists of
			//the EPA ID number, Occurred On date, and RCRA Sequence number. The Occurred On date is the only element that can be changed by the user, so we will check to 
			//see if it has changed. If so, that constitutes a key change.
			if ( inspection.CMEProgramElementID == (int)CMEProgramElementID.HazardousWasteRCRALargeQuantityGenerator
				&& inspection.OccurredOn != originalOccurredOn  )
			{
				inspection.RCRASequence = CalculateRCRASequence(inspection, repository, cmePackageInspections);
			}

        }

        public static void PreUpdate( this Violation violation, ICERSRepositoryManager repository, CallerContext callerContext = CallerContext.UI, bool voided = false )
        {
            // If CallerContext is UI, set additional fields:
            if ( callerContext == CallerContext.UI )
            {
                // Any time a Violation is created/updated from the UI, update the RegulatorActionDateTime
                violation.RegulatorActionDateTime = DateTime.Now;

                // If this Violation is linked to an EDT Transaction through CMEBatch.EDTTransmissionID,
                // spawn a new non-EDT CMEBatch and link this Violation to the new CMEBatch
                if ( violation.CMEBatch.EDTTransmissionID.HasValue )
                {
                    CMEBatch cmeBatch = new CMEBatch();
                    violation.CMEBatch = cmeBatch;
                    violation.CMEBatch.PopulateCMEEntityCounts();
                    violation.CMEBatch.CallerContextID = (int)callerContext;
                    violation.CMEBatch.SetCommonFields( repository.ContextAccountID, creating:true );
                }
            }

            // Make sure Inspection Violation Counts are up-to-date:
            if ( violation.Inspection != null )
            {
                // Retrieve Inspection Counts
                var classIViolationCount = violation.Inspection.Violations.Count( v => v.Class == "1" && v.CMEDataStatusID != (int)CMEDataStatus.Deleted && !v.Voided && v.GuidanceMessages.Count( g => g.LevelID == (int)GuidanceLevel.Required ) == 0 );
                var classIIViolationCount = violation.Inspection.Violations.Count( v => v.Class == "2" && v.CMEDataStatusID != (int)CMEDataStatus.Deleted && !v.Voided && v.GuidanceMessages.Count( g => g.LevelID == (int)GuidanceLevel.Required ) == 0 );
                var minorViolationCount = violation.Inspection.Violations.Count( v => v.Class == "9" && v.CMEDataStatusID != (int)CMEDataStatus.Deleted && !v.Voided && v.GuidanceMessages.Count( g => g.LevelID == (int)GuidanceLevel.Required ) == 0 );

                // If these do not match the Summary Counts on the Inspection, update the Inspection:
                if ( classIViolationCount != violation.Inspection.ClassIViolationCount ||
                    classIIViolationCount != violation.Inspection.ClassIIViolationCount ||
                    minorViolationCount != violation.Inspection.MinorViolationCount )
                {
                    violation.Inspection.ClassIViolationCount = classIViolationCount;
                    violation.Inspection.ClassIIViolationCount = classIIViolationCount;
                    violation.Inspection.MinorViolationCount = minorViolationCount;
                    violation.Inspection.SetDDCommonFields( repository.ContextAccountID, creating:violation.Inspection.ID == 0, voided:violation.Inspection.Voided, edtClientKey:violation.Inspection.EDTClientKey );
                }
            }

            // TODO: Add advanced RCRA-Logic such that modification of any RCRA Primary Key fields deletes the current record and spawns a copy with a new RCRASequence
			//John Miller 9/9/2015: It is not necessary to add RCRA-specific logic here, as the key can not be changed by user input

            violation.SetDDCommonFields( repository.ContextAccountID, creating:false, voided:voided );
        }

        public static void PreUpdate( this Enforcement enforcement, ICERSRepositoryManager repository, CallerContext callerContext = CallerContext.UI, bool voided = false, List<Enforcement> cmePackageEnforcements = null )
        {
            // If CallerContext is UI, set additional fields:
            if ( callerContext == CallerContext.UI )
            {
                // Any time an Enforcement is created/updated from the UI, update the RegulatorActionDateTime
                enforcement.RegulatorActionDateTime = DateTime.Now;

                // If this Enforcement is linked to an EDT Transaction through CMEBatch.EDTTransmissionID,
                // spawn a new non-EDT CMEBatch and link this Enforcement to the new CMEBatch
                if ( enforcement.CMEBatch.EDTTransmissionID.HasValue )
                {
                    CMEBatch cmeBatch = new CMEBatch();
                    enforcement.CMEBatch = cmeBatch;
                    enforcement.CMEBatch.PopulateCMEEntityCounts();
                    enforcement.CMEBatch.CallerContextID = (int)callerContext;
                    enforcement.CMEBatch.SetCommonFields( repository.ContextAccountID, creating:true );
                }
            }

            // If EnforcementViolation child entities are linked under a different Regulator ID
            // (this would happen if the user updates the Enforcement's Regulator), update
            // the EnforcementViolation child to belong to the same Regulator as the Enforcement:
            foreach ( var enforcementViolation in enforcement.EnforcementViolations.Where( ev => !ev.Voided ) )
            {
                if ( enforcement.RegulatorID != enforcementViolation.RegulatorID )
                {
                    enforcementViolation.RegulatorID = enforcement.RegulatorID;
                    enforcementViolation.SetCommonFields( repository.ContextAccountID );
                }
            }

            // TODO: Add advanced RCRA-Logic such that modification of any RCRA Primary Key fields deletes the current record and spawns a copy with a new RCRASequence

			//Need to get the original value of the OccurredOn property for this entity before it was modified
			var stateEntry = repository.DataModel.ObjectStateManager.GetObjectStateEntry(enforcement);
			DateTime originalOccurredOn = (DateTime)stateEntry.OriginalValues["OccurredOn"];

			//Call Calculate RCRA sequence number method if the composite key has changed. Composite key consists of
			//the EPA ID number, Occurred On date, and RCRA Sequence number. The Occurred On date is the only element that can be changed by the user, so we will check to 
			//see if it has changed. If so, that constitutes a key change.
			if ( enforcement.OccurredOn != originalOccurredOn )
			{
				enforcement.CalculateRCRASequence( repository, cmePackageEnforcements );
			}

            enforcement.SetDDCommonFields( repository.ContextAccountID, creating:false, voided:voided );
        }

        public static void PreUpdate( this EnforcementViolation enforcementViolation, ICERSRepositoryManager repository, CallerContext callerContext = CallerContext.UI, bool voided = false )
        {
            // If CallerContext is UI, set additional fields:
            if ( callerContext == CallerContext.UI )
            {
                // Any time a Violation is created/updated from the UI, update the RegulatorActionDateTime
                enforcementViolation.RegulatorActionDateTime = DateTime.Now;

                // If this Violation is linked to an EDT Transaction through CMEBatch.EDTTransmissionID,
                // spawn a new non-EDT CMEBatch and link this Violation to the new CMEBatch
                if ( enforcementViolation.CMEBatch.EDTTransmissionID.HasValue )
                {
                    CMEBatch cmeBatch = new CMEBatch();
                    enforcementViolation.CMEBatch = cmeBatch;
                    enforcementViolation.CMEBatch.PopulateCMEEntityCounts();
                    enforcementViolation.CMEBatch.CallerContextID = (int)callerContext;
                    enforcementViolation.CMEBatch.SetCommonFields( repository.ContextAccountID, creating:true );
                }
            }

            // TODO: Add advanced RCRA-Logic such that modification of any RCRA Primary Key fields deletes the current record and spawns a copy with a new RCRASequence

            enforcementViolation.SetDDCommonFields( repository.ContextAccountID, creating:false, voided:voided );
        }

        #endregion CME PreUpdate Methods

        #region CME Historical Entity Translation Methods

        #region CME InspectionHistory-to-Inspection Translation Methods

        /// <summary>
        /// Translates an InspectionHistory Entity into an Inspection Entity.  This method is primarily used
        /// to help rebuild a CMEPackage based on an old EDT Transaction, in which some of the CME Entities
        /// have since been modified (and therefore copied into the corresponding "...History" table).
        /// </summary>
        /// <param name="inspectionHistory"></param>
        /// <returns></returns>
        public static Inspection ToInspection( this InspectionHistory inspectionHistory )
        {
            Inspection inspection = new Inspection();

            inspection.ID = inspectionHistory.InspectionID;
            inspection.Key = inspectionHistory.Key;
            inspection.CERSID = inspectionHistory.CERSID;
            inspection.RegulatorID = inspectionHistory.RegulatorID;
            inspection.CMEDataStatusID = inspectionHistory.CMEDataStatusID;
            inspection.CMEBatchID = inspectionHistory.CMEBatchID;
            inspection.RegulatorActionDateTime = inspectionHistory.RegulatorActionDateTime;
            inspection.RCRASequence = inspectionHistory.RCRASequence;
            inspection.CMEProgramElementID = inspectionHistory.CMEProgramElementID;
            inspection.OccurredOn = inspectionHistory.OccurredOn;
            inspection.Type = inspectionHistory.Type;
            inspection.ClassIViolationCount = inspectionHistory.ClassIViolationCount;
            inspection.ClassIIViolationCount = inspectionHistory.ClassIIViolationCount;
            inspection.MinorViolationCount = inspectionHistory.MinorViolationCount;
            inspection.SOCDetermination = inspectionHistory.SOCDetermination;
            inspection.ViolationsRTCOn = inspectionHistory.ViolationsRTCOn;
            inspection.Comment = inspectionHistory.Comment;
            inspection.EDTClientKey = inspectionHistory.EDTClientKey;
            inspection.CreatedOn = inspectionHistory.CreatedOn;
            inspection.CreatedByID = inspectionHistory.CreatedByID;
            inspection.UpdatedOn = inspectionHistory.UpdatedOn;
            inspection.UpdatedByID = inspectionHistory.UpdatedByID;
            inspection.Voided = inspectionHistory.Voided;

            return inspection;
        }

        public static List<Inspection> ToInspections( this IEnumerable<InspectionHistory> inspectionHistories )
        {
            List<Inspection> inspections = new List<Inspection>();
            foreach ( var inspectionHistory in inspectionHistories )
            {
                inspections.Add( inspectionHistory.ToInspection() );
            }
            return inspections;
        }

        #endregion CME InspectionHistory-to-Inspection Translation Methods

        #region CME ViolationHistory-to-Violation Translation Methods

        public static Violation ToViolation( this ViolationHistory violationHistory )
        {
            Violation violation = new Violation();

            // TODO: Build Violation from ViolationHistory Entity
            return violation;
        }

        public static List<Violation> ToViolations( this IEnumerable<ViolationHistory> violationHistories )
        {
            List<Violation> violations = new List<Violation>();
            foreach ( var violationHistory in violationHistories )
            {
                //violations.Add(violationHistory.ToViolation());
            }
            return violations;
        }

        #endregion CME ViolationHistory-to-Violation Translation Methods

        #region CME EnforcementHistory-to-Enforcement Translation Methods

        public static Enforcement ToEnforcement( this EnforcementHistory enforcementHistory )
        {
            Enforcement enforcement = new Enforcement();

            // TODO: Build Enforcement from EnforcementHistory Entity
            return enforcement;
        }

        public static List<Enforcement> ToEnforcements( this IEnumerable<EnforcementHistory> enforcementHistories )
        {
            List<Enforcement> enforcements = new List<Enforcement>();
            foreach ( var enforcementHistory in enforcementHistories )
            {
                //enforcements.Add(enforcementHistory.ToEnforcement());
            }
            return enforcements;
        }

        #endregion CME EnforcementHistory-to-Enforcement Translation Methods

        #region CME EnforcementViolationHistory-to-EnforcementViolation Translation Methods

        public static EnforcementViolation ToEnforcementViolation( this EnforcementViolationHistory enforcementViolationHistory )
        {
            EnforcementViolation enforcementViolation = new EnforcementViolation();

            // TODO: Build EnforcementViolation from EnforcementViolationHistory Entity
            return enforcementViolation;
        }

        public static List<EnforcementViolation> ToEnforcementViolations( this IEnumerable<EnforcementViolationHistory> enforcementViolationHistories )
        {
            List<EnforcementViolation> enforcementViolations = new List<EnforcementViolation>();
            foreach ( var enforcementViolationHistory in enforcementViolationHistories )
            {
                //enforcementViolations.Add(enforcementViolationHistory.ToEnforcementViolation());
            }
            return enforcementViolations;
        }

        #endregion CME EnforcementViolationHistory-to-EnforcementViolation Translation Methods

        #endregion CME Historical Entity Translation Methods

        #region Populate CME Entity Counts

        /// <summary>
        /// Counts CME Data Entities (Inspections, Violations, Enforcements, EnforcementViolations) linked
        /// to a CMEBatch, and updates the CMEBatch's ...Count properties accordingly.
        /// </summary>
        /// <param name="cmeBatch"></param>
        /// <param name="includeVoidedRecords"></param>
        public static void PopulateCMEEntityCounts( this CMEBatch cmeBatch, bool includeDeletedRecords = false )
        {
            if ( cmeBatch == null )
            {
                throw new ArgumentNullException( "cmeBatch" );
            }

            if ( includeDeletedRecords )
            {
                cmeBatch.InspectionCount = cmeBatch.Inspections.Count;
                cmeBatch.ViolationCount = cmeBatch.Violations.Count;
                cmeBatch.EnforcementCount = cmeBatch.Enforcements.Count;
                cmeBatch.EnforcementViolationCount = cmeBatch.EnforcementViolations.Count;
            }
            else
            {
                cmeBatch.InspectionCount = cmeBatch.Inspections.Count( i => i.CMEDataStatusID != (int)CMEDataStatus.Deleted );
                cmeBatch.ViolationCount = cmeBatch.Violations.Count( v => v.CMEDataStatusID != (int)CMEDataStatus.Deleted );
                cmeBatch.EnforcementCount = cmeBatch.Enforcements.Count( e => e.CMEDataStatusID != (int)CMEDataStatus.Deleted );
                cmeBatch.EnforcementViolationCount = cmeBatch.EnforcementViolations.Count( ev => ev.CMEDataStatusID != (int)CMEDataStatus.Deleted );
            }
        }

        #endregion Populate CME Entity Counts

        #region GetHMISPageCount Method

        // Determine page count for HMIS Matrix Report (8-per page) grouped by
        // Location, EPCRA Confidential Location Flag, and Trade Secret Flag
        public static int GetHMISPageCount( this IEnumerable<BPFacilityChemical> chemicals )
        {
            int MAX_CHEMICAL_PER_PAGE = 8;
            int totalPageCount = 0;
            int quotient = 0;
            int mod = 0;
            var locations = chemicals.Select( c => new { ChemicalLocation = c.ChemicalLocation == null ? string.Empty : c.ChemicalLocation.Trim().ToUpper() } ).Distinct();

            foreach ( var location in locations )
            {
                //reset quotient and mod
                quotient = 0;
                mod = 0;

                chemicals = chemicals
                    .Where( c =>
                        ( ( c.ChemicalLocation == null && location.ChemicalLocation == null )
                        || ( c.ChemicalLocation != null
                            && location.ChemicalLocation != null
                            && c.ChemicalLocation.Trim().ToUpper().Equals( location.ChemicalLocation.Trim().ToUpper() ) ) )
                            );

                quotient = Math.DivRem( chemicals.Count(), MAX_CHEMICAL_PER_PAGE, out quotient );
                quotient = ( quotient == 0 ) ? quotient + 1 : quotient;
                if ( chemicals.Count() > MAX_CHEMICAL_PER_PAGE )
                {
                    mod = chemicals.Count() % MAX_CHEMICAL_PER_PAGE;
                    mod = ( mod == 0 ) ? 0 : 1;
                }

                totalPageCount += quotient + mod;
            }

            return totalPageCount;
        }

        #endregion GetHMISPageCount Method

        #region Contains [GuidanceMessage]

        public static bool Exists( this IEnumerable<GuidanceMessage> list, GuidanceMessage gm )
        {
            bool result = false;

            string message = gm.Message.ToLower().Trim();
            GuidanceLevel level = (GuidanceLevel)gm.LevelID;

            var formatted = from l in list
                            select new
                            {
                                Message = l.Message.ToLower().Trim(),
                                Level = (GuidanceLevel)l.LevelID
                            };

            var count = formatted.Count( p => p.Message == message && p.Level == level );
            result = count > 0;

            return result;
        }

        #endregion Contains [GuidanceMessage]

        #region Contains [GuidanceMessage]

        public static GuidanceMessage TryFind( this IEnumerable<GuidanceMessage> list, GuidanceMessage gm )
        {
            GuidanceMessage result = null;

            string message = gm.Message.ToLower().Trim();
            int level = gm.LevelID;

            result = list.FirstOrDefault( p => p.Message.ToLower().Trim() == message && p.LevelID == level && p.ResourceTypeID == gm.ResourceTypeID );

            return result;
        }

        #endregion Contains [GuidanceMessage]

		#region Calulate RCRA Sequence Method

		//John Miller 8/24/2015:
		//Per CAL EPA and US EPA agreement regarding the RCRA sequence number assignment and ensuring unique composite keys, we are redesigning
		//the way RCRA sequence numbers are assigned and how the composite key is built during an Inspection create operation. From US EPA perspective, 
		//the composite key consists of the EPAID, Occurred On, and RCRA Sequence Number. The CERSID cannot be an element of the key because we could have 
		//several CERSIDs under the same EPA ID. Using the previous logic, wach of these CERSIDs would have been assigned RCRASequence 601. These would have 
		//been seen as duplicates by US EPA, as they all have the same handler ID. We will now assign the RCRA sequence using the EPAID instead of the CERSID

		public static int CalculateRCRASequence(Inspection inspection, ICERSRepositoryManager repository, List<Inspection> cmePackageInspections = null)
		{
			int rcraSequence;
			int? maxRcraSequence = null;

			// NOTE: When searching for RCRA Sequences for this EPAID on the same day,
			// we are *including* Voided records.  Reasoning - It may be possible that
			// we have sent a now-Voided record to RCRA, and we do not want to re-use
			// that RCRASequence.

			//Need to get all sequence numbers from both the Inspection and InspectionHistory tables that have ever been assigned for this unique EPAID/OccurredOn key,
			//merge them into one array, and then take the largest value. This ensures that we never reuse a key.
			var rcraSequences = 
				repository.DataModel.Inspections
				.Where
				(
					i => i.Facility.EPAID == inspection.Facility.EPAID
					&& i.OccurredOn == inspection.OccurredOn
					&& i.CMEProgramElementID == inspection.CMEProgramElementID
				)
				.Select(i => i.RCRASequence)
				.Union
				(
					repository.DataModel.InspectionHistories
					.Where(ih => ih.Facility.EPAID == inspection.Facility.EPAID
					&& ih.OccurredOn == inspection.OccurredOn
					&& ih.CMEProgramElementID == inspection.CMEProgramElementID
				)
				.Select(ih => ih.RCRASequence));

			//Now need to check the RCRA Sequence numbers we have already assigned to Inspections in the CME Package if coming in via EDT or CME Upload
			var rcraSequencesCMEPackage = cmePackageInspections != null ? cmePackageInspections
				.Where(cmepi => cmepi.Facility != null
                    && cmepi.Facility.EPAID == inspection.Facility.EPAID
					&& cmepi.OccurredOn == inspection.OccurredOn
					&& cmepi.CMEProgramElementID == inspection.CMEProgramElementID
					&& cmepi != inspection)
					.Select(cmepi => cmepi.RCRASequence) : null;

			//Need Union for RCRA Sequence numbers that have been assigned in current CME package if this Inspection is part of a CME batch
			//coming in via EDT or CME Upload
			rcraSequences =
				rcraSequencesCMEPackage == null ? rcraSequences :
				rcraSequences.Union(rcraSequencesCMEPackage);
			
			// Set maxRcraSequence only if elements are returned:
			if ( rcraSequences.Any() )
			{
				maxRcraSequence = rcraSequences.Max();
			}

			// If a maxRcraSequence was not found, or one was found less than 601 (invalid for
			// Unified Program LQG Inspections), then start at 601:
			if ( maxRcraSequence == null || maxRcraSequence.Value < 601 )
			{
				rcraSequence = 601;
			}
			else
			{
				rcraSequence = maxRcraSequence.Value + 1;
			}

			return rcraSequence;
		}

		public static int CalculateRCRASequence(Violation violation, ICERSRepositoryManager repository, List<Violation> cmePackageViolations = null)
		{
			int rcraSequence;
			int? maxRcraSequence = null;

			// NOTE: When searching for RCRA Sequences for this EPAID,
			// we are *including* Voided records.  Reasoning - It may be possible that
			// we have sent a now-Voided record to RCRA, and we do not want to re-use
			// that RCRASequence.

			//Note: Do not need to include a union with ViolationHistory because the RCRA sequence number can never be changed after it is assigned 
			var rcraSequences = repository.DataModel.Violations
			.Where(v => v.Inspection.Facility.EPAID == violation.Inspection.Facility.EPAID
				&& v.ViolationType.ViolationCategory.ViolationProgramElementID == (int)ViolationProgramElementType.RCRALargeQuantityGenerator )
				.Select(v => v.RCRASequence);

			//Now need to check the RCRA Sequence numbers we have already assigned to Violations in the CME Package if coming in via EDT or CME Upload
			var rcraSequencesCMEPackage = cmePackageViolations != null ? cmePackageViolations
				.Where(cmepv => cmepv.ViolationType != null
                    && cmepv.Inspection != null
                    && cmepv.Inspection.Facility.EPAID == violation.Inspection.Facility.EPAID
					&& cmepv.ViolationType.ViolationCategory.ViolationProgramElementID == violation.ViolationType.ViolationCategory.ViolationProgramElementID
					&& cmepv != violation )
					.Select(cmepv => cmepv.RCRASequence ) : null;

			//Need Union for RCRA Sequence numbers that have been assigned in current CME package if this Violation is part of a CME package
			//coming in via EDT or CME Upload
			rcraSequences =
				rcraSequencesCMEPackage == null ? rcraSequences :
				rcraSequences.Union(rcraSequencesCMEPackage);

			// Set maxRcraSequence only if elements are returned:
			if ( rcraSequences.Any() )
			{
				maxRcraSequence = rcraSequences.Max();
			}

			// If a maxRcraSequence was not found, or one was found less than 6001, then start at 6001:
			if ( maxRcraSequence == null || maxRcraSequence.Value < 6001 )
			{
				rcraSequence = 6001;
			}
			else
			{
				rcraSequence = maxRcraSequence.Value + 1;
			}

			return rcraSequence;
		}

		public static void CalculateRCRASequence(this Enforcement enforcement, ICERSRepositoryManager repository, List<Enforcement> cmePackageEnforcements = null )
		{
			//int rcraSequence;
			int? maxRcraSequence = null;

			if ( enforcement.Facility == null )
			{
				enforcement.RCRASequence = 0;
			}
			else
			{
				// NOTE: When searching for RCRA Sequences for this EPAID on the same day,
				// we are *including* Voided records.  Reasoning - It may be possible that
				// we have sent a now-Voided record to RCRA, and we do not want to re-use
				// that RCRASequence.

				//Need to get all sequence numbers from both the Enforcement and EnforcementHistory tables that have ever been assigned for this unique EPAID/OccurredOn key,
				//merge them into one array, and then take the largest value. This ensures that we never reuse a key.
				var rcraSequences =
					repository.DataModel.Enforcements
					.Where
					(
						e => e.Facility.EPAID == enforcement.Facility.EPAID
						&& e.OccurredOn == enforcement.OccurredOn
					)
					.Select(e => e.RCRASequence)
					.Union
					(
						repository.DataModel.EnforcementHistories
						.Where(eh => eh.Facility.EPAID == enforcement.Facility.EPAID
						&& eh.OccurredOn == enforcement.OccurredOn
					)
					.Select(eh => eh.RCRASequence));


				//Now need to check the RCRA Sequence numbers we have already assigned to Enforcements in the CME Package if coming in via EDT or CME Upload
				var rcraSequencesCMEPackage = cmePackageEnforcements != null ? cmePackageEnforcements
					.Where(cmepe => cmepe.Facility.EPAID == enforcement.Facility.EPAID
						&& cmepe.OccurredOn == enforcement.OccurredOn
						&& cmepe != enforcement)
						.Select(cmepe => cmepe.RCRASequence) : null;

				//Need Union for RCRA Sequence numbers that have been assigned in current CME package if this Enforcement is part of a CME package
				//coming in via EDT or CME Upload
				rcraSequences =
					rcraSequencesCMEPackage == null ? rcraSequences :
					rcraSequences.Union(rcraSequencesCMEPackage);

				// Set maxRcraSequence only if elements are returned:
				if ( rcraSequences.Any() )
				{
					maxRcraSequence = rcraSequences.Max();
				}

				// If a maxRcraSequence was not found, or one was found less than 601, then start at 601:
				if ( maxRcraSequence == null || maxRcraSequence.Value < 601 )
				{
					enforcement.RCRASequence = 601;
				}
				else
				{
					enforcement.RCRASequence = maxRcraSequence.Value + 1;
				}
			}
		}
		#endregion

	}
}