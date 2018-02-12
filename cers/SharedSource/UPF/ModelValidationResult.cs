using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UPF
{
	public class ModelValidationResult
	{
		#region Fields

		private ErrorInfoCollection _Errors;

		#endregion Fields

		#region Constructors

		public ModelValidationResult()
		{
		}

		#endregion Constructors

		#region Properties

		public ErrorInfoCollection Errors
		{
			get
			{
				if (_Errors == null)
				{
					_Errors = new ErrorInfoCollection();
				}
				return _Errors;
			}
		}

		public bool IsValid
		{
			get
			{
				return (Errors.Count == 0);
			}
		}

		#endregion Properties

		#region AddError Method

		public ErrorInfo AddError(string propertyName, string errorMessage, object instance, object value)
		{
			return Errors.Add(propertyName, errorMessage, instance, value);
		}

		#endregion AddError Method

		#region GetErrors Method

		public string GetErrors(string delimitter = "\r\n")
		{
			StringBuilder result = new StringBuilder();
			int errorIndex = 0;
			foreach (var error in Errors)
			{
				result.Append(error.ToString());
				if (errorIndex < Errors.Count - 1)
				{
					result.Append(delimitter);
				}
				errorIndex++;
			}

			return result.ToString();
		}

		#endregion GetErrors Method

		#region GenerateException Method

		public void GenerateException<TModel>(TModel entity)
		{
			if (!IsValid)
			{
				throw new ModelValidationException(typeof(TModel), Errors);
			}
		}

		#endregion GenerateException Method
	}
}