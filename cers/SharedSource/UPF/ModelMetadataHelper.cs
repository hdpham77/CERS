using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace UPF
{
	public static class ModelMetadataHelper
	{
		public static TAttribute GetTypeAttribute<TAttribute>(Type type, bool inherit = true) where TAttribute : Attribute
		{
			TAttribute attribute = type.GetCustomAttributes(typeof(TAttribute), inherit).OfType<TAttribute>().FirstOrDefault();

			return attribute;
		}

		public static List<ModelAttributeCacheItem<TAttribute>> GetPropertyMetadataAttributesForType<TEntity, TAttribute>(ref object lockObject, ref Dictionary<Type, List<ModelAttributeCacheItem<TAttribute>>> cacheStore)
			where TEntity : class
			where TAttribute : Attribute
		{
			return GetPropertyMetadataAttributesForType<TAttribute>(typeof(TEntity), ref lockObject, ref cacheStore);
		}

		public static List<ModelAttributeCacheItem<TAttribute>> GetPropertyMetadataAttributesForType<TAttribute>(Type type, ref object lockObject, ref Dictionary<Type, List<ModelAttributeCacheItem<TAttribute>>> cacheStore)

			where TAttribute : Attribute
		{
			List<ModelAttributeCacheItem<TAttribute>> results = new List<ModelAttributeCacheItem<TAttribute>>();
			if (cacheStore == null)
			{
				//lock to prevent threading issues since it's static.
				lock (lockObject)
				{
                    // check here again in case another thread changed it before we locked 
                    if (cacheStore == null)
                    {
                        cacheStore = new Dictionary<Type, List<ModelAttributeCacheItem<TAttribute>>>();
                    }
				}
			}

			//check to see whether we have the information we need cached for this type.
			if (!cacheStore.ContainsKey(type))
			{
				//we don't have the information we need cache, so lets get it and then cache it. Let's lock to prevent threading issues since the variable is static.
				lock (lockObject)
				{
                    // check here again in case another thread changed it before we locked 
                    if (!cacheStore.ContainsKey(type))
                    {
                        //get the attributes for the model.
                        var typeAttributes = ModelMetadataHelper.GetPropertyMetadataAttributesForType<TAttribute>(type);

                        //add the typeAttributes object for each property to the cache for this type.
                        cacheStore.Add(type, typeAttributes.ToList());
                    }
				}
			}
			return cacheStore[type];
		}

		public static List<ModelAttributeCacheItem<TAttribute>> GetPropertyMetadataAttributesForType<TEntity, TAttribute>()

			where TEntity : class
			where TAttribute : Attribute
		{
			return GetPropertyMetadataAttributesForType<TAttribute>(typeof(TEntity));
		}

		public static List<ModelAttributeCacheItem<TAttribute>> GetPropertyMetadataAttributesForType<TAttribute>(Type type) where TAttribute : Attribute
		{
			//Try to get the MetadataType attribute from the object
			MetadataTypeAttribute metadataAttrib = type.GetCustomAttributes(typeof(MetadataTypeAttribute), true).OfType<MetadataTypeAttribute>().FirstOrDefault();

			//If the MetadataType attribute existed, get the metadata class or else just use the class of the object
			Type buddyClassOrModelClass = metadataAttrib != null ? metadataAttrib.MetadataClassType : type;

			//get the buddy class' property descriptors.
			IEnumerable<PropertyDescriptor> buddyClassProperties = TypeDescriptor.GetProperties(buddyClassOrModelClass).Cast<PropertyDescriptor>();

			//get he model class' property descriptors.
			IEnumerable<PropertyDescriptor> modelClassProperties = TypeDescriptor.GetProperties(type).Cast<PropertyDescriptor>();

			//lets join the buddy classes property descriptors with the model class' property descriptors and locate a validation attribute and then build a ModelValidationPropertyCacheItem with the info needed.
			var typeValidationAttributes = from buddyProp in buddyClassProperties
										   join modelProp in modelClassProperties on buddyProp.Name equals modelProp.Name
										   from attribute in buddyProp.Attributes.OfType<TAttribute>()
										   select new ModelAttributeCacheItem<TAttribute>
										   {
											   BuddyProperty = buddyProp,
											   ModelProperty = modelProp,
											   Attribute = attribute
										   };

			return typeValidationAttributes.ToList();
		}
	}
}