using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UPF.Configuration;

namespace UPF
{
	public static class EnvironmentProfileManager
	{
		private static object _lock = new object();
		private static EnvironmentProfileCollection _Profiles;

		public static EnvironmentProfileCollection Profiles
		{
			get
			{
				if (_Profiles == null)
				{
					LoadSettings();
				}
				return _Profiles;
			}
		}

		private static void LoadSettings()
		{
			lock (_lock)
			{
				EnvironmentConfigurationElement envConfig = UPFConfigurationSection.Current.Environment;
				if (envConfig != null)
				{
					if (envConfig.Profiles != null)
					{
						_Profiles = new EnvironmentProfileCollection(envConfig.Profiles);
					}
				}
				else
				{
					_Profiles = new EnvironmentProfileCollection();
				}
				CompileSettings();
			}
		}

		private static void CompileSettings()
		{
			foreach (var item in Enum.GetValues(typeof(RuntimeEnvironment)))
			{
				RuntimeEnvironment type = (RuntimeEnvironment)item;
				if (!_Profiles.Contains(type))
				{
					_Profiles.Add(type);
				}
			}
		}

		public static EnvironmentProfile Get(RuntimeEnvironment profileType)
		{
			return Profiles[profileType];
		}

		public static EnvironmentProfile Current
		{
			get
			{
				EnvironmentProfile result = null;
				if (UPFConfigurationSection.Current != null)
				{
					EnvironmentConfigurationElement envConfig = UPFConfigurationSection.Current.Environment;
					if (envConfig != null)
					{
						result = Profiles[envConfig.CurrentProfileKey];
					}
				}
				return result;
			}
		}
	}
}