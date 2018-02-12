using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UPF;

namespace CERS
{
	public class TimedCodeBlockCallbackInvoker : IDisposable
	{
		public DateTime Start { get; protected set; }

		public DateTime End { get; protected set; }

		public TimeSpan? Elapsed { get; protected set; }

		public Action<string> TargetMethod { get; protected set; }

		public string MessageFormatString { get; protected set; }

		public TimedCodeBlockCallbackInvoker(string messageFormatString, Action<string> method)
		{
			MessageFormatString = messageFormatString;
			TargetMethod = method;
			Start = DateTime.Now;

			if (TargetMethod != null)
			{
				TargetMethod("Begin: " + MessageFormatString + " @ " + Start.ToShortTimeString());
			}
		}

		public void Dispose()
		{
			End = DateTime.Now;
			Elapsed = DateUtilities.CalculateElapsedTime(Start, End);
			if (TargetMethod != null)
			{
				TargetMethod("End: " + MessageFormatString + " @ " + End.ToShortTimeString() + " - Duration: " + Elapsed.ToString());
			}
		}
	}
}