using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CERS
{
	public interface IAddressInformation
	{
		double Latitude { get; set; }

		double Longitude { get; set; }

		string Street { get; set; }

		string City { get; set; }

		string State { get; set; }

		string ZipCode { get; set; }

		string ErrorMessage { get; set; }

		string Warning { get; set; }

		string CountyFIPSCode { get; set; }

		string CountyID { get; set; }

		DateTime DataCollectionDate { get; set; }

		int AccuracyMeasure { get; set; }

		int CollectionMethodCode { get; set; }

		string CollectionMethodName { get; set; }

		int ReferencePointCode { get; set; }

		string ReferencePointName { get; set; }

		int ReferenceDatumCode { get; set; }

		string ReferenceDatumName { get; set; }

		int VerificationMethodCode { get; set; }

		string VerificationMethodName { get; set; }

		int GeometricTypeCode { get; set; }

		string GeometricTypeName { get; set; }

		string DataSourceName { get; set; }

		int DataSourceCode { get; set; }

		int InternetSourceID { get; set; }

		string InternetSourceName { get; set; }
	}
}