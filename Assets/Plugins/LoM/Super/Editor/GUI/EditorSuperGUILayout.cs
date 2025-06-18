using System;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using LoM.Super.Serialization;
using LoM.Super.Editor.Drawer;
using System.Reflection;

namespace LoM.Super.Editor
{
    /// <summary>
    /// Editor GUI Layout for SuperSerializedProperty. <br/>
    /// All methods are similar to their EditorGUILayout counterparts but take a SuperSerializedProperty instead of a SerializedProperty.
    /// </summary>
    public static class EditorSuperGUILayout
    {
        /// <summary>
        /// Make a field for SerializedProperty. 
        /// </summary>
        /// <param name="property">The SuperSerializedProperty to make the field for.</param>
        /// <param name="label">Optional label to use. If not specified the label of the property itself is used.<br/>Use GUIContent.none to not display a label at all.</param>
        /// <param name="includeChildren">If true the property including children is drawn; otherwise only the control itself (such as only a foldout but nothing below it).</param>
        /// <param name="options">An optional list of layout options that specify extra layout properties. Any values passed in here will override settings defined by the style.<br/> See Also GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight, GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
        /// <returns>True if the property has children and is expanded and includeChildren was set to false; otherwise false.</returns>
        public static bool PropertyField(SuperSerializedProperty property, GUIContent label, bool includeChildren = true, params GUILayoutOption[] options)
        {
            if (!property.Attributes.IsActive) return false;
            bool enabledBefore = GUI.enabled;
            if (property.Attributes.IsReadOnly) GUI.enabled = false;
            
            SuperPropertyDrawer drawer = SuperPropertyDrawerUtility.Instance[property];
            DrawAttributesBefore(property, ignoreFieldProperties: drawer == null);
            
            if (drawer != null)
            {
                VisualElement element = drawer.CreatePropertyGUI(property);
                if (element != null)
                {
                    // Draw UI Toolkit GUI
                    float height = drawer.GetPropertyHeight(property, label);
                    Rect position = EditorGUILayout.GetControlRect(true, height, options);
                    element.style.height = height;
                    element.style.width = position.width;
                    element.style.marginLeft = position.xMin;
                    element.style.marginTop = position.yMin;
                    element.style.marginRight = position.xMax;
                    element.style.marginBottom = position.yMax;
                    element.style.position = Position.Absolute;
                    element.MarkDirtyRepaint();
                }
                else
                {
                    // Draw IMGUI GUI
                    float height = drawer.GetPropertyHeight(property, label);
                    Rect position = EditorGUILayout.GetControlRect(true, height, options);
                    drawer.OnGUI(position, property, label);
                }
            }
            else
            {
                if (property.IsField)
                {
                    EditorGUILayout.PropertyField(property.Field, label, includeChildren, options);
                }
                else if (property.Type.IsSubclassOf(typeof(UnityEngine.Object)) || property.Type == typeof(UnityEngine.Object))
                {
                    // Draw default IMGUI GUI
                    UnityEngine.Object val = property.objectReferenceValue;
                    Rect position = EditorGUILayout.GetControlRect(true);
                    property.objectReferenceValue = EditorGUI.ObjectField(position, label, val, property.Type, true);
                }
                else
                {
                    // Draw read only text field
                    GUI.enabled = false;
                    Rect position = EditorGUILayout.GetControlRect(true);
                    EditorGUI.TextField(position, label, $"No property drawer found for {property.type}!");
                }
            }
            
            DrawAttributesAfter(property);
            
            // Reset GUI enabled
            GUI.enabled = enabledBefore;
            
            return false;
        }
        /// <summary>
        /// Make a field for SerializedProperty.
        /// </summary>
        /// <param name="property">The SuperSerializedProperty to make the field for.</param>
        /// <param name="includeChildren">If true the property including children is drawn; otherwise only the control itself (such as only a foldout but nothing below it).</param>
        /// <param name="options">An optional list of layout options that specify extra layout properties. Any values passed in here will override settings defined by the style.<br/> See Also GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight, GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
        /// <returns>True if the property has children and is expanded and includeChildren was set to false; otherwise false.</returns>
        public static bool PropertyField(SuperSerializedProperty property, bool includeChildren = true, params GUILayoutOption[] options)
        {
            return PropertyField(property, new GUIContent(property.displayName, property.tooltip), includeChildren, options);
        }
        /// <summary>
        /// Make a field for SerializedProperty.
        /// </summary>
        /// <param name="property">The SuperSerializedProperty to make the field for.</param>
        /// <param name="label">Optional label to use. If not specified the label of the property itself is used.<br/>Use GUIContent.none to not display a label at all.</param>
        /// <param name="options">An optional list of layout options that specify extra layout properties. Any values passed in here will override settings defined by the style.<br/> See Also GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight, GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
        /// <returns>True if the property has children and is expanded and includeChildren was set to false; otherwise false.</returns>
        public static bool PropertyField(SuperSerializedProperty property, GUIContent label, params GUILayoutOption[] options)
        {
            return PropertyField(property, label, true, options);
        }
        /// <summary>
        /// Make a field for SerializedProperty.
        /// </summary>
        /// <param name="property">The SuperSerializedProperty to make the field for.</param>
        /// <param name="includeChildren">If true the property including children is drawn; otherwise only the control itself (such as only a foldout but nothing below it).</param>
        /// <returns>True if the property has children and is expanded and includeChildren was set to false; otherwise false.</returns>
        public static bool PropertyField(SuperSerializedProperty property, params GUILayoutOption[] options)
        {
            return PropertyField(property, new GUIContent(property.displayName, property.tooltip), true, options);
        }
    
