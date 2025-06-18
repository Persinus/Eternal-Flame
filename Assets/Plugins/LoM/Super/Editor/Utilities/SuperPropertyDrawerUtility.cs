using System.Collections.Generic;
using System.Reflection;
using System.Linq; 
using System;
using UnityEngine;
using UnityEditor;
using LoM.Super.Serialization;
using LoM.Super.Editor.Drawer;

namespace LoM.Super.Editor
{
    internal class SuperPropertyDrawerUtility
    {
        // Singleton
        private static SuperPropertyDrawerUtility s_instance;
        internal static SuperPropertyDrawerUtility Instance => s_instance ?? (s_instance = new SuperPropertyDrawerUtility());
        
        // Member Variables
        private Dictionary<Type, Type> m_PropertyDrawers; // Type of the property, Type of the property drawer
        private Dictionary<Type, nint> m_PropertyDrawersOrderMap; // Type of the property, Order of the property drawer
        
        // Constructor
        private SuperPropertyDrawerUtility() 
        { 
            InitializePropertyDrawerList();
        }
        
        // Initialization
        internal void InitializePropertyDrawerList()
        {
            m_PropertyDrawers = new Dictionary<Type, Type>();
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            Type[] types = assemblies.SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.IsSubclassOf(typeof(SuperPropertyDrawer)))
                .ToArray();
            foreach (Type type in types)
            {
                CustomSuperPropertyDrawerAttribute[] attributes = (CustomSuperPropertyDrawerAttribute[])type.GetCustomAttributes(typeof(CustomSuperPropertyDrawerAttribute), true);
                foreach (CustomSuperPropertyDrawerAttribute attribute in attributes)
                {
                    Type targetType = attribute?.Type;
                    if (targetType == null) continue;
                    if (m_PropertyDrawers.ContainsKey(targetType)) 
                    {
                        if (attribute.Order > m_PropertyDrawersOrderMap[targetType])
                        {
                            m_PropertyDrawers[targetType] = type;
                            m_PropertyDrawersOrderMap[targetType] = attribute.Order;
                        }
                        continue;
                    }
                    m_PropertyDrawers.Add(targetType, type);
                }
            }
        }
        
        // Get Custom Property Drawer
        internal SuperPropertyDrawer GetPropertyDrawer(Type type, SuperSerializedProperty property)
        {
            // Check if identical type exists
            if (m_PropertyDrawers.ContainsKey(type))
            {
                return property.Drawer(m_PropertyDrawers[type]);
            }
            
            // Check if is inherited from type
            foreach (KeyValuePair<Type, Type> pair in m_PropertyDrawers)
            {
                if (pair.Key.IsAssignableFrom(type))
                {
                    return property.Drawer(pair.Value);
                }
            }
                    
            return null;
        }
        
        // [] Operator
        internal SuperPropertyDrawer this[SuperSerializedProperty property] => GetPropertyDrawer(property.Type, property);
        
        // Clear instance on domain reload
        [InitializeOnLoadMethod]
        private static void ClearInstance() => s_instance = null;
    }
}