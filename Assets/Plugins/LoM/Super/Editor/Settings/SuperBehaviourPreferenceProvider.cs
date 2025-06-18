using System;
using UnityEditor;
using UnityEngine;

namespace LoM.Super.Editor
{
    /// <summary>
    /// Base class for creating preference providers for SuperBehaviour<br/>
    /// Use this class to manage preferences for your custom modules<br/>
    /// <i>HINT: Use [InitializeOnLoadMethod] to register your provider on domain reload</i>
    /// </summary>
    public abstract class SuperBehaviourPreferenceProvider
    {
        /// <summary>
        /// Return Name of the preference provider (part of the final property path)
        /// </summary>
        public abstract string Name { get; }
        
        /// <summary>
        /// Return the order of the preference provider (higher order will be displayed first)<br/>
        /// <i>Do not use negative values for non built-in providers</i>
        /// </summary>
        public virtual int Order => 0;
        
        /// <summary>
        /// Render the GUI (only internal drawing allowed here)<br/>
        /// e.g. RenderTitle("Title"), RenderTextField("Label", "Key", "Default"), etc.
        /// </summary>
        public abstract void OnGUI();
        
        /// <summary>
        /// Render a title
        /// </summary>
        /// <param name="title"></param>
        protected void RenderTitle(string title)
        {
            float height = EditorGUIUtility.singleLineHeight * 2.7f;
            Rect rect = EditorGUILayout.GetControlRect(false, height);
            rect.y += EditorGUIUtility.singleLineHeight;
            rect.height = height - EditorGUIUtility.singleLineHeight * 1.2f;
            EditorGUI.LabelField(rect, $"  {title}", EditorStyles.boldLabel.Variant("SuperBehaviourPreferences_Title", style => {
                style.fontSize = 14;
                style.normal.background = SuperBehaviourPreferences.BackgroundTexture;
            }));
        }
        
        /// <summary>
        /// Render a collapsable title (use this to allowe users to enable or disable a module)
        /// </summary>
        protected bool RenderTitleCollapse(string title, string key)
        {
            bool value = EditorPrefs.GetBool(BuildKey(key), true);
            
            float height = EditorGUIUtility.singleLineHeight * 2.7f;
            Rect rectLabel = EditorGUILayout.GetControlRect(false, height);
            rectLabel.y += EditorGUIUtility.singleLineHeight;
            rectLabel.height = height - EditorGUIUtility.singleLineHeight * 1.2f;
            
            Rect checkboxRect = rectLabel;
            checkboxRect.width = 15;
            checkboxRect.x += 5;
            
            EditorGUI.DrawRect(rectLabel, SuperBehaviourPreferences.BackgroundColor);
            
            EditorGUI.BeginChangeCheck();
            EditorGUI.LabelField(rectLabel, $"       {title}", EditorStyles.boldLabel.Variant("SuperBehaviourPreferences_Title", style => {
                style.fontSize = 14;
            }));
            value = EditorGUI.Toggle(checkboxRect, value);
            if (EditorGUI.EndChangeCheck())
            {
                EditorPrefs.SetBool(BuildKey(key), value);
            }
            return value;
        }
        
        /// <summary>
        /// Render a text field
        /// </summary>
        protected string RenderTextField(string label, string key, string defaultValue)
        {
            EditorGUI.BeginChangeCheck();
            string value = EditorPrefs.GetString(BuildKey(key), defaultValue);
            value = EditorGUI.TextField(GetDefaultControlRect(), $"  {label}", value);
            if (EditorGUI.EndChangeCheck())
            {
                EditorPrefs.SetString(BuildKey(key), value);
            }
            return value;
        }
        
        /// <summary>
        /// Render an int field
        /// </summary>
        protected int RenderIntField(string label, string key, int defaultValue)
        {
            EditorGUI.BeginChangeCheck();
            int value = EditorPrefs.GetInt(BuildKey(key), defaultValue);
            value = EditorGUI.IntField(GetDefaultControlRect(), $"  {label}", value);
            if (EditorGUI.EndChangeCheck())
            {
                EditorPrefs.SetInt(BuildKey(key), value);
            }
            return value;
        }
        
        /// <summary>
        /// Render a toggle field
        /// </summary>
        protected bool RenderToggle(string label, string key, bool defaultValue)
        {
            EditorGUI.BeginChangeCheck();
            bool value = EditorPrefs.GetBool(BuildKey(key), defaultValue);
            value = EditorGUI.Toggle(GetDefaultControlRect(), $"  {label}", value);
            if (EditorGUI.EndChangeCheck())
            {
                EditorPrefs.SetBool(BuildKey(key), value);
            }
            return value;
        }
        /// <summary>
        /// Render a bool field
        /// </summary>
        protected bool RenderBoolField(string label, string key, bool defaultValue) => RenderToggle(label, key, defaultValue);
        
        /// <summary>
        /// Render a float field
        /// </summary>
        protected float RenderFloatField(string label, string key, float defaultValue)
        {
            EditorGUI.BeginChangeCheck();
            float value = EditorPrefs.GetFloat(BuildKey(key), defaultValue);
            value = EditorGUI.FloatField(GetDefaultControlRect(), $"  {label}", value);
            if (EditorGUI.EndChangeCheck())
            {
                EditorPrefs.SetFloat(BuildKey(key), value);
            }
            return value;
        }
        
        /// <summary>
        /// Render a range field
        /// </summary>
        protected float RenderRangeField(string label, string key, float defaultValue, float min, float max)
        {
            EditorGUI.BeginChangeCheck();
            float value = EditorPrefs.GetFloat(BuildKey(key), defaultValue);
            value = EditorGUI.Slider(GetDefaultControlRect(), $"  {label}", value, min, max);
            if (EditorGUI.EndChangeCheck())
            {
                EditorPrefs.SetFloat(BuildKey(key), value);
            }
            return value;
        }
        
        /// <summary>
        /// Render an Enum field
        /// </summary>
        protected T RenderEnumField<T>(string label, string key, T defaultValue) where T : Enum
        {
            EditorGUI.BeginChangeCheck();
            T value = (T)Enum.Parse(typeof(T), EditorPrefs.GetString(BuildKey(key), defaultValue.ToString()));
            value = (T)EditorGUI.EnumPopup(GetDefaultControlRect(), $"  {label}", value);
            if (EditorGUI.EndChangeCheck())
            {
                EditorPrefs.SetString(BuildKey(key), value.ToString());
            }
            return value;
        }
        
        // Helper: Render Object Field
        private Rect GetDefaultControlRect(float height = 0)
        {
            if (height == 0) height = EditorGUIUtility.singleLineHeight;
            Rect rect = EditorGUILayout.GetControlRect(false, height);
            rect.width = Mathf.Min(rect.width, 400);
            return rect;
        }
        
        // Helper: Build key
        private string BuildKey(string key) => $"SuperBehaviour{(Name == "Default" ? "" : $".{Name}")}.{key}";
    }
}