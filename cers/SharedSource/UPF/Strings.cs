using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace UPF
{
	/// <summary>
	/// Provides common useful string functions that are not native to .NET.
	/// </summary>
	public static class Strings
	{
		/// <summary>
		/// Email regular expression varlidation string.
		/// </summary>
		public const string EmailValidationExpression = @"^(([^<>()[\]\\.,;:\s@\""]+(\.[^<>()[\]\\.,;:\s@\""]+)*)|(\"".+\""))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$";

		/// <summary>
		/// Method used to create a string with line breaks based on a maximum number of characters per line.
		/// </summary>
		/// <param name="input">The raw string data.</param>
		/// <param name="maxNumberOfCharactersPerLine">The maximum # of characters per line.</param>
		/// <param name="breakExpression">The expression used to enforce the line breaks.</param>
		/// <returns>A string formatted with line breaks.</returns>
		public static string Break( string input, int maxNumberOfCharactersPerLine, string breakExpression )
		{
			StringBuilder result = new StringBuilder();
			if ( !string.IsNullOrEmpty( input ) )
			{
				string[] inputWords = input.Trim().Split( ' ' );
				string currentLine = string.Empty;

				for ( int i = 0; i <= inputWords.Length - 1; i++ )
				{
					if ( inputWords[i].Length > 0 )
					{
						currentLine += " " + inputWords[i].ToString();
						if ( currentLine.Length > maxNumberOfCharactersPerLine )
						{
							if ( ( ( currentLine.Length - 1 ) - inputWords[i].Length ) > 0 )
							{
								if ( ( currentLine.Length - inputWords[i].Length ) > maxNumberOfCharactersPerLine )
								{
									int j = 0;
									string tempWord = currentLine;
									while ( j < currentLine.Length )
									{
										string splitWord = string.Empty;
										if ( ( j + maxNumberOfCharactersPerLine ) <= currentLine.Length )
										{
											splitWord = tempWord.Substring( j, maxNumberOfCharactersPerLine );
										}
										else
										{
											splitWord = tempWord.Substring( j, ( tempWord.Length - j ) );
										}
										j += ( splitWord.Length );
										splitWord += breakExpression;
										result.Append( splitWord.Trim() );
									}
									currentLine = string.Empty;
								}
								else
								{
									currentLine = currentLine.Remove( ( currentLine.Length - inputWords[i].Length ), inputWords[i].Length );

									currentLine += breakExpression;
									result.Append( currentLine.Trim() );
									currentLine = string.Empty;
									currentLine = " " + inputWords[i].ToString();
								}
							}
							else
							{
								int j = 0;
								string tempWord = inputWords[i].ToString();
								while ( j < inputWords[i].Length )
								{
									string splitWord = string.Empty;
									if ( ( j + maxNumberOfCharactersPerLine ) <= inputWords[i].Length )
									{
										splitWord = tempWord.Substring( j, maxNumberOfCharactersPerLine );
									}
									else
									{
										splitWord = tempWord.Substring( j, ( tempWord.Length - j ) );
									}
									j += ( splitWord.Length );
									splitWord += breakExpression;
									result.Append( splitWord.Trim() );
								}
								currentLine = string.Empty;
							}
						}
					}

					if ( i == inputWords.Length - 1 )
					{
						currentLine += breakExpression;
						result.Append( currentLine.Trim() );
						currentLine = string.Empty;
					}
				}
			}
			return result.ToString();
		}

		/// <summary>
		/// Cleans string removes \n and whitspace
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public static string Clean( string input )
		{
			string result = input;
			if ( !string.IsNullOrWhiteSpace( result ) )
			{
				result = result.Replace( "\n", string.Empty );
				result = result.Trim();
			}
			return result;
		}

		public static string ConvertBooleanToString( bool value, string trueValue = "Y", string falseValue = "N" )
		{
			return ( ( value ) ? trueValue : falseValue );
		}

		public static string ConvertByteArrayToString( this byte[] data )
		{
			string results = string.Empty;
			try
			{
				MemoryStream stream = new MemoryStream( data );
				results = IOHelper.ReadToEnd( stream );
			}
			catch
			{
				results = string.Empty;
			}

			return results;
		}

		/// <summary>
		/// Converts a generic list of any type to a string, adding support to put a custom next item delimitter in.
		/// </summary>
		/// <typeparam name="T">The type to be flattened to a string. (Call .ToString() on).</typeparam>
		/// <param name="list">The generic list of items to flatten into a string.</param>
		/// <param name="delimitter">The delimitter to place between each item in the list.</param>
		/// <returns>A <see cref="String"/> containing all items in the list flattened down, separated by the <paramref name="nextLineExpression"/>.</returns>
		public static string ConvertListToString<T>( List<T> list, string delimitter )
		{
			StringBuilder result = new StringBuilder();
			for ( int index = 0; index <= ( list.Count - 1 ); index++ )
			{
				result.Append( list[index] );
				if ( index < ( list.Count - 1 ) )
				{
					result.Append( delimitter );
				}
			}

			return result.ToString();
		}

		/// <summary>
		///
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <returns></returns>
		public static string ConvertListToString<T>( List<T> list )
		{
			StringBuilder result = new StringBuilder();
			for ( int index = 0; index <= list.Count - 1; index++ )
			{
				result.Append( list[index] );
				if ( index < list.Count - 1 )
				{
					result.Append( ", " );
				}
			}
			return result.ToString();
		}

		public static string FormatDate( string input )
		{
			string result = string.Empty;
			if ( !IsNullOrEmpty( input ) )
			{
				DateTime inputDate;
				DateTime.TryParse( input, out inputDate );
				if ( inputDate != null )
				{
					result = inputDate.ToShortDateString();
				}
			}
			return result;
		}

		public static string FormatDateTime( string input )
		{
			string result = string.Empty;
			if ( !IsNullOrEmpty( input ) )
			{
				DateTime inputDate;
				DateTime.TryParse( input, out inputDate );
				if ( inputDate != null )
				{
					result = inputDate.ToShortDateString() + " " + inputDate.ToShortTimeString();
				}
			}
			return result;
		}

		/// <summary>
		/// Formats a string to be in phone number format, (###) ###-####.
		/// </summary>
		/// <param name="input">The string containing the input to format as a phone number.</param>
		/// <returns>The string formatted as a phone number.</returns>
		public static string FormatPhoneNumber( string input )
		{
			string result = string.Empty;
			if ( !String.IsNullOrWhiteSpace( input ) )
			{
				//if first character is letter then just spit it out as is
				if ( char.IsLetter( input.Trim()[0] ) )
				{
					result = input;
				}
				else
				{
					result = input.Trim().Replace( ".", "" ).Replace( "(", "" ).Replace( ")", "" ).Replace( "-", "" ).Replace( " ", "" );
					double y;
					if ( Double.TryParse( result, out y ) )
					{
						if ( y.ToString().StartsWith( "1" ) && y.ToString().Length > 1 )
						{
							y = Double.Parse( y.ToString().Substring( 1 ) );
						}
						result = string.Format( "{0:####################}", y );
					}

					//format long number properly, assumption if > 10 number it is an extension
					if ( result.Length > 10 )
					{
						result = Regex.Replace( result, "[^0-9]", "" );
						if ( result.StartsWith( "1" ) )
							result = result.Substring( 1 );
						if ( result.Length > 9 )
						{
							string ph = result.Substring( 0, 10 );
							string ext = result.Substring( 10 );
							if ( Double.TryParse( ph, out y ) )
							{
								ph = string.Format( "{0:(###) ###-####}", y );
							}
							result = ph + " ext." + ext;
						}
					}
					else
					{
						result = string.Format( "{0:(###) ###-####}", y );
					}
				}
			}
			return result;
		}

		public static string FormatZipCode( string zipCode, bool fiveDigitsOnly = true )
		{
			string result = zipCode;
			if ( !string.IsNullOrWhiteSpace( result ) )
			{
				result = result.Trim();
				int hyphenIndex = result.IndexOf( '-' );
				if ( hyphenIndex > -1 )
				{
					result = result.Replace( "-", "" );
				}

				if ( fiveDigitsOnly && result.Length > 5 )
				{
					result = result.Substring( 0, 5 );
				}
				else if ( result.Length == 9 )
				{
					result = result.Insert( 5, "-" );
				}
			}
			return result;
		}

		public static string FormatZipCode( string input )
		{
			string result = string.Empty;
			if ( !IsNullOrEmpty( input ) )
			{
				if ( input.Length > 5 )
				{
					input = input.Replace( "-", "" ).Trim();
					result = input.Substring( 0, 5 ) + "-" + input.Substring( 5 );
				}
				else
				{
					result = input;
				}
			}
			return result;
		}

		public static string GenerateHash( DateTime input, int maxLength = 8 )
		{
			string hash = GenerateHashString( input.Ticks.ToString() );
			if ( hash.Length > 8 )
			{
				hash = hash.Substring( 0, 8 );
			}
			return hash;
		}

		/// <summary>
		/// Method used to generate a unique hash with a specified length
		/// </summary>
		/// <param name="input">The value to be hashed</param>
		/// <param name="maxLength">The specified length</param>
		/// <returns></returns>
		public static string GenerateHashString( string input, int maxLength )
		{
			return GenerateHashString( input, maxLength, true );
		}

		/// <summary>
		/// Method used to generate a unique hash with a specified length
		/// </summary>
		/// <param name="input">The value to be hashed</param>
		/// <param name="maxLength">The specified length</param>
		/// <returns></returns>
		public static string GenerateHashString( string input, int maxLength, bool stripUrlChars )
		{
			string result = GenerateHashString( input );
			if ( stripUrlChars )
			{
				result = StripOutCharRange( result, 32, 47 );
				result = StripOutCharRange( result, 58, 64 );
				result = StripOutCharRange( result, 91, 96 );
				result = StripOutCharRange( result, 123, 126 );
				result = StripOutCharRange( result, 128, 255 );
			}

			if ( result.Length > maxLength )
			{
				result = result.Remove( maxLength - 1 );
			}
			return result;
		}

		/// <summary>
		/// Method used to generate a unique hash using the <see cref="SHA512Managed"/> hash provider.
		/// </summary>
		/// <param name="input">The value to be hashed</param>
		/// <returns></returns>
		public static string GenerateHashString( string input )
		{
			string result = string.Empty;
			SHA512Managed sha = new SHA512Managed();
			byte[] buffer = Encoding.UTF8.GetBytes( input );
			byte[] hashResult = sha.ComputeHash( buffer );
			result = Convert.ToBase64String( hashResult );
			return result;
		}

		public static string GetCharAt( this string value, int index, string defaultReturnValue )
		{
			if ( !string.IsNullOrEmpty( value ) && value.Length > index )
			{
				return value.Substring( index, 1 );
			}
			else
			{
				return defaultReturnValue;
			}
		}

		public static string GetContentBetweenHtmlTags( string body, string startTagName = "<body>", string endTagName = "</body>" )
		{
			string result = string.Empty;
			if ( !string.IsNullOrWhiteSpace( body ) )
			{
				int startTagPos = body.IndexOf( startTagName ) + startTagName.Length;
				result = body.Substring( startTagPos );

				int endTagPos = result.IndexOf( endTagName );

				result = result.Substring( 0, endTagPos );
			}
			return result;
		}

		/// <summary>
		/// Gets the domain portion of an email address (all characters after the @ symbol).
		/// </summary>
		/// <param name="email">The <see cref="String"/> containing a full and valid email adddress to get the domain part for.</param>
		/// <returns>The domain portion of the email address specified on the <paramref name="email"/></returns>
		public static string GetEmailDomain( string email )
		{
			if ( string.IsNullOrWhiteSpace( email ) )
			{
				throw new ArgumentNullException( "email" );
			}

			if ( !IsValidEmail( email ) )
			{
				throw new ArgumentException( "The value specified '" + email + "' is not valid email format.", "email" );
			}

			string result = String.Empty;
			int index = email.Trim().IndexOf( "@" );
			if ( index > -1 )
			{
				result = email.Trim().Substring( index + 1 );
			}
			return result;
		}

		public static List<string> GetHtmlImages( string input )
		{
			//set up the regex for finding images
			StringBuilder imgPattern = new StringBuilder();
			imgPattern.Append( "<img[^>]+" ); //start 'img' tag
			imgPattern.Append( "src\\s*=\\s*" ); //start src property
			imgPattern.Append( "(?:\"(?<src>[^\"]*)\"|'(?<src>[^']*)'|(?<src>[^\"'>\\s]+))" );
			imgPattern.Append( "[^>]*>" ); //end of tag

			Regex imgRegex = new Regex( imgPattern.ToString(), RegexOptions.IgnoreCase );

			//look for matches
			Match imgcheck = imgRegex.Match( input );
			List<string> imagelist = new List<string>();
			while ( imgcheck.Success )
			{
				string src = imgcheck.Groups["src"].Value;
				if ( !imagelist.Contains( src ) )
				{
					imagelist.Add( src );
				}
				imgcheck = imgcheck.NextMatch();
			}
			return imagelist;
		}

		/// <summary>
		/// Scrapes a web page and parses out all the links.
		/// </summary>
		/// <param name="url"></param>
		/// <param name="makeLinkable"></param>
		/// <returns></returns>
		public static List<string> GetHtmlLinks( string input )
		{
			//set up the regex for finding the link urls
			StringBuilder hrefPattern = new StringBuilder();
			hrefPattern.Append( "<a[^>]+" ); //start 'a' tag and anything that comes before 'href' tag
			hrefPattern.Append( "href\\s*=\\s*" ); //start href property
			hrefPattern.Append( "(?:\"(?<href>[^\"]*)\"|'(?<href>[^']*)'|(?<href>[^\"'>\\s]+))" );
			hrefPattern.Append( "[^>]*>.*?</a>" ); //end of 'a' tag

			Regex hrefRegex = new Regex( hrefPattern.ToString(), RegexOptions.IgnoreCase );

			//look for matches
			Match hrefcheck = hrefRegex.Match( input );
			List<string> linklist = new List<string>();
			while ( hrefcheck.Success )
			{
				string href = hrefcheck.Groups["href"].Value;
				if ( !linklist.Contains( href ) )
				{
					linklist.Add( href );
				}
				hrefcheck = hrefcheck.NextMatch();
			}
			return linklist;
		}

		public static string GetSHA1Hash( string input )
		{
			if ( string.IsNullOrWhiteSpace( input ) )
			{
				return input;
			}
			SHA1Managed sha = new SHA1Managed();
			byte[] data = Encoding.UTF8.GetBytes( input );
			byte[] hash = sha.ComputeHash( data );
			return Convert.ToBase64String( hash );
		}

		/// <summary>
		/// Removes all non numeric characters from a string in the order they are placed.
		/// </summary>
		/// <param name="input">The string possibly contain alpha and numeric characters to parse.</param>
		/// <returns>A <see cref="String"/> containing only numbers.</returns>
		public static string GleanNumbers( string input )
		{
			return GleanNumbers( input, false, false );
		}

		/// <summary>
		/// Removes all non numeric characters from a string in the order they are placed.
		/// </summary>
		/// <param name="input">The string possibly contain alpha and numeric characters to parse.</param>
		/// <param name="periodAllowed">Specifies whether periods are allowed to be returned.</param>
		/// <param name="hyphenAllowed">Specifies whether hyphens (-) are allowed to be returned.</param>
		/// <returns>A numbers only <see cref="String"/>.</returns>
		public static string GleanNumbers( string input, bool periodAllowed, bool hyphenAllowed )
		{
			StringBuilder result = new StringBuilder();
			char[] inputCharArray = input.ToCharArray();
			Int32 parsedCharacter;
			foreach ( char inputCharacter in inputCharArray )
			{
				if ( periodAllowed && inputCharacter == '.' )
				{
					result.Append( "." );
				}
				if ( hyphenAllowed && inputCharacter == '-' )
				{
					result.Append( "-" );
				}
				if ( Int32.TryParse( Convert.ToString( inputCharacter ), out parsedCharacter ) )
				{
					result.Append( inputCharacter );
				}
			}
			return result.ToString();
		}

		public static string HtmlDecode( this string value )
		{
			return System.Net.WebUtility.HtmlDecode( value );
		}

        public static string HtmlEncode( this string value )
        {
            return System.Net.WebUtility.HtmlEncode( value );
        }

		public static bool IsAllNullOrEmpty( params string[] args )
		{
			bool result = true;
			if ( args != null )
			{
				if ( args.Length > 1 )
				{
					foreach ( string arg in args )
					{
						result &= Strings.IsNullOrEmpty( arg );
					}
				}
				else if ( args.Length == 1 )
				{
					result = Strings.IsNullOrEmpty( args[0] );
				}
			}
			else
			{
				result = false;
			}
			return result;
		}

		/// <summary>
		/// Helper method to overcome the limitations of the standard <see cref="String.IsNullOrEmpty"/> method, by trimming the string if its not null.
		/// </summary>
		/// <param name="input">The <see cref="String"/> to determine if it is empty.</param>
		/// <returns></returns>
		public static bool IsNullOrEmpty( string input )
		{
			return IsNullOrEmpty( input, true );
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="input"></param>
		/// <param name="trim"></param>
		/// <returns></returns>
		public static bool IsNullOrEmpty( string input, bool trim )
		{
			bool result = true;
			if ( !string.IsNullOrEmpty( input ) )
			{
				if ( trim && input.Trim().Length > 0 )
				{
					result = false;
				}
				else if ( input.Length > 0 )
				{
					result = false;
				}
			}
			return result;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="emailAddressToValidate"></param>
		/// <returns></returns>
		public static bool IsValidEmail( string emailAddressToValidate )
		{
			Match match = Regex.Match( emailAddressToValidate, EmailValidationExpression, RegexOptions.IgnoreCase );
			return match.Success;
		}

		/// <summary>
		/// MD5 encodes the passed string.
		/// </summary>
		/// <param name="input">The string to encode.</param>
		/// <returns>An encoded string.</returns>
		public static string MD5String( string input, int? maxLength = null )
		{
			// Create a new instance of the MD5CryptoServiceProvider object.
			MD5 md5Hasher = MD5.Create();

			// Convert the input string to a byte array and compute the hash.
			byte[] data = md5Hasher.ComputeHash( Encoding.Default.GetBytes( input ) );

			// Create a new Stringbuilder to collect the bytes
			// and create a string.
			StringBuilder sBuilder = new StringBuilder();

			// Loop through each byte of the hashed data
			// and format each one as a hexadecimal string.
			for ( int i = 0; i < data.Length; i++ )
			{
				sBuilder.Append( data[i].ToString( "x2" ) );
			}

			string result = sBuilder.ToString();
			if ( maxLength.HasValue )
			{
				if ( result.Trim().Length > maxLength.Value )
				{
					result = result.Substring( 0, maxLength.Value );
				}
			}

			return result;
		}

		/// <summary>
		/// Verified a string against the passed MD5 hash.
		/// </summary>
		/// <param name="input">The string to compare.</param>
		/// <param name="hash">The hash to compare against.</param>
		/// <returns>True if the input and the hash are the same, false otherwise.</returns>
		public static bool MD5VerifyString( string input, string hash )
		{
			// Hash the input.
			string hashOfInput = MD5String( input );

			// Create a StringComparer an comare the hashes.
			StringComparer comparer = StringComparer.OrdinalIgnoreCase;

			if ( 0 == comparer.Compare( hashOfInput, hash ) )
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public static string NormalizeName( string input )
		{
			string result = string.Empty;

			if ( !IsNullOrEmpty( input ) )
			{
				result = input;
				//result = NormalizeName(result, " ");
				result = NormalizeName( result, "-" );
				result = NormalizeName( result, " " );
				result = NormalizeName( result, "'" );
				result = NormalizeName( result, "." );
				result = NormalizeName( result, "Mc" );
			}
			return result;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="input"></param>
		/// <param name="character"></param>
		/// <returns></returns>
		public static string NormalizeName( string input, string character )
		{
			string result = string.Empty;
			if ( !IsNullOrEmpty( input ) )
			{
				result = input;
				int hyphenPos = -1;

				if ( !string.IsNullOrWhiteSpace( character ) )
				{
					hyphenPos = input.IndexOf( character );
				}

				if ( hyphenPos > -1 )
				{
					string firstPart = input.Substring( 0, hyphenPos );
					string secondPart = input.Substring( hyphenPos + character.Length );
					if ( secondPart.Trim().Length > character.Length )
					{
						secondPart = TitleCase( secondPart );
					}
					result = firstPart + character + secondPart;
				}
				else
				{
					result = TitleCase( input );
				}
			}
			return result;
		}

		public static string NormalizeUrl( string url )
		{
			if ( !url.EndsWith( "/" ) )
			{
				url += "/";
			}
			return url;
		}

		/// <summary>
		/// Returns a string of length <paramref name="size"/> filled
		/// with random ASCII characters in the range A-Z, a-z. If <paramref name="lowerCase"/>
		/// is <c>true</c>, then the range is only a-z.
		/// </summary>
		/// <param name="size">The size.</param>
		/// <param name="lowerCase">if set to <c>true</c> [lower case].</param>
		/// <returns></returns>
		public static string RandomString( int size, bool lowerCase )
		{
			StringBuilder builder = new StringBuilder();
			Random random = new Random( unchecked( (int)DateTime.UtcNow.Ticks ) );
			char ch;
			for ( int i = 0; i < size; i++ )
			{
				ch = Convert.ToChar( Convert.ToInt32( Math.Floor( 26 * random.NextDouble() + 65 ) ) );
				builder.Append( ch );
			}
			if ( lowerCase )
			{
				return builder.ToString().ToLower();
			}
			return builder.ToString();
		}

		/// <summary>
		/// Ensures that within <paramref name="str"/> there are no two
		/// consecutive whitespace characters.
		/// </summary>
		/// <param name="input">The STR.</param>
		/// <returns></returns>
		public static string RemoveConsecutiveWhitespace( string input )
		{
			if ( string.IsNullOrWhiteSpace( input ) )
			{
				return input;
			}
			return ReplaceConsecutiveWhitespace( input, " " );
		}

		/// <summary>
		/// Ensures that within <paramref name="str"/> there are no two
		/// consecutive whitespace characters.
		/// </summary>
		/// <param name="input">The <see cref="String"/> containing the data to replace consective whitespace.</param>
		/// <param name="replacement">The <see cref="String"/> to replace consecutive whitespace in.</param>
		/// <returns></returns>
		public static string ReplaceConsecutiveWhitespace( string input, string replacement )
		{
			string result = input;
			if ( !string.IsNullOrWhiteSpace( input ) )
			{
				result = Regex.Replace( result, @"\s+", replacement, RegexOptions.Compiled );
			}
			return result;
		}

		/// <summary>
		/// Converts a string to sentence case.
		/// </summary>
		/// <param name="input">The string to convert.</param>
		/// <returns>A string</returns>
		public static string SentenceCase( string input )
		{
			if ( input.Length < 1 )
				return input;

			string sentence = input.ToLower();
			return sentence[0].ToString().ToUpper() + sentence.Substring( 1 );
		}

		/// <summary>
		/// Splits a <see cref="String"/> by a <see cref="Char"/> delimiter.
		/// </summary>
		/// <param name="list">The <see cref="String"/> containg the data to split out into a list.</param>
		/// <param name="delimiter">The <see cref="Char"/> containing the character to split by.</param>
		/// <returns>A generic list of strings split out from the <paramref name="list"/> and <paramref name="delimiter"/>.</returns>
		public static List<string> Split( string list, char delimitter )
		{
			List<string> result = new List<string>();
			string[] items = list.Split( new char[] { delimitter }, StringSplitOptions.RemoveEmptyEntries );
			foreach ( string item in items )
			{
				if ( item.Trim().Length > 0 )
				{
					result.Add( item.Trim() );
				}
			}
			return result;
		}

		public static List<int> SplitToIntegers( string list, char delimitter = ',' )
		{
			List<int> result = new List<int>();
			if ( !string.IsNullOrWhiteSpace( list ) )
			{
				int temp;
				string[] items = list.Split( new char[] { delimitter }, StringSplitOptions.RemoveEmptyEntries );
				foreach ( string item in items )
				{
					temp = int.Parse( item );
					result.Add( temp );
				}
			}
			return result;
		}

		public static string StripNonAlphaCharacters( string str )
		{
			StringBuilder sb = new StringBuilder();
			foreach ( char c in str )
			{
				if ( ( c >= '0' && c <= '9' ) || ( c >= 'A' && c <= 'Z' ) || ( c >= 'a' && c <= 'z' ) )
				{
					sb.Append( c );
				}
			}
			return sb.ToString();
		}

		public static string StripNonNumerics( string str )
		{
			StringBuilder sb = new StringBuilder();
			foreach ( char c in str )
			{
				if ( c >= '0' && c <= '9' )
				{
					sb.Append( c );
				}
			}
			return sb.ToString();
		}

		public static string StripOutCharRange( string input, int start, int end )
		{
			string result = input;
			for ( int c = start; c <= end; c++ )
			{
				char character = (char)c;
				string stringChar = character.ToString();
				result = result.Replace( stringChar, "" );
			}

			return result;
		}

		/// <summary>
		/// Removes all HTML tags from the passed string
		/// </summary>
		/// <param name="input">The string whose values should be replaced.</param>
		/// <returns>A string.</returns>
		public static string StripTags( string input )
		{
			Regex stripTags = new Regex( "<(.|\n)+?>" );
			return stripTags.Replace( input, "" );
		}

		public static string SubStringPro( string input, int maxNumberOfCharacters, bool removeConsecutiveWhiteSpaces = false, string truncationSuffix = "" )
		{
			//Trim the input
			string result = input;
			if ( !string.IsNullOrWhiteSpace( result ) )
			{
				result = result.Trim();

				if ( removeConsecutiveWhiteSpaces == true )
				{
					result = Strings.RemoveConsecutiveWhitespace( result );
				}

				//Check if the length of the input greater than maxNumberOfCharacters
				if ( result.Length > maxNumberOfCharacters )
				{
					result = result.Substring( 0, maxNumberOfCharacters );
					if ( !string.IsNullOrWhiteSpace( truncationSuffix ) )
					{
						result += truncationSuffix;
					}
				}
				else
				{
					result = input;
				}
			}
			return result;
		}

		/// <summary>
		/// Converts a string to title case.
		/// </summary>
		/// <param name="input">The string to convert.</param>
		/// <returns>A string.</returns>
		public static string TitleCase( string input )
		{
			return TitleCase( input, true );
		}

		/// <summary>
		/// Converts a string to title case.
		/// </summary>
		/// <param name="input">The string to convert.</param>
		/// <param name="ignoreShortWords">If true, does not capitalize words like
		/// "a", "is", "the", etc.</param>
		/// <returns>A string.</returns>
		public static string TitleCase( string input, bool ignoreShortWords )
		{
			List<string> ignoreWords = null;
			if ( ignoreShortWords )
			{
				//TODO: Add more ignore words?
				ignoreWords = new List<string>();
				ignoreWords.Add( "a" );
				ignoreWords.Add( "is" );
				ignoreWords.Add( "was" );
				ignoreWords.Add( "the" );
			}

			string[] tokens = input.Split( new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries );
			StringBuilder sb = new StringBuilder( input.Length );
			foreach ( string s in tokens )
			{
				if ( ignoreShortWords == true
					&& s != tokens[0]
					&& ignoreWords.Contains( s.ToLower() ) )
				{
					sb.Append( s + " " );
				}
				else
				{
					sb.Append( s[0].ToString().ToUpper() );
					sb.Append( s.Substring( 1 ).ToLower() );
					sb.Append( " " );
				}
			}

			return sb.ToString().Trim();
		}

        /// <summary>
        /// generate acronym for a given input i.e.
        ///     Federal Bureau of Investigation => FBI
        ///     Under Storage Tank => UST
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string GetAcronym( this string input )
        {
            string acronymn = "";
            for ( int i = 0; i < input.Length; i++ )
            {
                if ( input[i].ToString() == input[i].ToString().ToUpper() )
                {
                    acronymn += input[i].ToString();
                }
            }
            return acronymn;
        }

		/// <summary>
		/// Takes care of NULL strings. Returns empty string in case of NULL input value
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public static string TrimPro( string input )
		{
			if ( string.IsNullOrWhiteSpace( input ) )
			{
				return string.Empty;
			}
			return input.Trim();
		}

		/// <summary>
		/// Truncates a string to the specified maximum number of words.
		/// </summary>
		/// <param name="input">The <see cref="String"/> containing the text to truncate.</param>
		/// <param name="maxNumberOfCharacters">The <see cref="Int32"/> value representing the maximum number of characters the result will contain.</param>
		/// <returns>A <see cref="String"/> containing the result of truncation.</returns>
		/// <remarks>
		/// This overload will not truncate on words containing the following predefined expressions:
		///
		/// - Hyphen
		/// \ - Backward Slash
		/// / - Forward Slash
		/// , - Comma
		/// </remarks>
		public static string Truncate( string input, int maxNumberOfCharacters, bool smart = true )
		{
			string result = input;
			if ( smart )
			{
				result = Truncate( input, maxNumberOfCharacters, "-", @"\", "/", "," );
			}
			else
			{
				result = input.SubStringPro( maxNumberOfCharacters, true, "..." );
			}
			return result;
		}

		/// <summary>
		/// Truncates a string to the specified maximum number of words.
		/// </summary>
		/// <param name="input">The <see cref="String"/> containing the text to truncate.</param>
		/// <param name="maxNumberOfCharacters">The <see cref="Int32"/> value representing the maximum number of characters the result will contain.</param>
		/// <param name="noTruncateExpressions">An array of <see cref="String"/>'s that contain expressions that prevent truncation of a word with one of the expressions.</param>
		/// <returns>A <see cref="String"/> containing the result of truncation.</returns>
		public static string Truncate( string input, int maxNumberOfCharacters, params string[] noTruncateExpressions )
		{
			StringBuilder result = new StringBuilder();
			input = input.Trim();
			if ( input.Length > maxNumberOfCharacters )
			{
				string lastWord = string.Empty;
				string word = string.Empty;
				string[] words = input.Replace( "  ", " " ).Split( ' ' );
				int lengthAppended = 0;
				for ( int w = 0; w <= words.Length - 1; w++ )
				{
					word = words[w];
					if ( ( lengthAppended < maxNumberOfCharacters ) && ( ( lengthAppended + word.Length ) < maxNumberOfCharacters ) )
					{
						result.Append( word + " " );
						lengthAppended += word.Length + 1;
					}
					else if ( ( lengthAppended < maxNumberOfCharacters ) && ( ( lengthAppended + word.Length ) > maxNumberOfCharacters ) )
					{
						//it will overflow it, so we need to stop processing.
						break;
					}
					lastWord = word;
				}

				string temp = result.ToString().Trim();
				result.Clear();
				result.Append( temp );
				result.Append( "..." );
			}
			else
			{
				result.Append( input );
			}
			return result.ToString().Trim();
		}

		private static bool IsNoTruncateExpression( string text, params string[] noTruncateExpressions )
		{
			bool result = false;
			if ( !string.IsNullOrEmpty( text ) && noTruncateExpressions != null )
			{
				foreach ( string noTruncateExpression in noTruncateExpressions )
				{
					if ( string.Compare( text.Trim(), noTruncateExpression.Trim(), true ) == 0 )
					{
						result = true;
						break;
					}
				}
			}
			return result;
		}
	}
}