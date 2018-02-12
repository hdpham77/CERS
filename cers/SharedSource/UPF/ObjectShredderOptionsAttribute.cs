using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UPF
{
	/// <summary>
	/// An attribute that can be placed on a property that contains optons the <see cref="ObjectShredder{T}"/> will take into consideration when "shredding" the object
	/// down to a <see cref="DataTable"/>.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
	public class ObjectShredderOptionsAttribute : Attribute
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ObjectShredderOptionsAttribute"/> attribute.
		/// </summary>
		public ObjectShredderOptionsAttribute()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ObjectShredderOptionsAttribute"/> attribute.
		/// </summary>
		/// <param name="ignore">Specifies that the <see cref="ObjectShredder{T}"/> will ignore the property this attribute decorates.</param>
		public ObjectShredderOptionsAttribute(bool ignore)
		{
		}

		/// <summary>
		/// Specifies that the <see cref="ObjectShredder{T}"/> will ignore the property this attribute decorates.
		/// </summary>
		public bool Ignore { get; set; }
	}
}