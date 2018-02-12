using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace UPF
{
	public static class StringsExtensionMethods
	{
		private static readonly Regex NameExpression = new Regex( "([A-Z]+(?=$|[A-Z][a-z])|[A-Z]?[a-z]+)", RegexOptions.Compiled );

		public static StringBuilder AddDebugInfo( this StringBuilder sb, string text, string end = "<br/>" )
		{
			return sb.Append( text ).Append( end );
		}

		public static StringBuilder AppendHtmlElementEnd( this StringBuilder builder, string tag )
		{
			builder.Append( "</" ).Append( tag ).Append( ">" );
			return builder;
		}

		public static StringBuilder AppendHtmlElementStart( this StringBuilder builder, string tag, string cssClass = null, string style = "" )
		{
			builder.Append( "<" ).Append( tag );
			if ( !string.IsNullOrWhiteSpace( cssClass ) )
			{
				builder.Append( " class=\"" ).Append( cssClass ).Append( "\"" );
			}

			if ( !string.IsNullOrWhiteSpace( style ) )
			{
				builder.Append( " style=\"" ).Append( style ).Append( "\"" );
			}
			builder.Append( ">" );
			return builder;
		}

		public static void AppendHtmlFooter( this StringBuilder stringBuilder, string footerContent = null )
		{
			if ( !string.IsNullOrWhiteSpace( footerContent ) )
			{
				stringBuilder.Append( footerContent );
			}
			stringBuilder.Append( "</body>" );
			stringBuilder.Append( "</html>" );
		}

		public static void AppendHtmlHeader( this StringBuilder stringBuilder, string title, string style = null )
		{
			stringBuilder.Append( "<html><head><title>" ).Append( title ).Append( "</title>" );
			stringBuilder.Append( "<style type=\"text/css\">" );
			if ( string.IsNullOrWhiteSpace( style ) )
			{
				stringBuilder.Append( "body {\r\n\tfont-family:Calibri,Arial,Verdana, Sans;\r\n\tfont-size:.8em;\r\n}" );
			}
			else
			{
				stringBuilder.Append( style );
			}
			stringBuilder.Append( "</style>" );
			stringBuilder.Append( "</head>" );
			stringBuilder.Append( "<body>" );
		}

		public static StringBuilder AppendHtmlTable( this StringBuilder builder, string cssClass = null, string style = "border:solid 1px #000;width:100%;" )
		{
			return builder.AppendHtmlElementStart( "table", cssClass, style );
		}

		public static StringBuilder AppendHtmlTableCell( this StringBuilder builder, string text, string cssClass = null, string style = null )
		{
			builder.AppendHtmlElementStart( "td", cssClass, style );
			if ( !string.IsNullOrWhiteSpace( text ) )
			{
				builder.Append( text );
				builder.AppendHtmlElementEnd( "td" );
			}
			return builder;
		}

		public static StringBuilder AppendHtmlTableHeaderCell( this StringBuilder builder, string text, string cssClass = null, string style = null )
		{
			builder.AppendHtmlElementStart( "th", cssClass, style );
			if ( !string.IsNullOrWhiteSpace( text ) )
			{
				builder.Append( text );
				builder.AppendHtmlElementEnd( "th" );
			}
			return builder;
		}

		public static StringBuilder AppendHtmlTableRow( this StringBuilder builder, string cssClass = null, string style = null )
		{
			return builder.AppendHtmlElementStart( "tr", cssClass, style );
		}

		public static void AppendIfNotEmpty( this StringBuilder result, string text, string prefix = null, string suffix = null )
		{
			if ( result == null )
			{
				throw new ArgumentNullException( "result" );
			}
			if ( !string.IsNullOrWhiteSpace( text ) )
			{
				if ( !string.IsNullOrWhiteSpace( prefix ) )
				{
					result.Append( prefix );
				}
				result.Append( text );

				if ( !string.IsNullOrWhiteSpace( suffix ) )
				{
					result.Append( suffix );
				}
			}
		}

		public static StringBuilder AppendLabelValue( this StringBuilder builder, string label, object value, string end = "<br/>", string nullValue = "--" )
		{
			builder.Append( "<b>" ).Append( label ).Append( "</b>: " );
			if ( value != null )
			{
				builder.Append( value );
			}
			else
			{
				builder.Append( nullValue );
			}
			builder.Append( end );
			return builder;
		}

		public static void AppendLineFormat( this StringBuilder stringBuilder, string source, params object[] args )
		{
			stringBuilder.AppendFormat( source + "\r\n", args );
		}

		public static StringBuilder AppendNewLine( this StringBuilder builder, int count = 1 )
		{
			if ( count > 1 )
			{
				for ( int i = 0; i <= count - 1; i++ )
				{
					builder.Append( "\r\n" );
				}
			}
			else
			{
				builder.Append( "\r\n" );
			}
			return builder;
		}

		public static StringBuilder AppendSubTitle( this StringBuilder builder, string title, string end = "<br/>" )
		{
			builder.Append( "<b>-----" ).Append( title ).Append( "-----</b>" ).Append( end );
			return builder;
		}

		public static StringBuilder AppendTab( this StringBuilder builder, int count = 1 )
		{
			if ( count > 1 )
			{
				for ( int i = 0; i <= count - 1; i++ )
				{
					builder.Append( "\t" );
				}
			}
			else
			{
				builder.Append( "\t" );
			}
			return builder;
		}

		public static StringBuilder AppendTitle( this StringBuilder builder, string title, string end = "<br/>" )
		{
			builder.Append( "<b>=========" ).Append( title ).Append( "=========</b>" ).Append( end );
			return builder;
		}

		public static string AsTitle( this string value )
		{
			if ( value == null )
			{
				return string.Empty;
			}

			int lastIndex = value.LastIndexOf( ".", StringComparison.Ordinal );

			if ( lastIndex > -1 )
			{
				value = value.Substring( lastIndex + 1 );
			}

			return value.ToPascalCase();
		}

		public static float CalculateSimilarity( this string first, string second )
		{
			return LevenshteinDistance.GetSimilarity( first, second );
		}

		public static string Clean( this string input )
		{
			return Strings.Clean( input );
		}

        public static string CleanUpEmailSubject( this string subject )
        {
            subject = subject.Replace( '\r', ' ' ).Replace( '\n', ' ' );
            subject = Regex.Replace( subject, @"[^ -~]", "" );

            int MAX_SUBJECT_LENGTH = 165;
            if ( subject.Length > MAX_SUBJECT_LENGTH )
            {
                subject = subject.SubStringPro( MAX_SUBJECT_LENGTH, true, "..." );
            }

            return subject;
        }

		public static IEnumerable<string> Format( this IEnumerable<string> strings, string formatString )
		{
			return from s in strings
				   select string.Format( formatString, s );
		}

		public static string FormatFacilityID( this string input )
		{
			string result = input ?? "";
			if ( !string.IsNullOrWhiteSpace( result ) )
			{
				//trim it.
				result = result.Trim();
				//if there is any formatting (hyphens) we take as is and do not parse it.
				if ( !result.Contains( "-" ) )
				{
					if ( result.Length == 11 )
					{
						string temp = result.Substring( 0, 2 );
						temp += "-" + result.Substring( 2, 3 );
						temp += "-" + result.Substring( 5 );
						result = temp;
					}
				}
			}

			return result;
		}

		public static string FormatPhoneNumber( this string input )
		{
			return Strings.FormatPhoneNumber( input );
		}

		public static string FormatUSCountry( this string country )
		{
			string result = country;

			if ( string.IsNullOrEmpty( country ) || country.ToLower() == "usa" || country.ToLower() == "united states of america" || country.ToLower() == "us" || country.ToLower() == "united states" )
			{
				result = "United States";
			}

			return result;
		}

		public static string GenerateHash( this DateTime input, int maxLength = 8 )
		{
			long seed = DateTime.Now.Ticks + RNGHelper.GenerateRandomInt( int.MaxValue );
			return Strings.GenerateHashString( seed.ToString(), 8, true ).ToUpper();
		}

		public static string GetContentBetweenHtmlTags( this string body, string startTagName = "<body>", string endTagName = "</body>" )
		{
			return Strings.GetContentBetweenHtmlTags( body, startTagName, endTagName );
		}

		public static string GetEmailDomain( this string input )
		{
			return Strings.GetEmailDomain( input );
		}

		public static string GetUTF8String( this byte[] data )
		{
			string result = string.Empty;
			if ( data != null && data.Length > 0 )
			{
				result = Encoding.UTF8.GetString( data );
			}
			return result;
		}

		public static string GleanNumbers( this string input, bool periodAllowed = false, bool hyphenAllowed = false )
		{
			return Strings.GleanNumbers( input, periodAllowed, hyphenAllowed );
		}

		public static string IfNullOrEmpty( this string input, string text )
		{
			string result = input;
			if ( string.IsNullOrWhiteSpace( input ) )
			{
				result = text;
			}
			return result;
		}

		public static int IndexOfNth( this string input, string str, int n)
		{
            int result = -1;
            if ( !string.IsNullOrWhiteSpace( input ) )
            {
                for ( int i = 0; i < n; i++ )
                {
                    result = input.IndexOf( str, result + 1 );
                    if ( result == -1 )

                        break;
                }
            }
            return result;
		}

		public static bool IsDecimal( this string input )
		{
			bool result = false;

			decimal temp;
			if ( decimal.TryParse( input, out temp ) )
			{
				result = true;
			}

			return result;
		}

		/// <summary>
		/// Whether a given character is allowed by XML 1.0.
		/// </summary>
		public static bool IsLegalXmlChar( int character )
		{
			return
			(
				 character == 0x9 /* == '\t' == 9   */          ||
				 character == 0xA /* == '\n' == 10  */          ||
				 character == 0xD /* == '\r' == 13  */          ||
				( character >= 0x20 && character <= 0xD7FF ) ||
				( character >= 0xE000 && character <= 0xFFFD ) ||
				( character >= 0x10000 && character <= 0x10FFFF )
			);
		}

		public static bool IsNullOrWhiteSpace( this string input )
		{
			return string.IsNullOrWhiteSpace( input );
		}

		public static void IsNullOrWhiteSpace( this string input, Action trueLogic )
		{
			if ( input.IsNullOrWhiteSpace() )
			{
				trueLogic();
			}
		}

		public static bool IsTrue( this string input, string trueValue = "y" )
		{
			bool result = false;
			if ( !string.IsNullOrWhiteSpace( input ) )
			{
				trueValue = ( string.IsNullOrWhiteSpace( trueValue ) ? "Y" : trueValue );
				result = ( string.Compare( input.Trim(), trueValue.Trim(), true ) == 0 );
			}
			return result;
		}

		public static bool IsValidEmail( this string input )
		{
			return Strings.IsValidEmail( input );
		}

		public static bool IsValidXmlString( this string str )
		{
			bool result = true;
			if ( !string.IsNullOrWhiteSpace( str ) )
			{
				foreach ( char c in str )
				{
					if ( !IsValidXmlChar( c ) )
					{
						result = false;
						break;
					}
				}
			}
			return result;
		}

		public static bool IsWellFormedXML( this string value )
		{
			bool isValid = true;
			try
			{
				// Check we actually have a value
				if ( string.IsNullOrEmpty( value ) == false )
				{
					// Try to load the value into a document
					XmlDocument xmlDoc = new XmlDocument();
					xmlDoc.LoadXml( value );
				}
				else
				{
					// A blank value is not valid xml
					isValid = false;
				}
			}
			catch ( System.Xml.XmlException )
			{
				isValid = false;
			}

			return isValid;
		}

		public static string NormalizeName( this string name )
		{
			return Strings.NormalizeName( name );
		}

		public static string NormalizeUrl( this string url )
		{
			return Strings.NormalizeUrl( url );
		}

		public static string Obfuscate( this string input, char character = '*' )
		{
			string result = input;
			if ( !string.IsNullOrWhiteSpace( input ) )
			{
				int atPos = input.IndexOf( '@' );
				if ( atPos > -1 )
				{
					//we think this is an email address, so lets treat it like it is.
					//get the prefix of the email address (everything prior to the @ sign)
					string prefix = input.Substring( 0, atPos );
					string suffix = input.Substring( atPos + 1 );

					int originalLength = prefix.Length;
					string newPrefix = "";
					if ( prefix.Length > 4 )
					{
						for ( int i = 0; i <= prefix.Length - 1; i++ )
						{
							if ( i < 2 || i > ( ( prefix.Length - 1 ) - 2 ) )
							{
								newPrefix += prefix.ElementAt( i ).ToString();
							}
							else
							{
								newPrefix += "*";
							}
						}
					}
					else if ( prefix.Length > 1 && prefix.Length <= 4 )
					{
						for ( int i = 0; i <= prefix.Length - 1; i++ )
						{
							if ( i < 2 )
							{
								newPrefix += prefix.ElementAt( i ).ToString();
							}
							else
							{
								newPrefix += "*";
							}
						}
					}
					else
					{
						for ( int i = 0; i <= prefix.Length - 1; i++ )
						{
							if ( i == 0 )
							{
								newPrefix += prefix.ElementAt( i ).ToString();
							}
							else
							{
								newPrefix += "*";
							}
						}
					}

					result = newPrefix + "@" + suffix;
				}
			}

			return result;
		}

		public static string RemoveBeginning( this string input, string beginning )
		{
			string result = input;
			if ( !string.IsNullOrWhiteSpace( input ) )
			{
				if ( input.StartsWith( beginning ) )
				{
					result = input.Substring( beginning.Trim().Length - 1 );
				}
			}

			return result;
		}

        /// <summary>
        /// this extension method will remove the EXACT beginning substring if found within the original string without trimming/modifying it
        /// </summary>
        /// <param name="input"></param>
        /// <param name="beginning"></param>
        /// <returns></returns>
        public static string RemoveBeginningPro( this string input, string beginning )
        {
            string result = input;
            if ( !string.IsNullOrWhiteSpace( input ) )
            {
                if ( input.StartsWith( beginning ) )
                {
                    result = input.Substring( beginning.Trim().Length );
                }
            }

            return result;
        }

		/// <summary>
		/// Ensures that within <paramref name="str"/> there are no two
		/// consecutive whitespace characters.
		/// </summary>
		/// <param name="input">The STR.</param>
		/// <returns></returns>
		public static string RemoveConsecutiveWhitespace( this string input )
		{
			string result = input;
			if ( !string.IsNullOrWhiteSpace( result ) )
			{
				result = Strings.RemoveConsecutiveWhitespace( result );
				result = input.Trim();
			}
			return result;
		}

		/// <summary>
		/// Ensures that within <paramref name="str"/> there are no two
		/// consecutive whitespace characters.
		/// </summary>
		/// <param name="input">The <see cref="String"/> containing the data to replace consective whitespace.</param>
		/// <param name="replacement">The <see cref="String"/> to replace consecutive whitespace in.</param>
		/// <returns></returns>
		public static string ReplaceConsecutiveWhitespace( this string input, string replacement )
		{
			string result = input;
			if ( !string.IsNullOrWhiteSpace( result ) )
			{
				result = Strings.ReplaceConsecutiveWhitespace( result, replacement );
			}
			return result;
		}

		/// <summary>
		/// Remove illegal XML characters from a string.
		/// </summary>
		public static string SantizeStringForXml( this string xml )
		{
			if ( xml == null )
			{
				throw new ArgumentNullException( "xml" );
			}

			StringBuilder buffer = new StringBuilder( xml.Length );

			foreach ( char c in xml )
			{
				if ( IsLegalXmlChar( c ) )
				{
					buffer.Append( c );
				}
			}

			return buffer.ToString();
		}

        public static string SanitizeStringForExcel(this string input)
        {
            string result = input;
            
            if (!string.IsNullOrWhiteSpace(result))
            {
                result = input.IfNullOrEmpty("").StripNonPrintableCharacters().Trim();

                //check to see if string begins with an "=". If so, need to escape it with a single quote or Excel will think it is a function and it will cause errors during the export to Excel.
                if (result.IndexOf("=") == 0)
                {
                    result = "'" + result;
                }
            }

            return result;
        }

        public static string SentanceCase( this string input )
		{
			return Strings.SentenceCase( input );
		}

		public static string SplitCamelCase( this string input )
		{
			return Regex.Replace(
				Regex.Replace(
					input,
					@"(\P{Ll})(\P{Ll}\p{Ll})",
					"$1 $2"
				),
				@"(\p{Ll})(\P{Ll})",
				"$1 $2"
			);
		}

		public static List<int> SplitToIntegers( this string list, char delimitter = ',' )
		{
			return Strings.SplitToIntegers( list, delimitter );
		}

		public static string[] SplitUpperCase( this string source )
		{
			if ( source == null )
			{
				return new string[] { }; //Return empty array.
			}
			if ( source.Length == 0 )
			{
				return new string[] { "" };
			}

			StringCollection words = new StringCollection();
			int wordStartIndex = 0;

			char[] letters = source.ToCharArray();
			char previousChar = char.MinValue;

			// Skip the first letter. we don't care what case it is.
			for ( int i = 1; i < letters.Length; i++ )
			{
				if ( char.IsUpper( letters[i] ) && !char.IsWhiteSpace( previousChar ) )
				{
					//Grab everything before the current character.
					words.Add( new String( letters, wordStartIndex, i - wordStartIndex ) );
					wordStartIndex = i;
				}
				previousChar = letters[i];
			}

			//We need to have the last word.
			words.Add( new String( letters, wordStartIndex,
			  letters.Length - wordStartIndex ) );

			string[] wordArray = new string[words.Count];
			words.CopyTo( wordArray, 0 );
			return wordArray;
		}

		public static string SplitUpperCaseToString( this string source )
		{
			return string.Join( " ", SplitUpperCase( source ) );
		}

		public static string StripNonAlphaCharacters( this string str )
		{
			return Strings.StripNonAlphaCharacters( str );
		}

		public static string StripNonASCIICharacters( this string input )
		{
			string result = input;
			if ( !string.IsNullOrWhiteSpace( input ) )
			{
				result = Regex.Replace( input, @"[^\u0000-\u007F]", string.Empty );
			}
			return result;
		}

		public static string StripNonNumerics( this string str )
		{
			return Strings.StripNonNumerics( str );
		}

        public static string StripNonPrintableCharacters( this string input )
        {
            string result = input;
            if ( !string.IsNullOrWhiteSpace( input ) )
            {
                result = Regex.Replace( input, @"[^\u0020-\u00FF]", string.Empty );
            }
            return result;
        }

		/// <summary>
		/// Removes a start and end tag block from a string and returns the output.
		/// </summary>
		/// <param name="input">The content to be parsed</param>
		/// <param name="tag">Do not include brackets</param>
		/// <returns></returns>
		public static string StripTagBlockOut( this string input, string tag = "style" )
		{
			string result = Regex.Replace( input, @"<" + tag + "(.|\n)*?</" + tag + ">", string.Empty );
			return result;
		}

		public static string SubStringPro( this string input, int maxNumberOfCharacters, bool removeConsecutiveWhiteSpaces = false, string truncationSuffix = "" )
		{
			return Strings.SubStringPro( input, maxNumberOfCharacters, removeConsecutiveWhiteSpaces, truncationSuffix );
		}

		public static IEnumerable<string> Surround( this IEnumerable<string> strings, string begin, string end )
		{
			return from s in strings
				   select begin + s + end;
		}

        /// <summary>
        /// To Convert Excel Column to integer
        ///  A -> 1
        ///  B -> 2
        ///  Z -> 26
        ///  AA -> 27
        ///  AB -> 28
        /// </summary>
        public static int TextToNumber( this string text )
        {
            return text
                .ToUpper()
                .Select( c => c - 'A' + 1 )
                .Aggregate( ( sum, next ) => sum * 26 + next );
        }

		public static string TitleCase( this string input, bool ignoreShortWords = true )
		{
			return Strings.TitleCase( input, ignoreShortWords );
		}

        public static string ToAcronym( this string input )
        {
            return Strings.GetAcronym( input );
        }

		public static string ToCamelCase( this string instance )
		{
			return instance[0].ToString().ToLower() + instance.Substring( 1 );
		}

		public static Guid? ToGuid( this string guid )
		{
			Guid? result = null;
			Guid tempGuid;
			if ( Guid.TryParse( guid, out tempGuid ) )
			{
				result = tempGuid;
			}

			return result;
		}

		public static int ToInt32( this string input )
		{
			int result = 0;
			if ( !string.IsNullOrWhiteSpace( input ) )
			{
				int.TryParse( input, out result );
			}
			return result;
		}

		public static string ToPascalCase( this string value )
		{
			return NameExpression.Replace( value, " $1" ).Trim();
		}

		public static string ToSHA1Hash( this string input )
		{
			return Strings.GetSHA1Hash( input );
		}

		public static string ToString( this bool value, string trueValue = "Y", string falseValue = "N" )
		{
			return Strings.ConvertBooleanToString( value, trueValue, falseValue );
		}

		public static string ToZipCodeFormat( this string input, bool fiveDigitsOnly = true )
		{
			return Strings.FormatZipCode( input, fiveDigitsOnly );
		}

		/// <summary>
		/// Takes care of NULL strings. Returns empty string in case of NULL input value
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public static string TrimPro( this string input )
		{
			return Strings.TrimPro( input );
		}

		public static string Truncate( this string input, int maxNumberOfCharacters, bool smart = true )
		{
			return Strings.Truncate( input, maxNumberOfCharacters, smart );
		}

		public static string Truncate( this string input, int maxNumberOfCharacters, params string[] noTruncateExpressions )
		{
			return Strings.Truncate( input, maxNumberOfCharacters, noTruncateExpressions );
		}

		public static void WriteToFile( this string content, string fileName, bool append = false )
		{
			if ( string.IsNullOrWhiteSpace( fileName ) )
			{
				throw new ArgumentNullException( "fileName" );
			}

			if ( !string.IsNullOrWhiteSpace( content ) )
			{
				IOHelper.WriteFile( fileName, content, append );
			}
		}

		/// <summary>
		/// Whether a given character is allowed by XML 1.0.
		/// </summary>
		private static bool IsValidXmlChar( int character )
		{
			return
			(
				 character == 0x9 /* == '\t' == 9   */          ||
				 character == 0xA /* == '\n' == 10  */          ||
				 character == 0xD /* == '\r' == 13  */          ||
				( character >= 0x20 && character <= 0xD7FF ) ||
				( character >= 0xE000 && character <= 0xFFFD ) ||
				( character >= 0x10000 && character <= 0x10FFFF )
			);
		}
	}
}