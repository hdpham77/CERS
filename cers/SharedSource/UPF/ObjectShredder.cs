using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Data;
using System.Diagnostics;

namespace UPF {
	/// <summary>
	/// Adds the ability to serialize an object down to a <see cref="DataTable"/>.
	/// </summary>
	/// <remarks>
	/// Works by using reflection to get a list of Properties on an object and then going through the <see cref="IEnumerable{T}"/> source and converting each object into a <see cref="DataRow"/>
	/// and then adding it to a <see cref="DataTable"/> for return. Use the <see cref="ObjectShredderOptionsAttribute"/> to customize "shredding" of a particular property.
	/// </remarks>
	/// <typeparam name="T">The <see cref="Type"/> to shred to a <see cref="DataTable"/>.</typeparam>
	public class ObjectShredder<T> : ObjectPropertyCache<T> {

		#region Fields

		private Dictionary<string, int> _OrdinalMap;
		private Type _Type;

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the Object Shredder class.
		/// </summary>
		public ObjectShredder() {
			_Type = typeof(T);
			_OrdinalMap = new Dictionary<string, int>();
			BuildPropertyCache();
		}

		#endregion

		#region Shred Methods

		#region Shred a Collection of Items.

		/// <summary>
		/// Shreds an <see cref="IEnumerable{T}"/> source and all of each items data down to a <see cref="DataTable"/>.
		/// </summary>
		/// <param name="source">The <see cref="IEnumerable{T}"/> datasource containing the items to shred to a <see cref="DataTable"/>.</param>
		/// <returns>A <see cref="DataTable"/> containing all the data from the <paramref name="source"/>.</returns>
		public DataTable Shred(IEnumerable<T> source) {
			return Shred(source, null, null);
		}

		/// <summary>
		/// Shreds an <see cref="IEnumerable{T}"/> source and all of each items data down to a <see cref="DataTable"/>.
		/// </summary>
		/// <param name="source">The <see cref="IEnumerable{T}"/> datasource containing the items to shred to a <see cref="DataTable"/>.</param>
		/// <param name="options">Any <see cref="LoadOption"/>'s.</param>
		/// <returns>A <see cref="DataTable"/> containing all the data from the <paramref name="source"/>.</returns>
		public DataTable Shred(IEnumerable<T> source, LoadOption? options) {
			return Shred(source, null, options);
		}

		/// <summary>
		/// Shreds an <see cref="IEnumerable{T}"/> source and all of each items data down to a <see cref="DataTable"/>.
		/// </summary>
		/// <param name="source">The <see cref="IEnumerable{T}"/> datasource containing the items to shred to a <see cref="DataTable"/>.</param>
		/// <param name="table">A <see cref="DataTable"/> to extend and fill with data. If null, a new table will be created, extended and returned.</param>
		/// <param name="options">Any <see cref="LoadOption"/>'s.</param>
		/// <returns>A <see cref="DataTable"/> containing all the data from the <paramref name="source"/>.</returns>
		public DataTable Shred(IEnumerable<T> source, DataTable table, LoadOption? options) {
			if (typeof(T).IsPrimitive) {
				return ShredPrimitive(source, table, options);
			}

			WriteTrace("Begin Shred(IEnumerable<T>, DataTable, LoadOption?)");
			if (table == null) {
				table = new DataTable(typeof(T).Name);
			}

			WriteTrace("--> Extending Table");
			// now see if need to extend datatable base on the type T + build ordinal map
			table = ExtendTable(table);

			WriteTrace("--> Begin Loading Data");
			table.BeginLoadData();
			using (IEnumerator<T> e = source.GetEnumerator()) {
				while (e.MoveNext()) {
					if (options != null) {
						table.LoadDataRow(ShredObject(table, e.Current), (LoadOption)options);
					}
					else {
						table.LoadDataRow(ShredObject(table, e.Current), true);
					}
				}
			}
			table.EndLoadData();
			WriteTrace("--> End Loading Data");
			WriteTrace("End Shred(IEnumerable<T>, DataTable, LoadOption?)");
			return table;
		}

		#endregion

		protected virtual void WriteTrace(string action) {

			string text = this.GetType().Name + " (" + typeof(T).Name + ") - " + action + ": " + DateTime.Now.ToString();
			Debug.WriteLine(text);
		}

		#region Shred a Single Item

