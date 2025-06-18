using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using LoM.Super.Serialization;
using System;

namespace LoM.Super.Editor.Drawer
{
    [CustomSuperPropertyDrawer(typeof(Enum))]
    public class EnumDrawer : SuperPropertyDrawer
    {
        // Static Variables
        private static GUIStyle s_style_tab_inactive;
        private static GUIStyle s_style_tab_active;
        private static Color m_backgroundColor = new Color(88f / 255f, 88f / 255f, 88f / 255f);
        
        // Static Getters
        public static GUIStyle StyleTabInactive
        {
            get
            {
                if (s_style_tab_inactive != null) return s_style_tab_inactive;
                GUIStyle inactive = new GUIStyle(EditorStyles.label);
                inactive.alignment = TextAnchor.MiddleCenter;
                inactive.padding = new RectOffset(3, 3, 0, 2);
                #if UNITY_6000_0_OR_NEWER
                inactive.clipping = TextClipping.Ellipsis;
                #endif
                s_style_tab_inactive = inactive;
                return s_style_tab_inactive;
            }
        }
        public static GUIStyle StyleTabActive
        {
            get
            {
                if (s_style_tab_active != null) return s_style_tab_active;
                GUIStyle active = new GUIStyle(StyleTabInactive);
                string p = "Assets/Plugins/LoM/Super/Images/TabBackground.png";
                Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(p);
                active.normal.background = texture;
                active.hover.background = texture;
                active.active.background = texture;
                s_style_tab_active = active;
                return s_style_tab_active;
            }
        }
        public static Color BackgroundColor => m_backgroundColor;
        
        // GUI Draw
        public override void OnGUI(Rect position, SuperSerializedProperty property, GUIContent label)
        {            
            // Check if has Flag attribute
            bool isFlag = property.Type.GetCustomAttributes(typeof(FlagsAttribute), false).Length > 0;
            bool isTabs = property.Type.GetCustomAttributes(typeof(TabsAttribute), false).Length > 0;
            
            if (isFlag)
            {
                // Draw flag enum
                EditorGUI.showMixedValue = MixModeEnabledFlag(property);
                EditorGUI.BeginChangeCheck();
                Enum enumValue = (Enum)Enum.ToObject(property.Type, property.enumValueFlag);
                enumValue = EditorGUI.EnumFlagsField(position, label, enumValue);
                if (EditorGUI.EndChangeCheck())
                {
                    property.enumValueFlag = Convert.ToInt32(enumValue);
                }
            }
            else if (isTabs)
            {
                EditorGUI.showMixedValue = false;
                // Draw tabs enum
                EditorGUI.BeginChangeCheck();
                var values = Enum.GetValues(property.Type);
                Rect rect = EditorGUILayout.GetControlRect(false, EditorGUIUtility.singleLineHeight);
                float width = rect.width / values.Length;
                rect.width = width;
                foreach (Enum value in values)
                {
                    bool isSelected = property.intValue == Convert.ToInt32(value);
                    if (GUI.Button(rect, value.ToString(), isSelected ? StyleTabActive : StyleTabInactive))
                    {
                        property.intValue = Convert.ToInt32(value);
                    }
                    rect.x += width;
                }
                rect.x -= width * values.Length;
                rect.y += EditorGUIUtility.singleLineHeight - 2;
                rect.width *= values.Length;
                rect.height = 2;
                EditorGUI.DrawRect(rect, m_backgroundColor);
                if (EditorGUI.EndChangeCheck())
                {
                    property.intValue = Convert.ToInt32(property.intValue);
                }
            }
            else
            {
                // Draw normal enum
                EditorGUI.showMixedValue = MixModeEnabledEnum(property);
                EditorGUI.BeginChangeCheck();
                Enum enumValue = (Enum)Enum.ToObject(property.Type, property.intValue);
                enumValue = EditorGUI.EnumPopup(position, label, enumValue);
                if (EditorGUI.EndChangeCheck())
                {
                    property.intValue = Convert.ToInt32(enumValue);
                }
            }
            
            EditorGUI.showMixedValue = false;
        }
        
        private bool MixModeEnabledEnum(SuperSerializedProperty property)
        {
            if (property.superSerializedObject.isEditingMultipleObjects)
            {
                int[] values = property.superSerializedObject.FindAllPropertiesByPath(property.propertyPath).Select(p => p.enumValueIndexSingle).ToArray();
                return values.Any(v => v != values[0]);
            }
            return false;
        }
        
        private bool MixModeEnabledFlag(SuperSerializedProperty property)
        {
            if (property.superSerializedObject.isEditingMultipleObjects)
            {
                int[] values = property.superSerializedObject.FindAllPropertiesByPath(property.propertyPath).Select(p => p.enumValueFlagSingle).ToArray();
                return values.Any(v => v != values[0]);
            }
            return false;
        }
    }
}