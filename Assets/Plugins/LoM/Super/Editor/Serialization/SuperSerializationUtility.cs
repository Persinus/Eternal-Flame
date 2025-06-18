using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace LoM.Super.Serialization
{
    /// <summary>
    /// Internal Utility class for SuperBehaviour Serialization
    /// </summary>
    internal static class SuperSerializationUtility
    {
        // Static Variables
        private static object s_lock = new();
        private static ConcurrentDictionary<Type, UnityObjectInfo> s_unityObjectInfoCache = new();
        private static bool s_isCodeSmellAllowed = false;
        
        // Get Unity Object Info
        internal static UnityObjectInfo GetUnityObjectInfo(Type type, SerializedObject serializedObject)
        {
            if (s_unityObjectInfoCache.TryGetValue(type, out var info)) return info;
            info = new UnityObjectInfo(type, serializedObject);
            s_unityObjectInfoCache.TryAdd(type, info);
            return info;
        }
        internal static UnityObjectInfo GetUnityObjectInfo(Type type)
        {
            if (s_unityObjectInfoCache.TryGetValue(type, out var info)) return info;
            info = new UnityObjectInfo(type);
            s_unityObjectInfoCache.TryAdd(type, info);
            return info;
        }
        
        // Pre-Cache
        private static void PreCache()
        {
            // Load Settings
            if (!EditorPrefs.GetBool("SuperBehaviour.PreCache.Enabled", true)) return;
            List<string> paths = new();
            paths.Add(EditorPrefs.GetString("SuperBehaviour.PreCache.ScriptsPath", "Scripts"));
            if (EditorPrefs.GetBool("SuperBehaviour.PreCache.SuperPlugins", true))
            {
                paths.Add("Plugins");
            }
            paths = paths.Where(p => !string.IsNullOrEmpty(p)).ToList();
            if (paths.Count == 0) return;
            
            // Sanetize Paths
            List<string> sanetizedPaths = new();
            foreach (string path in paths)
            {
                string sanetizedPath = path.Replace('\\', '/').ToLowerInvariant();
                if (!path.StartsWith("assets/")) sanetizedPath = "assets/" + sanetizedPath;
                sanetizedPath = sanetizedPath.Replace("//", "/");
                sanetizedPaths.Add(sanetizedPath);
            }
            
            // Get Assemblies and Populate Cache (Parallel ForEach)
            Assembly[] assemblies = GetFilteredAssemblies(sanetizedPaths.ToArray());
            Parallel.ForEach(assemblies, assembly =>
            {
                Type[] types = assembly.GetTypes();
                Parallel.ForEach(types, type =>
                {
                    if (type.IsSubclassOf(typeof(SuperBehaviour)) || type == typeof(SuperBehaviour))
                    {
                        lock (s_lock)
                        {
                            if (!s_unityObjectInfoCache.ContainsKey(type))
                            {
                                s_unityObjectInfoCache.TryAdd(type, new UnityObjectInfo(type));
                            }
                        }
                    }
                });
            });
        }
        private static Assembly[] GetFilteredAssemblies(string[] paths)
        {
            var allCompiledAssemblies = UnityEditor.Compilation.CompilationPipeline.GetAssemblies();
            HashSet<string> pluginAssemblyNames = new(
                allCompiledAssemblies
                    .Where(assembly => assembly.sourceFiles.Any(s => paths.Any(p => s.Replace('\\', '/').ToLowerInvariant().Contains(p))))
                    .Select(assembly => assembly.name)
            );
            Assembly[] allLoadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();
            return allLoadedAssemblies.Where(assembly => pluginAssemblyNames.Contains(assembly.GetName().Name)).ToArray();
        }
        
        // Clear cache after domain reload
        [InitializeOnLoadMethod]
        private static void ClearCache()
        {
            s_unityObjectInfoCache.Clear();
            s_isCodeSmellAllowed = EditorPrefs.GetBool("SuperBehaviour.CodeSmells.Enabled", true);
            PreCache();
        }
        
        // Internal class
        internal struct UnityObjectInfo
        {
            // Member Variables
            private Type m_type;
            private FieldInfo[] m_fields;
            private PropertyInfo[] m_properties;
            private MethodInfo[] m_methods;
            
            // Getters
            public Type Type => m_type;
            public FieldInfo[] Fields => m_fields;
            public PropertyInfo[] Properties => m_properties;
            public MethodInfo[] Methods => m_methods;
            
            // Constructor
            internal UnityObjectInfo(Type type)
            {
                m_type = type;
                m_fields = null;
                m_properties = null;
                m_methods = null;
                PopulateFields();
                PopulateProperties();
                PopulateMethods();
            }
            internal UnityObjectInfo(Type type, SerializedObject serializedObject)
            {
                m_type = type;
                m_fields = null;
                m_properties = null;
                m_methods = null;
                PopulateFields(serializedObject);
                PopulateProperties();
                PopulateMethods();
            }
            
            // Populate Fields
            private void PopulateFields(SerializedObject serializedObject)
            {
                List<FieldInfo> fields = new();
                var serializedProperty = serializedObject.GetIterator();
                if (serializedProperty.NextVisible(true))
                {
                    do
                    {
                        if (serializedProperty.name == "m_Script") continue;
                        FieldInfo field = m_type.GetField(serializedProperty.name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                        if (field == null) continue;
                        fields.Add(field);
                    } while (serializedProperty.NextVisible(false));
                }
                m_fields = fields.ToArray();
            }
            private void PopulateFields()
            {
                m_fields = m_type
                    .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                    .Where(f => (f.GetCustomAttributes(typeof(SerializeField), true).Length > 0 || f.GetCustomAttributes(typeof(SerializeReference), true).Length > 0 || f.GetCustomAttributes(typeof(SerializableAttribute), true).Length > 0 || (f.IsPublic && s_isCodeSmellAllowed))
                                && f.GetCustomAttributes(typeof(SerializePropertyAttribute), true).Length == 0
                                && f.GetCustomAttributes(typeof(NonSerializedAttribute), true).Length == 0
                                && f.GetCustomAttributes(typeof(HideInInspector), true).Length == 0
                                && f.GetCustomAttributes(typeof(ObsoleteAttribute), true).Length == 0)
                    .ToArray();
            }
            
            // Populate Properties
            private void PopulateProperties()
            {
                m_properties = m_type
                    .GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                    .Where(p => p.GetCustomAttributes(typeof(SerializePropertyAttribute), true).Length > 0)
                    .ToArray();
            }
            
            // Populate Methods
            private void PopulateMethods()
            {
                m_methods = m_type
                    .GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)
                    .ToArray();
            }
        }
    }
}