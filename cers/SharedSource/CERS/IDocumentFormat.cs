using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CERS
{
    public interface IDocumentFormat : ISystemLookupEntity
    {
        string FileExtensions { get; set; }
        bool? IsAllowed { get; set; }
    }
}
