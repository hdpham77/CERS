using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UPF;

namespace CERS
{
	public interface IModelEntityWithEDTIdentityKey : IModelEntityWithID
	{
		int EDTIdentityKey { get; set; }
	}
}