using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UPF {
	/// <summary>
	/// Mapping helper class that maps one column name to another column name when doing custom export to excel. 
	/// </summary>
	public class ExcelColumnMapping {
		/// <summary>
		/// The source name of the column to map data from. 
		/// </summary>
		public string SourceName { get; set; }

		/// <summary>
		/// The target name of the column to map data to.
		/// </summary>
		public string TargetName { get; set; }

		public ExcelColumnMapping() : this(string.Empty, string.Empty)  {

		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ExcelColumnMapping"/>.
		/// </summary>
		/// <param name="name"></param>
		public ExcelColumnMapping(string name) {
			SourceName = name;
			TargetName = name;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sourceName"></param>
		/// <param name="targetName"></param>
		public ExcelColumnMapping(string sourceName, string targetName) {
			SourceName = sourceName;
			TargetName = targetName;
		}
	}
}
