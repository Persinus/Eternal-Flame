# if UNITY_EDITOR
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using LoM.Super.Serialization;
using UnityEditorInternal;

namespace LoM.Super.Editor.Drawer
{
    [CustomSuperPropertyDrawer(typeof(LayerMask))]
    public class LayerMaskDrawer : SuperPropertyDrawer
    {
        public override void OnGUI(Rect position, SuperSerializedProperty property, GUIContent label)
        {
            EditorGUI.showMixedValue = MixModeEnabled(property);
            
            LayerMask value = EditorGUI.MaskField(position, label, property.intValue, InternalEditorUtility.layers);
            if (value != property.intValue) property.intValue = value;
            
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
# endif