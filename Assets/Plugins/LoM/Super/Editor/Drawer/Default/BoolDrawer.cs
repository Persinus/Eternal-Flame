using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using LoM.Super.Serialization;

namespace LoM.Super.Editor.Drawer
{
    [CustomSuperPropertyDrawer(typeof(bool))]
    public class BoolDrawer : SuperPropertyDrawer
    {
        public override void OnGUI(Rect position, SuperSerializedProperty property, GUIContent label)
        {
            EditorGUI.showMixedValue = MixModeEnabled(property);
            
            bool value = EditorGUI.Toggle(position, label, property.boolValue);
            if (!property.Attributes.IsReadOnly) property.boolValue = value;
            
            EditorGUI.showMixedValue = false;
        }
        
        private bool MixModeEnabled(SuperSerializedProperty property)
        {
            if (property.superSerializedObject.isEditingMultipleObjects)
            {
                bool[] values = property.superSerializedObject.FindAllPropertiesByPath(property.propertyPath).Select(p => p.boolValueSingle).ToArray();
                return values.Any(v => v != values[0]);
            }
            return false;
        }
    }
}