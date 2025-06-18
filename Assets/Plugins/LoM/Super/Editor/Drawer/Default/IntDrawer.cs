using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using LoM.Super.Serialization;
    
namespace LoM.Super.Editor.Drawer
{
    [CustomSuperPropertyDrawer(typeof(sbyte))]
    [CustomSuperPropertyDrawer(typeof(byte))]
    [CustomSuperPropertyDrawer(typeof(short))]
    [CustomSuperPropertyDrawer(typeof(ushort))]
    [CustomSuperPropertyDrawer(typeof(int))]
    [CustomSuperPropertyDrawer(typeof(uint))]
    public class IntDrawer : SuperPropertyDrawer
    {
        public override void OnGUI(Rect position, SuperSerializedProperty property, GUIContent label)
        {
            EditorGUI.showMixedValue = MixModeEnabled(property);
            
            if (property.Attributes.IsRange)
            {
                int value = EditorGUI.IntSlider(position, label, property.intValue, (int)property.Attributes.Range.Min, (int)property.Attributes.Range.Max);
                if (value != property.intValue) property.intValue = value;
            }
            else if (property.Attributes.IsLayer)
            {
                int value = EditorGUI.LayerField(position, label, property.intValue);
                if (value != property.intValue) property.intValue = value;
            }
            else 
            {
                int value = EditorGUI.IntField(position, label, property.intValue);
                if (value != property.intValue) property.intValue = value;
            }
            
            EditorGUI.showMixedValue = false;
        }
        
        private bool MixModeEnabled(SuperSerializedProperty property)
        {
            if (property.superSerializedObject.isEditingMultipleObjects)
            {
                int[] values = property.superSerializedObject.FindAllPropertiesByPath(property.propertyPath).Select(p => p.intValueSingle).ToArray();
                return values.Any(v => v != values[0]);
            }
            return false;
        }
    }
}