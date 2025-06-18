using System.Collections.Generic;
using System.Linq; 
using System.Reflection;
using UnityEngine;
using UnityEditor;
using System;
using LoM.Super.Serialization;
using LoM.Super.Internal;

namespace LoM.Super.Editor
{
    /// <summary>
    /// Derive from this base class to create a custom inspector or editor for your SuperBehaviour object.
    /// </summary>
    public abstract class SuperEditor<T> : UnityEditor.Editor where T : SuperBehaviour
    {        
        // Static Variables
        private static Color s_controlBackgroundColor = new Color(48f / 255f, 48f / 255f, 48f / 255f);
        private static Color s_toolbarBackgroundColor = new Color(63f / 255f, 63f / 255f, 63f / 255f);
        
        // Member Variables
        private SuperSerializedObject m_serializedObject;
        private SuperBehaviourIcon m_lastIconAssigned = SuperBehaviourIcon.None;
        private Texture2D m_smellIcon;
        
        /// <summary>
        /// The serialized object of this editor.<br/>
        /// <i>Use this to access the advance properties of the serialized object.</i>
        /// </summary>
        public SuperSerializedObject superSerializedObject
        {
            get
            {
                if (m_serializedObject == null) 
                {
                    if (targets.Length == 1)
                    { 
                        m_serializedObject = new SuperSerializedObject(target);
                    }
                    else
                    {
                        m_serializedObject = new SuperSerializedObject(targets);
                    }
                }
                return m_serializedObject;
            }
        }
        
        /// <summary>
        /// The target object of this editor
        /// </summary>
        public new T target => base.target as T;
        
        /// <summary>
        /// The target objects of this editor
        /// </summary>
        public new T[] targets => base.targets.Cast<T>().ToArray();
        
        /// <summary>
        /// Returns if script should be displayed in inspector
        /// </summary>
        public virtual bool ShowScript() => true;
        
        /// <summary>
        /// Override OnInspectorFieldsGUI or OnInspectorPropertiesGUI instead
        /// </summary>
        public sealed override void OnInspectorGUI()
        {
            UpdateIcon();
            OnInspectorFieldsGUI();
            OnInspectorPropertiesGUI();
            DrawToolboxAfter();
        }
        
        // On Enable
        private void OnEnable()
        {
            if (targets.Length == 1)
            { 
                m_serializedObject = new SuperSerializedObject(target);
            }
            else
            {
                m_serializedObject = new SuperSerializedObject(targets);
            }
            UpdateIcon();
            AfterOnEnable();
        }
        
        /// <summary>
        /// Implement this function to use the OnEnable function.
        /// </summary>
        protected virtual void AfterOnEnable() {}
        
        // On Disable
        private void OnDisable()
        {
            m_serializedObject?.Dispose();
            AfterOnDisable();
        }
        
        /// <summary>
        /// Implement this function to use the OnDisable function.
        /// </summary>
        protected virtual void AfterOnDisable() {}
        
        /// <summary>
        /// Implement this function to make a custom inspector.
        /// </summary>
        public virtual void OnInspectorFieldsGUI()
        {
            DrawDefaultInspector();
        }
        
        // <summar>
        // Implement this function to draw Handles in the Scene View.
        // </summary>
        public virtual void OnSceneGUI() 
        {
            EditorGUI.BeginChangeCheck();
            foreach (SuperSerializedProperty field in m_serializedObject.Fields)
            {
                SuperPropertyDrawerUtility.Instance[field]?.OnSceneGUI(field);
            }
            if (EditorGUI.EndChangeCheck())
            {
                m_serializedObject.ApplyModifiedProperties();
            }
        }
        
        /// <summary>
        /// Implement this function to override the properties inspector.
        /// </summary>
        public new void DrawDefaultInspector()
        {
            // Preview the script
            if (ShowScript())
            {
                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour(target), typeof(MonoScript), false);
                EditorGUI.EndDisabledGroup();
            }
            
            DrawToolboxBefore();
            
