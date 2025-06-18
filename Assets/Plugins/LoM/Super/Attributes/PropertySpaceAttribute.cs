using System;
using UnityEngine;

namespace LoM.Super
{
    /// <summary>
    /// Use this attribute to add a space between properties in the inspector.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class PropertySpaceAttribute : Attribute
    {
        private readonly float height;
        public float Height => height;
        public PropertySpaceAttribute()
        {
            height = 8f;
        }
        public PropertySpaceAttribute(float height)
        {
            this.height = height;
        }
        public PropertySpaceAttribute(SpaceAttribute space)
        {
            height = space.height;
        }
    }
}