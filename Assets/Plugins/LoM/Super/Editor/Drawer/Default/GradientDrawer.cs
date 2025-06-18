using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using LoM.Super.Serialization;

namespace LoM.Super.Editor.Drawer
{
    [CustomSuperPropertyDrawer(typeof(Gradient))]
    public class GradientDrawer : SuperPropertyDrawer
    {
        public override void OnGUI(Rect position, SuperSerializedProperty property, GUIContent label)
        {
            EditorGUI.showMixedValue = MixModeEnabled(property);
            
            Gradient value = EditorGUI.GradientField(position, label, property.gradientValue);
            if (value != property.gradientValue) property.gradientValue = value;
            
            EditorGUI.showMixedValue = false;
        }
        
        private bool MixModeEnabled(SuperSerializedProperty property)
        {
            if (property.superSerializedObject.isEditingMultipleObjects)
            {
                Gradient[] values = property.superSerializedObject.FindAllPropertiesByPath(property.propertyPath).Select(p => p.gradientValueSingle).ToArray();
                return values.Any(v => !AreGradientsEqual(v, values[0]));
            }
            return false;
        }
        
        private static bool AreGradientsEqual(Gradient a, Gradient b)
        {
            if (a == null && b == null) return true;
            if (a == null || b == null) return false;
            if (a.colorKeys.Length != b.colorKeys.Length) return false;
            if (a.alphaKeys.Length != b.alphaKeys.Length) return false;
            for (int i = 0; i < a.colorKeys.Length; i++)
            {
                if (a.colorKeys[i].color != b.colorKeys[i].color) return false;
                if (a.colorKeys[i].time != b.colorKeys[i].time) return false;
            }
            for (int i = 0; i < a.alphaKeys.Length; i++)
            {
                if (a.alphaKeys[i].alpha != b.alphaKeys[i].alpha) return false;
                if (a.alphaKeys[i].time != b.alphaKeys[i].time) return false;
            }
            return true;
        }
    }
}