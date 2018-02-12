using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Winnovative.ExcelLib;

namespace UPF
{
	public static class Excel
	{
		/// <summary>
  /// Provides backward compatibility for existing code utilizing the Winnovative Excel library.
  /// This change on 12/9/2013 utilizes the new LicenseKey API.
  /// </summary>
		public static string LicenseKey
		{
			get
			{
				return LicenseKeys.GetLicenseKey( BuiltInLicenseKey.WinnovativeSuite );
			}
		}

		public static ExcelWorkbook GenerateExcelWorkbook( DataSet dataSet )
		{
			return GenerateExcelWorkbook( ExcelWorkbookFormat.Xls_2003, dataSet );
		}

		public static ExcelWorkbook GenerateExcelWorkbook( ExcelWorkbookFormat format )
		{
			ExcelWorkbook workbook = new ExcelWorkbook( format );
			workbook.LicenseKey = GetWinnovativeExcelLicenseKey();
			return workbook;
		}

		public static ExcelWorkbook GenerateExcelWorkbook( ExcelWorkbookFormat format, DataSet dataSet )
		{
			ExcelWorkbook workbook = new ExcelWorkbook( format );
			workbook.LicenseKey = GetWinnovativeExcelLicenseKey();
			return workbook;
		}

		public static ExcelWorkbook GenerateExcelWorkbook( DataTable table )
		{
			return GenerateExcelWorkbook( ExcelWorkbookFormat.Xls_2003, table );
		}

		/// <summary>
  /// Builds an Excel Workbook in memory with data specified from a data table.
  /// </summary>
  /// <param name="format">The <see cref="ExcelWorkbookFormat"/> value indicating what version of
  /// the document to create.</param>
  /// <param name="table">The <see cref="DataTable"/> that contains the data to load into the first
  /// worksheet.</param>
  /// <returns>An <see cref="ExcelWorkbook"/> object with the data loaded.</returns>
		public static ExcelWorkbook GenerateExcelWorkbook( ExcelWorkbookFormat format, DataTable table )
		{
			ExcelWorkbook workbook = new ExcelWorkbook( format );
			workbook.LicenseKey = GetWinnovativeExcelLicenseKey();

			//since we are generating a new workbook, we can assume this will have a single sheet.
			ExcelWorksheet sheet = workbook.Worksheets[0];
			if ( sheet != null )
			{
				sheet.LoadDataTable( table, 1, 1, true );
				sheet.AutofitColumns();
			}

			return workbook;
		}

		/// <summary>
  /// Builds an excel workbook in memory and loads the data in a <see cref="DataTable"/> and then
  /// saves to a particular filename.
  /// </summary>
  /// <param name="fileName"></param>
  /// <param name="format"></param>
  /// <param name="table"></param>
		public static void GenerateExcelWorkbook( string fileName, ExcelWorkbookFormat format, DataTable table )
		{
			ExcelWorkbook workbook = GenerateExcelWorkbook( format, table );
			workbook.Save( fileName );
		}

		/// <summary>
  /// Builds an excel workbook in memory and loads the data in a <see cref="DataTable"/> and then
  /// saves to a particular filename.
  /// </summary>
  /// <param name="fileName"></param>
  /// <param name="table"></param>
		public static void GenerateExcelWorkbook( string fileName, DataTable table )
		{
			GenerateExcelWorkbook( fileName, ExcelWorkbookFormat.Xls_2003, table );
		}

		public static void GenerateExcelWorkbook( string fileName, DataTable sourceTable, params ExcelColumnMapping[] columns )
		{
			GenerateExcelWorkbook( fileName, ExcelWorkbookFormat.Xls_2003, sourceTable, columns );
		}

		public static void GenerateExcelWorkbook( string fileName, ExcelWorkbookFormat format, DataTable sourceTable, params ExcelColumnMapping[] columns )
		{
			ExcelWorkbook workbook = GenerateExcelWorkbook( format, sourceTable, columns );
			workbook.Save( fileName );
		}

		public static ExcelWorkbook GenerateExcelWorkbook( DataTable sourceTable, params ExcelColumnMapping[] columns )
		{
			return GenerateExcelWorkbook( sourceTable, columns );
		}

		public static ExcelWorkbook GenerateExcelWorkbook( ExcelWorkbookFormat format, DataTable sourceTable, params ExcelColumnMapping[] columns )
		{
			ExcelWorkbook workbook = null;

			if ( columns != null && columns.Length > 0 )
			{
				workbook = new ExcelWorkbook( format );
				workbook.LicenseKey = GetWinnovativeExcelLicenseKey();
				ExcelWorksheet sheet = workbook.Worksheets[0];

				DataTable newTable = new DataTable( "NewData" );
				foreach ( ExcelColumnMapping column in columns )
				{
					DataColumn existingColumn = sourceTable.Columns[column.SourceName];
					if ( existingColumn != null )
					{
						DataColumn newColumn = new DataColumn( existingColumn.ColumnName, existingColumn.DataType );
						newTable.Columns.Add( newColumn );
					}
				}

				//load the data.
				newTable.BeginLoadData();
				foreach ( DataRow oldRow in sourceTable.Rows )
				{
					DataRow newRow = newTable.NewRow();
					foreach ( ExcelColumnMapping column in columns )
					{
                        if ( oldRow[column.SourceName].GetType().UnderlyingSystemType.Name == "String" )
                        {
                            newRow[column.SourceName] = oldRow[column.SourceName].ToString().RemoveBeginningPro( "=" );
                        }
                        else
                        {
                            newRow[column.SourceName] = oldRow[column.SourceName];
                        }
					}
					newTable.Rows.Add( newRow );
				}
				newTable.EndLoadData();

				//rename columns.
				foreach ( ExcelColumnMapping column in columns )
				{
					DataColumn datacolumn = newTable.Columns[column.SourceName];
					datacolumn.ColumnName = column.TargetName;
				}

				sheet.LoadDataTable( newTable, 1, 1, true );
				sheet.AutofitColumns();
			}
			else
			{
				workbook = GenerateExcelWorkbook( format, sourceTable );
			}

			return workbook;
		}

		public static string GetWinnovativeExcelLicenseKey()
		{
			string result = string.Empty;

			result = LicenseKey;

			return result;
		}
	}
}