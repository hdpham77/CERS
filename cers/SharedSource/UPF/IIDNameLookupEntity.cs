using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UPF
{
    public interface IIDNameLookupEntity : IEntityWithID
    {
        string Name { get; set; }
    }
}