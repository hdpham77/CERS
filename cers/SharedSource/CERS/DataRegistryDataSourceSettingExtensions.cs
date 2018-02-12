using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CERS
{
	public static class DataRegistryDataSourceSettingExtensions
	{
		public static bool Contains(this List<DataRegistryDataSourceSetting> list, DataRegistryDataSourceType type)
		{
			bool result = false;
			result = (list.Count(p => p.Type == type) > 0);
			return result;
		}

		public static void AddIfNotExists(this List<DataRegistryDataSourceSetting> list, DataRegistryDataSourceType type, string acronym, CacheStrategy cacheStrategy)
		{
			if (!list.Contains(type))
			{
				DataRegistryDataSourceSetting setting = new DataRegistryDataSourceSetting(type, acronym, cacheStrategy);
				list.Add(setting);
			}
		}
	}
}