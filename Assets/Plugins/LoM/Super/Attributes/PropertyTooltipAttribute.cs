using System;
using UnityEngine;

namespace LoM.Super
{
    /// <summary>
    /// Attribute to specify a tooltip for a property.
    /// </summary>
    /// <param name="tooltip">The tooltip to display.</param>
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = true)]
    public class PropertyTooltipAttribute : Attribute
    {
        private readonly string tooltip;
        public string Tooltip => tooltip;
        public PropertyTooltipAttribute(string tooltip)
        {
            this.tooltip = tooltip;
        }
        public PropertyTooltipAttribute(TooltipAttribute tooltip)
        {
            this.tooltip = tooltip.tooltip;
        }
    }
}