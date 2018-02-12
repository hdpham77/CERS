using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace UPF
{
	public class IOHelper
	{
		public static byte[] ConvertToBytes( Stream stream )
		{
			var memoryStream = new MemoryStream();
			stream.Seek( 0, SeekOrigin.Begin );
			stream.CopyTo( memoryStream );
			return memoryStream.ToArray();
		}

		public static Stream ConvertToStream( byte[] bytes )
		{
			return new MemoryStream( bytes );
		}

		public static void CopyStream( Stream input, Stream output )
		{
			input.Seek( 0, SeekOrigin.Begin );
			byte[] buffer = new byte[32768];
			while ( true )
			{
				int read = input.Read( buffer, 0, buffer.Length );
				if ( read <= 0 )
				{
					return;
				}
				output.Write( buffer, 0, read );
			}
		}

		public static string GetContentType( string fileName )
		{
			string contentType = "application/octetstream";
			string ext = System.IO.Path.GetExtension( fileName ).ToLower();
			Microsoft.Win32.RegistryKey registryKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey( ext );
			if ( registryKey != null && registryKey.GetValue( "Content Type" ) != null )
			{
				contentType = registryKey.GetValue( "Content Type" ).ToString();
			}
			return contentType;
		}

		public static string GetFileMD5( string fileName )
		{
			string result = string.Empty;
			using ( var stream = System.IO.File.OpenRead( fileName ) )
			{
				result = MD5Stream( stream );
			}
			return result;
		}

		/// <summary>
		/// Validates that the data in teh stream is a valid Zip file.
		/// Check the first four bytes, a ZIP file begins with 'P' 'K' 0x03 0x04.
		/// </summary>
		/// <param name="stream"></param>
		/// <returns></returns>
		public static bool IsValidZip( Stream stream )
		{
			bool isValid = false;
			byte[] buffer = new byte[4];

			// Make sure Stream is at the beginning
			stream.Seek( 0, SeekOrigin.Begin );

			// Read first four bytes
			stream.Read( buffer, 0, buffer.Length );

			// If first four bytes are 'P', 'K', 0x03, 0x04, then assume this is a valid ZIP file:
			if ( buffer[0] == 0x50 && buffer[1] == 0x4b && buffer[2] == 0x03 && buffer[3] == 0x04 )
			{
				isValid = true;
			}

			// rewind to the beginning
			stream.Seek( 0, SeekOrigin.Begin );
			return isValid;
		}

		/// <summary>
		/// validates that the data in the stream is a well formed xml file.
		/// </summary>
		/// <param name="stream"></param>
		/// <returns></returns>
		public static bool IsWellFormedXML( Stream stream )
		{
			var value = ReadToEnd( stream );
			return value.IsWellFormedXML();
		}

		public static string MD5Stream( Stream stream )
		{
			string result = string.Empty;
			using ( var md5 = MD5.Create() )
			{
				var data = md5.ComputeHash( stream );

				// Create a new Stringbuilder to collect the bytes
				// and create a string.
				StringBuilder sBuilder = new StringBuilder();

				// Loop through each byte of the hashed data
				// and format each one as a hexadecimal string.
				for ( int i = 0; i < data.Length; i++ )
				{
					sBuilder.Append( data[i].ToString( "x2" ) );
				}
				result = sBuilder.ToString();
			}

			return result;
		}

		public static string ReadToEnd( Stream stream )
		{
			stream.Seek( 0, SeekOrigin.Begin );
			StreamReader sr = new StreamReader( stream );
			return sr.ReadToEnd();
		}

		public static void WriteFile( string fileName, string content, bool append = false )
		{
			using ( StreamWriter stream = new StreamWriter( fileName, append ) )
			{
				stream.WriteLine( content );
				stream.Close();
			}
		}
	}
}