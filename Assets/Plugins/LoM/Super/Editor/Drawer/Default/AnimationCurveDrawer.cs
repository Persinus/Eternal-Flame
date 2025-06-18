using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using LoM.Super.Serialization;
using UnityEngine.UI;

namespace LoM.Super.Editor.Drawer
{
    [CustomSuperPropertyDrawer(typeof(AnimationCurve))]
    public class AnimationCurveDrawer : SuperPropertyDrawer
    {
        private static Texture2D s_transparentBackground;
        private static Texture2D TransparentBackground
        {
            get
            {
                if (s_transparentBackground == null)
                {
                    s_transparentBackground = new Texture2D(1, 1);
                    s_transparentBackground.SetPixel(0, 0, new Color(42f / 255f, 42f / 255f, 42f / 255f, 0.85f));
                    s_transparentBackground.Apply();
                }
                return s_transparentBackground;
            }
        }
        
        public override void OnGUI(Rect position, SuperSerializedProperty property, GUIContent label)
        {
            EditorGUI.showMixedValue = MixModeEnabled(property);
            
            AnimationCurve value = EditorGUI.CurveField(position, label, property.animationCurveValue);
            if (value != property.animationCurveValue) property.animationCurveValue = value;
            
            if (EditorGUI.showMixedValue)
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
                EditorGUI.LabelField(position, " —", EditorStyles.label.Variant("AnimationCurveDrawer_FakeMixedValue", style => {
                    style.alignment = TextAnchor.MiddleLeft;
                    style.normal.textColor = new Color(107f / 255f, 107f / 255f, 107f / 255f, 1f);
                    style.normal.background = TransparentBackground;
                }));
            }
            
            EditorGUI.showMixedValue = false;
        }
        
        private bool MixModeEnabled(SuperSerializedProperty property)
        {
            if (property.superSerializedObject.isEditingMultipleObjects)
            {
                AnimationCurve[] values = property.superSerializedObject.FindAllPropertiesByPath(property.propertyPath).Select(p => p.animationCurveValueSingle).ToArray();
                return values.Any(v => !AreCurvesEqual(v, values[0]));
            }
            return false;
        }
        
        // Helper: Compare two animation curves
        private static bool AreCurvesEqual(AnimationCurve curve1, AnimationCurve curve2)
        {
            if (curve1 == null && curve2 == null) return true;
            if (curve1 == null || curve2 == null) return false;
            if (curve1.length != curve2.length) return false;
            for (int i = 0; i < curve1.length; i++)
            {
                if (!AreKeysEqual(curve1[i], curve2[i])) return false;
            }
            if (curve1.preWrapMode != curve2.preWrapMode || curve1.postWrapMode != curve2.postWrapMode) return false;
            return true;
        }
        private static bool AreKeysEqual(Keyframe key1, Keyframe key2)
        {
            // Compare time, value, inTangent, outTangent, and weighted properties if needed
            return Mathf.Approximately(key1.time, key2.time) &&
                Mathf.Approximately(key1.value, key2.value) &&
                Mathf.Approximately(key1.inTangent, key2.inTangent) &&
                Mathf.Approximately(key1.outTangent, key2.outTangent) &&
                key1.weightedMode == key2.weightedMode &&
                Mathf.Approximately(key1.inWeight, key2.inWeight) &&
                Mathf.Approximately(key1.outWeight, key2.outWeight);
        }
    }
}