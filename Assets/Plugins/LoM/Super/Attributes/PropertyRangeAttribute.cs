using System;
using UnityEngine;

namespace LoM.Super
{
    /// <summary>
    /// Attribute to specify a range for a property.
    /// <b>Only works for float and int.</b>
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = true)]
    public class PropertyRangeAttribute : Attribute
    {
        private readonly float min;
        private readonly float max;
        public float Min => min;
        public float Max => max;
        public PropertyRangeAttribute(float min, float max)
        {
            this.min = min;
            this.max = max;
        }
        public PropertyRangeAttribute(RangeAttribute range)
        {
            min = range.min;
            max = range.max;
        }
    }
}