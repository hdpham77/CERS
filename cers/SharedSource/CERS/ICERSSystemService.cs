using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UPF;

namespace CERS
{
	public interface ICERSSystemService
	{
		event EventHandler<ActivityLogEntryReceivedEventArgs> ActivityLogEntryReceived;

		IAccount ContextAccount { get; }

		int ContextAccountID { get; }

		ICERSSystemServiceManager Services { get; }
	}
}