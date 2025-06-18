using System;

namespace LoM.Super
{
    /// <summary>
    /// Use this attribute to make a field a layer field. <br/>
    /// <b>Only works on int fields.</b>
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public class LayerAttribute : Attribute
    {
        public LayerAttribute() { }
    }
}