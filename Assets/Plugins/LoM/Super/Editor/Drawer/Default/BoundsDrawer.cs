using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using LoM.Super.Serialization;

namespace LoM.Super.Editor.Drawer
{
    [CustomSuperPropertyDrawer(typeof(Bounds))]
    public class BoundsDrawer : SuperPropertyDrawer
    {
        public override float GetPropertyHeight(SuperSerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight * 3 + 5;
        }
        
        public override void OnGUI(Rect position, SuperSerializedProperty property, GUIContent label)
        {
            EditorGUI.showMixedValue = MixModeEnabled(property);
            
            Bounds value = EditorGUI.BoundsField(position, label, property.boundsValue);
            if (value != property.boundsValue) property.boundsValue = value;
            
            EditorGUI.showMixedValue = false;
        }
        
        private bool MixModeEnabled(SuperSerializedProperty property)
        {
            if (property.superSerializedObject.isEditingMultipleObjects)
            {
                Bounds[] values = property.superSerializedObject.FindAllPropertiesByPath(property.propertyPath).Select(p => p.boundsValueSingle).ToArray();
                return values.Any(v => v != values[0]);
            }
            return false;
        }
    }
}