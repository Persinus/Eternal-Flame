using System;

namespace LoM.Super 
{
    /// <summary>
    /// Use this attribute to make a field read-only in the inspector.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class ReadOnlyAttribute : Attribute { }
}