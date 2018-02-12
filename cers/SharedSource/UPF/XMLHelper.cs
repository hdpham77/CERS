using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.Schema;

namespace UPF
{
	public static class XMLHelper
	{
		public static XmlSchemaValidationResult IsValidAgainstSchema(this string xml, string schemaLocation)
		{
			XmlSchemaValidationResult result = new XmlSchemaValidationResult();

			result.IsValid = false;

			if (!string.IsNullOrWhiteSpace(xml) && xml.IsWellFormedXML())
			{
				XDocument xdoc = XDocument.Parse(xml);
				XmlSchemaSet xss = new XmlSchemaSet();
				XNamespace rn = xdoc.Root.Name.Namespace;
				XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";

				if (!string.IsNullOrWhiteSpace(schemaLocation))
				{
					if (schemaLocation.Split(' ').Count() == 2)
					{
						schemaLocation = schemaLocation.Split(' ')[0] + schemaLocation.Split(' ')[1];
					}
					xss.Add(rn.ToString(), schemaLocation);
					xdoc.Validate(xss, (o, e) => { result.Errors.Add(e.Message); }, true);
					result.IsValid = (result.Errors.Count == 0);
				}
			}

			return result;
		}
	}
}