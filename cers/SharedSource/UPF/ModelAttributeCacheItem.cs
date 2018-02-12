using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace UPF
{
	public class ModelAttributeCacheItem<TAttribute> where TAttribute : Attribute
	{
		public PropertyDescriptor ModelProperty { get; set; }

		public PropertyDescriptor BuddyProperty { get; set; }

		public TAttribute Attribute { get; set; }

		public object Tag { get; set; }
	}

	public class ModelAttributeCacheItem<TAttribute, TMetadataInfo> : ModelAttributeCacheItem<TAttribute>
		where TAttribute : Attribute
		where TMetadataInfo : class
	{
		public TMetadataInfo Metadata { get; set; }
	}
}