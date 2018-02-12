using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.SessionState;

namespace UPF.Web
{
	public static class HttpUtility
	{
		private const string LastServerErrorKey = "LastServerError";

		public static Exception LastServerError
		{
			get { return Session[LastServerErrorKey] as Exception; }
			set { Session[LastServerErrorKey] = value; }
		}

		public static HttpSessionState Session
		{
			get { return Context.Session; }
		}

		public static HttpContext Context
		{
			get { return HttpContext.Current; }
		}

		public static HttpRequest Request
		{
			get { return Context.Request; }
		}

		public static HttpResponse Response
		{
			get { return Context.Response; }
		}

		public static string ReadCookie(string name)
		{
			string result = string.Empty;
			HttpCookie cookie = Request.Cookies[name];
			if (cookie != null)
			{
				result = cookie.Value;
			}
			return result;
		}

		public static void WriteCookie(string name, string value)
		{
			DateTime expirationDate = DateTime.Now.AddDays(30);
			WriteCookie(name, value, expirationDate);
		}

		public static void WriteCookie(string name, string value, DateTime expirationDate)
		{
			HttpCookie cookie = new HttpCookie(name);
			cookie.Value = value;
			cookie.Expires = expirationDate;
			Response.Cookies.Add(cookie);
		}
	}
}