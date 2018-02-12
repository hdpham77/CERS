using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CERS
{
	public class DataElementItem : IDataElementItem
	{
		public decimal? CERSDataRegistryID { get; set; }

		public string CERSDataRegistryIDDisplay
		{
			get
			{
				string result = "";
				if ( CERSDataRegistryID != null )
				{
					result = CERSDataRegistryID.ToString();
				}
				return result;
			}
		}

		public bool CERSMinRequired { get; set; }

		public IEnumerable<IDataElementCode> Codes { get; set; }

		public int DataContainerID { get; set; }

		public string DataContainerName { get; set; }

		public string DataElementIdentifier { get; set; }

		public short? DataLength { get; set; }

		public string DataSourceAcronym { get; set; }

		public string DataStandardName { get; set; }

		public string DataType { get; set; }

		public int? DeltaChangeSignificanceID { get; set; }

		public string Description { get; set; }

		public string FieldFormat { get; set; }

		public bool FieldHelpAvailable { get; set; }

		public string FieldHelpText { get; set; }

		public string FieldLabelText { get; set; }

		public string FieldName { get; set; }

		public string GeneralDataType { get; set; }

		public int ID { get; set; }

		public string Instructions { get; set; }

		public DateTime LastUpdated { get; set; }

        public bool Obsolete { get; set; }

        public string TableName { get; set; }

		public UIElementType? UIElementType
		{
			get
			{
				UIElementType? type = null;
				if ( UIElementTypeID != null )
				{
					type = (UIElementType) UIElementTypeID;
				}
				return type;
			}
		}

		public int? UIElementTypeID { get; set; }

		public string ValidationRegularExpression { get; set; }

		public string XmlTagName { get; set; }

		public override int GetHashCode()
		{
			return ID.GetHashCode();
		}

		public override string ToString()
		{
			string result = ID.ToString();
			if ( !string.IsNullOrWhiteSpace( DataElementIdentifier ) )
			{
				result += "/DataElementIdentifer:" + DataElementIdentifier;
			}

			if ( !string.IsNullOrWhiteSpace( CERSDataRegistryIDDisplay ) )
			{
				result += "/CERSDataRegistryID:" + CERSDataRegistryIDDisplay;
			}
			return result;
		}

		public bool ValidateRegulatorExpression(string input)
		{
			if ( string.IsNullOrWhiteSpace( input ) )
			{
				throw new ArgumentNullException( "input" );
			}
			if ( string.IsNullOrWhiteSpace( ValidationRegularExpression ) )
			{
				throw new InvalidOperationException( "There is no ValidationRegularExpression specified to be able to validation against" );
			}
			Regex exp = new Regex( ValidationRegularExpression, RegexOptions.Compiled );
			return exp.IsMatch( input );
		}
	}
}