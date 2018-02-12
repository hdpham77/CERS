using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace UPF.Windows
{
	public class WindowBase : Window
	{
		#region Fields

		public const string AllFilesFileFilter = "All Files|*.*";
		private Brush _DefaultBackground = Brushes.White;
		private BackgroundWorker _DefaultWorker;
		private int _PercentComplete;
		private int _WorkerPercentCompleted;

		#endregion Fields

		#region Properties

		public string AssemblyDescription
		{
			get
			{
				string result = string.Empty;

				AssemblyDescriptionAttribute attribute = GetAssemblyAttribute<AssemblyDescriptionAttribute>( this.GetType() );
				if ( attribute != null )
				{
					result = attribute.Description;
				}

				return result;
			}
		}

		

		public string AssemblyProductName
		{
			get
			{
				string result = string.Empty;

				AssemblyProductAttribute attribute = GetAssemblyAttribute<AssemblyProductAttribute>( this.GetType() );
				if ( attribute != null )
				{
					result = attribute.Product;
				}

				return result;
			}
		}

		public string AssemblyTitle
		{
			get
			{
				string result = string.Empty;

				AssemblyTitleAttribute attribute = GetAssemblyAttribute<AssemblyTitleAttribute>( this.GetType() );
				if ( attribute != null )
				{
					result = attribute.Title;
				}

				return result;
			}
		}

		public string AssemblyTitleAndVersion
		{
			get { return AssemblyTitle + " v" + AssemblyVersion; }
		}

		public string AssemblyVersion
		{
			get
			{
				string result = string.Empty;
				AssemblyVersionAttribute attribute = GetAssemblyAttribute<AssemblyVersionAttribute>( this.GetType() );
				if ( attribute != null )
				{
					result = attribute.Version;
				}
				return result;
			}
		}

		public Brush DefaultBackground
		{
			get { return _DefaultBackground; }
			set { _DefaultBackground = value; }
		}

		public BackgroundWorker DefaultWorker
		{
			get { return _DefaultWorker; }
			set { _DefaultWorker = value; }
		}

		public bool IsGlassEnabled { get { return GlassHelper.CompositionEnabled; } }

		#endregion Properties

		#region Worker Helper Methods

		#region CalculateAndReportWorkerProgress Methods

		public void CalculateAndReportWorkerProgress( BackgroundWorker worker, int currentIndex, int totalCount, ref int percentComplete, ref int workerPercentCompleted )
		{
			if ( worker != null )
			{
				percentComplete = (int) ( (float) currentIndex / (float) totalCount * 100 );
				if ( percentComplete > workerPercentCompleted )
				{
					workerPercentCompleted = percentComplete;
					worker.ReportProgress( percentComplete );
				}
			}
		}

		public virtual void CalculateAndReportWorkerProgress( BackgroundWorker worker, int currentIndex, int totalCount )
		{
			if ( worker != null )
			{
				CalculateAndReportWorkerProgress( worker, currentIndex, totalCount, ref _PercentComplete, ref _WorkerPercentCompleted );
			}
		}

		public virtual void CalculateAndReportWorkerProgress( int currentIndex, int totalCount )
		{
			if ( DefaultWorker != null )
			{
				CalculateAndReportWorkerProgress( DefaultWorker, currentIndex, totalCount, ref _PercentComplete, ref _WorkerPercentCompleted );
			}
		}

		#endregion CalculateAndReportWorkerProgress Methods

		public void InitBackgroundWorker( ref BackgroundWorker worker, bool reportsProgress, DoWorkEventHandler doWork, ProgressChangedEventHandler progressChanged, RunWorkerCompletedEventHandler runWorkerCompleted )
		{
			worker = new BackgroundWorker();
			worker.WorkerReportsProgress = reportsProgress;
			worker.DoWork += doWork;
			if ( progressChanged != null )
			{
				worker.ProgressChanged += progressChanged;
			}
			worker.RunWorkerCompleted += runWorkerCompleted;
		}

		protected virtual void DoWork( DoWorkEventArgs e )
		{
		}

		protected virtual void InitDefaultWorker( bool reportsProgress )
		{
			InitBackgroundWorker( ref _DefaultWorker, reportsProgress, DefaultWorker_DoWork, DefaultWorker_ProgressChanged, DefaultWorker_RunWorkerCompleted );
		}

		protected virtual void ProgressChanged( ProgressChangedEventArgs e )
		{
		}

		protected virtual void RunInBackground<TArgs>( TArgs args )
		{
			if ( DefaultWorker != null )
			{
				DefaultWorker.RunWorkerAsync( args );
			}
		}

		protected virtual void RunWorkerCompleted( RunWorkerCompletedEventArgs e )
		{
		}

		private void DefaultWorker_DoWork( object sender, DoWorkEventArgs e )
		{
			DoWork( e );
		}

		private void DefaultWorker_ProgressChanged( object sender, ProgressChangedEventArgs e )
		{
			ProgressChanged( e );
		}

		private void DefaultWorker_RunWorkerCompleted( object sender, RunWorkerCompletedEventArgs e )
		{
			RunWorkerCompleted( e );
		}

		#endregion Worker Helper Methods

		#region Validation Helper Methods

		public virtual void SetControlValidationState( Control control, string tip, bool isValid )
		{
			if ( isValid )
			{
				control.BorderBrush = (Brush) TryFindResource( "DefaultBorderBrush" );
				tip = null;
			}
			else
			{
				control.BorderBrush = (Brush) TryFindResource( "FaultyBorderBrush" );
			}

			control.ToolTip = tip;
		}

		#endregion Validation Helper Methods

		#region ExtendGlassFrame Methods

		protected virtual void ExtendGlassFrame()
		{
			ExtendGlassFrame( new Thickness( -1 ) );
		}

		protected virtual void ExtendGlassFrame( Brush nonGlassBackground )
		{
			ExtendGlassFrame( new Thickness( -1 ), nonGlassBackground );
		}

		protected virtual void ExtendGlassFrame( Thickness thickness )
		{
			ExtendGlassFrame( thickness, DefaultBackground );
		}

		protected virtual void ExtendGlassFrame( Thickness thickness, Brush nonGlassBackground )
		{
			GlassHelper.ExtendGlassFrame( this, thickness, nonGlassBackground );
		}

		#endregion ExtendGlassFrame Methods

		#region BindComboBox Methods

		public virtual void BindComboBox<T>( ComboBox comboBox, IEnumerable<T> source, string displayMemberName, string selectedValuePathName )
		{
			comboBox.ItemsSource = source;
			comboBox.DisplayMemberPath = displayMemberName;
			comboBox.SelectedValuePath = selectedValuePathName;
		}

		#endregion BindComboBox Methods

		#region ValidationHelper Methods

		public virtual bool ValidateComboBox( ComboBox comboBox, string fieldName )
		{
			return ValidateComboBox( comboBox, fieldName, null );
		}

		public virtual bool ValidateComboBox( ComboBox comboBox, string fieldName, string tip )
		{
			return comboBox.IsValid( fieldName, tip, SetControlValidationState );
		}

		public virtual bool ValidateControlTag( Control control, string fieldName )
		{
			return control.IsValidTag( fieldName, null, SetControlValidationState );
		}

		public virtual bool ValidateControlTag( Control control, string fieldName, string tip )
		{
			return control.IsValidTag( fieldName, tip, SetControlValidationState );
		}

		public virtual bool ValidateNumericShortTextBox( TextBox textBox, string fieldName )
		{
			return ValidateNumericTextBox( textBox, fieldName, short.MinValue, short.MaxValue );
		}

		public virtual bool ValidateNumericTextBox( TextBox textBox, string fieldName, long minValue, long maxValue )
		{
			return textBox.IsNumeric( fieldName, fieldName + " is required and must be between " + minValue + " and " + maxValue, minValue, maxValue, SetControlValidationState );
		}

		public virtual bool ValidateTextBox( TextBox textBox, string fieldName )
		{
			return ValidateTextBox( textBox, fieldName, null );
		}

		public virtual bool ValidateTextBox( TextBox textBox, string fieldName, string tip )
		{
			return ValidateTextBox( textBox, fieldName, tip, 1 );
		}

		public virtual bool ValidateTextBox( TextBox textBox, string fieldName, int minLength )
		{
			return ValidateTextBox( textBox, fieldName, null, minLength );
		}

		public virtual bool ValidateTextBox( TextBox textBox, string fieldName, string tip, int minLength )
		{
			return ValidateTextBox( textBox, fieldName, tip, minLength, int.MaxValue );
		}

		public virtual bool ValidateTextBox( TextBox textBox, string fieldName, int minLength, int maxLength )
		{
			return ValidateTextBox( textBox, fieldName, null, minLength, maxLength );
		}

		public virtual bool ValidateTextBox( TextBox textBox, string fieldName, string tip, int minLength, int maxLength )
		{
			return textBox.IsValid( fieldName, tip, SetControlValidationState, minLength, maxLength );
		}

		#endregion ValidationHelper Methods

		#region DisplayErrorDialog Methods

		protected virtual void DisplayErrorDialog( Exception ex )
		{
			DisplayErrorDialog( null, null, ex, null );
		}

		protected virtual void DisplayErrorDialog( string message, Exception ex )
		{
			DisplayErrorDialog( null, message, ex, null );
		}

		protected virtual void DisplayErrorDialog( Exception ex, string dialogTitle )
		{
			DisplayErrorDialog( null, null, ex, dialogTitle );
		}

		protected virtual void DisplayErrorDialog( string message, Exception ex, string dialogTitle )
		{
			DisplayErrorDialog( null, message, ex, dialogTitle );
		}

		protected virtual void DisplayErrorDialog( Window owner, string message, Exception ex, string dialogTitle )
		{
			ErrorWindow window = new ErrorWindow( message, ex, dialogTitle );
			if ( owner != null )
			{
				window.Owner = owner;
			}
			else
			{
				window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
			}
			window.ShowDialog();
		}

		#endregion DisplayErrorDialog Methods

		#region SendExceptionNotification Method

		public virtual void SendExceptionNotification( string message, Exception ex )
		{
		}

		#endregion SendExceptionNotification Method

		#region GetAssemblyAttribute Method

		public AT GetAssemblyAttribute<AT>( Type theTypeInAssembly ) where AT : Attribute
		{
			AT result = null;
			result = Attribute.GetCustomAttribute( theTypeInAssembly.Assembly, typeof( AT ) ) as AT;
			return result;
		}

		#endregion GetAssemblyAttribute Method

		#region SetSelectedItem Method

		public virtual void SetSelectedItem( ListView listView )
		{
			listView.SelectedItem = listView.Tag;
			listView.ScrollIntoView( listView.Tag );
		}

		#endregion SetSelectedItem Method

		#region AddObjectToList Method

		protected virtual void AddObjectToList<T>( T obj )
		{
		}

		#endregion AddObjectToList Method

		#region AddObjectToListFromThread

		protected virtual void AddObjectToListFromThread<T>( T obj )
		{
			AddObjectToListMethod<T> method = AddObjectToList<T>;
			Dispatcher.Invoke( method, obj );
		}

		#endregion AddObjectToListFromThread

		#region InvokeOnUIThread Method

		protected virtual void InvokeOnUIThread( Action method )
		{
			Dispatcher.Invoke( method );
		}

		#endregion InvokeOnUIThread Method

		#region NotifyMessage

		protected virtual void NotifyMessage( string message )
		{
		}

		protected virtual void NotifyMessage<T>( string message, T obj )
		{
		}

		#endregion NotifyMessage

		#region NotifyMessageFromThread Method

		protected virtual void NotifyMessageFromThread( string message )
		{
			NotifyMessageMethod method = NotifyMessage;
			Dispatcher.Invoke( method, message );
		}

		protected virtual void NotifyMessageFromThread<T>( string message, T obj )
		{
			NotifyMessageMethod<T> method = NotifyMessage<T>;
			Dispatcher.Invoke( method, message, obj );
		}

		#endregion NotifyMessageFromThread Method

		#region GetParentWindow Method

		public static Window GetParentWindow( DependencyObject child )
		{
			DependencyObject parentObject = VisualTreeHelper.GetParent( child );

			if ( parentObject == null )
			{
				return null;
			}

			Window parent = parentObject as Window;
			if ( parent != null )
			{
				return parent;
			}
			else
			{
				return GetParentWindow( parentObject );
			}
		}

		#endregion GetParentWindow Method

		#region SaveFile Method(s)

		public static void SaveFile( Window owner, string data, string title = "Save File", string filter = AllFilesFileFilter, bool promptAutoOpen = true )
		{
			SaveFileDialog sfd = new SaveFileDialog();
			sfd.Title = title;
			if ( !filter.Contains( AllFilesFileFilter ) )
			{
				sfd.Filter = filter + "|" + AllFilesFileFilter;
			}
			if ( sfd.ShowDialog() == true )
			{
				try
				{
					data.WriteToFile( sfd.FileName );
					if ( promptAutoOpen )
					{
						if ( MessageBox.Show( owner, "Would you like to view the file downloaded?", title, MessageBoxButton.YesNo, MessageBoxImage.Question ) == MessageBoxResult.Yes )
						{
							Process.Start( new ProcessStartInfo( sfd.FileName ) );
						}
					}
				}
				catch ( Exception ex )
				{
					MessageBox.Show( owner, ex.Message, title, MessageBoxButton.OK, MessageBoxImage.Error );
				}
			}
		}

		public static void SaveFile( Window owner, byte[] data, string title = "Save File", string filter = AllFilesFileFilter, bool promptAutoOpen = true )
		{
			SaveFileDialog sfd = new SaveFileDialog();
			sfd.Title = title;
			if ( !filter.Contains( AllFilesFileFilter ) )
			{
				sfd.Filter = filter + "|" + AllFilesFileFilter;
			}
			if ( sfd.ShowDialog() == true )
			{
				try
				{
					data.WriteToFile( sfd.FileName );

					if ( promptAutoOpen )
					{
						if ( MessageBox.Show( owner, "Would you like to view the file downloaded?", title, MessageBoxButton.YesNo, MessageBoxImage.Question ) == MessageBoxResult.Yes )
						{
							Process.Start( new ProcessStartInfo( sfd.FileName ) );
						}
					}
				}
				catch ( Exception ex )
				{
					MessageBox.Show( owner, ex.Message, title, MessageBoxButton.OK, MessageBoxImage.Error );
				}
			}
		}

		#endregion SaveFile Method(s)

		#region OpenFile Method

		public static string OpenFile( Window owner, string title = "Open File", string filter = AllFilesFileFilter )
		{
			string result = null;
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.Title = title;
			if ( !filter.Contains( AllFilesFileFilter ) )
			{
				ofd.Filter = filter + "|" + AllFilesFileFilter;
			}
			if ( ofd.ShowDialog( owner ) == true )
			{
				result = ofd.FileName;
			}
			return result;
		}

		#endregion OpenFile Method

		#region UpdateControlUsability Method

		protected virtual void UpdateControlUsability( bool enabled, params Control[] controls )
		{
			if ( controls != null )
			{
				foreach ( var control in controls )
				{
					control.IsEnabled = enabled;
				}
			}
		}

		#endregion UpdateControlUsability Method
	}
}