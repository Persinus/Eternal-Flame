using System.IO;
using UnityEditor;
using UnityEngine;
using LoM.Super.Editor;

namespace LoM.Super.Internal
{
    [InitializeOnLoad]
    public class CreateDialog : ScriptableWizard
    {
        private string _name;
        private string _template;
        private string _path;
        
        public static void ShowWindow(string name, string template, string path)
        {
            CreateDialog window = DisplayWizard<CreateDialog>("Create", "Create");
            window._name = name;
            window._template = template;
            window._path = path;
        }
        
        private void OnGUI()
        {
            // Override height of window
            minSize = new Vector2(300, 80);
            maxSize = new Vector2(300, 80);
            
            // Draw Input Fields
            Rect rect = EditorGUILayout.GetControlRect();
            rect.x += 10;
            rect.width -= 20;
            rect.height = EditorGUIUtility.singleLineHeight;
            rect.y += 10;
            _name = EditorGUI.TextField(rect, "", _name);
            
            // If Empty write placeholder text over the text field in grey
            if (string.IsNullOrEmpty(_name))
            {
                rect.x += 2;
                EditorGUI.LabelField(rect, "Enter the filename", EditorStyles.centeredGreyMiniLabel.Variant("CreateDialog_Label", style => style.alignment = TextAnchor.MiddleLeft));
            }
            
            // Draw Buttons
            rect = EditorGUILayout.GetControlRect();
            rect.x += 10;
            rect.width -= 20;
            rect.height = EditorGUIUtility.singleLineHeight * 1.5f;
            rect.y += 20;
            if (GUI.Button(rect, "Create"))
            {
                OnWizardCreate();
                Close();
            }
        }

        private void OnWizardCreate()
        {
            string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath($"{_path}/{_name}.cs");
            File.WriteAllText(assetPathAndName, _template.Replace("{Name}", _name));
            AssetDatabase.Refresh();
        }
    }
}