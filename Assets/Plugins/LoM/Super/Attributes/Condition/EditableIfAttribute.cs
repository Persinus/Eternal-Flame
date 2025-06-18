using System;

namespace LoM.Super 
{
    /// <summary>
    /// Attribute to make a field editable if a condition is met.<br/>
    /// **Note:** If the target field is a bool it will check for true/false, otherwise it will check if the field is not null.
    /// <hr/>
    /// <example>
    /// <code>
    /// [SerializeField] private bool m_UseFeature;
    /// [SerializeField, EditableIf("m_UseFeature")] private Transform m_ObjectReference; // Will only be editable if m_UseFeature is true.
    /// </code>
    /// </example>
    /// <hr/>
    /// </summary>
    /// <param name="fieldName">The name of the field to check.</param>
    /// <param name="value">The value to check for.</param>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class EditableIfAttribute : ShowIfAttribute
    {
        /// <summary>
        /// Attribute to make a field editable if a condition is met.
        /// </summary>
        /// <param name="fieldName">The name of the field to check.</param>
        /// <param name="value">The value to check for.</param>
        public EditableIfAttribute(string fieldName, bool value = true) : base(fieldName, value) { }
        
        /// <summary>
        /// Attribute to make a field editable if a condition is met. [Enum]
        /// <hr/>
        /// <example>
        /// <code>
        /// [SerializeField] 
        /// private SomeEnum m_Enum;
        /// [SerializeField, EditableIf(nameof(m_Enum), (int)SomeEnum.Option2)] 
        /// private Transform m_ObjectReference; // Will only be editable if m_Enum is Option2.
        /// </code>
        /// </example>
        /// <hr/>
        /// </summary>
        /// <param name="fieldName">The name of the field to check.</param>
        /// <param name="enumValue">The enum value to check for (as an int).</param>
        /// <param name="isEqualTo">If the field should be editable if the enum value is equal or inequal to the provided value.</param>
        public EditableIfAttribute(string fieldName, int enumValue, bool isEqualTo = true) : base(fieldName, enumValue, isEqualTo) { }
        
        /// <summary>
        /// Override this method to calculate if the field is active or not.
        /// </summary>
        /// <param name="target">The object to evaluate the condition for.</param>
        /// <returns>True if the field is active; otherwise false.</returns>
        public override bool EvaluateActive(object target) 
        {
            return true;
        }
        
        /// <summary>
        /// Override this method to calculate if the field is read only or not.
        /// </summary>
        /// <param name="target">The object to evaluate the condition for.</param>
        /// <returns>True if the field is read only; otherwise false.</returns>
        public override bool EvaluateReadOnly(object target) 
        {
            return !base.EvaluateActive(target);
        }
    }
}