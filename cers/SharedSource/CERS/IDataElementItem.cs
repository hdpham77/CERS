using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CERS
{
	public interface IDataElementItem
	{
		decimal? CERSDataRegistryID { get; set; }

		string CERSDataRegistryIDDisplay
		{
			get;
		}

		bool CERSMinRequired { get; set; }

		IEnumerable<IDataElementCode> Codes { get; set; }

		int DataContainerID { get; set; }

		string DataContainerName { get; set; }

		string DataElementIdentifier { get; set; }

		short? DataLength { get; set; }

		string DataSourceAcronym { get; set; }

		string DataStandardName { get; set; }

		string DataType { get; set; }

		int? DeltaChangeSignificanceID { get; set; }

		string Description { get; set; }

		string FieldFormat { get; set; }

		bool FieldHelpAvailable { get; set; }

		string FieldHelpText { get; set; }

		string FieldLabelText { get; set; }

		string FieldName { get; set; }

		string GeneralDataType { get; set; }

		string Instructions { get; set; }

        bool Obsolete { get; set; }

        DateTime LastUpdated { get; set; }

		string TableName { get; set; }

		UIElementType? UIElementType { get; }

		int? UIElementTypeID { get; set; }

		string ValidationRegularExpression { get; set; }

		string XmlTagName { get; set; }

		bool ValidateRegulatorExpression(string input);

	}
}