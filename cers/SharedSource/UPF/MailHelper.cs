using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;

namespace UPF
{
	/// <summary>
	/// Encapsulates standard email functionality.
	/// </summary>
	public static class MailHelper
	{

		/// <summary>
		/// Send's an email.
		/// </summary>
		/// <param name="from">The email address to use as the from address.</param>
		/// <param name="to">The address(s) to send the email to.</param>
		/// <param name="subject">The message to place on the subject line of the email.</param>
		/// <param name="body">The body of the email.</param>
		public static void Send(string from, string to, string subject, string body)
		{
			Send(from, to, subject, body, MailPriority.Normal, false);
		}

		/// <summary>
		/// Send's an email.
		/// </summary>
		/// <param name="from">The email address to use as the from address.</param>
		/// <param name="to">The address(s) to send the email to.</param>
		/// <param name="subject">The message to place on the subject line of the email.</param>
		/// <param name="body">The body of the email.</param>
		/// <param name="priority">The value specifying the importance of this message.</param>
		/// <param name="isBodyHtml">Specifies whether the body of this email contains HTML markup and should be displayed in end-users clients as HTML emails.</param>
		public static void Send(string from, string to, string subject, string body, MailPriority priority, bool isBodyHtml)
		{
			Send(from, to, string.Empty, string.Empty, subject, body, priority, isBodyHtml);
		}

		/// <summary>
		/// Send's an email.
		/// </summary>
		/// <param name="from">The email address to use as the from address.</param>
		/// <param name="to">The address(s) to send the email to.</param>
		/// <param name="cc">The address(s) to place on the cc field of the email.</param>
		/// <param name="bcc">The address(s) to place on the bcc field of the email.</param>
		/// <param name="subject">The message to place on the subject line of the email.</param>
		/// <param name="body">The body of the email.</param>
		/// <param name="priority">The value specifying the importance of this message.</param>
		/// <param name="isBodyHtml">Specifies whether the body of this email contains HTML markup and should be displayed in end-users clients as HTML emails.</param>
		public static void Send(string from, string to, string cc, string bcc, string subject, string body, MailPriority priority, bool isBodyHtml)
		{

			MailMessage msg = new MailMessage();

			AttachAddressesFromString(msg, MailRecipientType.From, from);
			AttachAddressesFromString(msg, MailRecipientType.To, to);
			AttachAddressesFromString(msg, MailRecipientType.CC, cc);
			AttachAddressesFromString(msg, MailRecipientType.BCC, bcc);

			msg.Subject = subject;
			msg.Body = body;
			msg.IsBodyHtml = isBodyHtml;

			SmtpClient client = new SmtpClient();
			client.Send(msg);

		}

		/// <summary>
		/// Parses out an email address from a string.
		/// </summary>
		/// <remarks>
		/// 
		/// </remarks>
		/// <param name="rawAddress"></param>
		/// <returns></returns>
		private static MailAddress ParseMailAddress(string rawAddress)
		{
			if (!Strings.IsNullOrEmpty(rawAddress))
			{
				rawAddress = rawAddress.Trim();
				if (rawAddress.IndexOf("|") > -1)
				{
					string[] addressData = rawAddress.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
					if (IsValidEmail(addressData[0]))
					{
						return new MailAddress(addressData[0], addressData[1]);
					}
					else
					{
						throw new ArgumentException("Invalid Email Address - " + addressData[0]);
					}
				}
				else
				{
					if (IsValidEmail(rawAddress))
					{
						return new MailAddress(rawAddress);
					}
					else
					{
						throw new ArgumentException("Invalid Email Address - " + rawAddress);
					}
				}
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		/// <param name="recipientType"></param>
		/// <param name="rawAddresses"></param>
		private static void AttachAddressesFromString(MailMessage message, MailRecipientType recipientType, string rawAddresses)
		{
			if (!string.IsNullOrEmpty(rawAddresses))
			{
				if (rawAddresses.IndexOf(";") > -1)
				{
					string[] addressesData = rawAddresses.Split(';');
					foreach (string addressData in addressesData)
					{
						MailAddress address = ParseMailAddress(addressData);
						if (address != null)
						{
							if (recipientType == MailRecipientType.To)
							{
								message.To.Add(address);
							}
							else if (recipientType == MailRecipientType.From)
							{
								message.From = address;
							}
							else if (recipientType == MailRecipientType.CC)
							{
								message.CC.Add(address);
							}
							else if (recipientType == MailRecipientType.BCC)
							{
								message.Bcc.Add(address);
							}
						}
					}
				}
				else
				{
					if (recipientType == MailRecipientType.To)
					{
						message.To.Add(ParseMailAddress(rawAddresses));
					}
					else if (recipientType == MailRecipientType.From)
					{
						message.From = ParseMailAddress(rawAddresses);
					}
					else if (recipientType == MailRecipientType.CC)
					{
						message.CC.Add(ParseMailAddress(rawAddresses));
					}
					else if (recipientType == MailRecipientType.BCC)
					{
						message.Bcc.Add(ParseMailAddress(rawAddresses));
					}
				}
			}
		}

		public static bool IsValidEmail(string emailAddressToValidate)
		{
			return Strings.IsValidEmail(emailAddressToValidate);
		}


	}
}
