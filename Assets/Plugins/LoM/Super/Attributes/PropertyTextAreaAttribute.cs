using System;
using UnityEngine;

namespace LoM.Super
{
    /// <summary>
    /// Attribute to make a field be drawn as a text area.
    /// <b>Only works on string fields.</b>
    /// <hr/>
    /// <example>
    /// <code>
    /// [PropertyTextArea(3, 10)]
    /// public string text;
    /// </code>
    /// </example>
    /// <hr/>
    /// </summary>
    /// <param name="minLines">Minimum amount of lines to display.</param>
    /// <param name="maxLines">Maximum amount of lines to display.</param>
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = true)]
    public class PropertyTextAreaAttribute : Attribute
    {
        public readonly int minLines;
        public readonly int maxLines;
        public int MinLines => minLines;
        public int MaxLines => maxLines;
        public PropertyTextAreaAttribute() 
        { 
            minLines = 3;
            maxLines = 3;
        }
        public PropertyTextAreaAttribute(int minLines, int maxLines)
        {
            this.minLines = minLines;
            this.maxLines = maxLines;
        }
        public PropertyTextAreaAttribute(TextAreaAttribute textArea)
        {
            minLines = textArea.minLines;
            maxLines = textArea.maxLines;
        }
    }
}