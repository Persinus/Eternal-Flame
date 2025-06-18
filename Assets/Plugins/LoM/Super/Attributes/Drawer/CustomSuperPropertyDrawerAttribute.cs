using System;
using UnityEngine;

namespace LoM.Super.Editor
{
    /// <summary>
    /// Attribute to specify a custom SuperPropertyDrawer for a field or property.
    /// </summary>
    /// <param name="type">The type of the SuperPropertyDrawer.</param>
    /// <param name="order">The order of the SuperPropertyDrawer.</param>
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class CustomSuperPropertyDrawerAttribute : Attribute
    {
        private readonly Type type;
        private nint order = 0;
        
        public Type Type => type;
        public nint Order => order;
        
        /// <summary>
        /// Attribute to specify a custom SuperPropertyDrawer for a field or property.
        /// </summary>
        /// <param name="type">The type of the SuperPropertyDrawer.</param>
        public CustomSuperPropertyDrawerAttribute(Type type)
        {
            this.type = type;
        }
        
        /// <summary>
        /// Attribute to specify a custom SuperPropertyDrawer for a field or property.
        /// </summary>
        /// <param name="type">The type of the SuperPropertyDrawer.</param>
        /// <param name="order">The order of the SuperPropertyDrawer.</param>
        public CustomSuperPropertyDrawerAttribute(Type type, nint order)
        {
            this.type = type;
            this.order = order;
        }
    }
}