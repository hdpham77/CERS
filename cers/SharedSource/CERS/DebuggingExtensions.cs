using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace CERS
{
    public static class DebuggingExtensions
    {
        public static void Print<TModel>(IEnumerable<TModel> entities, DebugOutputLayout layout = DebugOutputLayout.RowPropertyList)
        {
#if DEBUG
            Debug.WriteLine("Printing Object Values For List Of Type: " + typeof(TModel).Name);
            var properties = typeof(TModel).GetProperties(BindingFlags.Instance | BindingFlags.Public).ToList();
            if (layout == DebugOutputLayout.RowPropertyList)
            {
                foreach (var property in properties)
                {
                    Debug.Write(property.Name + "\t");
                }
                Debug.WriteLine("");
            }

            foreach (var entity in entities)
            {
                Print(entity, properties, layout);
                Debug.WriteLine("");
            }
#endif
        }

        public static void Print<TModel>(TModel entity, List<PropertyInfo> properties = null, DebugOutputLayout layout = DebugOutputLayout.RowPropertyList)
        {
#if DEBUG
            if (properties == null)
            {
                properties = typeof(TModel).GetProperties(BindingFlags.Instance | BindingFlags.Public).ToList();
            }

            if (properties != null)
            {
                object propertyValue = null;
                string propertyValueDisplay = null;
                foreach (var property in properties)
                {
                    propertyValue = property.GetValue(entity, null);
                    propertyValueDisplay = propertyValue == null ? "NULL" : propertyValue.ToString();
                    if (layout == DebugOutputLayout.RowPropertyList)
                    {
                        Debug.Write(propertyValueDisplay + "\t");
                    }
                    else
                    {
                        Debug.WriteLine(property.Name + " (" + property.PropertyType.Name + "): " + propertyValueDisplay);
                    }
                }
            }
#endif
        }

        public static void DebugOutput<TModel>(this IEnumerable<TModel> entities, DebugOutputLayout layout = DebugOutputLayout.RowPropertyList)
        {
            Print(entities, layout);
        }

        public static void DebugOutput<TModel>(this TModel entity, DebugOutputLayout layout = DebugOutputLayout.RowPropertyList)
        {
            Print(entity, layout: layout);
        }
    }
}