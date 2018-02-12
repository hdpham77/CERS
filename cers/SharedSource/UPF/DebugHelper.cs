using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using UPF.Configuration;

namespace UPF
{
	public static class DebugHelper
	{
		#region FormatException Method

		public static void AddExceptionTypeName( this StringBuilder body, Exception exception, bool htmlFormat = true, bool addColonSuffix = true )
		{
			if ( htmlFormat )
			{
				body.Append( "<span style=\"color:Blue;font-size:.85em;\">" );
			}

			body.Append( exception.GetType().FullName );

			if ( addColonSuffix )
			{
				body.Append( ": " );
			}

			if ( htmlFormat )
			{
				body.Append( "</span>" );
			}
		}

		public static void FlattenExceptionHeiarchy( List<Exception> exceptions, Exception exception )
		{
			exceptions.Add( exception );

			if ( exception.InnerException != null )
			{
				FlattenExceptionHeiarchy( exceptions, exception.InnerException );
			}
		}

		/// <summary>
  /// Formats a <see cref="Exception"/> to a string, and includes all inner exceptions.
  /// </summary>
  /// <param name="exception">The <see cref="Exception"/> to format to a see
  /// cref="String"/>.</param>
  /// <param name="newLineExpression">A <see cref="Boolean"/> value used</param>
  /// <returns></returns>
		public static string FormatException( Exception exception, bool htmlFormat = true, bool lastFirst = true )
		{
			string newLineExpression = htmlFormat ? "<br/>" : "/r/n";
			StringBuilder result = new StringBuilder();

			if ( exception != null )
			{
				if ( lastFirst )
				{
					if ( htmlFormat )
					{
						result.Append( "<b>Note:</b> The exceptions are reverse ordered so that the exception that is generally the error you need to focus on is listed first." ).Append( newLineExpression );
						result.Append( newLineExpression );
					}

					List<Exception> exceptions = new List<Exception>();
					FlattenExceptionHeiarchy( exceptions, exception );
					exceptions.Reverse();

					Exception exceptionItem = null;
					for ( int eIndex = 0; eIndex <= exceptions.Count - 1; eIndex++ )
					{
						exceptionItem = exceptions[eIndex];
						if ( htmlFormat )
						{
							result.Append( "<span style=\"color:#FF0000;font-weight:bold;\">" );
						}

						result.Append( exceptionItem.Message );
						result.Append( newLineExpression );
						result.AddExceptionTypeName( exceptionItem, htmlFormat, false );

						if ( htmlFormat )
						{
							result.Append( "</span>" );
						}

						result.Append( newLineExpression );
						result.Append( exceptionItem.FormatStackTrace( htmlFormat ) );

						if ( ( eIndex < ( exceptions.Count - 1 ) ) && !htmlFormat )
						{
							result.Append( newLineExpression );
							result.Append( "-------------------------------------------" ).Append( newLineExpression );
						}
					}
				}
				else
				{
					if ( htmlFormat )
					{
						result.Append( "<span style=\"color:#FF0000;font-weight:bold;\">" );
					}

					result.Append( exception.Message );
					result.Append( newLineExpression );
					result.AddExceptionTypeName( exception, htmlFormat, false );

					if ( htmlFormat )
					{
						result.Append( "</span>" );
					}

					result.Append( newLineExpression );
					result.Append( exception.FormatStackTrace( htmlFormat ) );
					if ( exception.InnerException != null )
					{
						if ( !htmlFormat )
						{
							result.Append( newLineExpression );
							result.Append( "-------------------------------------------" ).Append( newLineExpression );
						}
						result.Append( FormatException( exception.InnerException, htmlFormat ) );
					}
				}
			}
			return result.ToString();
		}

