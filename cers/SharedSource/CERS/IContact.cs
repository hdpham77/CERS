using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UPF;

namespace CERS
{
	public interface IContact : IModelEntityWithID
	{
		int? AccountID { get; set; }

		string FirstName { get; set; }

		string LastName { get; set; }

		string Email { get; set; }

		string FullName { get; set; }
	}
}