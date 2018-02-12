using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using CERS.Security;

namespace CERS
{
	public class ErrorReportExtended : ErrorReport, IErrorReportExtended
	{
		#region Fields

		private PermissionRoleMatrixCollection _Permissions;
		private NameValueCollection _FormVariables;
		private NameValueCollection _QueryStringVariables;
		private NameValueCollection _ServerVariables;

		#endregion Fields

		#region Account Properties

		public string AccountFullName { get; set; }

		public string AccountEmail { get; set; }

		public string AccountUserName { get; set; }

		public PermissionRoleMatrixCollection Permissions
		{
			get
			{
				if (_Permissions == null)
				{
					_Permissions = new PermissionRoleMatrixCollection();
				}
				return _Permissions;
			}
			set
			{
				_Permissions = value;
			}
		}

		#endregion Account Properties

		#region Core Properties

		public string EnvironmentName { get; set; }

		public string SystemPortalName { get; set; }

		#endregion Core Properties

		#region Facility Properties

		public string FacilityName { get; set; }

		#endregion Facility Properties

		#region Regulator Properties

		public string RegulatorName { get; set; }

		#endregion Regulator Properties

		#region Organization Properties

		public string OrganizationName { get; set; }

		public string OrganizationHeadquarters { get; set; }

		#endregion Organization Properties

		#region Additional State Properties

		public NameValueCollection FormVariables
		{
			get
			{
				if (_FormVariables == null)
				{
					_FormVariables = new NameValueCollection();
				}
				return _FormVariables;
			}
			set
			{
				_FormVariables = value;
			}
		}

		public NameValueCollection QueryStringVariables
		{
			get
			{
				if (_QueryStringVariables == null)
				{
					_QueryStringVariables = new NameValueCollection();
				}
				return _QueryStringVariables;
			}
			set
			{
				_FormVariables = value;
			}
		}

		public NameValueCollection ServerVariables
		{
			get
			{
				if (_ServerVariables == null)
				{
					_ServerVariables = new NameValueCollection();
				}
				return _ServerVariables;
			}
			set
			{
				_ServerVariables = value;
			}
		}

		#endregion Additional State Properties

		#region Constructor

		public ErrorReportExtended()
		{
		}

		#endregion Constructor
	}
}