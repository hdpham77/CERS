using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CERS.Model;
using UPF;

namespace CERS
{
	public interface IDDModelEntity : IModelEntityWithID
	{
		Guid Key { get; set; }

		string EDTClientKey { get; set; }
	}
}