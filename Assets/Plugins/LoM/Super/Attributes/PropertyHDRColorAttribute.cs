using System;

namespace LoM.Super
{
    /// <summary>
    /// Attribute to make a field be drawn as HDR color field.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class PropertyHDRColorAttribute : Attribute { }
}