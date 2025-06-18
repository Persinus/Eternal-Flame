using System.IO;
using LoM.Super.Editor;
using UnityEditor;
using UnityEngine;

namespace LoM.Super.Internal
{
    public static class SuperBehaviourMenu
    {
        // Constants
        private const string SUPER_BAHAVIOUR_TEMPLATE = "using System.Collections;\nusing System.Collections.Generic;\nusing UnityEngine;\nusing LoM.Super;\n\npublic class {Name} : SuperBehaviour\n{\n\t\n}";
        private const string ENUM_TEMPLATE = "using UnityEngine;\n\npublic enum {Name}\n{\n\tNone = 0,\n}";
        private const string INTERFACE_TEMPLATE = "using UnityEngine;\n\npublic interface {Name}\n{\n\t\n}";
        private const string SCRIPTABLE_OBJECT_TEMPLATE = "using UnityEngine;\n\n[CreateAssetMenu(fileName = \"{Name}\", menuName = \"{Name}\", order = 0)]\npublic class {Name} : ScriptableObject\n{\n\t\n}";
        private const string STRUCT_TEMPLATE = "using UnityEngine;\n\npublic struct {Name}\n{\n\t\n}";
        
        [MenuItem("Assets/Create/Scripts/SuperBehaviour", false, 80)]
        public static void CreateSuperBehaviour()
        {
            string path = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (string.IsNullOrEmpty(path)) path = "Assets/";
            CreateDialog.ShowWindow("", SUPER_BAHAVIOUR_TEMPLATE, path);
        }
        
        [MenuItem("Assets/Create/Scripts/Enum", false, 80)]
        public static void CreateEnum()
        {
            string path = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (string.IsNullOrEmpty(path)) path = "Assets/";
            CreateDialog.ShowWindow("", ENUM_TEMPLATE, path);
        }
        
        [MenuItem("Assets/Create/Scripts/Interface", false, 80)]
        public static void CreateInterface()
        {
            string path = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (string.IsNullOrEmpty(path)) path = "Assets/";
            CreateDialog.ShowWindow("I", INTERFACE_TEMPLATE, path);
        }
        
        [MenuItem("Assets/Create/Scripts/ScriptableObject", false, 80)]
        public static void CreateScriptableObject()
        {
            string path = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (string.IsNullOrEmpty(path)) path = "Assets/";
            CreateDialog.ShowWindow("", SCRIPTABLE_OBJECT_TEMPLATE, path);
        }
    }
}