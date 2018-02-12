using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CERS
{
    public interface IDocumentSource : ISystemLookupEntity
    {
        int SortOrder { get; set; }
    }
}