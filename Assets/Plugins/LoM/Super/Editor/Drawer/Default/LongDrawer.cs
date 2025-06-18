using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using LoM.Super.Serialization;
    
namespace LoM.Super.Editor.Drawer
{
    [CustomSuperPropertyDrawer(typeof(ulong))]
    [CustomSuperPropertyDrawer(typeof(long))]
    public class LongDrawer : SuperPropertyDrawer
    {
        public override void OnGUI(Rect position, SuperSerializedProperty property, GUIContent label)
        {
            EditorGUI.showMixedValue = MixModeEnabled(property);
            
            long value = EditorGUI.LongField(position, label, property.longValue);
            if (property.longValue != value) property.longValue = value;
            
            EditorGUI.showMixedValue = false;
        }
        
        private bool MixModeEnabled(SuperSerializedProperty property)
        {
            if (property.superSerializedObject.isEditingMultipleObjects)
            {
                long[] values = property.superSerializedObject.FindAllPropertiesByPath(property.propertyPath).Select(p => p.longValueSingle).ToArray();
                return values.Any(v => v != values[0]);
            }
            return false;
        }
    }
}