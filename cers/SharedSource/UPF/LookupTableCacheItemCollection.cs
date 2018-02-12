using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace UPF
{
    public class LookupTableCacheItemCollection<T> : Collection<LookupTableCacheItem<T>> where T : ILookupEntity
    {

        public LookupTableCacheItem<T> Add(string name, IEnumerable<T> values)
        {
            List<T> newValues = new List<T>();
            newValues.AddRange(values);
            LookupTableCacheItem<T> item = new LookupTableCacheItem<T>(name, newValues);
            this.Add(item);
            return item;
        }

        public List<T> this[string name]
        {
            get
            {
                List<T> results = null;
                foreach (var item in this)
                {
                    if (string.Compare(item.Name, name, true) == 0)
                    {
                        results = item.Values;
                        break;
                    }
                }
                return results;
            }
        }

    }
}
