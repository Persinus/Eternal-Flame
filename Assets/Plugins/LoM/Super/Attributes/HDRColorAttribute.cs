using System;

namespace LoM.Super
{
    /// <summary>
    /// Attribute to mark a field as a HDR color.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public class HDRColorAttribute : Attribute { }
}