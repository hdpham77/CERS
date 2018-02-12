using CERS.Model;
using CERS.Repository;
using CERS.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace CERS
{
	public static class XmlSchemas
	{
		#region Fields

		private static bool _CacheLoaded;
		private static object _Lock = new object();
		private static XmlSchemaCollection _SchemaCache;

		#endregion Fields

		#region Constants

		public const string W3CSchema = "http://www.w3.org/2001/XMLSchema-instance";
		public const string XSIPrefix = "xsi";

		public static XNamespace W3CNamespace
		{
			get
			{
				XNamespace ns = W3CSchema;
				return ns;
			}
		}

		public static XAttribute W3CXAttribute
		{
			get
			{
				return new XAttribute( XNamespace.Xmlns + XSIPrefix, W3CSchema );
			}
		}

		public static XAttribute GetSchemaLocationXAttribute( IXmlSchemaMetadata schema )
		{
			return new XAttribute( W3CNamespace + "schemaLocation", schema.XSISchemaLocation );
		}

		#endregion Constants

		#region Properties

		public static XmlSchemaCollection Cache
		{
			get
			{
				Initialize();
				return _SchemaCache;
			}
		}

		#endregion Properties

		#region Initialize Method

		private static void Initialize()
		{
			if ( !_CacheLoaded )
			{
				lock ( _Lock )
				{
					_SchemaCache = XmlSchemaRepository.GetSchemasMetadata();
				}
			}
		}

		#endregion Initialize Method

		#region GetSchema Method

		public static IXmlSchemaMetadata GetSchemaMetadata( XmlSchema schema, string versionIdentifier = null )
		{
			IXmlSchemaMetadata result = null;
			if ( !string.IsNullOrWhiteSpace( versionIdentifier ) )
			{
				//find the one with the requested version.
				result = Cache.SingleOrDefault( p =>
					p.XmlSchemaID == (int)schema &&
					p.VersionIdentifier.ToLower().Trim() == versionIdentifier.ToLower().Trim() &&
					p.StatusID == (int)XmlSchemaVersionStatus.Active
					);
			}
			else
			{
				//use the one marked as default.
				result = Cache.FirstOrDefault( p =>
					p.XmlSchemaID == (int)schema &&
					p.IsDefault && p.StatusID == (int)XmlSchemaVersionStatus.Active
					);
			}

			return result;
		}

		public static IXmlSchemaMetadata GetSchemaMetdataForNamespace( XmlSchema schema, XNamespace targetNamespace )
		{
			IXmlSchemaMetadata result = null;

			result = Cache.SingleOrDefault( p =>
				p.XmlSchemaID == (int)schema &&
				p.Namespace == targetNamespace.NamespaceName &&
				p.StatusID == (int)XmlSchemaVersionStatus.Active
			);

			return result;
		}

		#endregion GetSchema Method

		#region GetSchemaByNamespace

		public static IXmlSchemaMetadata GetSchemaMetdataForNamespace( string schemaNamespace )
		{
			return Cache.SingleOrDefault( p => p.Namespace.ToLower().Trim() == schemaNamespace.ToLower().Trim() );
		}

		#endregion GetSchemaByNamespace

		#region IsValidNamespace

		public static bool IsValidNamespace( string schemaNamespace )
		{
			bool result = false;

			IXmlSchemaMetadata schema = GetSchemaMetdataForNamespace( schemaNamespace );
			result = ( schema != null );

			return result;
		}

		#endregion IsValidNamespace

		#region RebuildCache

		public static void RebuildCache()
		{
			_CacheLoaded = false;
			Initialize();
		}

		#endregion RebuildCache
	}
}