using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CERS
{
    /// <summary>
    /// Specfies what <see cref="ResourceType"/> an entity is, as well as some basic governence rules.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class FacilitySubmittalElementResourceEntityAttribute : Attribute
    {
        private int? _MinimumRequiredCount;
        private int? _MaximumRequiredCount;

        /*
        /// <summary>
        /// Gets or sets the number of fields that are defined as
        /// </summary>
        public int? NumberOfMinimumRequiredFields { get; set; }
         */

        /// <summary>
        /// Gets or sets the <see cref="ResourceType"/> this entity is associated with.
        /// </summary>
        public ResourceType Type { get; set; }

        public GuidanceLevel Level { get; set; }

        /// <summary>
        /// Gets or sets the minimum number of instances are required of this resource entity.
        /// </summary>
        public int MinimumRequiredCount
        {
            get
            {
                int result = 0;
                if (_MinimumRequiredCount.HasValue)
                {
                    result = _MinimumRequiredCount.Value;
                }
                return result;
            }
            set
            {
                if (value > 0)
                {
                    _MinimumRequiredCount = value;
                }
                else
                {
                    _MinimumRequiredCount = null;
                }
            }
        }

        /// <summary>
        /// Gets or sets the maximum number of instances are required of this resource entity.
        /// </summary>
        public int MaximumRequiredCount
        {
            get
            {
                int result = 0;
                if (_MaximumRequiredCount.HasValue)
                {
                    result = _MaximumRequiredCount.Value;
                }
                return result;
            }
            set
            {
                if (value > 0)
                {
                    _MaximumRequiredCount = value;
                }
                else
                {
                    _MaximumRequiredCount = null;
                }
            }
        }

        public int? GetMaximumRequiredCount()
        {
            return _MaximumRequiredCount;
        }

        public int? GetMinimumRequiredCount()
        {
            return _MinimumRequiredCount;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FacilitySubmittalResourceEntityAttribute"/> class.
        /// </summary>
        /// <param name="type">The <see cref="ResourceType"/> the entity is bound to.</param>
        public FacilitySubmittalElementResourceEntityAttribute(ResourceType type, GuidanceLevel level = GuidanceLevel.Advisory)
        {
            Type = type;
            Level = level;
        }
    }
}