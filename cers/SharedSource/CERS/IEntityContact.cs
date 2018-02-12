using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UPF;

namespace CERS
{
	public interface IEntityContact : IModelEntityWithID
	{
		int ContactID { get; set; }

		string Title { get; set; }

		string Phone { get; set; }
	}
}