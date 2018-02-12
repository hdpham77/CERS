using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UPF {
	public interface ICommonFields {

		int CreatedBy { get; set; }
		DateTime CreatedOn { get; set; }
		int UpdatedBy { get; set; }
		DateTime UpdatedOn { get; set; }
		bool Voided { get; set; }
	}
}