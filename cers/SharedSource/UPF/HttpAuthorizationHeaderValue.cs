using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using UPF;

namespace UPF
{
	public class HttpAuthorizationHeaderValue
	{
		public string Mode { get; set; }

		public string Username { get; set; }

		public string Password { get; set; }

		public virtual string HashedPassword
		{
			get
			{
				string result = string.Empty;
				if (!string.IsNullOrWhiteSpace(Password))
				{
					SHA1 sha = SHA1.Create();
					var hashedResult = sha.ComputeHash(Encoding.UTF8.GetBytes(Password));
					result = Convert.ToBase64String(hashedResult);
				}
				return result;
			}
		}

		public bool IsEmpty
		{
			get
			{
				bool result = false;
				if (!string.IsNullOrWhiteSpace(Username))
				{
					result = true;
				}

				if (!string.IsNullOrWhiteSpace(Password))
				{
					result = true;
				}
				return result;
			}
		}

		public HttpAuthorizationHeaderValue()
			: this(string.Empty, string.Empty)
		{
		}

		public HttpAuthorizationHeaderValue(string username, string password, string mode = "user")
		{
			Mode = mode;
			Username = username;
			Password = password;
		}

		public HttpAuthorizationHeaderValue(string authorizationHeader)
		{
			ParseTo(authorizationHeader, this);
		}

		public override string ToString()
		{
			return Mode + " " + Username + ":" + Password;
		}

		public string ToHashedPasswordString()
		{
			return Mode + " " + Username + ":" + HashedPassword;
		}

		public static HttpAuthorizationHeaderValue Parse(string input)
		{
			input.CheckNull("input");
			HttpAuthorizationHeaderValue result = new HttpAuthorizationHeaderValue();
			ParseTo(input, result);
			return result;
		}

		private static void ParseTo(string input, HttpAuthorizationHeaderValue result)
		{
			//first find space.
			int spacePos = input.IndexOf(" ");
			if (spacePos > -1)
			{
				result.Mode = input.Substring(0, spacePos);
				input = input.Substring(spacePos + 1);

				var parts = input.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
				if (parts.Length == 2)
				{
					result.Username = parts[0];
					result.Password = parts[1];
				}
			}
		}
	}
}