            // Draw all fields
            m_serializedObject.Update();
            foreach (SuperSerializedProperty field in m_serializedObject.Fields)
            {
                if (!field.Attributes.IsActive) continue;
                if (field.Attributes.IsHidden) continue;
                
                bool enabledBefore = GUI.enabled;
                if (field.Attributes.IsReadOnly) GUI.enabled = false;
                
                if (field.Attributes.IsLayer 
                 || field.Attributes.IsTag 
                 || field.Attributes.IsTab
                 || field.Attributes.IsHDRColor)
                {
                    DrawImprovedFields(field);
                    GUI.enabled = enabledBefore;
                    continue;
                }
                
                EditorSuperGUILayout.PropertyField(field);
                GUI.enabled = enabledBefore;
            }
            
            // Draw Code Smell Summary
            DrawCodeSmellSummary();
            
            // Apply changes
            if (GUI.changed)
            {
                m_serializedObject.ApplyModifiedProperties();
            }
        }
        
        /// <summary>
        /// Implement this function to override the properties inspector.
        /// </summary>
        public virtual void OnInspectorPropertiesGUI()
        {
            if (m_serializedObject.Properties.Length == 0) return;
            
            // Draw separator
            Rect rect = EditorGUILayout.GetControlRect(false, 16);
            rect.xMin = 0;
            rect.width += 15;
            rect.height += 4;
            rect.y += EditorGUIUtility.standardVerticalSpacing * 3;
            EditorGUI.DrawRect(rect, s_controlBackgroundColor);
            
            // Draw Collapasable Label
            rect.x += 18;
            target.ShowProperties = EditorGUI.Foldout(rect, target.ShowProperties, "Properties", true, EditorStyles.foldout.Variant("SuperEditor_Foldout", style => {
                style.normal.background = null;
                style.normal.textColor = Color.gray;
                style.onNormal.textColor = Color.gray;
            }));
            if (!target.ShowProperties) return;
            
            // Fix height
            rect = EditorGUILayout.GetControlRect(false, 13);
            
            // Draw all properties
            m_serializedObject.Update();
            foreach (SuperSerializedProperty property in m_serializedObject.Properties)
            {                
                if (!property.Attributes.IsActive) continue;
                if (property.Attributes.IsHidden) continue;
                bool enabledBefore = GUI.enabled;
                if (property.Attributes.IsReadOnly || !Application.isPlaying || m_serializedObject.isEditingMultipleObjects) GUI.enabled = false;
                EditorSuperGUILayout.PropertyField(property);
                GUI.enabled = enabledBefore;
            }
        }
        
        // DrawImprovedFields
        private void DrawImprovedFields(SuperSerializedProperty field)
        {
            if (!field.IsField) return;
            
            // Draw Attributes
            EditorSuperGUILayout.DrawAttributesBefore(field);
            
            // Label
            GUIContent label = new GUIContent(field.displayName, field.tooltip);
            
            // Layer
            if (field.Attributes.IsLayer)
            {
                EditorGUI.showMixedValue = MixModeEnabledLayer(field);
                int layer = field.intValue;
                int newLayer = EditorGUILayout.LayerField(label, layer);
                if (!field.Attributes.IsReadOnly && layer != newLayer) 
                {
                    field.intValue = newLayer;
                    EditorUtility.SetDirty(target);
                }
            }
            
            // Tag
            else if (field.Attributes.IsTag)
            {
                EditorGUI.showMixedValue = MixModeEnabledTag(field);
                string tag = field.stringValue;
                string newTag = EditorGUILayout.TagField(label, tag);
                if (!field.Attributes.IsReadOnly && tag != newTag) 
                {
                    field.stringValue = newTag;
                    EditorUtility.SetDirty(target);
                }
            }
            
            // HDR Color
            else if (field.Attributes.IsHDRColor)
            {
                EditorGUI.showMixedValue = MixModeEnabledHDRColor(field);
                Color color = field.colorValue;
                Color newColor = EditorGUILayout.ColorField(label, color);
                if (!field.Attributes.IsReadOnly && color != newColor) 
                {
                    field.colorValue = newColor;
                    EditorUtility.SetDirty(target);
                }
            }
            
            // Tabs
            else if (field.Attributes.IsTab)
            {
                EditorGUI.showMixedValue = false;
                EditorGUI.BeginChangeCheck();
                var values = Enum.GetValues(field.Type);
                Rect rect = EditorGUILayout.GetControlRect(false, EditorGUIUtility.singleLineHeight);
                float width = rect.width / values.Length;
                rect.width = width;
                foreach (Enum value in values)
                {
                    bool isSelected = field.intValue == Convert.ToInt32(value);
                    if (GUI.Button(rect, value.ToString(), isSelected ? Drawer.EnumDrawer.StyleTabActive : Drawer.EnumDrawer.StyleTabInactive))
                    {
                        field.intValue = Convert.ToInt32(value);
                    }
                    rect.x += width;
                }
                rect.x -= width * values.Length;
                rect.y += EditorGUIUtility.singleLineHeight - 2;
                rect.width *= values.Length;
                rect.height = 2;
                EditorGUI.DrawRect(rect, Drawer.EnumDrawer.BackgroundColor);
                if (EditorGUI.EndChangeCheck())
                {
                    field.intValue = Convert.ToInt32(field.intValue);
                    EditorUtility.SetDirty(target);
                }
            }
            
            // Draw After Attributes
            EditorSuperGUILayout.DrawAttributesAfter(field);
            
            EditorGUI.showMixedValue = false;
        }
        
