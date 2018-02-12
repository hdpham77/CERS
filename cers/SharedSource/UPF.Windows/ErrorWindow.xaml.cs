using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

//using UPF.Core;

namespace UPF.Windows
{
	/// <summary>
	/// Interaction logic for ErrorWindow.xaml
	/// </summary>
	public partial class ErrorWindow : WindowBase
	{
		public ErrorWindow( Exception exception )
			: this( null, exception, null )
		{
		}

		public ErrorWindow( string message, Exception exception )
			: this( message, exception, null )
		{
		}

		public ErrorWindow( string message, Exception exception, string dialogTitle )
		{
			Exception = exception;
			Message = message;
			DialogTitle = dialogTitle;
			InitializeComponent();
		}

		public string DialogTitle { get; protected set; }

		public Exception Exception { get; protected set; }

		public string Message { get; protected set; }

		private void btnOK_Click( object sender, RoutedEventArgs e )
		{
			DialogResult = true;
		}

		private void btnSendReport_Click( object sender, RoutedEventArgs e )
		{
			SendExceptionNotification( Message, Exception );
			DialogResult = true;
		}

		private void WindowBase_Loaded( object sender, RoutedEventArgs e )
		{
			string body = "An error occurred. This could be due to a configuration error.";
			if ( !string.IsNullOrWhiteSpace( Message ) )
			{
				body = Message;
			}

			body += "\r\n\r\nPlease contact the application administrator for assistance.";
			tbGeneralInfo.Text = body;

			if ( Exception != null )
			{
				tbDetailedErrorInfo.Text = "Error Information:\r\n" + this.Exception.Format( false );
			}

			if ( string.IsNullOrWhiteSpace( DialogTitle ) )
			{
				DialogTitle = "Error Notification";
			}

			this.Title = DialogTitle;
		}
	}
}