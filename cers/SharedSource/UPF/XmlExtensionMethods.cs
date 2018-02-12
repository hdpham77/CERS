using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace UPF
{
	public static class XmlExtensionMethods
	{
		#region Fields

		private static XName _nillableAttributeName = "{http://www.w3.org/2001/XMLSchema-instance}nil";

		#endregion Fields

		#region ToXml Method

		public static XElement ToXml( this byte[] data )
		{
			XElement result = null;
			string value = data.GetUTF8String();
			if ( !string.IsNullOrEmpty( value ) )
			{
				result = XElement.Parse( value );
			}
			return result;
		}

		#endregion ToXml Method

		#region WriteToFile Method

		public static void WriteToFile( this XElement element, string fileName, bool append = false )
		{
			if ( string.IsNullOrWhiteSpace( fileName ) )
			{
				throw new ArgumentNullException( "fileName" );
			}

			if ( element != null )
			{
				string xml = element.ToString();
				xml.WriteToFile( fileName, append );
			}
		}

		#endregion WriteToFile Method

		#region ToXmlFormat Method(s)

		public static string ToXmlFormat( this DateTime dateTime, bool dateOnly = false )
		{
			string value = dateTime.ToUniversalTime().ToString( "s" ) + "Z";
			if ( dateOnly )
			{
				int tPos = value.IndexOf( "T" );
				if ( tPos > -1 )
				{
					value = value.Substring( 0, tPos );
				}
			}
			return value;
			//return dateTime.ToUniversalTime().ToString("s") + "Z";
		}

		public static string ToXmlFormat( this DateTime? dateTime, bool dateOnly = false )
		{
			return ( dateTime == null ) ? "" : dateTime.Value.ToXmlFormat( dateOnly );
		}

		#endregion ToXmlFormat Method(s)

		#region ToXmlDateOnlyFormat Method(s)

		public static string ToXmlDateOnlyFormat( this DateTime dateTime )
		{
			return dateTime.ToString( "yyyy-MM-dd" );
		}

		public static string ToXmlDateOnlyFormat( this DateTime? dateTime )
		{
			return ( dateTime == null ) ? "" : dateTime.Value.ToXmlDateOnlyFormat();
		}

		#endregion ToXmlDateOnlyFormat Method(s)

		#region ToTypedValue Method

		public static T ToTypedValue<T>( this XElement element, T defaultValue = default(T) )
		{
			T result = defaultValue;
			if ( element != null )
			{
				string rawValue = element.Value;
				if ( !string.IsNullOrWhiteSpace( rawValue ) )
				{
					result = Data.ChangeType<T>( rawValue );
				}
			}
			return result;
		}

		#endregion ToTypedValue Method

		#region SetNillableElementValue Method

		public static void SetNillableElementValue( this XElement parentElement, XName elementName, object value )
		{
			parentElement.SetElementValue( elementName, value );
			parentElement.Element( elementName ).MakeNillable();
		}

		#endregion SetNillableElementValue Method

		#region MakeNillable Method

		public static XElement MakeNillable( this XElement element )
		{
			var hasNillableAttribute = element.Attribute( _nillableAttributeName ) != null;
			if ( string.IsNullOrEmpty( element.Value ) )
			{
				if ( !hasNillableAttribute )
					element.Add( new XAttribute( _nillableAttributeName, true ) );
			}
			else
			{
				if ( hasNillableAttribute )
					element.Attribute( _nillableAttributeName ).Remove();
			}
			return element;
		}

		#endregion MakeNillable Method

		#region ToNullIfNil Method

		/// <summary>
		/// Returns null if the provided XElement has an "xsi:nil" attribute with a value of "true".  This extension method
		/// is used to help with XElement explicit type conversions, such that empty elements provided with xsi:nil="true"
		/// do not cause conversion to fail for types such as DateTime? or decimal?.
		/// </summary>
		/// <param name="nillableXElement">XElement with optional xsi:nil attribute.</param>
		/// <returns>Original XElement or null.</returns>
		public static XElement ToNullIfNil( this XElement nillableXElement )
		{
			if ( nillableXElement != null )
			{
				var nillableAttribute = nillableXElement.Attribute( _nillableAttributeName );
				if ( nillableAttribute != null && (bool) nillableAttribute )
				{
					return null;
				}
			}
			return nillableXElement;
		}

		#endregion ToNullIfNil Method
	}
}