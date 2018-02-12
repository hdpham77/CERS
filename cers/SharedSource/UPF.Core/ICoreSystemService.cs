using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPF.Core
{
	public interface ICoreSystemService
	{
		IAccount ContextAccount { get; }

		int ContextAccountID { get; }

		ICoreSystemServiceManager Services { get; }
	}
}