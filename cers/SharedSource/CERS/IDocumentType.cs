using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CERS
{
	public interface IDocumentType : ISystemLookupEntity
	{
		Context? Context { get; }

		int? ContextID { get; set; }

		Qualifier? Qualifier { get; }

		int? QualifierID { get; set; }

		bool UPManaged { get; set; }
	}
}