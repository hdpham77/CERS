using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CERS
{
    public interface IResourceType : ISystemLookupEntity
    {
        int? SubmittalElementID { get; set; }

        int? DocumentTypeID { get; set; }

        string DisplayName { get; set; }

        DocumentType? DocumentType { get; }
    }
}