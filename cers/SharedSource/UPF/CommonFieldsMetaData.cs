using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace UPF {
	public class CommonFieldsMetaData {
		[DisplayName("Created By:")]
		public int CreatedBy { get; set; }

		[DisplayName("Created On:")]
		public DateTime CreatedOn { get; set; }

		[DisplayName("Updated By:")]
		public int UpdatedBy { get; set; }

		[DisplayName("UpdatedOn:")]
		public DateTime UpdatedOn { get; set; }

		[DisplayName("Deleted")]
		public bool Voided { get; set; }
	}
}