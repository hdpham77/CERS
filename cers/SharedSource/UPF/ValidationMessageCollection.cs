using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace UPF
{
    public class ValidationMessageCollection : Collection<ValidationMessage>
    {

        private ValidationMessage Add(string propertyName, string message)
        {
            ValidationMessage validationMessage = new ValidationMessage(propertyName, message);
            this.Add(validationMessage);
            return validationMessage;
        }
    }
}
