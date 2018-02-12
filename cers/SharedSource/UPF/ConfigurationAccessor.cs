using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UPF.Configuration;

namespace UPF
{
    public static class ConfigurationAccessor
    {
        private static UPFConfigurationSection _Current;
        private static object _Lock = new object();

        private static void Init()
        {
            if (_Current == null)
            {
                lock (_Lock)
                {
                    // check again after we have the lock
                    if (_Current == null)
                    {
                        _Current = UPFConfigurationSection.Current;
                    }
                }
            }
        }

        public static UPFConfigurationSection Current
        {
            get
            {
                Init();
                return _Current;
            }
        }
    }
}