		public static string FormatStackTrace( this Exception exception, bool htmlFormat = true )
		{
			string newLineExpression = htmlFormat ? "<br/>" : "/r/n";
			StringBuilder result = new StringBuilder();
			StackTrace trace = new StackTrace( exception, true );
			StackFrame frame = null;

			string boldTextExpression = htmlFormat ? "<span style=\"font-weight:bold;\">" : "";
			string smallTextExpression = htmlFormat ? "<span style=\"font-size:.85em;\">" : "";

			string openSpanExpression = htmlFormat ? "<span>" : "";
			string closeSpanExpression = htmlFormat ? "</span>" : "";

			if ( htmlFormat )
			{
				result.Append( "<table>" );
			}

			for ( int fIndex = 0; fIndex < trace.FrameCount; fIndex++ )
			{
				frame = trace.GetFrame( fIndex );

				if ( htmlFormat )
				{
					result.Append( "<tr>" );
					result.Append( "<td style=\"vertical-align:top;\">" );
				}

				result.Append( fIndex + 1 ).Append( "." );

				if ( htmlFormat )
				{
					result.Append( "</td>" );
					result.Append( "<td style=\"vertical-align:top;\">" );
				}

				result.Append( openSpanExpression ).Append( frame.GetMethod() ).Append( closeSpanExpression );
				string fileName = frame.GetFileName();
				int lineNumber = frame.GetFileLineNumber();
				if ( !string.IsNullOrWhiteSpace( fileName ) )
				{
					result.Append( newLineExpression );
					result.Append( smallTextExpression );
					result.Append( "File: " ).Append( fileName );
					if ( lineNumber > 0 )
					{
						//result.Append(newLineExpression);
						result.Append( boldTextExpression ).Append( " Line: " ).Append( closeSpanExpression ).Append( lineNumber );
					}
					result.Append( closeSpanExpression );
				}

				if ( htmlFormat )
				{
					result.Append( "</td></tr>" );
				}
			}

			if ( htmlFormat )
			{
				result.Append( "</table>" );
			}

			return result.ToString();
		}

		#endregion FormatException Method

		public static void CheckNull<T>( this T item, string parameterName ) where T : class
		{
			if ( item == null )
			{
				throw new ArgumentNullException( parameterName );
			}
		}

		public static void CheckNull( this string item, string parameterName )
		{
			if ( string.IsNullOrWhiteSpace( item ) )
			{
				throw new ArgumentNullException( parameterName );
			}
		}

		public static bool Contains( this Exception exception, string message )
		{
			bool result = false;
			if ( exception != null && !string.IsNullOrWhiteSpace( message ) )
			{
				string exceptionMessage = exception.Message.ToLower().Trim();
				if ( exceptionMessage.Contains( message.ToLower().Trim() ) )
				{
					result = true;
				}

				if ( !result && ( exception.InnerException != null ) )
				{
					//check the inner exception.
					result = exception.InnerException.Contains( message );
				}
			}

			return result;
		}

		public static string Format( this Exception exception, bool htmlFormat = true )
		{
			return FormatException( exception, htmlFormat );
		}

		public static void LogActivity( ActivityLogEventLevel level, string message, Exception exception = null, string typeName = null, string sourceName = null )
		{
			bool enabled = false;
			ActivityLogCaptureLevel captureLevel = ActivityLogCaptureLevel.ErrorOnly;
			var config = UPFConfigurationSection.Current;
			if ( config != null )
			{
				enabled = config.Diagnostics.EventLogEnabled;
				captureLevel = config.Diagnostics.CaptureLevel;
			}

			if ( !string.IsNullOrWhiteSpace( typeName ) )
			{
				message = "Type: " + typeName + " >> " + message;
			}

			if ( enabled )
			{
				if ( captureLevel == ActivityLogCaptureLevel.ErrorOnly )
				{
					if ( level != ActivityLogEventLevel.Error )
					{
						return;
					}
				}
				else if ( captureLevel == ActivityLogCaptureLevel.Medium )
				{
					if ( level == ActivityLogEventLevel.Detail )
					{
						return;
					}
				}

				if ( string.IsNullOrWhiteSpace( sourceName ) )
				{
					sourceName = "Not Specified";
				}

				string logName = "CERS System";
				string eventDetails = message;
				try
				{
					EventLogEntryType entryType = EventLogEntryType.Information;
					if ( exception != null )
					{
						message += "\r\n\r\nException Details:\r\n" + exception.Format( false );
						entryType = EventLogEntryType.Error;
					}

					if ( !EventLog.SourceExists( sourceName ) )
					{
						EventLog.CreateEventSource( sourceName, logName );
					}

					EventLog.WriteEntry( sourceName, eventDetails, entryType );
				}
				catch
				{
					Debug.Write( "Unable to write to event log >> " );
				}
			}
			Console.WriteLine( DateTime.Now.ToString() + " - " + message );
		}

		public static void WriteLine( string message )
		{
			DateTime now = DateTime.Now;
			Debug.WriteLine( now.ToShortDateString() + " " + now.ToLongTimeString() + "." + now.Millisecond + ": " + message );
		}
	}
}