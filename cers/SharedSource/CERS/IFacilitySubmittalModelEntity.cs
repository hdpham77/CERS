using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CERS.Guidance;
using CERS.Model;

namespace CERS
{
    public interface IFacilitySubmittalModelEntity : IDDModelEntity
    {
        FacilitySubmittalElementResource FacilitySubmittalElementResource { get; set; }
    }
}