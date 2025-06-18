using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace LoM.Super.Serialization
{
    /// <summary>
    /// SuperSerializedObject and SuperSerializedProperty are classes for editing serialized field
    /// on Object|Unity objects in a completely generic way. These classes automatically
    /// handle dirtying individual serialized fields so they will be processed by the
    /// Undo system and styled correctly for Prefab overrides when drawn in the Inspector.<br/>
    /// Unlike the SerializedObject and SerializedProperty classes provided by Unity,
    /// this class supports Properties and Fields
    /// </summary>
    public class SuperSerializedObject : SerializedObject
    {
        // Member Variables
        private Type m_type;
        private string m_typeName;
        private Dictionary<UnityEngine.Object, SerializedObject> m_innerObjects = new();
        private Dictionary<UnityEngine.Object, SuperSerializedProperty[]> m_all = new();
        private Dictionary<UnityEngine.Object, SuperSerializedProperty[]> m_fields = new();
        private Dictionary<UnityEngine.Object, SuperSerializedProperty[]> m_properties = new();
        private Dictionary<UnityEngine.Object, MethodInfo[]> m_methods = new();
        
        // Getters
        public Type Type => m_type;
        public string TypeName => m_typeName;
        public SuperSerializedProperty[] All => m_all.FirstOrDefault().Value;
        public SuperSerializedProperty[] Fields => m_fields.FirstOrDefault().Value;
        public SuperSerializedProperty[] Properties => m_properties.FirstOrDefault().Value;
        public MethodInfo[] Methods => m_methods.FirstOrDefault().Value;
        
        // Constructors
        /// <summary>
        /// Create SerializedObject for inspected object.
        /// </summary>
        public SuperSerializedObject(UnityEngine.Object obj) : base(obj)
        {
            InitProperties(obj);
        }
        /// <summary>
        /// Create SerializedObject for inspected object by specifying a context to be used
        /// to store and resolve ExposedReference types.
        /// </summary>
        public SuperSerializedObject(UnityEngine.Object obj, UnityEngine.Object context) : base(obj, context)
        {
            InitProperties(obj, context);
        }
        /// <summary>
        /// Create SerializedObject for inspected object.
        /// </summary>
        public SuperSerializedObject(UnityEngine.Object[] objs) : base(objs)
        {
            foreach (UnityEngine.Object obj in objs)
            {
                InitProperties(obj);
            }
        }
        /// <summary>
        /// Create SerializedObject for inspected object by specifying a context to be used
        /// to store and resolve ExposedReference types.
        /// </summary>
        public SuperSerializedObject(UnityEngine.Object[] objs, UnityEngine.Object context) : base(objs, context)
        {
            foreach (UnityEngine.Object obj in objs)
            {
                InitProperties(obj, context);
            }
        }
    
        /// <summary>
        /// Initialize properties.
        /// </summary>
        private void InitProperties(UnityEngine.Object obj, UnityEngine.Object context = null)
        {
            m_innerObjects[obj] = context == null ? new SerializedObject(obj) : new SerializedObject(obj, context);
            m_type = obj.GetType();
            m_typeName = m_type.Name;
            SuperSerializationUtility.UnityObjectInfo objInfo = SuperSerializationUtility.GetUnityObjectInfo(m_type, this);
            m_methods[obj] = objInfo.Methods;
            int fieldsLength = objInfo.Fields.Length;
            int propertiesLength = objInfo.Properties.Length;
            SuperSerializedProperty[] all = new SuperSerializedProperty[fieldsLength + propertiesLength];
            SuperSerializedProperty[] fields = new SuperSerializedProperty[fieldsLength];
            SuperSerializedProperty[] properties = new SuperSerializedProperty[propertiesLength];
            for (int i = 0; i < fieldsLength; i++)
            {
                FieldInfo fieldInfo = objInfo.Fields[i];
                SuperSerializedProperty property = new(this, base.FindProperty(fieldInfo.Name), m_innerObjects[obj].FindProperty(fieldInfo.Name), fieldInfo, obj);
                fields[i] = property;
                all[i] = property;
            }
            for (int i = 0; i < propertiesLength; i++)
            {
                SuperSerializedProperty property = new(this, objInfo.Properties[i], obj);
                properties[i] = property;
                all[i + fieldsLength] = property;
            }
            m_all[obj] = all;
            m_fields[obj] = fields;
            m_properties[obj] = properties;
        }
        
        /// <summary>
        /// Update serialized object's representation.
        /// </summary>
        public new void Update() 
        {
            base.Update();
            foreach (UnityEngine.Object obj in m_innerObjects.Keys)
            {
                m_innerObjects[obj].Update();
            }
            
            foreach (UnityEngine.Object obj in m_all.Keys)
            {
                foreach (SuperSerializedProperty property in m_all[obj])
                {
                    property.Reevaluate();
                }
            }
        }
    
        // Dispose 
        public new void Dispose() 
        {
            foreach (UnityEngine.Object obj in m_properties.Keys)
            {
                foreach (SuperSerializedProperty property in m_properties[obj])
                {
                    property.Dispose();
                }
            }
            base.Dispose();
            foreach (UnityEngine.Object obj in m_innerObjects.Keys)
            {
                m_innerObjects[obj].Dispose();
            }
        }
    
        [Obsolete("Use FindPropertyByPath instead.", false)]
        public new SerializedProperty FindProperty(string propertyPath) => FindPropertyByPath(propertyPath);
        /// <summary>
        /// Find serialized property by name.
        /// </summary>
        public SuperSerializedProperty FindPropertyByPath(string propertyPath) 
        {
            foreach (UnityEngine.Object obj in m_all.Keys)
            {
                foreach (SuperSerializedProperty property in m_all[obj])
                {
                    if (property.propertyPath == propertyPath) return property;
                }
            }
            return null;
        }
        public SuperSerializedProperty[] FindAllPropertiesByPath(string propertyPath) 
        {
            List<SuperSerializedProperty> properties = new();
            foreach (UnityEngine.Object obj in m_all.Keys)
            {
                foreach (SuperSerializedProperty property in m_all[obj])
                {
                    if (property.propertyPath == propertyPath) properties.Add(property);
                }
            }
            return properties.ToArray();
        }
    
    
        //* ////////////////////////////////////////////////////////////////////
        //* Explicit base calls (for internal use only)
        //* ////////////////////////////////////////////////////////////////////
        internal SerializedProperty GetIteratorInternal() => base.GetIterator();
        internal SerializedProperty FindPropertyInternal(string propertyPath) => base.FindProperty(propertyPath);
    
    
        //* ////////////////////////////////////////////////////////////////////
        //* NEW Method Functions
        //* ////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// Invoke a method on the target object / objects.
        /// </summary>
        /// <param name="methodName">Name of the method to be invoked</param>
        public void InvokeMethod(string methodName) 
        {
            List<string> names = new();
            foreach (UnityEngine.Object obj in m_methods.Keys)
            {
                foreach (MethodInfo method in m_methods[obj])
                {
                    try
                    {

                        if (method.Name == methodName)
                        {
                            if (method.IsStatic)
                            {
                                if (names.Contains(method.Name)) continue;
                                names.Add(method.Name);
                            }
                            method.Invoke(obj, null);
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.LogWarningFormat("Failed to invoke method {0} on {1}. Error: {2}", methodName, obj.name, e.Message);
                    }
                }
            }
        }
        
        /// <summary>
        /// Invoke a method on a specific target object.
        /// </summary>
        /// <param name="methodName">Name of the method to be invoked</param>
        /// <param name="obj">Target object</param>
        public void InvokeMethod(string methodName, UnityEngine.Object obj) 
        {
            if (!m_methods.ContainsKey(obj)) return;
            List<string> names = new();
            foreach (MethodInfo method in m_methods[obj])
            {
                try
                {
                    if (method.Name == methodName)
                    {
                        if (method.IsStatic)
                        {
                            if (names.Contains(method.Name)) continue;
                            names.Add(method.Name);
                        }
                        method.Invoke(obj, null);
                    }
                }
                catch (Exception e)
                {
                    Debug.LogWarningFormat("Failed to invoke method {0} on {1}. Error: {2}", methodName, obj.name, e.Message);
                }
            }
        }
    
        /// <summary>
        /// Invoke a method on the target object / objects with parameters.
        /// </summary>
        /// <param name="methodName">Name of the method to be invoked</param>
        /// <param name="parameters">Parameters to be passed to the method</param>
        /// <typeparam name="T">Type of the parameter</typeparam>
        /// <param name="obj">Target object</param>
        /// <returns>Return value of the method</returns>
        public T InvokeMethod<T>(string methodName, params object[] parameters) 
        {
            foreach (UnityEngine.Object obj in m_methods.Keys)
            {
                foreach (MethodInfo method in m_methods[obj])
                {
                    try
                    {
                        if (method.Name == methodName)
                        {
                            return (T)method.Invoke(obj, parameters);
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.LogWarningFormat("Failed to invoke method {0} on {1} with parameters {2}. Error: {3}", methodName, obj.name, parameters, e.Message);
                    }
                }
            }
            return default;
        }
    
        /// <summary>
        /// Invoke a method on a specific target object with parameters.
        /// </summary>
        /// <param name="methodName">Name of the method to be invoked</param>
        /// <param name="parameters">Parameters to be passed to the method</param>
        /// <typeparam name="T">Type of the parameter</typeparam>
        /// <param name="obj">Target object</param>
        /// <returns>Return value of the method</returns>
        public T InvokeMethod<T>(string methodName, UnityEngine.Object obj, params object[] parameters) 
        {
            if (!m_methods.ContainsKey(obj)) return default;
            foreach (MethodInfo method in m_methods[obj])
            {
                try
                {
                    if (method.Name == methodName)
                    {
                        return (T)method.Invoke(obj, parameters);
                    }
                }
                catch (Exception e)
                {
                    Debug.LogWarningFormat("Failed to invoke method {0} on {1} with parameters {2}. Error: {3}", methodName, obj.name, parameters, e.Message);
                }
            }
            return default;
        }
        
        
    
    
    
    
    
    
    
        //! ////////////////////////////////////////////////////////////////////
        //! NOT IMPLEMENTED
        //! ////////////////////////////////////////////////////////////////////
        // Getters
        /// <summary>
        /// Is true when the SerializedObject has a modified property that has not been applied.
        /// </summary>
        public new bool hasModifiedProperties => base.hasModifiedProperties;
        /// <summary>
        /// Defines the maximum size beyond which arrays cannot be edited when multiple objects
        /// are selected.
        /// </summary>
        public new int maxArraySizeForMultiEditing => base.maxArraySizeForMultiEditing;
        /// <summary>
        /// Controls the visibility of the child hidden fields.
        /// </summary>
        public new bool forceChildVisibility => base.forceChildVisibility;
        
        // Methods
        /// <summary>
        /// Apply property modifications.
        /// </summary>
        public new bool ApplyModifiedProperties() => base.ApplyModifiedProperties();
        /// <summary>
        /// Applies property modifications without registering an undo operation.
        /// </summary>
        public new bool ApplyModifiedPropertiesWithoutUndo() => base.ApplyModifiedPropertiesWithoutUndo();
        /// <summary>
        /// Copies a value from a SerializedProperty to the corresponding serialized property
        /// on the serialized object.
        /// </summary>
        public new void CopyFromSerializedProperty(SerializedProperty prop) => base.CopyFromSerializedProperty(prop);
        /// <summary>
        /// Copies a changed value from a SerializedProperty to the corresponding serialized
        /// property on the serialized object.
        /// </summary>
        public new bool CopyFromSerializedPropertyIfDifferent(SerializedProperty prop) => base.CopyFromSerializedPropertyIfDifferent(prop);
        /// <summary>
        /// Get the first serialized property.
        /// </summary>
        public new SerializedProperty GetIterator() => base.GetIterator();
        /// <summary>
        /// Update hasMultipleDifferentValues cache on the next Update() call.
        /// </summary>
        public new void SetIsDifferentCacheDirty() => base.SetIsDifferentCacheDirty();
        /// <summary>
        /// This has been made obsolete. See SerializedObject.UpdateIfRequiredOrScript instead.
        /// </summary>
        [Obsolete("UpdateIfDirtyOrScript has been deprecated. Use UpdateIfRequiredOrScript instead.", false)]
        public new void UpdateIfDirtyOrScript() => base.UpdateIfDirtyOrScript();
        /// <summary>
        /// Update serialized object's representation, only if the object has been modified
        /// since the last call to Update or if it is a script.
        /// </summary>
        public new void UpdateIfRequiredOrScript() => base.UpdateIfRequiredOrScript();
        
        
        
        //! ////////////////////////////////////////////////////////////////////
        //! IRRELEVANT
        //! ////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Does the serialized object represents multiple objects due to multi-object editing?
        /// (Read Only)
        /// </summary>
        public new bool isEditingMultipleObjects => base.isEditingMultipleObjects;
        /// <summary>
        /// The context used to store and resolve ExposedReference types. This is set by
        /// the SerializedObject constructor.
        /// </summary>
        public new UnityEngine.Object context => base.context;
        /// <summary>
        /// The inspected objects (Read Only).
        /// </summary>
        public new UnityEngine.Object targetObject => base.targetObject;
        /// <summary>
        /// The inspected object (Read Only).
        /// </summary>
        public new UnityEngine.Object[] targetObjects => base.targetObjects;
        
    }
}