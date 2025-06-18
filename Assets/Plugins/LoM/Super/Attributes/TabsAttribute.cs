using System;

namespace LoM.Super
{
    /// <summary>
    /// Use this attribute to make a field a tag field.
    /// <b>Only works on Enum fields.</b>
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class TabsAttribute : Attribute
    {
        public TabsAttribute() { }
    }
}