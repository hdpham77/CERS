using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UPF
{
    public class ErrorInfo
    {
        #region Properties

        public string PropertyName { get; private set; }
        public string ErrorMessage { get; private set; }
        public object Instance { get; set; }
        public object Value { get; set; }

        #endregion

        #region Constructor

        public ErrorInfo(string propertyName, string errorMessage, Object instance = null, object value = null)
        {
            PropertyName = propertyName;
            ErrorMessage = errorMessage;
            Instance = instance;
            Value = value;
        }

        #endregion

        #region ToString Override Method

        public override string ToString()
        {
            return PropertyName + ": " + ErrorMessage;
        }

        #endregion

    }
}
