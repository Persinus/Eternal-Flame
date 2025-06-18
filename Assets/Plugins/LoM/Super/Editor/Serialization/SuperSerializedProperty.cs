using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using LoM.Super.Editor.Drawer;
using UnityEditor;
using UnityEngine;

// TODO: As soon as Unity allows inheritance of SerializedProperty change this class to inherit from it
// Therefor leaving all methods and properties with bad syntax

namespace LoM.Super.Serialization
{
    /// <summary>
    /// SuperSerializedProperty and SuperSerializedObject are classes for editing properties on
    /// objects in a completely generic way that automatically handles undo, multi-object
    /// editing and Prefab overrides.<br/>
    /// Unlike the SerializedObject and SerializedProperty classes provided by Unity,
    /// this class supports Properties and Fields
    /// </summary>
    public class SuperSerializedProperty
    {
        //* ////////////////////////////////////////////////////////////////////
        //* Member Variables
        //* ////////////////////////////////////////////////////////////////////
        private SuperSerializedObject m_serializedObject;
        private UnityEngine.Object m_targetObject;
        private PropertyInfo m_propertyInfo;
        private FieldInfo m_fieldInfo;
        private SerializedProperty m_serializedProperty;
        private SerializedProperty m_serializedPropertySingle;
        private SerializedAttributes m_attributes;
        private uint m_contentHash;
        private string m_propertyPath;
        private SuperPropertyDrawer m_drawerInstance;
        
        // Member Field Attributes
        private string m_displayName;
        
        // Property Only
        private bool m_isExpanded;
        
        //* ////////////////////////////////////////////////////////////////////
        //* Getters
        //* ////////////////////////////////////////////////////////////////////
        public bool IsField => m_serializedProperty != null;
        public SerializedProperty Field => m_serializedProperty;
        public SerializedAttributes Attributes => m_attributes;
        public MemberInfo Info => IsField ? (MemberInfo)m_fieldInfo : (MemberInfo)m_propertyInfo;
        /// <summary>
        /// Target object of this property (Read Only).
        /// </summary>
        public UnityEngine.Object TargetObject => m_targetObject;
        /// <summary>
        /// SerializedObject this property belongs to (Read Only).
        /// </summary>
        public SerializedObject serializedObject => m_serializedObject;
        /// <summary>
        /// SerializedObject this property belongs to (Read Only).
        /// </summary>
        public SuperSerializedObject superSerializedObject => m_serializedObject;
        /// <summary>
        /// Full path of the property. (Read Only)
        /// </summary>
        public string propertyPath => m_propertyPath;
        /// <summary>
        /// Tooltip of the property. (Read Only)
        /// </summary>
        public string tooltip => Attributes.Tooltip?.Tooltip ?? "";
        /// <summary>
        /// Is this property editable? (Read Only)
        /// </summary>
        public bool editable => Attributes.IsReadOnly == false;
        /// <summary>
        /// Is this property a fixed buffer? (Read Only)
        /// </summary>
        public bool isFixedBuffer => Attributes.IsFixedBuffer;
        /// <summary>
        /// The number of elements in the fixed buffer. (Read Only)
        /// </summary>
        public int fixedBufferSize => Attributes.FixedBufferSize;
        /// <summary>
        /// Nice display name of the property. (Read Only)
        /// </summary>
        public string displayName => Attributes.Label;
        /// <summary>
        /// Name of the property. (Read Only)
        /// </summary>
        public string name 
        { 
            get 
            {
                if (IsField) return m_serializedProperty.name;
                return m_propertyInfo.Name;
            }
        }
        /// <summary>
        /// Is this property expanded in the inspector?
        /// </summary>
        public bool isExpanded 
        {
            get
            {
                if (IsField) return m_serializedProperty.isExpanded;
                return m_isExpanded;
            }
            set
            {
                if (IsField) m_serializedProperty.isExpanded = value;
                m_isExpanded = value;
            }
        }
        /// <summary>
        /// Type name of the property. (Read Only)
        /// </summary>
        public string type 
        { 
            get 
            {
                if (IsField) return m_serializedProperty.type;
                return m_propertyInfo.PropertyType.Name;
            }
        }
        /// <summary>
        /// Type of the property. (Read Only)
        /// </summary>
        public Type Type 
        { 
            get 
            {
                if (IsField) return m_fieldInfo.FieldType;
                return m_propertyInfo.PropertyType;
            }
        }
        /// <summary>
        /// Type of this property (Read Only).
        /// </summary>
        public SerializedPropertyType propertyType 
        { 
            get 
            {
                if (IsField) return m_serializedProperty.propertyType;
                var val = m_propertyInfo.GetValue(m_targetObject);
                switch (val)
                {
                    case sbyte _:
                    case byte _:
                    case short _:
                    case ushort _:
                    case int _:
                    case uint _:
                    case long _:
                    case ulong _: return SerializedPropertyType.Integer;
                    case bool _: return SerializedPropertyType.Boolean;
                    case float _: 
                    case double _: return SerializedPropertyType.Float;
                    case string _: return SerializedPropertyType.String;
                    case Color _: return SerializedPropertyType.Color;
                    case LayerMask _: return SerializedPropertyType.LayerMask;
                    case Enum _: return SerializedPropertyType.Enum;
                    case Vector2 _: return SerializedPropertyType.Vector2;
                    case Vector3 _: return SerializedPropertyType.Vector3;
                    case Vector4 _: return SerializedPropertyType.Vector4;
                    case Rect _: return SerializedPropertyType.Rect;
                    case char _: return SerializedPropertyType.Character;
                    case AnimationCurve _: return SerializedPropertyType.AnimationCurve;
                    case Bounds _: return SerializedPropertyType.Bounds;
                    case Gradient _: return SerializedPropertyType.Gradient;
                    case Quaternion _: return SerializedPropertyType.Quaternion;
                    case Vector2Int _: return SerializedPropertyType.Vector2Int;
                    case Vector3Int _: return SerializedPropertyType.Vector3Int;
                    case RectInt _: return SerializedPropertyType.RectInt;
                    case BoundsInt _: return SerializedPropertyType.BoundsInt;
                    case Hash128 _: return SerializedPropertyType.Hash128;
                }
                if (val is UnityEngine.Object) return SerializedPropertyType.ObjectReference;
                return SerializedPropertyType.Generic;
            }
        }
        /// <summary>
        /// Return the precise type for Integer and Floating point properties. (Read Only)
        /// </summary>
        public SerializedPropertyNumericType numericType 
        { 
            get 
            {
                if (IsField) return m_serializedProperty.numericType;
                var val = m_propertyInfo.GetValue(m_targetObject);
                switch (val)
                {
                    case sbyte _: return SerializedPropertyNumericType.Int8;
                    case byte _: return SerializedPropertyNumericType.UInt8;
                    case short _: return SerializedPropertyNumericType.Int16;
                    case ushort _: return SerializedPropertyNumericType.UInt16;
                    case int _: return SerializedPropertyNumericType.Int32;
                    case uint _: return SerializedPropertyNumericType.UInt32;
                    case long _: return SerializedPropertyNumericType.Int64;
                    case ulong _: return SerializedPropertyNumericType.UInt64;
                    case float _: return SerializedPropertyNumericType.Float;
                    case double _: return SerializedPropertyNumericType.Double;
                    default: return SerializedPropertyNumericType.Unknown;
                }
            }
        }
        /// <summary>
        /// Allows you to check whether his property is a PrefabUtility.IsDefaultOverride|default
        /// override. Certain properties on Prefab instances are default overrides. See PrefabUtility.IsDefaultOverride
        /// for more information.
        /// </summary>
        public bool isDefaultOverride 
        { 
            get 
            {
                if (IsField) return isDefaultOverride;
                if (m_targetObject is not UnityEngine.Object) return false;
                PropertyModification mod = PrefabUtility.GetPropertyModifications((UnityEngine.Object)m_targetObject)
                                                        .Where(m => m.propertyPath == m_propertyPath)
                                                        .FirstOrDefault();
                if (mod == null) return false;
                return PrefabUtility.IsDefaultOverride(mod);
            }
        }
        /// <summary>
        /// Is property part of a Prefab instance? (Read Only)
        /// </summary>
        public bool isInstantiatedPrefab 
        { 
            get 
            {
                if (IsField) return isInstantiatedPrefab;
                if (m_targetObject is not UnityEngine.Object) return false;
                return PrefabUtility.GetPrefabInstanceStatus((UnityEngine.Object)m_targetObject) != PrefabInstanceStatus.NotAPrefab;
            }
        }
        /// <summary>
        /// Allows you to check whether a property's value is overriden (i.e. different to
        /// the Prefab it belongs to).
        /// </summary>
        public bool prefabOverride 
        {
            get
            {
                if (IsField) return m_serializedProperty.prefabOverride;
                if (m_targetObject is not UnityEngine.Object) return false;
                PropertyModification mod = PrefabUtility.GetPropertyModifications((UnityEngine.Object)m_targetObject)
                                                        .Where(m => m.propertyPath == m_propertyPath)
                                                        .FirstOrDefault();
                if (mod == null) return false;
                return PrefabUtility.GetPropertyModifications((UnityEngine.Object)m_targetObject)
                                    .Where(m => m.propertyPath == m_propertyPath)
                                    .FirstOrDefault() != null;
            }
            set
            {
                if (IsField) m_serializedProperty.prefabOverride = value;
                if (m_targetObject is not UnityEngine.Object) return;
                if (value) PrefabUtility.SetPropertyModifications((UnityEngine.Object)m_targetObject, new PropertyModification[] { new PropertyModification() { propertyPath = m_propertyPath } });
                else PrefabUtility.SetPropertyModifications((UnityEngine.Object)m_targetObject, new PropertyModification[] { });
            }
        }
        /// <summary>
        /// Is this property animated? (Read Only)
        /// </summary>
        public bool isAnimated 
        { 
            get 
            {
                if (IsField) return m_serializedProperty.isAnimated;
                if (m_targetObject is not UnityEngine.Object) return false;
                return AnimationMode.IsPropertyAnimated((UnityEngine.Object)m_targetObject, propertyPath);
            }
        }
        /// <summary>
        /// Provides the hash value for the property. (Read Only)
        /// </summary>
        public uint contentHash 
        { 
            get 
            {
                if (IsField) return m_serializedProperty.contentHash;
                return m_contentHash;
            }
        }
        
