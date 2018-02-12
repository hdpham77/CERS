using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UPF.Core
{
	public class AuthenticationResult
	{
		public AuthenticationStatus Status { get; set; }

		public int AuthenticationAttemptID { get; set; }

		public AuthenticationResult()
		{
		}

		public AuthenticationResult(AuthenticationStatus status, int authenticationAttemptID = 0)
		{
			Status = status;
			AuthenticationAttemptID = authenticationAttemptID;
		}
	}
}