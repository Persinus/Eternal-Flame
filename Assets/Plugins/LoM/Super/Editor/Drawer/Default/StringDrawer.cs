using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using LoM.Super.Serialization;
using System;

namespace LoM.Super.Editor.Drawer
{
    [CustomSuperPropertyDrawer(typeof(string))]
    public class StringDrawer : SuperPropertyDrawer
    {
        public override void OnGUI(Rect position, SuperSerializedProperty property, GUIContent label)
        {
            EditorGUI.showMixedValue = MixModeEnabled(property);
            
            if (property.Attributes.IsTag)
            {
                string value = EditorGUI.TagField(position, label, property.stringValue);
                if (value != property.stringValue) property.stringValue = value;
            }
            else if (property.Attributes.IsTextArea)
            {
                // Label
                if (!string.IsNullOrEmpty(property.Attributes.Label))
                {
                    Rect labelRect = position;
                    labelRect.height = EditorGUIUtility.singleLineHeight;
                    EditorGUI.LabelField(labelRect, label);
                    position.y += EditorGUIUtility.singleLineHeight;
                }
                
                int minLines = property.Attributes.TextArea.MinLines;
                int maxLines = property.Attributes.TextArea.MinLines > property.Attributes.TextArea.MaxLines ? property.Attributes.TextArea.MinLines : property.Attributes.TextArea.MaxLines;
                string value = EditorGUILayout.TextArea(property.stringValue, GUILayout.MinHeight(minLines * EditorGUIUtility.singleLineHeight), GUILayout.MaxHeight(maxLines * EditorGUIUtility.singleLineHeight));
                if (value != property.stringValue) property.stringValue = value;
            }
            else
            {
                string value = EditorGUI.TextField(position, label, property.stringValue);
                if (value != property.stringValue) property.stringValue = value;
            }
            
            EditorGUI.showMixedValue = false;
        }
        
        private bool MixModeEnabled(SuperSerializedProperty property)
        {
            if (property.superSerializedObject.isEditingMultipleObjects)
            {
                string[] values = property.superSerializedObject.FindAllPropertiesByPath(property.propertyPath).Select(p => p.stringValueSingle).ToArray();
                return values.Any(v => v != values[0]);
            }
            return false;
        }
    }
}