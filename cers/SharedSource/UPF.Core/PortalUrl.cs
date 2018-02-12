using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPF.Core
{
	public class PortalUrl
	{
		public bool Enabled { get; set; }

		public int ID { get; set; }

		public int PreferenceSequence { get; set; }

		public bool Primary { get; set; }

		public string Url { get; set; }
	}
}