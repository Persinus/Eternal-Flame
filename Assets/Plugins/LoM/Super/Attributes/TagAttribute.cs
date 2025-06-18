using System;

namespace LoM.Super
{
    /// <summary>
    /// Use this attribute to make a field a tag field.
    /// <b>Only works on string fields.</b>
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public class TagAttribute : Attribute
    {
        public TagAttribute() { }
    }
}