using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UPF
{
	public enum ActivityLogCaptureLevel
	{
		Verbose,
		Medium,
		ErrorOnly
	}

	public enum ActivityLogEventLevel
	{
		Error = 3,
		Information = 1,
		Warning = 4,
		Detail = 2
	}

	public enum BuiltInLicenseKey
	{
		WinnovativeSuite,
		MelissaData
	}

	public enum ICAPAction
	{
		ALLOW = 0,
		INFO = 100,
		BLOCK = 400,
		ERROR = 500
	}

	public enum ICAPBalanceMode
	{
		Random,
		Sequential
	}

	public enum ICAPServerResultConnectionStatus
	{
		Successful,
		HostError,
		IOError,
		SocketError
	}

	/// <summary>
	/// Values for specifying how to send emails to individuals.
	/// </summary>
	public enum MailRecipientType
	{
		/// <summary>
		/// Place address on the to address line.
		/// </summary>
		To,

		/// <summary>
		/// Place address as the from address line.
		/// </summary>
		From,

		/// <summary>
		/// Place address on the carbon copy (CC) line.
		/// </summary>
		CC,

		/// <summary>
		/// Place address on the B carbon copy (BCC) line.
		/// </summary>
		BCC
	}

	/// <summary>
	/// Runtime environment types.
	/// </summary>
	public enum RuntimeEnvironment
	{
		/// <summary>
		/// Specifies the current runtime environment is the production location.
		/// </summary>
		Production = 5,

		/// <summary>
		/// Specifies the current runtime environment is the development location, usually localhost.
		/// </summary>
		Development = 1,

		/// <summary>
		/// Specifies the current runtime environment is the shared development or testing location.
		/// Clients should never see an app running in this environment.
		/// </summary>
		Testing = 2,

		/// <summary>
		/// Specifies the current runtime environment is the staging server, typically where clients
		/// review products produced by development team. These types of builds are usually of release
		/// quality.
		/// </summary>
		Staging = 3,

		/// <summary>
		/// Specifies the current runtime environment is the training server.
		/// </summary>
		Training = 4,

		/// <summary>
		/// Specifies the current runtime environment is the documentation server.
		/// </summary>
		Documentation = 6
	}
}