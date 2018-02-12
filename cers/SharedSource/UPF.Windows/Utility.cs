using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace UPF.Windows
{
	public class Utility
	{
		public static byte[] ReadBinaryFile( string fileName )
		{
			byte[] data = null;
			using ( FileStream stream = new FileStream( fileName, FileMode.Open, FileAccess.Read, FileShare.Read ) )
			{
				data = new byte[stream.Length];
				stream.Read( data, 0, (int) stream.Length );
				stream.Close();
			}
			return data;
		}

		public static string ReadTextFile( string fileName )
		{
			string result = string.Empty;
			using ( StreamReader reader = new StreamReader( fileName ) )
			{
				result = reader.ReadToEnd();
				reader.Close();
			}
			return result;
		}

		public static string SelectFolder( string defaultPath = "" )
		{
			string result = defaultPath;
			System.Windows.Forms.FolderBrowserDialog dlg = new System.Windows.Forms.FolderBrowserDialog();
			if ( dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK )
			{
				result = dlg.SelectedPath;
			}

			return result;
		}

		public static void WriteToFile( byte[] data, string fileName )
		{
			if ( !string.IsNullOrWhiteSpace( fileName ) && data != null )
			{
				try
				{
					using ( FileStream stream = new FileStream( fileName, FileMode.Create, FileAccess.ReadWrite, FileShare.Read ) )
					{
						stream.Write( data, 0, data.Length );
						stream.Close();
					}
				}
				catch ( Exception ex )
				{
					throw new Exception( "Unable to save the content to:\r\n" + fileName + "\r\n\r\n" + ex.Message, ex );
				}
			}
		}

		public static void WriteToFile( string data, string fileName )
		{
			if ( !string.IsNullOrWhiteSpace( fileName ) && data != null )
			{
				try
				{
					using ( FileStream stream = new FileStream( fileName, FileMode.Create, FileAccess.ReadWrite, FileShare.Read ) )
					{
						byte[] buffer = Encoding.UTF8.GetBytes( data );
						stream.Write( buffer, 0, buffer.Length );
						stream.Close();
					}
				}
				catch ( Exception ex )
				{
					throw new Exception( "Unable to save the content to:\r\n" + fileName + "\r\n\r\n" + ex.Message, ex );
				}
			}
		}
	}
}