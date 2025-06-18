using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using LoM.Super;
using LoM.Super.Serialization;
using System.Reflection;
using UnityEngine.UIElements;

namespace LoM.Super.Editor.Drawer
{
    /// <summary>
    /// Derive from this class to create a custom property drawer for a property.
    /// </summary>
    public abstract class SuperPropertyDrawer : GUIDrawer
    {
        // Member Variables
        private SerializedAttributes m_Attributes;
        private MemberInfo m_FieldInfo;
        private string m_PreferredLabel;
        
        /// <summary>
        /// Attributes of the property.
        /// </summary>
        public SerializedAttributes attribute => m_Attributes;
        
        /// <summary>
        /// FieldInfo or PropertyInfo of the property.
        /// </summary>
        public MemberInfo fieldInfo => m_FieldInfo;
        
        /// <summary>
        /// Preferred label of the property.
        /// </summary>
        public string preferredLabel => m_PreferredLabel;
        
        // Pseudo Constructor
        internal SuperPropertyDrawer Init(SuperSerializedProperty property)
        {
            m_Attributes = property.Attributes;
            m_FieldInfo = property.Info;
            m_PreferredLabel = property.Attributes.Label;
            return this;
        }
        
        /// <summary>
        /// Override this method to make your own UI Toolkit based GUI for the property.
        /// </summary>
        /// <param name="property">The SuperSerializedProperty to make the custom GUI for.</param>
        /// <returns>The element containing the custom GUI.</returns>
        public virtual VisualElement CreatePropertyGUI(SerializedProperty property) => null;
        
        /// <summary>
        /// Override this method to specify how tall the GUI for this field is in pixels.
        /// </summary>
        /// <param name="property">The SuperSerializedProperty to get the height for.</param>
        /// <param name="label">The label of the property.</param>
        /// <returns>The height of the GUI for this field in pixels.</returns>
        public virtual float GetPropertyHeight(SuperSerializedProperty property, GUIContent label) => EditorGUIUtility.singleLineHeight;
        
        /// <summary>
        /// Override this method to make your own IMGUI based GUI for the property.
        /// </summary>
        /// <param name="position">Rectangle on the screen to use for the property GUI.</param>
        /// <param name="property">The SuperSerializedProperty to make the custom GUI for.</param>
        /// <param name="label">The label of the property.</param>
        public virtual void OnGUI(Rect position, SuperSerializedProperty property, GUIContent label) {}
        
        /// <summary>
        /// Override this method to draw your own Scene GUI for the property.
        /// </summary>
        /// <param name="property">The SuperSerializedProperty to draw the Scene GUI for.</param>
        public virtual void OnSceneGUI(SuperSerializedProperty property) {}
    }
}