using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace LoM.Super.Editor
{
    /// <summary>
    /// Dialog for displaying simple richtext messages
    /// </summary>
    public class RichtextDialog : EditorWindow
    {
        // Member Variables
        private string _richtext;
        
        // Draw
        private void OnGUI()
        {
            if (!string.IsNullOrEmpty(_richtext))
            {
                GUIStyle style = new GUIStyle();
                style.richText = true;
                style.wordWrap = true;
                style.padding = new RectOffset(10, 10, 10, 10);
                style.normal.textColor = EditorStyles.label.normal.textColor;
                EditorGUILayout.LabelField(_richtext, style);
            }
            
            // Set Size
            if (Event.current.type == EventType.Repaint)
            {
                float height = GUILayoutUtility.GetLastRect().yMax;
                minSize = new Vector2(minSize.x, height);
                maxSize = new Vector2(maxSize.x, height);
            }
        }
        
        /// <summary>
        /// Show a richtext dialog
        /// </summary>
        /// <param name="name">Name of the dialog</param>
        /// <param name="richtext">Richtext to display</param>
        public static void Show(string name, string richtext, int width = 400, int height = 200)
        {
            RichtextDialog dialog = CreateInstance<RichtextDialog>();
            dialog.titleContent = new GUIContent(name);
            dialog._richtext = richtext;
            dialog.minSize = new Vector2(width, height);
            if (width != 400 || height != 200) dialog.maxSize = new Vector2(width, height);
            dialog.position = new Rect(Screen.width / 2 - width / 2, Screen.height / 2 - height / 2, width, height);
            dialog.ShowUtility();
        }
    }
}