using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace UPF.Core
{
	public static class Validations
	{
		public static bool ValidatePassword( this string password, out List<string> problems )
		{
			if ( string.IsNullOrWhiteSpace( password ) )
			{
				problems = new List<string>();
				problems.Add( "Password is null or empty" );
				return false;
			}

			bool result = true;

			problems = new List<string>();
			if ( password.Length < 8 )
			{
				problems.Add( "Must be at least eight characters." );
				result = false;
			}

			Regex regex = new Regex( @"(?=.*\d)" );
			if ( regex.IsMatch( password ) == false )
			{
				problems.Add( "Must have at least one number." );
				result = false;
			}

			regex = new Regex( "(?=.*[A-Z])" );
			if ( regex.IsMatch( password ) == false )
			{
				problems.Add( "Must contain at least one upper case letter." );
				result = false;
			}

			return result;
		}

		public static PasswordValidationResult ValidatePasswordInput( this string password, string confirmPassword )
		{
			PasswordValidationResult result = PasswordValidationResult.NotProvided;
			if ( !string.IsNullOrWhiteSpace( password ) )
			{
				if ( string.IsNullOrWhiteSpace( confirmPassword ) )
				{
					result = PasswordValidationResult.ConfirmNotProvided;
				}
			}

			if ( !string.IsNullOrWhiteSpace( password ) && !string.IsNullOrWhiteSpace( confirmPassword ) )
			{
				if ( string.Compare( password, confirmPassword ) != 0 )
				{
					result = PasswordValidationResult.NoMatch;
				}
				else
				{
					result = PasswordValidationResult.Match;
				}
			}

			return result;
		}

		public static UsernameFormatValidationResult VerifyUsernameFormat( this string username, string originalUsername = null )
		{
			UsernameFormatValidationResult result = new UsernameFormatValidationResult();

			const int minLength = 5;
			const int maxLength = 16;

			if ( string.IsNullOrWhiteSpace( username ) )
			{
				result.AddError( "Username is required" );
				return result;
			}

			username = username.Trim();

			//only username is passed in.
			//look for spaces first.
			if ( username.IndexOf( " " ) > -1 )
			{
				result.AddError( "Username cannot contain spaces" );
			}

			//check for special characters.
			string characters = @"~`!@#$%^&*()_-+=><.?/[]}{|\'"";:";
			string specialCharactersFound = new string( ( from s in characters where username.ToArray().ToList().Contains( s ) select s ).ToArray() );

			if ( !string.IsNullOrWhiteSpace( specialCharactersFound ) )
			{
				result.AddError( "Username cannot contain special characters (" + specialCharactersFound + ")" );
			}

			if ( username.Length < minLength )
			{
				result.AddError( "Username must be at least " + minLength + " characters" );
			}

			if ( username.Length > maxLength )
			{
				result.AddError( "Username cannot be more than " + maxLength + " characters" );
			}

			return result;
		}
	}
}