        //* ////////////////////////////////////////////////////////////////////
        //* Value Properties
        //* ////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Value of a gradient property.
        /// </summary>
        public Gradient gradientValue 
        {
            get
            {
                if (IsField) return m_serializedProperty.gradientValue;
                return (Gradient)m_propertyInfo.GetValue(m_targetObject);
            }
            set
            {
                if (!Attributes.IsActive || Attributes.IsReadOnly) return;
                if (IsField) m_serializedProperty.gradientValue = value;
                else if (!Application.isPlaying) return;
                else m_propertyInfo.SetValue(m_targetObject, value);
            }
        }
        /// <summary>
        /// Value of an object reference property.
        /// </summary>
        public UnityEngine.Object objectReferenceValue 
        {
            get
            {
                if (IsField) return m_serializedProperty.objectReferenceValue;
                if (m_propertyInfo.PropertyType.IsSubclassOf(typeof(UnityEngine.Object)) || m_propertyInfo.PropertyType == typeof(UnityEngine.Object))
                    return (UnityEngine.Object)m_propertyInfo.GetValue(m_targetObject);
                return null;
            }
            set
            {
                if (!Attributes.IsActive || Attributes.IsReadOnly) return;
                if (IsField) m_serializedProperty.objectReferenceValue = value;
                else if (!Application.isPlaying) return;
                else m_propertyInfo.SetValue(m_targetObject, value);
            }
        }
        /// <summary>
        /// Value of a animation curve property.
        /// </summary>
        public AnimationCurve animationCurveValue 
        {
            get
            {
                if (IsField) return m_serializedProperty.animationCurveValue;
                return (AnimationCurve)m_propertyInfo.GetValue(m_targetObject);
            }
            set
            {
                if (!Attributes.IsActive || Attributes.IsReadOnly) return;
                if (IsField) m_serializedProperty.animationCurveValue = value;
                else if (!Application.isPlaying) return;
                else m_propertyInfo.SetValue(m_targetObject, value);
            }
        }
        /// <summary>
        /// Value of a color property.
        /// </summary>
        public Color colorValue 
        {
            get
            {
                if (IsField) return m_serializedProperty.colorValue;
                return (Color)m_propertyInfo.GetValue(m_targetObject);
            }
            set
            {
                if (!Attributes.IsActive || Attributes.IsReadOnly) return;
                if (IsField) m_serializedProperty.colorValue = value;
                else if (!Application.isPlaying) return;
                else m_propertyInfo.SetValue(m_targetObject, value);
            }
        }
        /// <summary>
        /// Value of a string property.
        /// </summary>
        public string stringValue 
        {
            get
            {
                if (IsField) return m_serializedProperty.stringValue;
                return (string)m_propertyInfo.GetValue(m_targetObject);
            }
            set
            {
                if (!Attributes.IsActive || Attributes.IsReadOnly) return;
                if (IsField) m_serializedProperty.stringValue = value;
                else if (!Application.isPlaying) return;
                else m_propertyInfo.SetValue(m_targetObject, value);
            }
        }
        /// <summary>
        /// Value of a float property.
        /// </summary>
        public float floatValue 
        {
            get
            {
                if (IsField) return m_serializedProperty.floatValue;
                return (float)m_propertyInfo.GetValue(m_targetObject);
            }
            set
            {
                if (!Attributes.IsActive || Attributes.IsReadOnly) return;
                if (IsField) m_serializedProperty.floatValue = value;
                else if (!Application.isPlaying) return;
                else m_propertyInfo.SetValue(m_targetObject, value);
            }
        }
        /// <summary>
        /// Value of a boolean property.
        /// </summary>
        public bool boolValue 
        {
            get
            {
                if (IsField) return m_serializedProperty.boolValue;
                return (bool)m_propertyInfo.GetValue(m_targetObject);
            }
            set
            {
                if (!Attributes.IsActive || Attributes.IsReadOnly) return;
                if (IsField) m_serializedProperty.boolValue = value;
                else if (!Application.isPlaying) return;
                else m_propertyInfo.SetValue(m_targetObject, value);
            }
        }
        /// <summary>
        /// Value of an integer property as an unsigned int.
        /// </summary>
        public uint uintValue 
        {
            get
            {
                if (IsField) return m_serializedProperty.uintValue;
                return (uint)m_propertyInfo.GetValue(m_targetObject);
            }
            set
            {
                if (!Attributes.IsActive || Attributes.IsReadOnly) return;
                if (IsField) m_serializedProperty.uintValue = value;
                else if (!Application.isPlaying) return;
                else m_propertyInfo.SetValue(m_targetObject, value);
            }
        }
        /// <summary>
        /// Value of an integer property as an unsigned long.
        /// </summary>
        public ulong ulongValue 
        {
            get
            {
                if (IsField) return m_serializedProperty.ulongValue;
                return (ulong)m_propertyInfo.GetValue(m_targetObject);
            }
            set
            {
                if (!Attributes.IsActive || Attributes.IsReadOnly) return;
                if (IsField) m_serializedProperty.ulongValue = value;
                else if (!Application.isPlaying) return;
                else m_propertyInfo.SetValue(m_targetObject, value);
            }
        }
        /// <summary>
        /// Value of an integer property as a long.
        /// </summary>
        public long longValue 
        {
            get
            {
                if (IsField) return m_serializedProperty.longValue;
                return (long)m_propertyInfo.GetValue(m_targetObject);
            }
            set
            {
                if (!Attributes.IsActive || Attributes.IsReadOnly) return;
                if (IsField) m_serializedProperty.longValue = value;
                else if (!Application.isPlaying) return;
                else m_propertyInfo.SetValue(m_targetObject, value);
            }
        }
        /// <summary>
        /// Value of an integer property.
        /// </summary>
        public int intValue 
        {
            get
            {
                if (IsField) return m_serializedProperty.intValue;
                if (m_propertyInfo.PropertyType == typeof(LayerMask)) return (LayerMask)m_propertyInfo.GetValue(m_targetObject);
                if (m_propertyInfo.PropertyType == typeof(Enum)) return (int)m_propertyInfo.GetValue(m_targetObject);
                return (int)Convert.ChangeType(m_propertyInfo.GetValue(m_targetObject), typeof(int));
            }
            set
            {
                if (!Attributes.IsActive || Attributes.IsReadOnly) return;
                if (IsField) m_serializedProperty.intValue = value;
                else if (!Application.isPlaying) return;
                else if (m_propertyInfo.PropertyType == typeof(Enum)) m_propertyInfo.SetValue(m_targetObject, value);
                else m_propertyInfo.SetValue(m_targetObject, value);
            }
        }
        /// <summary>
        /// Value of a float property as a double.
        /// </summary>
        public double doubleValue 
        {
            get
            {
                if (IsField) return m_serializedProperty.doubleValue;
                return (double)m_propertyInfo.GetValue(m_targetObject);
            }
            set
            {
                if (!Attributes.IsActive || Attributes.IsReadOnly) return;
                if (IsField) m_serializedProperty.doubleValue = value;
                else if (!Application.isPlaying) return;
                else m_propertyInfo.SetValue(m_targetObject, value);
            }
        }
        /// <summary>
        /// Enum index of an enum property.
        /// </summary>
        [Obsolete("Avoid using index to reference enum properties. Use intValue instead.")]
        public int enumValueIndex 
        {
            get
            {
                if (IsField) return m_serializedProperty.enumValueIndex;
                Enum e = (Enum)m_propertyInfo.GetValue(m_targetObject);
                return Convert.ToInt32(e);
            }
            set
            {
                if (!Attributes.IsActive || Attributes.IsReadOnly) return;
                if (IsField) m_serializedProperty.enumValueIndex = value;
                else if (!Application.isPlaying) return;
                else m_propertyInfo.SetValue(m_targetObject, value);
            }
        }
        /// <summary>
        /// Int32 representation of an enum property with Mixed Values.
        /// </summary>
        public int enumValueFlag 
        {
            get
            {
                if (IsField) return m_serializedProperty.enumValueFlag;
                Enum e = (Enum)m_propertyInfo.GetValue(m_targetObject);
                return Convert.ToInt32(e);
            }
            set
            {
                if (!Attributes.IsActive || Attributes.IsReadOnly) return;
                if (IsField) m_serializedProperty.enumValueFlag = value;
                else if (!Application.isPlaying) return;
                else m_propertyInfo.SetValue(m_targetObject, value);
            }
        }
        /// <summary>
        /// Display-friendly names of enumeration of an enum property.
        /// </summary>
        public string[] enumDisplayNames 
        { 
            get 
            {
                if (IsField) return m_serializedProperty.enumDisplayNames;
                return m_propertyInfo.PropertyType.GetEnumNames().Select(x => ObjectNames.NicifyVariableName(x)).ToArray();
            }
        }
        /// <summary>
        /// Names of enumeration of an enum property.
        /// </summary>
        public string[] enumNames 
        { 
            get 
            {
                if (IsField) return m_serializedProperty.enumNames;
                return m_propertyInfo.PropertyType.GetEnumNames();
            }
        }
        /// <summary>
        /// The value of a Hash128 property.
        /// </summary>
        public Hash128 hash128Value 
        {
            get
            {
                if (IsField) return m_serializedProperty.hash128Value;
                return (Hash128)m_propertyInfo.GetValue(m_targetObject);
            }
            set
            {
                if (!Attributes.IsActive || Attributes.IsReadOnly) return;
                if (IsField) m_serializedProperty.hash128Value = value;
                else if (!Application.isPlaying) return;
                else m_propertyInfo.SetValue(m_targetObject, value);
            }
        }
        /// <summary>
        /// Value of bounds with integer values property.
        /// </summary>
        public BoundsInt boundsIntValue 
        {
            get
            {
                if (IsField) return m_serializedProperty.boundsIntValue;
                return (BoundsInt)m_propertyInfo.GetValue(m_targetObject);
            }
            set
            {
                if (!Attributes.IsActive || Attributes.IsReadOnly) return;
                if (IsField) m_serializedProperty.boundsIntValue = value;
                else if (!Application.isPlaying) return;
                else m_propertyInfo.SetValue(m_targetObject, value);
            }
        }
        /// <summary>
        /// Value of bounds property.
        /// </summary>
        public Bounds boundsValue 
        {
            get
            {
                if (IsField) return m_serializedProperty.boundsValue;
                return (Bounds)m_propertyInfo.GetValue(m_targetObject);
            }
            set
            {
                if (!Attributes.IsActive || Attributes.IsReadOnly) return;
                if (IsField) m_serializedProperty.boundsValue = value;
                else if (!Application.isPlaying) return;
                else m_propertyInfo.SetValue(m_targetObject, value);
            }
        }
        /// <summary>
        /// Value of a rectangle with integer values property.
        /// </summary>
        public RectInt rectIntValue 
        {
            get
            {
                if (IsField) return m_serializedProperty.rectIntValue;
                return (RectInt)m_propertyInfo.GetValue(m_targetObject);
            }
            set
            {
                if (!Attributes.IsActive || Attributes.IsReadOnly) return;
                if (IsField) m_serializedProperty.rectIntValue = value;
                else if (!Application.isPlaying) return;
                else m_propertyInfo.SetValue(m_targetObject, value);
            }
        }
        /// <summary>
        /// Value of a rectangle property.
        /// </summary>
        public Rect rectValue 
        {
            get
            {
                if (IsField) return m_serializedProperty.rectValue;
                return (Rect)m_propertyInfo.GetValue(m_targetObject);
            }
            set
            {
                if (!Attributes.IsActive || Attributes.IsReadOnly) return;
                if (IsField) m_serializedProperty.rectValue = value;
                else if (!Application.isPlaying) return;
                else m_propertyInfo.SetValue(m_targetObject, value);
            }
        }
        /// <summary>
        /// Value of a quaternion property.
        /// </summary>
        public Quaternion quaternionValue 
        {
            get
            {
                if (IsField) return m_serializedProperty.quaternionValue;
                return (Quaternion)m_propertyInfo.GetValue(m_targetObject);
            }
            set
            {
                if (!Attributes.IsActive || Attributes.IsReadOnly) return;
                if (IsField) m_serializedProperty.quaternionValue = value;
                else if (!Application.isPlaying) return;
                else m_propertyInfo.SetValue(m_targetObject, value);
            }
        }
        /// <summary>
        /// Value of a 3D integer vector property.
        /// </summary>
        public Vector3Int vector3IntValue 
        {
            get
            {
                if (IsField) return m_serializedProperty.vector3IntValue;
                return (Vector3Int)m_propertyInfo.GetValue(m_targetObject);
            }
            set
            {
                if (!Attributes.IsActive || Attributes.IsReadOnly) return;
                if (IsField) m_serializedProperty.vector3IntValue = value;
                else if (!Application.isPlaying) return;
                else m_propertyInfo.SetValue(m_targetObject, value);
            }
        }
        /// <summary>
        /// Value of a 2D integer vector property.
        /// </summary>
        public Vector2Int vector2IntValue 
        {
            get
            {
                if (IsField) return m_serializedProperty.vector2IntValue;
                return (Vector2Int)m_propertyInfo.GetValue(m_targetObject);
            }
            set
            {
                if (!Attributes.IsActive || Attributes.IsReadOnly) return;
                if (IsField) m_serializedProperty.vector2IntValue = value;
                else if (!Application.isPlaying) return;
                else m_propertyInfo.SetValue(m_targetObject, value);
            }
        }
        /// <summary>
        /// Value of a 4D vector property.
        /// </summary>
        public Vector4 vector4Value 
        {
            get
            {
                if (IsField) return m_serializedProperty.vector4Value;
                return (Vector4)m_propertyInfo.GetValue(m_targetObject);
            }
            set
            {
                if (!Attributes.IsActive || Attributes.IsReadOnly) return;
                if (IsField) m_serializedProperty.vector4Value = value;
                else if (!Application.isPlaying) return;
                else m_propertyInfo.SetValue(m_targetObject, value);
            }
        }
        /// <summary>
        /// Value of a 3D vector property.
        /// </summary>
        public Vector3 vector3Value 
        {
            get
            {
                if (IsField) return m_serializedProperty.vector3Value;
                return (Vector3)m_propertyInfo.GetValue(m_targetObject);
            }
            set
            {
                if (!Attributes.IsActive || Attributes.IsReadOnly) return;
                if (IsField) m_serializedProperty.vector3Value = value;
                else if (!Application.isPlaying) return;
                else m_propertyInfo.SetValue(m_targetObject, value);
            }
        }
        /// <summary>
        /// Value of a 2D vector property.
        /// </summary>
        public Vector2 vector2Value 
        {
            get
            {
                if (IsField) return m_serializedProperty.vector2Value;
                return (Vector2)m_propertyInfo.GetValue(m_targetObject);
            }
            set
            {
                if (!Attributes.IsActive || Attributes.IsReadOnly) return;
                if (IsField) m_serializedProperty.vector2Value = value;
                else if (!Application.isPlaying) return;
                else m_propertyInfo.SetValue(m_targetObject, value);
            }
        }
        /// <summary>
        /// Value of the SerializedProperty, boxed as a System.Object.
        /// </summary>
        public object boxedValue 
        {
            get
            {
                if (IsField) return m_serializedProperty.boxedValue;
                return m_propertyInfo.GetValue(m_targetObject);
            }
            set
            {
                if (!Attributes.IsActive || Attributes.IsReadOnly) return;
                if (IsField) m_serializedProperty.boxedValue = value;
                else if (!Application.isPlaying) return;
                else m_propertyInfo.SetValue(m_targetObject, value);
            }
        }
        /// <summary>
        /// A reference to another Object in the Scene. This reference is resolved in the
        /// context of the SerializedObject containing the SerializedProperty.
        /// </summary>
        public UnityEngine.Object exposedReferenceValue 
        {
            get
            {
                if (IsField) return m_serializedProperty.exposedReferenceValue;
                return (UnityEngine.Object)m_propertyInfo.GetValue(m_targetObject);
            }
            set
            {
                if (!Attributes.IsActive || Attributes.IsReadOnly) return;
                if (IsField) m_serializedProperty.exposedReferenceValue = value;
                else if (!Application.isPlaying) return;
                else m_propertyInfo.SetValue(m_targetObject, value);
            }
        }
        /// <summary>
        /// Object ID of an object reference property.
        /// </summary>
        public int objectReferenceInstanceIDValue 
        {
            get
            {
                if (IsField) return m_serializedProperty.objectReferenceInstanceIDValue;
                UnityEngine.Object o = (UnityEngine.Object)m_propertyInfo.GetValue(m_targetObject);
                return o.GetInstanceID();
            }
            set
            {
                if (!Attributes.IsActive || Attributes.IsReadOnly) return;
                if (IsField) m_serializedProperty.objectReferenceInstanceIDValue = value;
                else if (!Application.isPlaying) return;
                else
                { 
                    UnityEngine.Object o = EditorUtility.InstanceIDToObject(value);
                    if (o != null) m_propertyInfo.SetValue(m_targetObject, o);
                }
            }
        }
        /// <summary>
        /// String corresponding to the value of the managed reference object (dynamic) full
        /// type string.
        /// </summary>
        public string managedReferenceFullTypename { 
            get 
            {
                if (IsField) return m_serializedProperty.managedReferenceFullTypename;
                return m_propertyInfo.PropertyType.FullName;
            }
        }
        /// <summary>
        /// Id associated with a managed reference.
        /// </summary>
        public long managedReferenceId 
        {
            get
            {
                if (IsField) return m_serializedProperty.managedReferenceId;
                ISerializable s = (ISerializable)m_propertyInfo.GetValue(m_targetObject);
                SerializationInfo info = new SerializationInfo(m_propertyInfo.PropertyType, new FormatterConverter());
                StreamingContext context = new StreamingContext();
                s.GetObjectData(info, context);
                return info.GetInt64("$id");
            }
            set
            {
                if (!Attributes.IsActive || Attributes.IsReadOnly) return;
                if (IsField) m_serializedProperty.managedReferenceId = value;
                else if (!Application.isPlaying) return;
                else
                {
                    ISerializable s = (ISerializable)m_propertyInfo.GetValue(m_targetObject);
                    SerializationInfo info = new SerializationInfo(m_propertyInfo.PropertyType, new FormatterConverter());
                    StreamingContext context = new StreamingContext();
                    s.GetObjectData(info, context);
                    info.AddValue("$id", value);
                }
            }
        }
        /// <summary>
        /// The object assigned to a field with SerializeReference attribute.
        /// </summary>
        public object managedReferenceValue 
        {
            get
            {
                if (IsField) return m_serializedProperty.managedReferenceValue;
                ISerializable s = (ISerializable)m_propertyInfo.GetValue(m_targetObject);
                SerializationInfo info = new SerializationInfo(m_propertyInfo.PropertyType, new FormatterConverter());
                StreamingContext context = new StreamingContext();
                s.GetObjectData(info, context);
                return info.GetValue("$data", m_propertyInfo.PropertyType);
            }
            set
            {
                if (!Attributes.IsActive || Attributes.IsReadOnly) return;
                if (IsField) m_serializedProperty.managedReferenceValue = value;
                else if (!Application.isPlaying) return;
                else
                {
                    ISerializable s = (ISerializable)m_propertyInfo.GetValue(m_targetObject);
                    SerializationInfo info = new SerializationInfo(m_propertyInfo.PropertyType, new FormatterConverter());
                    StreamingContext context = new StreamingContext();
                    s.GetObjectData(info, context);
                    info.AddValue("$data", value);
                }
            }
        }
        /// <summary>
        /// String corresponding to the value of the managed reference field full type string.
        /// </summary>
        public string managedReferenceFieldTypename { 
            get 
            {
                if (IsField) return m_serializedProperty.managedReferenceFieldTypename;
                return m_propertyInfo.PropertyType.AssemblyQualifiedName;
            }
        }
        
