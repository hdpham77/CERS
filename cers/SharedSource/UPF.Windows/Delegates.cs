using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace UPF.Windows {

	public delegate void ControlValidationStateHandler(Control control, string tip, bool isValid);
	public delegate void AddObjectToListMethod<T>(T obj);
	public delegate void NotifyMessageMethod<T>(string message, T obj);
	public delegate void NotifyMessageMethod(string message);

}
