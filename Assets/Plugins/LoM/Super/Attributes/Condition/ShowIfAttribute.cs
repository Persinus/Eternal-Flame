using System;
using UnityEngine;

namespace LoM.Super 
{
    /// <summary>
    /// Attribute to show or hide a field based on a condition.<br/>
    /// **Note:** If the target field is a bool it will check for true/false, otherwise it will check if the field is not null.
    /// <hr/>
    /// <example>
    /// <code>
    /// [SerializeField] private bool m_UseFeature;
    /// [SerializeField, ShowIf("m_UseFeature")] private Transform m_ObjectReference; // Will only be visible if m_UseFeature is true.
    /// </code>
    /// </example>
    /// <hr/>
    /// </summary>
    /// <param name="fieldName">The name of the field to check.</param>
    /// <param name="value">The value to check for.</param>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class ShowIfAttribute : ConditionAttribute 
    {
        // Member Variables
        private string m_fieldName;
        private bool m_value;
        private int m_enumValue = -1;
        
        // Getters
        public string FieldName => m_fieldName;
        public bool Value => m_value;
        public int EnumValue => m_enumValue;
        
        /// <summary>
        /// Attribute to show or hide a field based on a condition. (shown if condition is true)
        /// </summary>
        /// <param name="fieldName">The name of the field to check.</param>
        /// <param name="value">The value to check for.</param>
        public ShowIfAttribute(string fieldName, bool value = true) 
        {
            m_fieldName = fieldName;
            m_value = value;
        }
        
        /// <summary>
        /// Attribute to show or hide a field based on a condition. (shown if condition is true) [Enum]
        /// <hr/>
        /// <example>
        /// <code>
        /// [SerializeField]
        /// private SomeEnum m_Enum;
        /// [SerializeField, ShowIf(nameof(m_Enum), (int)SomeEnum.Option2)]
        /// private Transform m_ObjectReference; // Will only be visible if m_Enum is Option2.
        /// </code>
        /// </example>
        /// <hr/>
        /// </summary>
        /// <param name="fieldName">The name of the field to check.</param>
        /// <param name="enumValue">The enum value to check for (as an int).</param>
        /// <param name="isEqualTo">If the field should be visible if the enum value is equal or inequal to the provided value.</param>
        public ShowIfAttribute(string fieldName, int enumValue, bool isEqualTo = true)
        {
            m_fieldName = fieldName;
            m_enumValue = enumValue;
            m_value = isEqualTo;
        }
        
        /// <summary>
        /// Override this method to calculate if the field is active or not.
        /// </summary>
        /// <param name="target">The object to evaluate the condition for.</param>
        /// <returns>True if the field is active; otherwise false.</returns>
        public override bool EvaluateActive(object target) 
        {
            var type = target.GetType();
            var field = type.GetField(m_fieldName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            var property = type.GetProperty(m_fieldName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            var value = field != null ? field.GetValue(target) : property.GetValue(target);
            if (value is bool boolValue) 
            {
                return boolValue == m_value;
            }
            else if (typeof(Enum).IsAssignableFrom(value?.GetType()))
            {
                if (m_enumValue != -1)
                {
                    return (int)value == m_enumValue == m_value;
                }
                return m_value != (value != null && (int)value != 0);
            }
            else if (value is string stringValue) 
            {
                return (!string.IsNullOrEmpty(stringValue)) == m_value;
            }
            else if (value is UnityEngine.Object)
            {
                bool isNotNull = value?.ToString() != "null";
                return isNotNull == m_value;
            }
            else 
            {
                bool isNotEmpty = value != null;
                return isNotEmpty == m_value;
            }
        }
        
        /// <summary>
        /// Override this method to calculate if the field is read only or not.
        /// </summary>
        /// <param name="target">The object to evaluate the condition for.</param>
        /// <returns>True if the field is read only; otherwise false.</returns>
        public override bool EvaluateReadOnly(object target) 
        {
            return false;
        }
    }
}