        //* ////////////////////////////////////////////////////////////////////
        //* Single Value Properties
        //* ////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Value of a gradient property (Single)<br/>
        /// Returns the item's single value (not the multi-editing value).
        /// </summary>
        public Gradient gradientValueSingle 
        {
            get
            {
                if (IsField) return m_serializedPropertySingle.gradientValue;
                return (Gradient)m_propertyInfo.GetValue(m_targetObject);
            }
        }
        /// <summary>
        /// Value of an object reference property (Single)<br/>
        /// Returns the item's single value (not the multi-editing value).
        /// </summary>
        public UnityEngine.Object objectReferenceValueSingle 
        {
            get
            {
                if (IsField) return m_serializedPropertySingle.objectReferenceValue;
                if (m_propertyInfo.PropertyType.IsSubclassOf(typeof(UnityEngine.Object)) || m_propertyInfo.PropertyType == typeof(UnityEngine.Object))
                    return (UnityEngine.Object)m_propertyInfo.GetValue(m_targetObject);
                return null;
            }
        }
        /// <summary>
        /// Value of a animation curve property (Single)<br/>
        /// Returns the item's single value (not the multi-editing value).
        /// </summary>
        public AnimationCurve animationCurveValueSingle 
        {
            get
            {
                if (IsField) return m_serializedPropertySingle.animationCurveValue;
                return (AnimationCurve)m_propertyInfo.GetValue(m_targetObject);
            }
        }
        /// <summary>
        /// Value of a color property (Single)<br/>
        /// Returns the item's single value (not the multi-editing value).
        /// </summary>
        public Color colorValueSingle 
        {
            get
            {
                if (IsField) return m_serializedPropertySingle.colorValue;
                return (Color)m_propertyInfo.GetValue(m_targetObject);
            }
        }
        /// <summary>
        /// Value of a string property (Single)<br/>
        /// Returns the item's single value (not the multi-editing value).
        /// </summary>
        public string stringValueSingle 
        {
            get
            {
                if (IsField) return m_serializedPropertySingle.stringValue;
                return (string)m_propertyInfo.GetValue(m_targetObject);
            }
        }
        /// <summary>
        /// Value of a float property (Single)<br/>
        /// Returns the item's single value (not the multi-editing value).
        /// </summary>
        public float floatValueSingle 
        {
            get
            {
                if (IsField) return m_serializedPropertySingle.floatValue;
                return (float)m_propertyInfo.GetValue(m_targetObject);
            }
        }
        /// <summary>
        /// Value of a boolean property (Single)<br/>
        /// Returns the item's single value (not the multi-editing value).
        /// <summary>
        public bool boolValueSingle 
        {
            get
            {
                if (IsField) return m_serializedPropertySingle.boolValue;
                return (bool)m_propertyInfo.GetValue(m_targetObject);
            }
        }
        /// <summary>
        /// Value of an integer property as an unsigned int (Single)<br/>
        /// Returns the item's single value (not the multi-editing value).
        /// </summary>
        public uint uintValueSingle 
        {
            get
            {
                if (IsField) return m_serializedPropertySingle.uintValue;
                return (uint)m_propertyInfo.GetValue(m_targetObject);
            }
        }
        /// <summary>
        /// Value of an integer property as an unsigned long (Single)<br/>
        /// Returns the item's single value (not the multi-editing value).
        /// </summary>
        public ulong ulongValueSingle 
        {
            get
            {
                if (IsField) return m_serializedPropertySingle.ulongValue;
                return (ulong)m_propertyInfo.GetValue(m_targetObject);
            }
        }
        /// <summary>
        /// Value of an integer property as a long (Single)<br/>
        /// Returns the item's single value (not the multi-editing value).
        /// </summary>
        public long longValueSingle 
        {
            get
            {
                if (IsField) return m_serializedPropertySingle.longValue;
                return (long)m_propertyInfo.GetValue(m_targetObject);
            }
        }
        /// <summary>
        /// Value of an integer property (Single)<br/>
        /// Returns the item's single value (not the multi-editing value).
        /// </summary>
        public int intValueSingle 
        {
            get
            {
                if (IsField) return m_serializedPropertySingle.intValue;
                if (m_propertyInfo.PropertyType == typeof(LayerMask)) return (LayerMask)m_propertyInfo.GetValue(m_targetObject);
                return (int)Convert.ChangeType(m_propertyInfo.GetValue(m_targetObject), typeof(int));
            }
        }
        /// <summary>
        /// Value of a float property as a double (Single)<br/>
        /// Returns the item's single value (not the multi-editing value).
        /// </summary>
        public double doubleValueSingle 
        {
            get
            {
                if (IsField) return m_serializedPropertySingle.doubleValue;
                return (double)m_propertyInfo.GetValue(m_targetObject);
            }
        }
        /// <summary>
        /// Enum index of an enum property (Single)<br/>
        /// Returns the item's single value (not the multi-editing value).
        /// </summary>
        public int enumValueIndexSingle 
        {
            get
            {
                if (IsField) return m_serializedPropertySingle.enumValueIndex;
                Enum e = (Enum)m_propertyInfo.GetValue(m_targetObject);
                return Convert.ToInt32(e);
            }
        }
        /// <summary>
        /// Int32 representation of an enum property with Mixed Values (Single)<br/>
        /// Returns the item's single value (not the multi-editing value).
        /// </summary>
        public int enumValueFlagSingle 
        {
            get
            {
                if (IsField) return m_serializedPropertySingle.enumValueFlag;
                Enum e = (Enum)m_propertyInfo.GetValue(m_targetObject);
                return Convert.ToInt32(e);
            }
        }
        /// <summary>
        /// Display-friendly names of enumeration of an enum property (Single)<br/>
        /// Returns the item's single value (not the multi-editing value).
        /// </summary>
        public string[] enumDisplayNamesSingle 
        { 
            get 
            {
                if (IsField) return m_serializedPropertySingle.enumDisplayNames;
                return m_propertyInfo.PropertyType.GetEnumNames().Select(x => ObjectNames.NicifyVariableName(x)).ToArray();
            }
        }
        /// <summary>
        /// Names of enumeration of an enum property (Single)<br/>
        /// Returns the item's single value (not the multi-editing value).
        /// </summary>
        public string[] enumNamesSingle 
        { 
            get 
            {
                if (IsField) return m_serializedPropertySingle.enumNames;
                return m_propertyInfo.PropertyType.GetEnumNames();
            }
        }
        /// <summary>
        /// The value of a Hash128 property (Single)<br/>
        /// Returns the item's single value (not the multi-editing value).
        /// </summary>
        public Hash128 hash128ValueSingle 
        {
            get
            {
                if (IsField) return m_serializedPropertySingle.hash128Value;
                return (Hash128)m_propertyInfo.GetValue(m_targetObject);
            }
        }
        /// <summary>
        /// Value of bounds with integer values property (Single)<br/>
        /// Returns the item's single value (not the multi-editing value).
        /// </summary>
        public BoundsInt boundsIntValueSingle 
        {
            get
            {
                if (IsField) return m_serializedPropertySingle.boundsIntValue;
                return (BoundsInt)m_propertyInfo.GetValue(m_targetObject);
            }
        }
        /// <summary>
        /// Value of bounds property (Single)<br/>
        /// Returns the item's single value (not the multi-editing value).
        /// </summary>
        public Bounds boundsValueSingle 
        {
            get
            {
                if (IsField) return m_serializedPropertySingle.boundsValue;
                return (Bounds)m_propertyInfo.GetValue(m_targetObject);
            }
        }
        /// <summary>
        /// Value of a rectangle with integer values property (Single)<br/>
        /// Returns the item's single value (not the multi-editing value).
        /// </summary>
        public RectInt rectIntValueSingle 
        {
            get
            {
                if (IsField) return m_serializedPropertySingle.rectIntValue;
                return (RectInt)m_propertyInfo.GetValue(m_targetObject);
            }
        }
        /// <summary>
        /// Value of a rectangle property (Single)<br/>
        /// Returns the item's single value (not the multi-editing value).
        /// </summary>
        public Rect rectValueSingle 
        {
            get
            {
                if (IsField) return m_serializedPropertySingle.rectValue;
                return (Rect)m_propertyInfo.GetValue(m_targetObject);
            }
        }
        /// <summary>
        /// Value of a quaternion property (Single)<br/>
        /// Returns the item's single value (not the multi-editing value).
        /// </summary>
        public Quaternion quaternionValueSingle 
        {
            get
            {
                if (IsField) return m_serializedPropertySingle.quaternionValue;
                return (Quaternion)m_propertyInfo.GetValue(m_targetObject);
            }
        }
        /// <summary>
        /// Value of a 3D integer vector property (Single)<br/>
        /// Returns the item's single value (not the multi-editing value).
        /// </summary>
        public Vector3Int vector3IntValueSingle 
        {
            get
            {
                if (IsField) return m_serializedPropertySingle.vector3IntValue;
                return (Vector3Int)m_propertyInfo.GetValue(m_targetObject);
            }
        }
        /// <summary>
        /// Value of a 2D integer vector property (Single)<br/>
        /// Returns the item's single value (not the multi-editing value).
        /// </summary>
        public Vector2Int vector2IntValueSingle 
        {
            get
            {
                if (IsField) return m_serializedPropertySingle.vector2IntValue;
                return (Vector2Int)m_propertyInfo.GetValue(m_targetObject);
            }
        }
        /// <summary>
        /// Value of a 4D vector property (Single)<br/>
        /// Returns the item's single value (not the multi-editing value).
        /// </summary>
        public Vector4 vector4ValueSingle 
        {
            get
            {
                if (IsField) return m_serializedPropertySingle.vector4Value;
                return (Vector4)m_propertyInfo.GetValue(m_targetObject);
            }
        }
        /// <summary>
        /// Value of a 3D vector property (Single)<br/>
        /// Returns the item's single value (not the multi-editing value).
        /// </summary>
        public Vector3 vector3ValueSingle 
        {
            get
            {
                if (IsField) return m_serializedPropertySingle.vector3Value;
                return (Vector3)m_propertyInfo.GetValue(m_targetObject);
            }
        }
        /// <summary>
        /// Value of a 2D vector property (Single)<br/>
        /// Returns the item's single value (not the multi-editing value).
        /// </summary>
        public Vector2 vector2ValueSingle 
        {
            get
            {
                if (IsField) return m_serializedPropertySingle.vector2Value;
                return (Vector2)m_propertyInfo.GetValue(m_targetObject);
            }
        }
        /// <summary>
        /// Value of the SerializedProperty, boxed as a System.Object (Single)<br/>
        /// Returns the item's single value (not the multi-editing value).
        /// </summary>
        public object boxedValueSingle 
        {
            get
            {
                if (IsField) return m_serializedPropertySingle.boxedValue;
                return m_propertyInfo.GetValue(m_targetObject);
            }
        }
        /// <summary>
        /// A reference to another Object in the Scene. This reference is resolved in the
        /// context of the SerializedObject containing the SerializedProperty (Single)<br/>
        /// Returns the item's single value (not the multi-editing value).
        /// </summary>
        public UnityEngine.Object exposedReferenceValueSingle 
        {
            get
            {
                if (IsField) return m_serializedPropertySingle.exposedReferenceValue;
                return (UnityEngine.Object)m_propertyInfo.GetValue(m_targetObject);
            }
        }
        /// <summary>
        /// Object ID of an object reference property (Single)<br/>
        /// Returns the item's single value (not the multi-editing value).
        /// </summary>
        public int objectReferenceInstanceIDValueSingle 
        {
            get
            {
                if (IsField) return m_serializedPropertySingle.objectReferenceInstanceIDValue;
                UnityEngine.Object o = (UnityEngine.Object)m_propertyInfo.GetValue(m_targetObject);
                return o.GetInstanceID();
            }
        }
        /// <summary>
        /// String corresponding to the value of the managed reference object (dynamic) full
        /// type string (Single)<br/>
        /// Returns the item's single value (not the multi-editing value).
        /// </summary>
        public string managedReferenceFullTypenameSingle { 
            get 
            {
                if (IsField) return m_serializedPropertySingle.managedReferenceFullTypename;
                return m_propertyInfo.PropertyType.FullName;
            }
        }
        /// <summary>
        /// Id associated with a managed reference (Single)<br/>
        /// Returns the item's single value (not the multi-editing value).
        /// </summary>
        public long managedReferenceIdSingle 
        {
            get
            {
                if (IsField) return m_serializedPropertySingle.managedReferenceId;
                ISerializable s = (ISerializable)m_propertyInfo.GetValue(m_targetObject);
                SerializationInfo info = new SerializationInfo(m_propertyInfo.PropertyType, new FormatterConverter());
                StreamingContext context = new StreamingContext();
                s.GetObjectData(info, context);
                return info.GetInt64("$id");
            }
        }
        /// <summary>
        /// The object assigned to a field with SerializeReference attribute (Single)<br/>
        /// Returns the item's single value (not the multi-editing value).
        /// </summary>
        public object managedReferenceValueSingle 
        {
            get
            {
                if (IsField) return m_serializedPropertySingle.managedReferenceValue;
                ISerializable s = (ISerializable)m_propertyInfo.GetValue(m_targetObject);
                SerializationInfo info = new SerializationInfo(m_propertyInfo.PropertyType, new FormatterConverter());
                StreamingContext context = new StreamingContext();
                s.GetObjectData(info, context);
                return info.GetValue("$data", m_propertyInfo.PropertyType);
            }
        }
        /// <summary>
        /// String corresponding to the value of the managed reference field full type string (Single)<br/>
        /// Returns the item's single value (not the multi-editing value).
        /// </summary>
        public string managedReferenceFieldTypenameSingle { 
            get 
            {
                if (IsField) return m_serializedPropertySingle.managedReferenceFieldTypename;
                return m_propertyInfo.PropertyType.AssemblyQualifiedName;
            }
        }
    
