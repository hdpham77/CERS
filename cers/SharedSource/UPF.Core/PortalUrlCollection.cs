using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace UPF.Core
{
	public class PortalUrlCollection : Collection<PortalUrl>
	{
		private PortalUrl _Current;

		public PortalUrlCollection()
		{
		}

		public PortalUrlCollection( IEnumerable<PortalUrl> items )
		{
			foreach ( var item in items )
			{
				this.Add( item );
			}
		}

		public PortalUrl Current
		{
			get
			{
				if ( _Current == null )
				{
					//default to the Pimary.
					_Current = this.SingleOrDefault( p => p.Enabled && p.Primary );

					//if we have an HttpContext and there is more than one URL defined, lets try and find the most appropriate URL that this code
					//is running under
					if ( HttpContext.Current != null && Count > 1 )
					{
						//find a match based on the current URL the code is running under.
						var temp = this.SingleOrDefault( p => p.Enabled && p.Url.ToLower().Trim() == WebHelper.ApplicationPath.ToLower().Trim() );
						if ( temp != null )
						{
							//we found a match, and so lets make this the current.
							_Current = temp;
						}
					}
				}

				return _Current;
			}
			set
			{
				_Current = value;
			}
		}
	}
}