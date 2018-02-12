using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace UPF
{
    public static class ModelExtensionMethods
    {
        #region Fields

        private static Dictionary<Type, List<ModelAttributeCacheItem<ValidationAttribute>>> _TypeValidationAttributeCache;
        private static object _Lock = new object();

        #endregion Fields

        #region Methods

        public static bool IfInRange(this DateTime? value, DateTime? start, DateTime? end, bool nullable = false, bool excludeTime = false)
        {
            //TODO: FINISH
            bool result = value == null && nullable;
            if (value != null)
            {
                if (!excludeTime)
                {
                    if (start != null && end == null)
                    {
                    }
                    else if (start == null && end != null)
                    {
                    }
                    else
                    {
                    }
                }
                else
                {
                }
            }
            return result;
        }

        public static bool IfInRange(this int value, int minValue, int maxValue)
        {
            int? tempValue = value;
            return tempValue.IfInRange(minValue, maxValue, false);
        }

        public static bool IfInRange(this int? value, int minValue, int maxValue, bool nullable = false)
        {
            bool result = value == null && nullable;
            if (value != null)
            {
                result = (value.Value >= minValue && value.Value <= maxValue);
            }
            return result;
        }

        public static bool IfInRange(this string value, string delimitter, params string[] valueRange)
        {
            bool result = false;
            //make sure this value is not null or empty before evaluation.
            if (!string.IsNullOrWhiteSpace(value))
            {
                //do we have a delimitter and does our value contain and instance of it?
                if (!string.IsNullOrWhiteSpace(delimitter) && value.IndexOf(delimitter) > -1)
                {
                    //split out the string based on the delimitter.
                    string[] multiValues = value.Split(new string[] { delimitter }, StringSplitOptions.RemoveEmptyEntries);
                    //loop through each of the values split out by delimitter and check against the list of possible values passed in.

                    result = multiValues.Any(multiValue =>
                    {
                        return valueRange.Any(valueRangeItem =>
                        {
                            return valueRangeItem == multiValue;
                        });
                    });
                }
                else
                {
                    result = value.IfInRange(valueRange);
                }
            }
            return result;
        }

        public static bool IfInRange<T>(this T currentItem, params T[] items) where T : IComparable
        {
            bool result = false;
            if (items != null)
            {
                foreach (T item in items)
                {
                    if (currentItem.Equals(item))
                    {
                        result = true;
                        break;
                    }
                }
            }
            return result;
        }

        public static PropertyInfo GetPropertyInfo<TModel, TValue>(Expression<Func<TModel, TValue>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            PropertyInfo property = null;

            if (expression.Body.NodeType == ExpressionType.MemberAccess)
            {
                MemberExpression memberExpression = (MemberExpression)expression.Body;
                property = memberExpression.Member as PropertyInfo;
            }

            return property;
        }

        public static PropertyInfo GetPropertyInfo<TModel, TValue>(this TModel parameter, Expression<Func<TModel, TValue>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            PropertyInfo property = null;

            if (expression.Body.NodeType == ExpressionType.MemberAccess)
            {
                MemberExpression memberExpression = (MemberExpression)expression.Body;
                property = memberExpression.Member as PropertyInfo;
            }

            return property;
        }

        public static void SetCommonFields(this IModelEntity entity, int currentUserID = -1, bool creating = false, bool voided = false)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            if (creating)
            {
                entity.CreatedByID = currentUserID;
                entity.CreatedOn = DateTime.Now;
            }

            entity.UpdatedByID = currentUserID;
            entity.UpdatedOn = DateTime.Now;
            entity.Voided = voided;
        }

        public static void SetCommonFields(this IEnumerable<IModelEntity> entities, int currentUserID = -1, bool creating = false, bool voided = false)
        {
            if (entities == null)
            {
                throw new ArgumentNullException("entities");
            }

            foreach (var entity in entities)
            {
                if (creating)
                {
                    entity.CreatedByID = currentUserID;
                    entity.CreatedOn = DateTime.Now;
                }

                entity.UpdatedByID = currentUserID;
                entity.UpdatedOn = DateTime.Now;
                entity.Voided = voided;
            }
        }

        /// <summary>
        /// Validates the entity against the Standard .NET Data Annotations API.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static ModelValidationResult ValidateValidatableObject<T>(this T instance) where T : class, IValidatableObject
        {
            ModelValidationResult result = new ModelValidationResult();

            //get the cached ModelValidationPropertyCacheItem for this type.
            var cachedItems = ModelMetadataHelper.GetPropertyMetadataAttributesForType<T, ValidationAttribute>(ref _Lock, ref _TypeValidationAttributeCache);

            //do the validation.
            var validationResults = from ci in cachedItems
                                    where !ci.Attribute.IsValid(ci.ModelProperty.GetValue(instance))
                                    select new ErrorInfo(ci.BuddyProperty.Name, ci.Attribute.FormatErrorMessage(String.Empty), instance, typeof(T).GetProperty(ci.BuddyProperty.Name).GetValue(instance, null));

            //add validation problems to the Errors collection of the result.
            result.Errors.AddRange(validationResults);

            return result;
        }

        public static TAttribute GetTypeAttribute<TModel, TAttribute>(this TModel model, bool inherit = false) where TAttribute : Attribute
        {
            Type type = typeof(TModel);
            return ModelMetadataHelper.GetTypeAttribute<TAttribute>(type, inherit);
        }

        /// <summary>
        /// Compares two object and filters out whatever is in the "ignore" params collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <param name="to"></param>
        /// <param name="ignore"></param>
        /// <returns></returns>
        public static bool IsEntityEqual<T>(this T self, T to, params string[] ignore) where T : class
        {
            //Both of the entities passed in must have values.
            if (self != null && to != null)
            {
                Type type = typeof(T);
                List<string> ignoreList = new List<string>(ignore);

                //Filter out properties that have associations.
                foreach (System.Reflection.PropertyInfo pi in type.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance).Where(f => !f.PropertyType.FullName.Contains("CERS.Model") && !f.PropertyType.FullName.Contains("System.Data")))
                {
                    if (!ignoreList.Contains(pi.Name))
                    {
                        object selfValue = type.GetProperty(pi.Name).GetValue(self, null);
                        object toValue = type.GetProperty(pi.Name).GetValue(to, null);

                        if (selfValue != toValue && (selfValue == null || !selfValue.Equals(toValue)))
                        {
                            return false;
                        }
                    }
                }
                return true;
            }
            return self == to;
        }

        #endregion Methods
    }
}