        //* ////////////////////////////////////////////////////////////////////
        //* Constructors
        //* ////////////////////////////////////////////////////////////////////
        private SuperSerializedProperty() {}
        internal SuperSerializedProperty(SuperSerializedObject serializedObject, SerializedProperty serializedProperty, SerializedProperty serializedPropertySingle, FieldInfo fieldInfo, UnityEngine.Object obj)
        {
            m_serializedObject = serializedObject;
            m_fieldInfo = fieldInfo;
            m_targetObject = obj;
            m_serializedProperty = serializedProperty;
            m_serializedPropertySingle = serializedPropertySingle;
            m_attributes = new SerializedAttributes(fieldInfo, obj);
            m_propertyPath = fieldInfo.Name;
        }
        internal SuperSerializedProperty(SuperSerializedObject serializedObject, PropertyInfo propertyInfo, UnityEngine.Object obj)
        {
            m_serializedObject = serializedObject;
            m_propertyInfo = propertyInfo;
            m_targetObject = obj;
            m_serializedProperty = null;
            m_attributes = new SerializedAttributes(propertyInfo, obj);
            m_propertyPath = propertyInfo.Name;
            
            // Fake content hash
            m_contentHash = (uint)propertyInfo.GetHashCode();
        }
    
        //* ////////////////////////////////////////////////////////////////////
        //* Internal Methods
        //* ////////////////////////////////////////////////////////////////////
        
