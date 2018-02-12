using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UPF
{
    public class IDNameLookupEntity : IIDNameLookupEntity
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public IDNameLookupEntity()
            : this(-1, "")
        {
        }

        public IDNameLookupEntity(int id, string name)
        {
            ID = id;
            Name = name;
        }
    }
}