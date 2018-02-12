using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UPF.Configuration;
using UPF.Web.Configuration;

namespace UPF.Web
{
	public static class WebIdentity
	{
		public static string ApplicationName
		{
			get
			{
				string result = "Unknown";
				if (UPFConfigurationSection.Current != null)
				{
					result = UPFConfigurationSection.Current.ApplicationIdentity.Name;
				}
				return result;
			}
		}

		public static string Version
		{
			get
			{
				string result = "Unknown";
				if (UPFConfigurationSection.Current != null)
				{
					result = UPFConfigurationSection.Current.ApplicationIdentity.Version;
				}
				return result;
			}
		}

		public static string Url
		{
			get
			{
				string result = "http://localhost/";
				if (WebConfigurationSection.CurrentConfig != null)
				{
					result = WebConfigurationSection.CurrentConfig.WebApplicationIdentity.Url;
				}
				return result;
			}
		}
	}
}
