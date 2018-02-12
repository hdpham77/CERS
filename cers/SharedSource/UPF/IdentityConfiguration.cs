using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UPF.Configuration;

namespace UPF
{
    public class IdentityConfiguration
    {
        private static UPFConfigurationSection Config
        {
            get
            {
                return ConfigurationAccessor.Current;
            }
        }

        public static string ApplicationName
        {
            get
            {
                string result = "Unknown";

                if (Config != null)
                {
                    result = Config.ApplicationIdentity.Name;
                }
                return result;
            }
        }

        public static string ShortApplicationName
        {
            get
            {
                string result = "Unkown";
                if (Config != null)
                {
                    result = Config.ApplicationIdentity.ShortName;
                }
                return result;
            }
        }

        public static string Version
        {
            get
            {
                string result = "Unknown";
                if (Config != null)
                {
                    result = Config.ApplicationIdentity.Version;
                    if (Config.ApplicationIdentity.UseAssemblyVersion)
                    {
                        var exec = System.Reflection.Assembly.GetExecutingAssembly();
                        result = ReformatVersionNumber(exec.GetName().Version.ToString());
                    }
                    if (Config.Environment.CurrentProfileKey == RuntimeEnvironment.Documentation)
                    {
                        result += "-DOC";
                    }
                }
                return result;
            }
        }

        private static string ReformatVersionNumber(string version)
        {
            string result = version;
            string[] parts = version.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            for (int partIndex = 0; partIndex <= parts.Length - 1; partIndex++)
            {
                if (partIndex == 0)
                {
                    result = parts[partIndex];
                }
                else if (partIndex == 1)
                {
                    result += "." + parts[partIndex].PadLeft(2, '0');
                }
                else if (partIndex == 2)
                {
                }
                else if (partIndex == 3)
                {
                    result += "." + parts[partIndex].PadLeft(4, '0');
                }
            }
            return result;
        }
    }
}