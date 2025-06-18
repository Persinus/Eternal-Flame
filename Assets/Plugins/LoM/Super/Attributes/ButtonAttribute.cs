using System;
using UnityEngine;

namespace LoM.Super
{
    /// <summary>
    /// Attribute to specify a inspector Button before or after a field, property or method.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class ButtonAttribute : Attribute
    {
        // Member Variables
        private bool m_isMethod = false;
        private string m_text;
        private bool m_displayAfter = false;
        private Action m_action;
        private string m_methodName;
        
        // Getters
        public bool IsMethod => m_isMethod;
        public bool DisplayAfter => m_displayAfter;
        public string Text => m_text;
        public string MethodName => m_methodName;
        
        /// <summary>
        /// Specify a inspector Button before or after a method
        /// </summary>
        /// <param name="text">Text to be displayed on the button (Default is method name)
        public ButtonAttribute(string text = null, bool displayAfter = false)
        {
            m_text = text;
            m_displayAfter = displayAfter;
        }
        
        /// <summary>
        /// Specify a inspector Button before or after a field or property
        /// </summary>
        /// <param name="text">Text to be displayed on the button</param>
        /// <param name="methodName">Name of the method to be called when the button is clicked</param>
        /// <param name="displayAfter">Whether the button should be displayed after the field or property</param>
        public ButtonAttribute(string text, string methodName, bool displayAfter = false)
        {
            m_text = text;
            m_methodName = methodName;
            m_displayAfter = displayAfter;
        }
    }
}