using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace UPF
{
	public static class DataExtensionMethods
	{
		public static T GetValue<T>(this DataRow row, string columnName, T defaultValue)
		{
			T result = defaultValue;
			if (row[columnName] != null && row[columnName] != DBNull.Value)
			{
				result = row.Field<T>(columnName);
			}
			return result;
		}

		/// <summary>
		/// Checks each column in a DataRow to see if any contain a non-null, non-empty value.  Returns true if any value is found, false if entire row is empty.
		/// </summary>
		/// <param name="row">DataRow to check for values</param>
		/// <returns></returns>
		public static bool IsRowEmpty(this DataRow row)
		{
			if (row == null) throw new ArgumentNullException("row");

			for (int i = row.Table.Columns.Count - 1; i >= 0; i--)
			{
				// Must be non-null and have a non-white-space value:
				if (!row.IsNull(i) && row[i].ToString().Trim().Length > 0)
				{
					return false;
				}
			}

			// If we have arrive here, no values were found; return true:
			return true;
		}

		public static string ToXmlDateTime(this DataRow row, string columnName, string defaultValue = "", bool dateOnly = false)
		{
			string result = defaultValue;

			if (row[columnName] != null && row[columnName] != DBNull.Value)
			{
				string data = row[columnName].ToString();
				DateTime tempResult;
				if (DateTime.TryParse(data, out tempResult))
				{
					result = tempResult.ToXmlFormat(dateOnly: dateOnly);
				}
			}

			return result;
		}

		public static string ToXmlDate(this DataRow row, string columnName, string defaultValue = "")
		{
			return row.ToXmlDateTime(columnName, defaultValue, true);
		}
	}
}