        /// <summary>
        /// Draw Space if the property has been marked with the SpaceAttribute.
        /// </summary>
        /// <param name="property">The SuperSerializedProperty to draw the space for.</param>
        /// <returns>True if space was drawn; otherwise false.</returns>
        public static bool DrawSpace(SuperSerializedProperty property, bool ignoreFieldProperties = false)
        {
            if (!property.Attributes.IsActive) return false;
            if (!property.Attributes.HasSpaces) return false;
            if (property.IsField && ignoreFieldProperties) return false; // Prevent double space
            foreach (PropertySpaceAttribute space in property.Attributes.Spcaes)
            {
                EditorGUILayout.Space(space.Height);
            }
            return true;
        }
    
        /// <summary>
        /// Draw headers for a property if it has any.
        /// </summary>
        /// <param name="property">The SuperSerializedProperty to draw the headers for.</param>
        /// <returns>True if headers were drawn; otherwise false.</returns>
        public static bool DrawHeaders(SuperSerializedProperty property, bool ignoreFieldProperties = false)
        {
            if (!property.Attributes.IsActive) return false;
            if (!property.Attributes.HasHeaders) return false;
            if (property.IsField && ignoreFieldProperties) return false; // Prevent double headers
            foreach (PropertyHeaderAttribute header in property.Attributes.Headers)
            {
                Rect rect = EditorGUILayout.GetControlRect(false, 26);
                rect.y += 8;
                EditorGUI.LabelField(rect, header.Header, EditorStyles.boldLabel);
            }
            return true;
        }
        
        /// <summary>
        /// Draw buttons for a property if it has any.
        /// </summary>
        /// <param name="property">The SuperSerializedProperty to draw the buttons for.</param>
        /// <param name="displayAfter">Whether to draw the buttons intended to be displayed after the property.</param>
        /// <returns>True if buttons were drawn; otherwise false.</returns>
        public static bool DrawButtons(SuperSerializedProperty property, bool displayAfter = false)
        {
            if (!property.Attributes.IsActive) return false;
            if (!property.Attributes.HasButtons) return false;
            foreach (ButtonAttribute button in property.Attributes.Buttons)
            {
                if (button.DisplayAfter != displayAfter) continue;
                if (GUILayout.Button(button.Text ?? "Do not press!"))
                {
                    if (button.IsMethod) continue;
                    property.InvokeMethod(button.MethodName);
                }
            }
            return true;
        }
    
        /// <summary>
        /// Draw Attributes for a method if it has any.
        /// </summary>
        /// <param name="serializedObject">The SerializedObject to draw the attributes for.</param>
        /// <param name="method">The MethodInfo to draw the attributes for.</param>
        /// <param name="displayAfter">Whether to draw the buttons intended to be displayed after the property.</param>
        /// <returns>True if attributes were drawn; otherwise false.</returns>
        public static bool DrawButtons(SuperSerializedObject serializedObject, MethodInfo method, bool displayAfter = false)
        {
            bool hasAttributes = false;
            foreach (ButtonAttribute button in method.GetCustomAttributes<ButtonAttribute>())
            {
                if (button.DisplayAfter != displayAfter) continue;
                hasAttributes = true;
                if (GUILayout.Button(button.Text ?? method.Name))
                {
                    serializedObject.InvokeMethod(method.Name);
                }
            }
            if (!displayAfter)
            {
                foreach (ContextMenu contextMenu in method.GetCustomAttributes<ContextMenu>())
                {
                    hasAttributes = true;
                    if (GUILayout.Button(contextMenu.menuItem))
                    {
                        serializedObject.InvokeMethod(method.Name);
                    }
                }
            }
            return hasAttributes;
        }
        
        /// <summary>
        /// Draw Attributes for a property if it has any. (Intended to be rendered before the property)
        /// </summary>
        /// <param name="property">The SuperSerializedProperty to draw the attributes for.</param>
        /// <returns>True if attributes were drawn; otherwise false.</returns>
        public static bool DrawAttributesBefore(SuperSerializedProperty property, bool ignoreFieldProperties = false)
        {
            return DrawSpace(property, ignoreFieldProperties)
                | DrawHeaders(property, ignoreFieldProperties)
                | DrawButtons(property);
        }
        
        /// <summary>
        /// Draw Attributes for a property if it has any. (Intended to be rendered after the property)
        /// </summary>
        /// <param name="property">The SuperSerializedProperty to draw the attributes for.</param>
        /// <returns>True if attributes were drawn; otherwise false.</returns>
        public static bool DrawAttributesAfter(SuperSerializedProperty property)
        {
            return DrawButtons(property, displayAfter: true);
        }
    }
}