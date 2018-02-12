using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Windows.Controls;
using System.Xml.Linq;

namespace UPF.Windows
{
	public static class ExtensionMethods
	{
		public static string GetString( this byte[] data, Encoding encoding = null )
		{
			if ( encoding == null )
			{
				encoding = Encoding.UTF8;
			}

			string result = null;
			if ( data != null )
			{
				result = encoding.GetString( data );
			}
			return result;
		}

		public static DateTime? ToDateTime( this TextBox textBox )
		{
			DateTime? result = null;

			if ( !string.IsNullOrWhiteSpace( textBox.Text ) )
			{
				DateTime temp;
				if ( DateTime.TryParse( textBox.Text, out temp ) )
				{
					result = temp;
				}
			}

			return result;
		}

		public static int? ToInt32( this TextBox textBox )
		{
			int? result = null;

			if ( !string.IsNullOrWhiteSpace( textBox.Text ) )
			{
				int temp = 0;
				if ( int.TryParse( textBox.Text, out temp ) )
				{
					result = temp;
				}
			}

			return result;
		}

		#region WriteToFile Method(s)

		public static void WriteToFile( this byte[] data, string fileName )
		{
			Utility.WriteToFile( data, fileName );
		}

		public static void WriteToFile( this string data, string fileName )
		{
			Utility.WriteToFile( data, fileName );
		}

		#endregion WriteToFile Method(s)
	}
}