        internal SuperPropertyDrawer Drawer(Type type)
        {
            if (m_drawerInstance != null && m_drawerInstance.GetType().IsAssignableFrom(type)) return m_drawerInstance;
            if (!type.IsSubclassOf(typeof(SuperPropertyDrawer))) return null;
            m_drawerInstance = ((SuperPropertyDrawer)Activator.CreateInstance(type)).Init(this);
            return m_drawerInstance;
        }
    
        //* ////////////////////////////////////////////////////////////////////
        //* Public Methods
        //* ////////////////////////////////////////////////////////////////////
        
        [Obsolete("Use SuperSerializedProperty.CopyPropery() instead.")]
        public SerializedProperty Copy() => m_serializedProperty.Copy();
        /// <summary>
        /// Returns a copy of the SerializedProperty iterator in its current state.
        /// </summary>
        public SuperSerializedProperty CopyPropery()
        {
            if (IsField) return new SuperSerializedProperty(m_serializedObject, m_serializedProperty.Copy(), m_serializedProperty.Copy(), m_fieldInfo, m_targetObject);
            return new SuperSerializedProperty(m_serializedObject, m_propertyInfo, m_targetObject);
        }
        
        /// <summary>
        /// Trigger re-evaluation of all condition attributes on this property.
        /// </summary>
        public void Reevaluate()
        {
            Attributes.EvaluateConditions(m_targetObject);
        }
        
