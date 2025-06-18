using System;
using UnityEngine;

namespace LoM.Super
{
    /// <summary>
    /// Attribute to specify a header for a property.
    /// <i>Is drawn above the property.</i>
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = true)]
    public class PropertyHeaderAttribute : Attribute
    {
        private readonly string header;
        public string Header => header;
        public PropertyHeaderAttribute(string header)
        {
            this.header = header;
        }
        public PropertyHeaderAttribute(HeaderAttribute header)
        {
            this.header = header.header;
        }
    }
}