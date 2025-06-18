using System;

namespace LoM.Super
{
    /// <summary>
    /// Attribute to make a property a layer field.
    /// <b>Only works on int fields.</b>
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class PropertyLayerAttribute : Attribute
    {
        public PropertyLayerAttribute() { }
    }
}