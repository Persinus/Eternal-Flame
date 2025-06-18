using System;

namespace LoM.Super
{
    /// <summary>
    /// Use this attribute to enable properties to be shown like fields in the inspector.<br/>
    /// **WARNING: This does NOT serialize or save the property in any way.**<br/>
    /// <br/>
    /// Due to to the limition mentioned above the attributes are only editable in the play mode.<br/>
    /// Any changes made to the properties in edit mode will not be saved.
    /// <hr/>
    /// <example>
    /// If you assign this attribute to a property with a private or protected setter, it will be shown in the inspector as a readonly field.<br/>
    /// <code>
    /// [SerializeProperty]
    /// public int PlayerHealth => m_PlayerHealth;
    /// </code>
    /// </example>
    /// <example>
    /// If you assign this attribute to a property with a public setter, it will be shown in the inspector as a normal field.<br/>
    /// <i>NOTE: It will still not be editable in edit mode.</i><br/>
    /// <code>
    /// [SerializeProperty]
    /// public int PlayerHealth { get; set; }
    /// </code>
    /// </example>
    /// <hr/>
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class SerializePropertyAttribute: Attribute
    {
        public SerializePropertyAttribute() { }
    }
}