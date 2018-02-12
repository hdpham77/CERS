using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace UPF
{
    public class ErrorInfoCollection : Collection<ErrorInfo>
    {

        public ErrorInfo Add(string propertyName, string errorMessage, object instance = null, object value = null)
        {
            ErrorInfo error = new ErrorInfo(propertyName, errorMessage, instance, value);
            this.Add(error);
            return error;
        }

        public void AddRange(IEnumerable<ErrorInfo> errors)
        {
            foreach (ErrorInfo error in errors)
            {
                this.Add(error);
            }
        }

    }
}