        // Draw Code Smell Summary
        private void DrawCodeSmellSummary()
        {
            if (!EditorPrefs.GetBool("SuperBehaviour.CodeSmells.Enabled", true)) return;
            if (!EditorPrefs.GetBool("SuperBehaviour.CodeSmells.Highlight", true)) return;
            if (!m_smellIcon) m_smellIcon = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Plugins/LoM/Super/Icons/CodeSmell.png");
            
            List<(string name, string explanation, int width, int height)> smells = new(); 
            
            // Check for public fields
            if (m_serializedObject.Fields.Any(f => f.IsField && ((FieldInfo)f.Info).IsPublic))
            {
                string description = "Avoid using public fields in your scripts. This may cause issues with encapsulation resulting in unexpected behavior and security vulnerabilities.";
                description = @"A Public Field has been detected in your code. While public fields may seem convenient, they can lead to issues in larger, scalable projects. Here’s why:

<b><size=150%>Why It's a Code Smell</size></b>

- <indent=15px><b>Encapsulation Violation</b>:
Public fields expose internal data directly, breaking the principle of encapsulation. This makes it harder to maintain and refactor code safely.</indent=15px>

- <indent=15px><b>Uncontrolled Access</b>: 
Any external class can directly read or modify the value without restrictions, potentially leading to unexpected behavior or bugs.</indent=15px>

<b><size=150%>Recommended Fix</size></b>
Use properties or private fields with [SerializeField] instead:

- <indent=15px><b>Properties</b>: 
Allow controlled access and validation logic.</indent=15px>

- <indent=15px><b>SerializedField</b>: 
Keeps fields private while still exposing them in the Unity Editor for designers.</indent=15px>";
                foreach (SuperSerializedProperty field in m_serializedObject.Fields)
                {
                    if (!field.IsField) continue;
                    if (!((FieldInfo)field.Info).IsPublic) continue;
                    smells.Add(($"Public Field: {field.displayName}", description, 400, 200));
                }
            }
            
            if (smells.Count == 0) return;
            
            // Draw Header
            Rect rect = EditorGUILayout.GetControlRect(false, EditorGUIUtility.singleLineHeight * 3);
            rect.y += EditorGUIUtility.singleLineHeight * 0.5f;
            float rectX = rect.xMin;
            rect.xMin = 0;
            rect.width = EditorGUIUtility.currentViewWidth;
            rect.height = EditorGUIUtility.singleLineHeight;
            EditorGUI.DrawRect(rect, new Color(140f / 255f, 101f / 255f, 59f / 255f));
            rect.xMin = rectX;
            rect.width = EditorGUIUtility.currentViewWidth - 32 - EditorGUIUtility.standardVerticalSpacing * 2;
            rect.height = EditorGUIUtility.singleLineHeight;
            EditorGUI.LabelField(rect, "Code Smell Detected", EditorStyles.boldLabel);
            rect.y += EditorGUIUtility.singleLineHeight;
            EditorGUI.LabelField(rect, "Disable Code Smells in SuperBehaviour Preferences to hide this warning.", EditorStyles.miniLabel);
            
            foreach ((string name, string explanation, int width, int height) in smells)
            {
                Rect smellRect = EditorGUILayout.GetControlRect(false, EditorGUIUtility.singleLineHeight);
                
                float iconSize = EditorGUIUtility.singleLineHeight;
                float buttonSize = 50;
                
                smellRect.y -= 1;
                smellRect.xMin = rectX;
                smellRect.width = iconSize - 4;
                smellRect.height = iconSize - 4;
                GUI.DrawTexture(smellRect, m_smellIcon);
                
                smellRect.y += 1;
                smellRect.x += iconSize + EditorGUIUtility.standardVerticalSpacing;
                smellRect.width = EditorGUIUtility.currentViewWidth - iconSize - buttonSize - rectX - EditorGUIUtility.standardVerticalSpacing * 2;
                EditorGUI.LabelField(smellRect, name, EditorStyles.boldLabel);
                
                smellRect.x = EditorGUIUtility.currentViewWidth - buttonSize - EditorGUIUtility.standardVerticalSpacing;
                smellRect.width = buttonSize;
                if (GUI.Button(smellRect, "More", EditorStyles.miniButton))
                {
                    RichtextDialog.Show(name, explanation, width, height);
                }
            }
        }
        
