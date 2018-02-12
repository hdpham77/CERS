using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CERS
{
    public interface IDocumentStorageProvider : ISystemLookupEntity
    {
        string ProviderName { get; set; }
    }
}
