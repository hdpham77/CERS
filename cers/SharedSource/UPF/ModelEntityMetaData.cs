using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace UPF
{
	public class ModelEntityMetaData
	{
		[Display(Name = "Created On")]
		public DateTime CreatedOn { get; set; }

		[Display(Name = "Created By")]
		public int CreatedByID { get; set; }

		[Display(Name = "Last Updated On")]
		public DateTime UpdatedOn { get; set; }

		[Display(Name = "Last Updated By")]
		public int UpdatedByID { get; set; }

		[Display(Name = "Deleted")]
		public bool Voided { get; set; }
	}
}