        //* ////////////////////////////////////////////////////////////////////
        //* Method-Reflection Methods
        //* ////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// Invoke a method on the target object / objects.
        /// </summary>
        /// <param name="methodName">Name of the method to be invoked</param>
        public void InvokeMethod(string methodName)
        {
            m_serializedObject.InvokeMethod(methodName);
        }
        
        /// <summary>
        /// Invoke a method on a specific target object.
        /// </summary>
        /// <param name="methodName">Name of the method to be invoked</param>
        /// <param name="obj">Target object</param>
        public void InvokeMethod(string methodName, UnityEngine.Object obj)
        {
            m_serializedObject.InvokeMethod(methodName, obj);
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
            return m_serializedObject.InvokeMethod<T>(methodName, parameters);
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
            return m_serializedObject.InvokeMethod<T>(methodName, parameters, obj);
        }
        
        //* ////////////////////////////////////////////////////////////////////
        //* Default Methods
        //* ////////////////////////////////////////////////////////////////////
        
        // Dispose
        public void Dispose()
        {
            if (IsField) m_serializedProperty.Dispose();
        }
        
        //* ////////////////////////////////////////////////////////////////////
        //* Operators
        //* ////////////////////////////////////////////////////////////////////
        
        // Implicit Cast
        public static implicit operator SerializedProperty(SuperSerializedProperty superSerializedProperty) => superSerializedProperty.m_serializedProperty;
    
