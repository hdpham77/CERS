using System;
using System.Collections.Generic;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Text;
using UPF;

namespace CERS
{
    public interface IEntityWithContacts<TContactEntity> : INamedEntity where TContactEntity : class, IEntityContact
    {
        EntityCollection<TContactEntity> Contacts { get; set; }
    }
}