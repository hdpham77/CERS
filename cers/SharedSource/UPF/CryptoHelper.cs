using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace UPF
{
	public static class CryptoHelper
	{
		public static string CalculateFileMD5Hash(string fileName)
		{
			if (string.IsNullOrWhiteSpace(fileName))
			{
				throw new ArgumentNullException("fileName");
			}

			if (!File.Exists(fileName))
			{
				throw new FileNotFoundException("File could not be found", fileName);
			}

			string hash = string.Empty;
			using (FileStream fs = new FileStream(fileName, FileMode.Open))
			{
				hash = CalculateMD5Hash(fs);
				fs.Close();
			}
			return hash;
		}

		public static string CalculateMD5Hash(Stream stream)
		{
			StringBuilder sb = new StringBuilder();
			MD5 md5 = new MD5CryptoServiceProvider();
			byte[] hash = null;
			hash = md5.ComputeHash(stream);

			foreach (byte hex in hash)
			{
				sb.Append(hex.ToString("x2"));
			}

			string md5sum = sb.ToString();
			return md5sum;
		}
	}
}