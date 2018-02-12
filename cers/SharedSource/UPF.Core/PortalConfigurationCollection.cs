using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace UPF.Core
{
	public class PortalConfigurationCollection : Collection<PortalConfiguration>
	{
		public PortalConfigurationCollection()
		{
		}

		public PortalConfigurationCollection( IEnumerable<PortalConfiguration> portals )
		{
			AddRange( portals );
		}

		public PortalConfiguration Current
		{
			get { return this.SingleOrDefault( p => p.IsCurrent ); }
		}

		public void AddRange( IEnumerable<PortalConfiguration> portals )
		{
			if ( portals == null )
			{
				return;
			}

			foreach ( var portal in portals )
			{
				this.Add( portal );
			}
		}
	}
}