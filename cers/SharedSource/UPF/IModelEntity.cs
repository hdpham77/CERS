using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UPF
{
	public interface IModelEntity : IValidatableObject
	{
		int CreatedByID { get; set; }

		DateTime CreatedOn { get; set; }

		int UpdatedByID { get; set; }

		DateTime UpdatedOn { get; set; }

		bool Voided { get; set; }
	}
}