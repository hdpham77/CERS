using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace UPF
{
	public static class WebHelper
	{
		public static string ApplicationPath
		{
			get
			{
				if ( HttpContext.Current == null )
				{
					throw new InvalidProgramException( "Cannot be used outside ASP.NET runtime." );
				}

				var context = HttpContext.Current;
				var baseUrl = context.Request.Url.Scheme + "://" + context.Request.Url.Host + context.Request.ApplicationPath;
				return baseUrl.NormalizeUrl();
			}
		}
	}
}