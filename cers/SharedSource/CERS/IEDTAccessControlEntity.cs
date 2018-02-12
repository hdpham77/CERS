using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CERS
{
	public interface IEDTAccessControlEntity : IModelEntityWithEDTIdentityKey
	{
		bool EDTEnabled { get; set; }
	}
}