        // Helper: Get if mix mode is enabled for Layer
        private bool MixModeEnabledLayer(SuperSerializedProperty property)
        {
            if (property.superSerializedObject.isEditingMultipleObjects)
            {
                int[] values = property.superSerializedObject.FindAllPropertiesByPath(property.propertyPath).Select(p => p.intValueSingle).ToArray();
                return values.Any(v => v != values[0]);
            }
            return false;
        }
        
        // Helper: Get if mix mode is enabled for Tag
        private bool MixModeEnabledTag(SuperSerializedProperty property)
        {
            if (property.superSerializedObject.isEditingMultipleObjects)
            {
                string[] values = property.superSerializedObject.FindAllPropertiesByPath(property.propertyPath).Select(p => p.stringValueSingle).ToArray();
                return values.Any(v => v != values[0]);
            }
            return false;
        }
        
        // Helper: Get if mix mode is enabled for HDR Color
        private bool MixModeEnabledHDRColor(SuperSerializedProperty property)
        {
            if (property.superSerializedObject.isEditingMultipleObjects)
            {
                Color[] values = property.superSerializedObject.FindAllPropertiesByPath(property.propertyPath).Select(p => p.colorValueSingle).ToArray();
                return values.Any(v => v != values[0]);
            }
            return false;
        }
        
        // Draw Toolbox Before
        private void DrawToolboxBefore() => DrawToolbox();
        
        // Draw Toolbox After
        private void DrawToolboxAfter() => DrawToolbox(true);
        
        // Draw Toolbox
        private void DrawToolbox(bool displayAfter = false)
        {
            if (m_serializedObject == null) return;
            if (m_serializedObject.Methods == null) return;
            if (m_serializedObject.Methods.Length == 0) return;
            int buttonCount = m_serializedObject.Methods.Sum(m => m.GetCustomAttributes<ButtonAttribute>().Where(b => b.DisplayAfter == displayAfter).Count());
            if(!displayAfter) buttonCount += m_serializedObject.Methods.Sum(m => m.GetCustomAttributes<ContextMenu>().Count());
            if (buttonCount == 0) return;
            
            Rect rect = EditorGUILayout.GetControlRect(false, displayAfter ? 10 : 7);
            rect.y += 4;
            rect.height = 24 * buttonCount + (displayAfter ? 8 : 6);
            rect.xMin = 0;
            rect.width = EditorGUIUtility.currentViewWidth;
            EditorGUI.DrawRect(rect, s_toolbarBackgroundColor);
            
            foreach (MethodInfo method in m_serializedObject.Methods)
            {
                EditorSuperGUILayout.DrawButtons(m_serializedObject, method, displayAfter);
            }
            
            EditorGUILayout.GetControlRect(false, displayAfter ? 0 : 10);
        }
        
        // Updates the icon of the class
        private void UpdateIcon()
        {
            SuperBehaviourIcon icon = IconUtility.GetIconByClassName(m_serializedObject?.TypeName);
            if (m_lastIconAssigned == icon) return;
            string path = $"Assets/Plugins/LoM/Super/Icons/{icon.ToString()}.png";
            if (target.GetType().GetCustomAttribute(typeof(SuperIcon)) != null)
            {
                SuperIcon ico = target.GetType().GetCustomAttribute(typeof(SuperIcon)) as SuperIcon;
                path = ico.GetPath();
            }
            Texture2D iconTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
            EditorGUIUtility.SetIconForObject(target, iconTexture);
        }
    }
}