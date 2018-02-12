using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPF
{
	public class ExcelColumnMappingCollection : Collection<ExcelColumnMapping>
	{
		public void Add( string sourceName, string targetName )
		{
			ExcelColumnMapping mapping = new ExcelColumnMapping( sourceName, targetName );
			this.Add( mapping );
		}
	}
}