using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UPF;

namespace CERS
{
	public class RepositoryNotificationReceivedEventArgs : EventArgs
	{
		public string Message { get; protected set; }

		public IModelEntityWithID Entity { get; protected set; }

		public RepositoryNotificationReceivedEventArgs(string message, IModelEntityWithID entity = null)
		{
			Entity = entity;
			Message = message;
		}
	}
}