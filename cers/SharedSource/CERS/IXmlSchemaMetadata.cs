using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using UPF;

namespace CERS
{
	public interface IXmlSchemaMetadata : IModelEntityWithID
	{
		string Description { get; set; }

		bool IsDefault { get; set; }

		string Name { get; set; }

		string Namespace { get; set; }

		DateTime PublishDate { get; set; }

		int StatusID { get; set; }

		string VersionIdentifier { get; set; }

		int XmlSchemaID { get; set; }

		XNamespace XNamespace { get; }

		string XSDFileName { get; set; }

		string XSDFullUri { get; set; }

		string XSDLocation { get; set; }

		string XSISchemaLocation { get; }
	}
}