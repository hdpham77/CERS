using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Ionic.Zip;

namespace UPF
{
	public static class ZipFileExtensionMethods
	{
		#region ContainsFile Method

		public static bool ContainsFile(this ZipFile zipFile, string fileName)
		{
			if (zipFile == null)
			{
				throw new ArgumentNullException("zipFile");
			}

			if (string.IsNullOrWhiteSpace(fileName))
			{
				throw new ArgumentNullException("fileName");
			}

			return zipFile.EntryFileNames.Contains(fileName, StringComparer.InvariantCultureIgnoreCase);
		}

		#endregion ContainsFile Method

		#region ExtractFile Method

		public static Stream ExtractFile(this ZipFile zipFile, string fileName)
		{
			if (zipFile == null)
			{
				throw new ArgumentNullException("zipFile");
			}

			if (string.IsNullOrWhiteSpace(fileName))
			{
				throw new ArgumentNullException("fileName");
			}

			//make sure the zipFile contains the file we want.
			if (!zipFile.ContainsFile(fileName))
			{
				throw new ArgumentException("The " + fileName + " was not found in the zip file", "fileName");
			}

			MemoryStream stream = null;

			try
			{
				stream = new MemoryStream();
				ZipEntry e = zipFile[fileName];
				e.Extract(stream);
				stream.Position = 0;
			}
			catch (Exception ex)
			{
				if (stream != null)
				{
					stream.Dispose();
					stream = null;
				}

				throw new Exception("Unable to extract the file " + fileName, ex);
			}
			return stream;
		}

		#endregion ExtractFile Method

		#region ExtractXmlFile Method

		public static XDocument ExtractXmlFile(this ZipFile zipFile, string fileName)
		{
			if (zipFile == null)
			{
				throw new ArgumentNullException("zipFile");
			}

			if (string.IsNullOrWhiteSpace(fileName))
			{
				throw new ArgumentNullException("fileName");
			}

			//make sure the zipFile contains the file we want.
			if (!zipFile.ContainsFile(fileName))
			{
				throw new ArgumentException("The " + fileName + " was not found in the zip file", "fileName");
			}

			var ms = zipFile.ExtractFile(fileName);
			XDocument xDoc = null;

			try
			{
				xDoc = XDocument.Load(ms);
				ms.Close();
				ms.Dispose();
			}
			catch (Exception ex)
			{
				throw new Exception("Unable to load the file " + fileName + " from the zip file as XML.", ex);
			}

			return xDoc;
		}

		#endregion ExtractXmlFile Method

		#region DoesZipContainSubDirectories Method

		/// <summary>
		/// Returns true if the zip file contains subdirectories
		/// </summary>
		/// <param name="zip"></param>
		/// <returns></returns>
		public static bool DoesZipContainSubDirectories(this ZipFile zip)
		{
			bool result = true;

			var subDirectories = zip.EntryFileNames.Where(fName => fName.Trim().Contains("/")).ToList();
			if (subDirectories.Count == 0)
			{
				result = false;
			}

			return result;
		}

		#endregion DoesZipContainSubDirectories Method
	}
}