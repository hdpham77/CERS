using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UPF
{
    public static class LookupEntityExtensionMethods
    {
        public static T ToEnumValue<T>(this ILookupEntity lookupEntity, T defaultValue)
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentNullException("ToEnumValue only works for enumerations.!");
            }

            T result = defaultValue;

            if (lookupEntity != null)
            {
                string valueName = Enum.GetName(typeof(T), lookupEntity.ID);
                return (T)Enum.Parse(typeof(T), valueName, true);
            }

            return result;
        }
    }
}