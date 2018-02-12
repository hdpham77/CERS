using CERS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UPF;
using UPF.Core;

namespace CERS
{
	public interface ICERSRepository
	{
		event EventHandler<RepositoryNotificationReceivedEventArgs> NotificationReceived;
	}
}