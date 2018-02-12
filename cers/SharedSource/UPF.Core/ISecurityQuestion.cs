using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UPF.Core
{
	public interface ISecurityQuestion : IModelEntityWithID
	{
		string Name { get; set; }

		bool AllowSelection { get; set; }
	}
}