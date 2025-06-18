using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using LoM.Super.Serialization;
    
namespace LoM.Super.Editor.Drawer
{
    [CustomSuperPropertyDrawer(typeof(double))]
    public class DoubleDrawer : SuperPropertyDrawer
    {
        public override void OnGUI(Rect position, SuperSerializedProperty property, GUIContent label)
        {
            EditorGUI.showMixedValue = MixModeEnabled(property);
            
            double value = EditorGUI.DoubleField(position, label, property.doubleValue);
            if (value != property.doubleValue) property.doubleValue = value;
            
            EditorGUI.showMixedValue = false;
        }
        
        private bool MixModeEnabled(SuperSerializedProperty property)
        {
            if (property.superSerializedObject.isEditingMultipleObjects)
            {
                double[] values = property.superSerializedObject.FindAllPropertiesByPath(property.propertyPath).Select(p => p.doubleValueSingle).ToArray();
                return values.Any(v => v != values[0]);
            }
            return false;
        }
    }
}