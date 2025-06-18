using System;

namespace LoM.Super 
{
    /// <summary>
    /// Attribute to specify conditions for a field or property.<br/>
    /// <i>NOTE: Use this to create your own custom conditions.</i>
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class ConditionAttribute : Attribute 
    {
        /// <summary>
        /// Override this method to calculate if the field is active or not.
        /// </summary>
        /// <param name="target">The object to evaluate the condition for.</param>
        /// <returns>True if the field is active; otherwise false.</returns>
        public virtual bool EvaluateActive(object target) 
        {
            return true;
        }
        
        /// <summary>
        /// Override this method to calculate if the field is read only or not.
        /// </summary>
        /// <param name="target">The object to evaluate the condition for.</param>
        /// <returns>True if the field is read only; otherwise false.</returns>
        public virtual bool EvaluateReadOnly(object target) 
        {
            return false;
        }
    }
}