		/// <summary>
		/// Shred an object into a single <see cref="DataRow"/> in a <see cref="DataTable"/>.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public DataTable Shred(T item) {
			return Shred(item, null, null);
		}

		/// <summary>
		/// Shred an object into a single <see cref="DataRow"/> in a <see cref="DataTable"/>.
		/// </summary>
		/// <param name="item"></param>
		/// <param name="table"></param>
		/// <param name="options"></param>
		/// <returns></returns>
		public DataTable Shred(T item, DataTable table, LoadOption? options) {
			List<T> items = new List<T>();
			items.Add(item);
			return Shred(items, table, options);
		}

		#endregion

		#endregion

		#region ShredPrimitive Method

		/// <summary>
		/// Shred's a primitive data type to a <see cref="DataTable"/>.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public DataTable ShredPrimitive(T item) {
			return ShredPrimitive(item, null, null);
		}

		/// <summary>
		/// Shred's a primitive data type to a <see cref="DataTable"/>.
		/// </summary>
		/// <param name="item"></param>
		/// <param name="table"></param>
		/// <param name="options"></param>
		/// <returns></returns>
		public DataTable ShredPrimitive(T item, DataTable table, LoadOption? options) {
			List<T> items = new List<T>();
			items.Add(item);
			return ShredPrimitive(items, table, options);
		}

		/// <summary>
		/// Shreds an <see cref="IEnumerable{T}"/> source and all of each items data down to a <see cref="DataTable"/>.
		/// </summary>
		/// <param name="source">The <see cref="IEnumerable{T}"/> datasource containing the items to shred to a <see cref="DataTable"/>.</param>
		/// <param name="table">A <see cref="DataTable"/> to extend and fill with data. If null, a new table will be created, extended and returned.</param>
		/// <param name="options">Any <see cref="LoadOption"/>'s.</param>
		/// <returns>A <see cref="DataTable"/> containing all the data from the <paramref name="source"/>.</returns>
		public DataTable ShredPrimitive(IEnumerable<T> source, DataTable table, LoadOption? options) {
			if (table == null) {
				table = new DataTable(typeof(T).Name);
			}
			WriteTrace("Begin ShredPrimitive(IEnumerable<T>, DataTable, LoadOption?)");
			if (!table.Columns.Contains("Value")) {
				table.Columns.Add("Value", typeof(T));
			}

			WriteTrace("--> Begin Loading Data");

			table.BeginLoadData();
			using (IEnumerator<T> e = source.GetEnumerator()) {
				Object[] values = new object[table.Columns.Count];
				while (e.MoveNext()) {
					values[table.Columns["Value"].Ordinal] = e.Current;

					if (options != null) {
						table.LoadDataRow(values, (LoadOption)options);
					}
					else {
						table.LoadDataRow(values, true);
					}
				}
			}
			table.EndLoadData();
			WriteTrace("--> End Loading Data");
			WriteTrace("End ShredPrimitive(IEnumerable<T>, DataTable, LoadOption?)");
			return table;
		}

		#endregion

		#region Private Helper Methods

		private DataTable ExtendTable(DataTable table) {
			foreach (PropertyInfo property in PropertyCache) {
				if (!_OrdinalMap.ContainsKey(property.Name)) {
					Type propertyType = property.PropertyType;
					if ((propertyType.IsGenericType) && (propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))) {
						propertyType = propertyType.GetGenericArguments()[0];
					}
					DataColumn column = table.Columns.Contains(property.Name) ? table.Columns[property.Name] : table.Columns.Add(property.Name, propertyType);
					_OrdinalMap.Add(property.Name, column.Ordinal);
				}
			}
			return table;
		}


		private object[] ShredObject(DataTable table, T instance) {
			object[] values = new object[table.Columns.Count];
			foreach (PropertyInfo property in PropertyCache) {
				try {
					values[_OrdinalMap[property.Name]] = property.GetValue(instance, null);
				}
				catch {

				}
			}
			return values;
		}

		protected override bool ShouldIgnoreProperty(PropertyInfo info) {
			bool result = false;
			ObjectShredderOptionsAttribute attribute = ObjectShredderOptionsAttribute.GetCustomAttribute(info, typeof(ObjectShredderOptionsAttribute)) as ObjectShredderOptionsAttribute;
			if (attribute != null) {
				result = attribute.Ignore;
			}
			return result;
		}

		#endregion
	}
}
