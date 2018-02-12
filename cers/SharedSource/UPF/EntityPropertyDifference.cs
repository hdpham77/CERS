using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace UPF
{
	public class EntityPropertyDifference<T>
	{
		public T First { get; set; }

		public object FirstValue { get; set; }

		public PropertyInfo Property { get; set; }

		public string PropertyName { get { return Property.Name; } }

		public T Second { get; set; }

		public object SecondValue { get; set; }
	}
}