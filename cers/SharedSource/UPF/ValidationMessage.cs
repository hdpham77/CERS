using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UPF
{
    public class ValidationMessage
    {
        public string PropertyName { get; set; }
        public string Message { get; set; }

        public ValidationMessage()
        {

        }

        public ValidationMessage(string propertyName, string message)
        {
            PropertyName = propertyName;
            Message = message;
        }

    }
}
