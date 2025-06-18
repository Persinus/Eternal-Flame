using System;
using UnityEngine;

namespace LoM.Super
{
    /// <summary>
    /// Attribute to specify a custom label for a field or property.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public class LabelAttribute : Attribute
    {
        private readonly string label;
        public string Label => label;
        
        public LabelAttribute(string label)
        {
            this.label = label;
        }
    }
}