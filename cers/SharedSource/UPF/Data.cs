using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Objects;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace UPF
{
	/// <summary>
	/// Helper class to provide safe access to data.
	/// </summary>
	public static class Data
	{
		#region SortBy IOrderedQueryable Methods

		public static IOrderedQueryable<T> SortBy<T>(this IOrderedQueryable<T> source, Expression<Func<T, int>> predicate, string sortDirection = "asc")
		{
			bool sortAscending = true;
			if (string.IsNullOrWhiteSpace(sortDirection))
			{
				sortDirection = "asc";
			}

			if (sortDirection.ToLower().IndexOf("asc") > -1)
			{
				sortAscending = true;
			}
			else
			{
				sortAscending = false;
			}
			return source.SortBy(predicate, sortAscending);
		}

		public static IOrderedQueryable<T> SortBy<T>(this IOrderedQueryable<T> source, Expression<Func<T, string>> predicate, string sortDirection = "asc")
		{
			bool sortAscending = true;
			if (string.IsNullOrWhiteSpace(sortDirection))
			{
				sortDirection = "asc";
			}

			if (sortDirection.ToLower().IndexOf("asc") > -1)
			{
				sortAscending = true;
			}
			else
			{
				sortAscending = false;
			}
			return source.SortBy(predicate, sortAscending);
		}

		public static IOrderedQueryable<T> SortBy<T>(this IOrderedQueryable<T> source, Expression<Func<T, int>> predicate, bool ascending = true)
		{
			if (ascending)
			{
				return source.OrderBy(predicate);
			}
			else
			{
				return source.OrderByDescending(predicate);
			}
		}

		public static IOrderedQueryable<T> SortBy<T>(this IOrderedQueryable<T> source, Expression<Func<T, string>> predicate, bool ascending = true)
		{
			if (ascending)
			{
				return source.OrderBy(predicate);
			}
			else
			{
				return source.OrderByDescending(predicate);
			}
		}

		#endregion SortBy IOrderedQueryable Methods

		#region SortBy IQueryable Methods

		public static IQueryable<T> SortBy<T>(this IQueryable<T> source, Expression<Func<T, int>> predicate, string sortDirection = "asc")
		{
			bool sortAscending = true;
			if (string.IsNullOrWhiteSpace(sortDirection))
			{
				sortDirection = "asc";
			}

			if (sortDirection.ToLower().IndexOf("asc") > -1)
			{
				sortAscending = true;
			}
			else
			{
				sortAscending = false;
			}
			return source.SortBy(predicate, sortAscending);
		}

		public static IQueryable<T> SortBy<T>(this IQueryable<T> source, Expression<Func<T, string>> predicate, string sortDirection = "asc")
		{
			bool sortAscending = true;
			if (string.IsNullOrWhiteSpace(sortDirection))
			{
				sortDirection = "asc";
			}

			if (sortDirection.ToLower().IndexOf("asc") > -1)
			{
				sortAscending = true;
			}
			else
			{
				sortAscending = false;
			}
			return source.SortBy(predicate, sortAscending);
		}

		public static IQueryable<T> SortBy<T>(this IQueryable<T> source, Expression<Func<T, int>> predicate, bool ascending = true)
		{
			if (ascending)
			{
				return source.OrderBy(predicate);
			}
			else
			{
				return source.OrderByDescending(predicate);
			}
		}

		public static IQueryable<T> SortBy<T>(this IQueryable<T> source, Expression<Func<T, string>> predicate, bool ascending = true)
		{
			if (ascending)
			{
				return source.OrderBy(predicate);
			}
			else
			{
				return source.OrderByDescending(predicate);
			}
		}

		#endregion SortBy IQueryable Methods

		#region Page Methods

		public static IQueryable<TSource> Page<TSource>(this IQueryable<TSource> source, int page, int pageSize)
		{
			int recordCount;
			int pageCount;
			return source.Page(page, pageSize, out recordCount, out pageCount);
		}

		//used by LINQ to SQL
		public static IQueryable<TSource> Page<TSource>(this IQueryable<TSource> source, int page, int pageSize, out int recordCount, out int totalPageCount)
		{
			recordCount = source.Count();
			float pageCount = (float)recordCount / (float)pageSize;
			int actualPageCount = (int)pageCount;
			if (pageCount > actualPageCount)
			{
				actualPageCount++;
			}
			totalPageCount = actualPageCount;
			return source.Skip((page - 1) * pageSize).Take(pageSize);
		}

		#endregion Page Methods

		#region ExecuteObjectContext Method

		public static void ExecuteObjectContext<T>(Action<T> action) where T : ObjectContext, new()
		{
			using (T context = new T())
			{
				action(context);
			}
		}

		#endregion ExecuteObjectContext Method

		#region GetValue<T> Methods

		//public static T GetValue<T>(Data)

		public static T GetValue<T>(DataRow row, string columnName, T defaultValue)
		{
			T result = defaultValue;
			if (row.Table.Columns.Contains(columnName))
			{
				object dbValue = row[columnName];
				if (dbValue != DBNull.Value)
				{
					result = ChangeType<T>(dbValue);
				}
			}
			return result;
		}

		public static T GetValue<T>(IDataReader reader, string columnName, T defaultValue)
		{
			T result = defaultValue;
			if (GetReaderColumns(reader).Contains(columnName))
			{
				object dbValue = reader[columnName];
				if (dbValue != DBNull.Value)
				{
					result = ChangeType<T>(dbValue);
				}
			}
			return result;
		}

		#endregion GetValue<T> Methods

		#region GetReaderColumns Method

		public static List<string> GetReaderColumns(IDataReader reader)
		{
			List<string> results = new List<string>();
			DataTable table = reader.GetSchemaTable();
			foreach (DataRow row in table.Rows)
			{
				results.Add(GetValue<string>(row, "ColumnName", string.Empty));
			}
			return results;
		}

		#endregion GetReaderColumns Method

		#region BuildDataTableStructureFromType<T> Method

		public static DataTable BuildDataTableStructureFromType<T>(string tableName)
		{
			DataTable table = new DataTable(tableName);
			Type TType = typeof(T);
			PropertyInfo[] properties = TType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
			foreach (PropertyInfo propertyInfo in properties)
			{
				try
				{
					table.Columns.Add(propertyInfo.Name, propertyInfo.PropertyType);
				}
				finally
				{
					//absorb error.
				}
			}
			return table;
		}

		#endregion BuildDataTableStructureFromType<T> Method

		#region BuildDataTableFromList<T> Method

		public static DataTable BuildDataTableFromList<T>(string tableName, List<T> list)
		{
			DataTable table = BuildDataTableStructureFromType<T>(tableName);
			foreach (T item in list)
			{
				AddRowFromObjectProperties(table, item);
			}
			return table;
		}

		#endregion BuildDataTableFromList<T> Method

		#region BuildRowFromObjectProperties Method

		public static DataRow BuildRowFromObjectProperties(DataTable table, object dataSource)
		{
			DataRow row = null;
			Type dataSourceType = dataSource.GetType();
			PropertyInfo[] properties = dataSourceType.GetProperties();
			row = table.NewRow();
			for (int propertyIndex = 0; propertyIndex <= properties.Length - 1; propertyIndex++)
			{
				PropertyInfo property = properties[propertyIndex];
				foreach (DataColumn column in table.Columns)
				{
					if (string.Compare(column.ColumnName, property.Name, true) == 0)
					{
						object value = property.GetValue(dataSource, null);
						row[column] = value;
					}
				}
			}
			return row;
		}

		#endregion BuildRowFromObjectProperties Method

		#region AddRowFromObjectProperties Method

		public static void AddRowFromObjectProperties(DataTable table, object dataSource)
		{
			DataRow row = BuildRowFromObjectProperties(table, dataSource);
			table.Rows.Add(row);
		}

		#endregion AddRowFromObjectProperties Method

		#region BuildTableFromList<T> Method

		public static void BuildTableFromList<T>(DataTable table, List<T> list)
		{
			foreach (T item in list)
			{
				AddRowFromObjectProperties(table, item);
			}
		}

		#endregion BuildTableFromList<T> Method

		#region VerifyInputArgs Methods

		internal static void VerifyInputArgs(DataRow row, string columnName)
		{
			if (row == null)
			{
				throw new ArgumentNullException("row");
			}

			if (string.IsNullOrEmpty(columnName))
			{
				throw new ArgumentException("The columnName argument is null or empty!", "columnName");
			}
		}

		internal static void VerifyInputArgs(IDataReader reader, string columnName)
		{
			if (reader == null)
			{
				throw new ArgumentNullException("reader");
			}

			if (string.IsNullOrEmpty(columnName))
			{
				throw new ArgumentException("The columnName argument is null or empty!", "columnName");
			}
		}

		#endregion VerifyInputArgs Methods

		#region GetString Methods

		public static string GetString(DataRow row, string columnName, string defaultValue)
		{
			VerifyInputArgs(row, columnName);
			if (row[columnName] != null && row[columnName] != DBNull.Value)
			{
				return row[columnName].ToString().Trim();
			}
			else
			{
				return defaultValue.Trim();
			}
		}

		public static string GetString(IDataReader reader, string columnName, string defaultValue)
		{
			VerifyInputArgs(reader, columnName);
			if (reader[columnName] != null && reader[columnName] != DBNull.Value)
			{
				return reader[columnName].ToString().Trim();
			}
			else
			{
				return defaultValue.Trim();
			}
		}

		#endregion GetString Methods

		#region GetInt16ColumnValue Methods

		public static short? GetInt16(IDataReader reader, string columnName, short? defaultValue)
		{
			VerifyInputArgs(reader, columnName);
			if (reader[columnName] != null && reader[columnName] != DBNull.Value)
			{
				try
				{
					int temp = Convert.ToInt32(reader[columnName]);
					return (short?)temp;
				}
				catch
				{
					return defaultValue;
				}
			}
			else
			{
				return defaultValue;
			}
		}

		public static short? GetInt16(DataRow row, string columnName, short? defaultValue)
		{
			VerifyInputArgs(row, columnName);
			if (row[columnName] != null && row[columnName] != DBNull.Value)
			{
				try
				{
					int temp = Convert.ToInt32(row[columnName]);
					return (short?)temp;
				}
				catch
				{
					return defaultValue;
				}
			}
			else
			{
				return defaultValue;
			}
		}

		#endregion GetInt16ColumnValue Methods

		#region GetInt32 Methods

		public static int? GetInt32(DataRow row, string columnName, int? defaultValue)
		{
			VerifyInputArgs(row, columnName);
			if (row[columnName] != null && row[columnName] != DBNull.Value)
			{
				try
				{
					int temp = Convert.ToInt32(row[columnName]);
					return (int?)temp;
				}
				catch
				{
					return defaultValue;
				}
			}
			else
			{
				return defaultValue;
			}
		}

		public static int? GetInt32(IDataReader reader, string columnName, int? defaultValue)
		{
			VerifyInputArgs(reader, columnName);
			if (reader[columnName] != null && reader[columnName] != DBNull.Value)
			{
				try
				{
					int temp = Convert.ToInt32(reader[columnName]);
					return (int?)temp;
				}
				catch
				{
					return defaultValue;
				}
			}
			else
			{
				return defaultValue;
			}
		}

		#endregion GetInt32 Methods

		#region GetInt64 Methods

		public static long? GetInt64(DataRow row, string columnName, long? defaultValue)
		{
			VerifyInputArgs(row, columnName);
			if (row[columnName] != null && row[columnName] != DBNull.Value)
			{
				try
				{
					long temp = Convert.ToInt64(row[columnName]);
					return (long?)temp;
				}
				catch
				{
					return defaultValue;
				}
			}
			else
			{
				return defaultValue;
			}
		}

		public static long? GetInt64(IDataReader reader, string columnName, long? defaultValue)
		{
			VerifyInputArgs(reader, columnName);
			if (reader[columnName] != null && reader[columnName] != DBNull.Value)
			{
				try
				{
					long temp = Convert.ToInt64(reader[columnName]);
					return (long?)temp;
				}
				catch
				{
					return defaultValue;
				}
			}
			else
			{
				return defaultValue;
			}
		}

		#endregion GetInt64 Methods

		#region GetDateTime Methods

		public static DateTime? GetDateTime(DataRow row, string columnName, DateTime? defaultValue)
		{
			VerifyInputArgs(row, columnName);
			if (row[columnName] != null && row[columnName] != DBNull.Value)
			{
				try
				{
					return (DateTime?)row[columnName];
				}
				catch
				{
					return defaultValue;
				}
			}
			else
			{
				return defaultValue;
			}
		}

		public static DateTime? GetDateTime(IDataReader reader, string columnName, DateTime? defaultValue)
		{
			VerifyInputArgs(reader, columnName);
			if (reader[columnName] != null && reader[columnName] != DBNull.Value)
			{
				try
				{
					return (DateTime?)reader[columnName];
				}
				catch
				{
					return defaultValue;
				}
			}
			else
			{
				return defaultValue;
			}
		}

		#endregion GetDateTime Methods

		#region GetDouble Methods

		public static double? GetDouble(DataRow row, string columnName, double? defaultValue)
		{
			VerifyInputArgs(row, columnName);
			if (row[columnName] != null && row[columnName] != DBNull.Value)
			{
				try
				{
					return (double?)row[columnName];
				}
				catch
				{
					return defaultValue;
				}
			}
			else
			{
				return defaultValue;
			}
		}

		public static double? GetDouble(IDataReader reader, string columnName, double? defaultValue)
		{
			VerifyInputArgs(reader, columnName);
			if (reader[columnName] != null && reader[columnName] != DBNull.Value)
			{
				try
				{
					return (double?)reader[columnName];
				}
				catch
				{
					return defaultValue;
				}
			}
			else
			{
				return defaultValue;
			}
		}

		#endregion GetDouble Methods

		#region GetDecimal Methods

		public static decimal? GetDecimal(DataRow row, string columnName, decimal? defaultValue)
		{
			VerifyInputArgs(row, columnName);
			if (row[columnName] != null && row[columnName] != DBNull.Value)
			{
				try
				{
					return (decimal?)row[columnName];
				}
				catch
				{
					return defaultValue;
				}
			}
			else
			{
				return defaultValue;
			}
		}

		public static decimal? GetDecimal(IDataReader reader, string columnName, decimal? defaultValue)
		{
			VerifyInputArgs(reader, columnName);
			if (reader[columnName] != null && reader[columnName] != DBNull.Value)
			{
				try
				{
					return (decimal?)reader[columnName];
				}
				catch
				{
					return defaultValue;
				}
			}
			else
			{
				return defaultValue;
			}
		}

		#endregion GetDecimal Methods

		#region GetBoolean Methods

		public static bool GetBoolean(DataRow row, string columnName, bool defaultValue)
		{
			VerifyInputArgs(row, columnName);
			if (row[columnName] != null && row[columnName] != DBNull.Value)
			{
				try
				{
					return (bool)row[columnName];
				}
				catch
				{
					return defaultValue;
				}
			}
			else
			{
				return defaultValue;
			}
		}

		public static bool GetBoolean(IDataReader reader, string columnName, bool defaultValue)
		{
			VerifyInputArgs(reader, columnName);
			if (reader[columnName] != null && reader[columnName] != DBNull.Value)
			{
				try
				{
					return (bool)reader[columnName];
				}
				catch
				{
					return defaultValue;
				}
			}
			else
			{
				return defaultValue;
			}
		}

		#endregion GetBoolean Methods

		#region GetRow Methods

		public static DataRow GetRow(DataTable table, int rowIndex)
		{
			DataRow row = null;
			if (table.Rows.Count - 1 >= rowIndex)
			{
				row = table.Rows[rowIndex];
			}
			return row;
		}

		public static DataRow GetRow(DataSet dataSet, string tableName, int rowIndex)
		{
			DataRow row = null;
			if (dataSet.Tables.Contains(tableName))
			{
				DataTable table = dataSet.Tables[tableName];
				row = GetRow(table, rowIndex);
			}
			return row;
		}

		#endregion GetRow Methods

		#region ChangeType Method

		public static object ChangeType(object value, Type conversionType)
		{
			if (conversionType == null)
			{
				throw new ArgumentNullException("conversionType");
			}

			if (conversionType.IsGenericType && conversionType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
			{
				if (value == null || value == DBNull.Value)
				{
					return null;
				}

				//NullableConverter converter = new NullableConverter(conversionType);
				//conversionType == converter.UnderlyingType;
				conversionType = Nullable.GetUnderlyingType(conversionType);
			}
			return value.GetType().Equals(conversionType) == true ? value : TypeDescriptor.GetConverter(conversionType).ConvertFrom(value);

			//return Convert.ChangeType(value, conversionType);
		}

		/// <summary>
		///
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="value"></param>
		/// <returns></returns>
		public static T ChangeType<T>(object value)
		{
			return (T)ChangeType(value, typeof(T));
		}

		#endregion ChangeType Method

		#region MakeInt32List

		public static List<int> MakeInt32List<T>(params T[] items) where T : struct
		{
			if (!typeof(T).IsEnum)
			{
				throw new ArgumentException("items can only be enumerations", "items");
			}

			List<int> result = new List<int>();
			foreach (T item in items)
			{
				result.Add((int)Enum.Parse(typeof(T), item.ToString()));
			}
			return result;
		}

		#endregion MakeInt32List
	}
}