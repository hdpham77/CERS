using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace UPF
{
	public static class DateUtilities
	{
		public static TimeSpan? CalculateElapsedTime( DateTime? start, DateTime? end )
		{
			TimeSpan? result = null;
			if ( start != null && end == null )
			{
				//calculate start time against now.
				result = DateTime.Now - start.Value;
			}
			else if ( start != null && end != null )
			{
				result = end.Value - start.Value;
			}
			return result;
		}

		/// <summary>
  /// ShortMonth - converts date to custom short month
  /// </summary>
  /// <param name="date"></param>
  /// <returns></returns>
		public static string ShortMonth( DateTime date )
		{
			try
			{
				CultureInfo culture = CustomFormats();
				string retVal = null;
				DateTime dt;

				if ( date == null || !DateTime.TryParse( date.ToString(), out dt ) )
				{
					// TODO: Provide return value for error condition
				}

				// parse custom culture settings...
				DateTime.Parse( date.ToString(), culture );
				retVal = Convert.ToString( culture.DateTimeFormat.AbbreviatedMonthNames.GetValue( date.Month ) );

				return retVal;
			}
			catch { }

			return string.Empty;
		}

		public static string ToDateText( this DateTime? input, string defaultValue = "" )
		{
			string result = defaultValue;
			if ( input.HasValue )
			{
				result = input.Value.ToShortDateString();
			}
			return result;
		}

		#region DateTime

		public static DateTime? ToBeginningOfDay( this DateTime? dateTime, DateTime? defaultValue = null )
		{
			DateTime? result = defaultValue;
			if ( dateTime != null )
			{
				result = dateTime.Value.ToBeginningOfDay();
			}
			return result;
		}

		public static DateTime ToBeginningOfDay( this DateTime dateTime )
		{
			return dateTime.Date;
		}

		public static DateTime? ToEndOfDay( this DateTime? dateTime, DateTime? defaultValue = null )
		{
			DateTime? result = defaultValue;
			if ( dateTime != null )
			{
				result = dateTime.Value.ToEndOfDay();
			}
			return result;
		}

		public static DateTime ToEndOfDay( this DateTime dateTime )
		{
			return dateTime.Date.AddHours( 23 ).AddMinutes( 59 ).AddSeconds( 59 );
		}

		#endregion DateTime

		private static CultureInfo CustomFormats()
		{
			var culture = new CultureInfo( "en-US" );

			#region Custom Date Formats

			// Customized culture settings for short Month name abbreviations
			culture.DateTimeFormat.AbbreviatedMonthNames = new string[]
                {
                "","Jan.", "Feb.", "Mar.", "Apr.", "May", "June", "July", "Aug.", "Sep.", "Oct.", "Nov.", "Dec."
                };

			#endregion Custom Date Formats

			return culture;
		}
	}
}