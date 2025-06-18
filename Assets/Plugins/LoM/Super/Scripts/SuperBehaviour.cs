using UnityEngine;

namespace LoM.Super
{
    /// <summary>
    /// SuperBehaviour is a MonoBehaviour that provides some additional functionality.<br/>
    /// It provides a cached version of the transform and gameObject properties.<br/>
    /// It also enables the use of SuperEditor and SerializedProperties.
    /// </summary>
    [SuperIcon(SuperBehaviourIcon.Default)]
    public class SuperBehaviour : MonoBehaviour 
    {
        // Member Variables
        private Transform m_transform;
        private RectTransform m_rectTransform;
        private GameObject m_gameObject;
        
        // Serialized Fields
        [SerializeField] 
        [HideInInspector]
        private bool m_ShowProperties = false;
        
        // Properties
        /// <summary>
        /// Defines if the SerializedProperties of this SuperBehaviour should be shown in the inspector.
        /// </summary>
        public bool ShowProperties { get => m_ShowProperties; set => m_ShowProperties = value; }
        
        /// <summary>
        /// The Transform attached to this SuperBehaviour.<br/>
        /// This is a cached version of the transform property.
        /// </summary>
        public new Transform transform
        {
            get
            {
                if (IsDestroyed) return null;
                try
                {
                    if (m_transform == null)
                    {
                        m_transform = base.transform;
                    }
                    return m_transform;
                }
                catch
                {
                    // In case unity uses they're null override to hide the object still exists
                    return null;
                }
            }
        }
        
        /// <summary>
        /// The GameObject attached to this SuperBehaviour.<br/>
        /// This is a cached version of the gameObject property.
        /// </summary>
        public new GameObject gameObject 
        {
            get
            {
                if (IsDestroyed) return null;
                try
                {
                    if (m_gameObject == null)
                    {
                        m_gameObject = base.gameObject;
                    }
                    return m_gameObject;
                }
                catch
                {
                    // In case unity uses they're null override to hide the object still exists
                    return null;
                }
            }
        }
        
        /// <summary>
        /// The SuperRectTransform attached to this SuperBehaviour.<br/>
        /// This is a cached version of the rectTransform property.<br/>
        /// This will return null if the SuperBehaviour is not attached to a RectTransform.
        /// </summary>
        public SuperRectTransform rectTransform
        {
            get
            {
                if (IsDestroyed) return null;
                try
                {
                    if (m_rectTransform == null)
                    {
                        TryGetComponent<RectTransform>(out var rectTransform);
                        m_rectTransform = rectTransform ? (SuperRectTransform)rectTransform : null;
                    }
                    return m_rectTransform;
                }
                catch
                {
                    // In case unity uses they're null override to hide the object still exists
                    return null;
                }
            }
        }
        
        /// <summary>
        /// Returns if this instance has been marked as destroyed by Unity.
        /// </summary>
        /// <returns>True if the instance has been destroyed, false otherwise.</returns>
        public bool IsDestroyed => this == null;
    }
}