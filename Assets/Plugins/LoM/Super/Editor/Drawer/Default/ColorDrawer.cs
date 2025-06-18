using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using LoM.Super.Serialization;

namespace LoM.Super.Editor.Drawer
{
    [CustomSuperPropertyDrawer(typeof(Color))]
    [CustomSuperPropertyDrawer(typeof(Color32))]
    public class ColorDrawer : SuperPropertyDrawer
    {
        public override void OnGUI(Rect position, SuperSerializedProperty property, GUIContent label)
        {
            EditorGUI.showMixedValue = MixModeEnabled(property);
            
            Color value = EditorGUI.ColorField(position, label, property.colorValue);
            if (property.colorValue != value) property.colorValue = value;
            
            if (EditorGUI.showMixedValue && property.Attributes.IsHDRColor)
            {
                if (label != GUIContent.none)
                {
                    position.x += EditorGUIUtility.labelWidth;
                    position.width -= EditorGUIUtility.labelWidth;
                }
                position.y += 2;
                position.height -= 3;
                position.x += 2;
                position.width -= 3;
                EditorGUI.LabelField(position, " —", EditorStyles.label.Variant("ColorDrawer_FakeMixedValue", style => {
                    style.alignment = TextAnchor.MiddleLeft;
                    style.normal.textColor = new Color(107f / 255f, 107f / 255f, 107f / 255f, 1f);
                    style.normal.background = new Texture2D(1, 1);
                }));
            }
            
            EditorGUI.showMixedValue = false;
        }
        
        private bool MixModeEnabled(SuperSerializedProperty property)
        {
            if (property.superSerializedObject.isEditingMultipleObjects)
            {
                Color[] values = property.superSerializedObject.FindAllPropertiesByPath(property.propertyPath).Select(p => p.colorValueSingle).ToArray();
                return values.Any(v => v != values[0]);
            }
            return false;
        }
    }
}