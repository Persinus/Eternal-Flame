using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using LoM.Super.Serialization;

namespace LoM.Super.Editor.Drawer
{
    [CustomSuperPropertyDrawer(typeof(BoundsInt))]
    public class BoundsIntDrawer : SuperPropertyDrawer
    {
        public override float GetPropertyHeight(SuperSerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight * 3 + 5;
        }
        
        public override void OnGUI(Rect position, SuperSerializedProperty property, GUIContent label)
        {
            EditorGUI.showMixedValue = MixModeEnabled(property);
            
            BoundsInt value = EditorGUI.BoundsIntField(position, label, property.boundsIntValue);
            if (value != property.boundsIntValue) property.boundsIntValue = value;
            
            EditorGUI.showMixedValue = false;
        }
        
        private bool MixModeEnabled(SuperSerializedProperty property)
        {
            if (property.superSerializedObject.isEditingMultipleObjects)
            {
                BoundsInt[] values = property.superSerializedObject.FindAllPropertiesByPath(property.propertyPath).Select(p => p.boundsIntValueSingle).ToArray();
                return values.Any(v => v != values[0]);
            }
            return false;
        }
    }
}