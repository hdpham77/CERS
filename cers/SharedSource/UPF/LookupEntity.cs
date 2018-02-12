using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UPF
{
    public class LookupEntity : ILookupEntity
    {
        public string Name
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public int ID
        {
            get;
            set;
        }

        public int CreatedByID
        {
            get;
            set;
        }

        public DateTime CreatedOn
        {
            get;
            set;
        }

        public int UpdatedByID
        {
            get;
            set;
        }

        public DateTime UpdatedOn
        {
            get;
            set;
        }

        public bool Voided
        {
            get;
            set;
        }
    }
}