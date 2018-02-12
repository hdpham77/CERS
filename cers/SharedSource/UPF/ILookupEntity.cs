using System;
using System.Collections.Generic;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Text;

namespace UPF
{
    public interface ILookupEntity : IModelEntityWithID, IIDNameLookupEntity
    {
        string Description { get; set; }
    }
}