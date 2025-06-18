using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using LoM.Super.Serialization;

namespace LoM.Super.Editor.Drawer
{
    [CustomSuperPropertyDrawer(typeof(float))]
    public class FloatDrawer : SuperPropertyDrawer
    {
        public override void OnGUI(Rect position, SuperSerializedProperty property, GUIContent label)
        {
            EditorGUI.showMixedValue = MixModeEnabled(property);
            
            float value = EditorGUI.FloatField(position, label, property.floatValue);
            if (value != property.floatValue) property.floatValue = value;
            
            EditorGUI.showMixedValue = false;
        }
        
        private bool MixModeEnabled(SuperSerializedProperty property)
        {
            if (property.superSerializedObject.isEditingMultipleObjects)
            {
                float[] values = property.superSerializedObject.FindAllPropertiesByPath(property.propertyPath).Select(p => p.floatValueSingle).ToArray();
                return values.Any(v => v != values[0]);
            }
            return false;
        }
    }
}