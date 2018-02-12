using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UPF
{
    public class DocumentStorageLocation
    {
        public string VirtualPath { get; set; }
        public string PhysicalPath { get; set; }

        public DocumentStorageLocation()
        {

        }

        public DocumentStorageLocation(string virtualPath, string physicalPath)
        {
            VirtualPath = virtualPath;
            PhysicalPath = physicalPath;
        }

    }
}
