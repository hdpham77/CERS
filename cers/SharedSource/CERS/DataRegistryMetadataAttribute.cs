using System;
using UPF;

namespace CERS
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DataRegistryMetadataAttribute : ModelMetaDataAttribute
    {
        public string DataRegistryNumberString { get; set; }

        public bool IgnoreValidation { get; set; }

        //Deferring this functionality until validation requirements have been finalized by the business
        //public bool IsObsolete
        //{
        //    get
        //    {
        //        bool result = false;

        //        var drElement = DataRegistry.GetDataElement(this);

        //        if (drElement != null)
        //        {
        //            result = drElement.Obsolete;
        //        }
        //        return result;
        //    }
        //}

        public decimal DataRegistryNumber
		{
			get
			{
				decimal result = 0;
				if (!string.IsNullOrWhiteSpace(DataRegistryNumberString))
				{
					decimal.TryParse(DataRegistryNumberString, out result);
				}
				return result;
			}
		}

		public string FieldIdentifier { get; set; }

		public DataRegistryDataSourceType DataSource { get; set; }

		public DataRegistryMetadataAttribute(string dataRegistryNumber)
		{
			DataRegistryNumberString = dataRegistryNumber;
		}

		public DataRegistryMetadataAttribute(string dataRegistryNumber, DataRegistryDataSourceType dataSource)
		{
			DataRegistryNumberString = dataRegistryNumber;
			DataSource = dataSource;
		}

		public DataRegistryMetadataAttribute(string dataRegistryNumber, string fieldIdentifier)
		{
			DataRegistryNumberString = dataRegistryNumber;
			FieldIdentifier = fieldIdentifier;
		}

		public DataRegistryMetadataAttribute(string dataRegistryNumber, string fieldIdentifier, DataRegistryDataSourceType dataSource)
		{
			DataRegistryNumberString = dataRegistryNumber;
			FieldIdentifier = fieldIdentifier;
			DataSource = dataSource;      
        }
	}
}