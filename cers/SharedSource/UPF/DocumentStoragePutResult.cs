using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UPF
{
    public class DocumentStoragePutResult
    {

        public bool Success { get; set; }

        public string VirtualPath { get; set; }
        public string PhysicalPath { get; set; }
        public int StorageProviderID { get; set; }
        public string DocumentKey { get; set; }

        public string StorageLocationKey
        {
            get
            {
                return VirtualPath + "\\" + DocumentKey;
            }
        }


        public DocumentStoragePutResult()
            : this(false, string.Empty, string.Empty, 0)
        {

        }

        public DocumentStoragePutResult(bool success, string virtualPath, string physicalPath, int storageProviderID)
        {
            Success = success;
            VirtualPath = virtualPath;
            PhysicalPath = physicalPath;
            StorageProviderID = storageProviderID;
        }
    }
}
