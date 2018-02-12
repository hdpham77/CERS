using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace UPF
{
	public interface IAccount : IModelEntityWithID
	{
		[Display( Name = "Activated" )]
		bool Approved { get; set; }

		[Display( Name = "Common Name" )]
		string CommonName { get; set; }

		bool Disabled { get; set; }

		[Display( Name = "Full Name" )]
		string DisplayName { get; set; }

		[Display( Name = "Email" )]
		string Email { get; set; }

		[Display( Name = "First Name" )]
		string FirstName { get; set; }

		bool IsAnonymous { get; }

		[Display( Name = "Last Name" )]
		string LastName { get; set; }

		bool LockedOut { get; set; }

		string SecondaryEmail { get; set; }

		bool ServiceAccount { get; set; }

		[Display( Name = "Username" )]
		string UserName { get; set; }
	}
}