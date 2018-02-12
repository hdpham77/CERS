using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CERS
{
	public class ActivityLogEntryReceivedEventArgs : EventArgs
	{
		public ActivityLogEntryReceivedEventArgs( string message, Exception exception )
		{
			Message = message;
			Exception = exception;
		}

		public Exception Exception { get; protected set; }

		public string Message { get; protected set; }
	}
}