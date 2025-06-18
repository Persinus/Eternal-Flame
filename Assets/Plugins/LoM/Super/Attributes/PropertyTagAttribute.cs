using System;

namespace LoM.Super
{
    /// <summary>
    /// Use this attribute to make a field a tag field.
    /// <b>Only works on string fields.</b>
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = true)]
    public class PropertyTagAttribute : Attribute
    {
        public PropertyTagAttribute() {}
    }
}