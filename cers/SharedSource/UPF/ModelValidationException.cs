using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UPF
{
    public class ModelValidationException : Exception
    {

        private const string ErrorInfoDataKey = "ErrorInfo";

        public ErrorInfoCollection Errors
        {
            get
            {
                ErrorInfoCollection errors = null;
                if (Data.Contains(ErrorInfoDataKey))
                {
                    if (Data[ErrorInfoDataKey] != null)
                    {
                        errors = Data[ErrorInfoDataKey] as ErrorInfoCollection;
                    }
                }

                if (errors == null)
                {
                    errors = new ErrorInfoCollection();
                }

                return errors;
            }
        }

        public ModelValidationException(Type type, ErrorInfoCollection errors)
            : this("Model validation failed for the " + type.Name + ".", errors)
        {
            Data.Add(ErrorInfoDataKey, errors);
        }

        public ModelValidationException(string message, ErrorInfoCollection errors)
            : base(message)
        {
            Data.Add(ErrorInfoDataKey, errors);
        }

    }
}
