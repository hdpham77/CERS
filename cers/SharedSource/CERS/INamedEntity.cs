using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UPF;

namespace CERS
{
    public interface INamedEntity : IModelEntityWithID
    {
        string Name { get; set; }
    }
}