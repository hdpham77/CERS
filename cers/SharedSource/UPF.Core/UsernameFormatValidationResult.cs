using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UPF.Core
{
	public class UsernameFormatValidationResult
	{
		private List<string> _Errors;

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

		public bool IsValid
		{
			get
			{
				return Errors.Count == 0;
			}
		}

		public virtual void AddError(string message)
		{
			if (string.IsNullOrWhiteSpace(message))
			{
				throw new ArgumentNullException("message");
			}
			Errors.Add(message);
		}
	}
}