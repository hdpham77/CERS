using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace UPF
{
	public static class CollectionExtensionMethods
	{
		public static T GetValue<T>( this NameValueCollection collection, string key, T defaultValue )
		{
			T result = defaultValue;
			if ( collection.AllKeys.Contains( key.Trim() ) )
			{
				result = Data.ChangeType<T>( collection[key] );
			}
			return result;
		}

		public static T GetValueLowercaseKey<T>( this NameValueCollection collection, string key, T defaultValue )
		{
			T result = defaultValue;
			if ( collection.AllKeys.Contains( key.ToLower().Trim() ) )
			{
				result = Data.ChangeType<T>( collection[key] );
			}
			return result;
		}

		public static string ToDelimitedString<T>( this IEnumerable<T> collection, string delimiter )
		{
			StringBuilder result = new StringBuilder();
			int count = collection.Count();
			int index = 0;
			foreach ( T item in collection )
			{
				result.Append( item );
				if ( index < ( count - 1 ) )
				{
					result.Append( delimiter );
				}
				index++;
			}
			return result.ToString();
		}

		public static NameValueCollection ToLowercaseKey( this NameValueCollection collection )
		{
			NameValueCollection results = new NameValueCollection();
			foreach ( var key in collection.AllKeys )
			{
				results.Add( key.ToLower(), collection[key] );
			}
			return results;
		}

		#region ToList Method

		public static List<string> ToList( this string input, char delimitter )
		{
			if ( string.IsNullOrWhiteSpace( input ) )
			{
				return new List<string>();
			}
			return input.Split( new char[] { delimitter }, StringSplitOptions.RemoveEmptyEntries ).ToList();
		}

		#endregion ToList Method
	}
}