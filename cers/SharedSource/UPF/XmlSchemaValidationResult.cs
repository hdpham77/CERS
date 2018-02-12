using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UPF
{
	public class XmlSchemaValidationResult
	{
		private List<string> _Errors;

		public bool IsValid { get; set; }

		public List<string> Errors
		{
			get
			{
				if (_Errors == null)
				{
					_Errors = new List<string>();
				}
				return _Errors;
			}
		}

		public string ErrorsString
		{
			get
			{
				return Errors.ToDelimitedString(",");
			}
		}
	}
}