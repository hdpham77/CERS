using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UPF
{
    public class LookupTableCacheItem<T> where T : ILookupEntity
    {
        public LookupTableCacheItem()
        {
            Values = new List<T>();
        }

        public LookupTableCacheItem(string name, List<T> values)
        {
            Name = name;
            Values = values;
        }

        public string Name { get; set; }
        public List<T> Values { get; protected set; }
    }
}