        //* ////////////////////////////////////////////////////////////////////
        //* Static
        //* ////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// Compares the data for two SerializedProperties. This method ignores paths and
        /// SerializedObjects.
        /// </summary>
        public static bool DataEquals(SerializedProperty x, SerializedProperty y)
        {
            return SerializedProperty.DataEquals(x, y);
        }
        
        /// <summary>
        /// See if contained serialized properties are equal.
        /// </summary>
        public static bool EqualContents(SerializedProperty x, SerializedProperty y)
        {
            return SerializedProperty.EqualContents(x, y);
        }
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
        //! ////////////////////////////////////////////////////////////////////
        //! NOT IMPLEMENTED
        //! ////////////////////////////////////////////////////////////////////
    
        /// <summary>
        /// The smallest number of elements in the array across all target objects. (Read
        /// Only)
        /// </summary>
        public int minArraySize { 
            get 
            {
                if (IsField) return m_serializedProperty.minArraySize;
                return default;
            }
        }
        /// <summary>
        /// The number of elements in the array.
        /// </summary>
        public int arraySize 
        {
            get
            {
                if (IsField) return m_serializedProperty.arraySize;
                return default;
            }
            set
            {
                if (IsField) m_serializedProperty.arraySize = value;
            }
        }
        /// <summary>
        /// Is this property an array? (Read Only)
        /// </summary>
        public bool isArray { 
            get 
            {
                if (IsField) return isArray;
                return default;
            }
        }
        /// <summary>
        /// Does this property represent multiple different values due to multi-object editing?
        /// (Read Only)
        /// </summary>
        public bool hasMultipleDifferentValues { 
            get 
            {
                if (IsField) return hasMultipleDifferentValues;
                return default;
            }
        }
        /// <summary>
        /// Nesting depth of the property. (Read Only)
        /// </summary>
        public int depth { 
            get 
            {
                if (IsField) return m_serializedProperty.depth;
                return default;
            }
        }
        /// <summary>
        /// Does it have child properties? (Read Only)
        /// </summary>
        public bool hasChildren 
        { 
            get 
            {
                if (IsField) return m_serializedProperty.hasChildren;
                return default;
            }
        }
        /// <summary>
        /// Does it have visible child properties? (Read Only)
        /// </summary>
        public bool hasVisibleChildren 
        { 
            get 
            {
                if (IsField) return m_serializedProperty.hasVisibleChildren;
                return default;
            }
        }
        /// <summary>
        /// Type name of the element in an array property. (Read Only)
        /// </summary>
        public string arrayElementType 
        { 
            get 
            {
                if (IsField) return m_serializedProperty.arrayElementType;
                return default;
            }
        }
        /// <summary>
        /// Remove all elements from the array.
        /// </summary>
        public void ClearArray()
        {
            if (IsField) m_serializedProperty.ClearArray();
        }
        /// <summary>
        /// Count visible children of this property, including this property itself.
        /// </summary>
        public int CountInProperty()
        {
            if (IsField) return m_serializedProperty.CountInProperty();
            return default;
        }
        /// <summary>
        /// Count remaining visible properties.
        /// </summary>
        public int CountRemaining()
        {
            if (IsField) return m_serializedProperty.CountRemaining();
            return default;
        }
        /// <summary>
        /// Delete the element at the specified index in the array.
        /// </summary>
        public void DeleteArrayElementAtIndex(int index)
        {
            if (IsField) m_serializedProperty.DeleteArrayElementAtIndex(index);
        }
        /// <summary>
        /// Deletes the array element referenced by the SerializedProperty.
        /// </summary>
        public bool DeleteCommand()
        {
            if (IsField) return m_serializedProperty.DeleteCommand();
            return default;
        }
        /// <summary>
        /// Duplicates the array element referenced by the SerializedProperty.
        /// </summary>
        public bool DuplicateCommand()
        {
            if (IsField) return m_serializedProperty.DuplicateCommand();
            return default;
        }
        /// <summary>
        /// Retrieves the SerializedProperty at a relative path to the current property.
        /// </summary>
        /// <param name="relativePropertyPath"></param>
        public SerializedProperty FindPropertyRelative(string relativePropertyPath)
        {
            if (IsField) return m_serializedProperty.FindPropertyRelative(relativePropertyPath);
            return default;
        }
        /// <summary>
        /// Returns the element at the specified index in the array.
        /// </summary>
        public SerializedProperty GetArrayElementAtIndex(int index)
        {
            if (IsField) return m_serializedProperty.GetArrayElementAtIndex(index);
            return default;
        }
        /// <summary>
        /// Retrieves the SerializedProperty that defines the end range of this property.
        /// </summary>
        public SerializedProperty GetEndProperty()
        {
            if (IsField) return m_serializedProperty.GetEndProperty();
            return default;
        }
        /// <summary>
        /// Retrieves the SerializedProperty that defines the end range of this property.
        /// </summary>
        public SerializedProperty GetEndProperty(bool includeInvisible)
        {
            if (IsField) return m_serializedProperty.GetEndProperty(includeInvisible);
            return default;
        }
        /// <summary>
        /// Retrieves an iterator for enumerating over the visible child properties of the
        /// current property. If the property is an array it will enumerate over the array
        /// elements.
        /// </summary>
        public IEnumerator GetEnumerator()
        {
            if (IsField) return m_serializedProperty.GetEnumerator();
            return default;
        }
        /// <summary>
        /// Returns the element at the specified index in the fixed buffer.
        /// </summary>
        public SerializedProperty GetFixedBufferElementAtIndex(int index)
        {
            if (IsField) return m_serializedProperty.GetFixedBufferElementAtIndex(index);
            return default;
        }
        /// <summary>
        /// Insert an new element at the specified index in the array.
        /// </summary>
        public void InsertArrayElementAtIndex(int index)
        {
            if (IsField) m_serializedProperty.InsertArrayElementAtIndex(index);
        }
        /// <summary>
        /// Move an array element from srcIndex to dstIndex.
        /// </summary>
        public bool MoveArrayElement(int srcIndex, int dstIndex)
        {
            if (IsField) return m_serializedProperty.MoveArrayElement(srcIndex, dstIndex);
            return default;
        }
        /// <summary>
        /// Move to next property.
        /// </summary>
        public bool Next(bool enterChildren)
        {
            if (IsField) return m_serializedProperty.Next(enterChildren);
            return default;
        }
        /// <summary>
        /// Move to next visible property.
        /// </summary>
        public bool NextVisible(bool enterChildren)
        {
            if (IsField) return m_serializedProperty.NextVisible(enterChildren);
            return default;
        }
        /// <summary>
        /// Move to first property of the object.
        /// </summary>
        public void Reset()
        {
            if (IsField) m_serializedProperty.Reset();
        }
    }
}