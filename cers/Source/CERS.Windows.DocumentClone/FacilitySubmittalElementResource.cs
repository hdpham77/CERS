//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CERS.Windows.DocumentClone
{
    using System;
    using System.Collections.Generic;
    
    public partial class FacilitySubmittalElementResource
    {
        public FacilitySubmittalElementResource()
        {
            this.Children = new HashSet<FacilitySubmittalElementResource>();
            this.Documents = new HashSet<FacilitySubmittalElementResourceDocument>();
        }
    
        public int ID { get; set; }
        public int FacilitySubmittalElementID { get; set; }
        public int TemplateResourceID { get; set; }
        public Nullable<int> ParentResourceID { get; set; }
        public int ResourceTypeID { get; set; }
        public Nullable<int> LastSubmittalDeltaID { get; set; }
        public int Order { get; set; }
        public bool IsStarted { get; set; }
        public bool IsDocument { get; set; }
        public bool IsRequired { get; set; }
        public Nullable<int> DocumentSourceID { get; set; }
        public bool MinRequiredFieldsSubmitted { get; set; }
        public int RequiredGuidanceMessageCount { get; set; }
        public int WarningGuidanceMessageCount { get; set; }
        public int AdvisoryGuidanceMessageCount { get; set; }
        public Nullable<int> ResourceEntityCount { get; set; }
        public string DisplayName { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public int CreatedByID { get; set; }
        public System.DateTime UpdatedOn { get; set; }
        public int UpdatedByID { get; set; }
        public bool Voided { get; set; }
    
        public virtual FacilitySubmittalElement FacilitySubmittalElement { get; set; }
        public virtual ICollection<FacilitySubmittalElementResource> Children { get; set; }
        public virtual FacilitySubmittalElementResource Parent { get; set; }
        public virtual ICollection<FacilitySubmittalElementResourceDocument> Documents { get; set; }
    }
}
