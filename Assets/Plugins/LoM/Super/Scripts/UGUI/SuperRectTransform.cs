using System;
using UnityEngine;

namespace LoM.Super
{
    /// <summary>
    /// Represents a UGUI RectTransform<br/>
    /// This class is used to simplify the access to the RectTransform of a UGUI GameObject, it provides a more intuitive way to access the RectTransform properties as they behave exactly like in the Inspector.
    /// <hr/>
    /// <example>
    /// To use the SuperRectTransform, simply cast the RectTransform to a SuperRectTransform and or use the AsSuperRectTransform extension method.<br/>
    /// <code>
    /// // Stretch All
    /// SuperRectTransform transform = GetComponent&lt;RectTransform&gt;();
    /// transform.Left = 10;
    /// transform.Right = 10;
    /// transform.Bottom = 10;
    /// transform.Top = 10;
    /// 
    /// // Center
    /// SuperRectTransform transform = GetComponent&lt;RectTransform&gt;();
    /// transform.PosX = 10;
    /// transform.PosY = 10;
    /// transform.Width = 100;
    /// transform.Height = 100;
    /// </code>
    /// </example>
    /// <hr/>
    /// </summary>
    public class SuperRectTransform
    {
        // Member Variables
        private RectTransform m_rectTransform;
        
        /// <summary>
        /// Distance from the left edge of the parent RectTransform
        /// </summary>
        public float Left
        {
            get
            {
                switch (Anchor)
                {
                    case UGUIAnchor.TopLeft: return m_rectTransform.offsetMin.x;
                    case UGUIAnchor.TopCenter: return ParentRect.rect.width / 2 + m_rectTransform.offsetMin.x;
                    case UGUIAnchor.TopRight: return ParentRect.rect.width + m_rectTransform.offsetMin.x;
                    case UGUIAnchor.MiddleLeft: return m_rectTransform.offsetMin.x;
                    case UGUIAnchor.MiddleCenter:  return ParentRect.rect.width / 2 + m_rectTransform.offsetMin.x;
                    case UGUIAnchor.MiddleRight: return ParentRect.rect.width + m_rectTransform.offsetMin.x;
                    case UGUIAnchor.BottomLeft: return m_rectTransform.offsetMin.x;
                    case UGUIAnchor.BottomCenter: return ParentRect.rect.width / 2 + m_rectTransform.offsetMin.x;
                    case UGUIAnchor.BottomRight: return ParentRect.rect.width + m_rectTransform.offsetMin.x;
                    case UGUIAnchor.TopStretch: return m_rectTransform.offsetMin.x;
                    case UGUIAnchor.MiddleStretch: return m_rectTransform.offsetMin.x;
                    case UGUIAnchor.BottomStretch: return m_rectTransform.offsetMin.x;
                    case UGUIAnchor.StretchLeft: return m_rectTransform.offsetMin.x;
                    case UGUIAnchor.StretchCenter: return ParentRect.rect.width / 2 + m_rectTransform.offsetMin.x;
                    case UGUIAnchor.StretchRight: return ParentRect.rect.width + m_rectTransform.offsetMin.x;
                    case UGUIAnchor.StretchAll: return m_rectTransform.offsetMin.x;
                    default: throw new ArgumentOutOfRangeException($"Unknown Anchor Preset: {Anchor}");
                }
            }
            set
            {
                switch (Anchor)
                {
                    case UGUIAnchor.TopLeft: Debug.LogWarning("Setting the Left of a TopLeft RectTransform is not supported"); break;
                    case UGUIAnchor.TopCenter: Debug.LogWarning("Setting the Left of a TopCenter RectTransform is not supported"); break;
                    case UGUIAnchor.TopRight: Debug.LogWarning("Setting the Left of a TopRight RectTransform is not supported"); break;
                    case UGUIAnchor.MiddleLeft: Debug.LogWarning("Setting the Left of a MiddleLeft RectTransform is not supported"); break;
                    case UGUIAnchor.MiddleCenter: Debug.LogWarning("Setting the Left of a Center RectTransform is not supported"); break;
                    case UGUIAnchor.MiddleRight: Debug.LogWarning("Setting the Left of a MiddleRight RectTransform is not supported"); break;
                    case UGUIAnchor.BottomLeft: Debug.LogWarning("Setting the Left of a BottomLeft RectTransform is not supported"); break;
                    case UGUIAnchor.BottomCenter: Debug.LogWarning("Setting the Left of a BottomCenter RectTransform is not supported"); break;
                    case UGUIAnchor.BottomRight: Debug.LogWarning("Setting the Left of a BottomRight RectTransform is not supported"); break;
                    case UGUIAnchor.TopStretch: m_rectTransform.offsetMin = new Vector2(value, m_rectTransform.offsetMin.y); break;
                    case UGUIAnchor.MiddleStretch: m_rectTransform.offsetMin = new Vector2(value, m_rectTransform.offsetMin.y); break;
                    case UGUIAnchor.BottomStretch: m_rectTransform.offsetMin = new Vector2(value, m_rectTransform.offsetMin.y); break;
                    case UGUIAnchor.StretchLeft: Debug.LogWarning("Setting the Left of a StretchLeft RectTransform is not supported"); break;
                    case UGUIAnchor.StretchCenter: Debug.LogWarning("Setting the Left of a StretchCenter RectTransform is not supported"); break;
                    case UGUIAnchor.StretchRight: Debug.LogWarning("Setting the Left of a StretchRight RectTransform is not supported"); break;
                    case UGUIAnchor.StretchAll: m_rectTransform.offsetMin = new Vector2(value, m_rectTransform.offsetMin.y); break;
                    default: throw new ArgumentOutOfRangeException($"Unknown Anchor Preset: {Anchor}");
                }
            
            }
        }
        
        /// <summary>
        /// Distance from the right edge of the parent RectTransform
        /// </summary>
        public float Right
        {
            get
            {
                switch (Anchor)
                {
                    case UGUIAnchor.TopLeft: return ParentRect.rect.width - m_rectTransform.offsetMax.x;
                    case UGUIAnchor.TopCenter: return ParentRect.rect.width / 2 - m_rectTransform.offsetMax.x;
                    case UGUIAnchor.TopRight: return -m_rectTransform.offsetMax.x;
                    case UGUIAnchor.MiddleLeft: return ParentRect.rect.width - m_rectTransform.offsetMax.x;
                    case UGUIAnchor.MiddleCenter: return ParentRect.rect.width / 2 - m_rectTransform.offsetMax.x;
                    case UGUIAnchor.MiddleRight: return -m_rectTransform.offsetMax.x;
                    case UGUIAnchor.BottomLeft: return ParentRect.rect.width - m_rectTransform.offsetMax.x;
                    case UGUIAnchor.BottomCenter: return ParentRect.rect.width / 2 - m_rectTransform.offsetMax.x;
                    case UGUIAnchor.BottomRight: return -m_rectTransform.offsetMax.x;
                    case UGUIAnchor.TopStretch: return -m_rectTransform.offsetMax.x;
                    case UGUIAnchor.MiddleStretch: return -m_rectTransform.offsetMax.x;
                    case UGUIAnchor.BottomStretch: return -m_rectTransform.offsetMax.x;
                    case UGUIAnchor.StretchLeft: return ParentRect.rect.width - m_rectTransform.offsetMax.x;
                    case UGUIAnchor.StretchCenter: return ParentRect.rect.width / 2 - m_rectTransform.offsetMax.x;
                    case UGUIAnchor.StretchRight: return -m_rectTransform.offsetMax.x;
                    case UGUIAnchor.StretchAll: return -m_rectTransform.offsetMax.x;
                    default: throw new ArgumentOutOfRangeException($"Unknown Anchor Preset: {Anchor}");
                }
            }
            set
            {
                switch (Anchor)
                {
                    case UGUIAnchor.TopLeft: Debug.LogWarning("Setting the Right of a TopLeft RectTransform is not supported"); break;
                    case UGUIAnchor.TopCenter: Debug.LogWarning("Setting the Right of a TopCenter RectTransform is not supported"); break;
                    case UGUIAnchor.TopRight: Debug.LogWarning("Setting the Right of a TopRight RectTransform is not supported"); break;
                    case UGUIAnchor.MiddleLeft: Debug.LogWarning("Setting the Right of a MiddleLeft RectTransform is not supported"); break;
                    case UGUIAnchor.MiddleCenter: Debug.LogWarning("Setting the Right of a Center RectTransform is not supported"); break;
                    case UGUIAnchor.MiddleRight: Debug.LogWarning("Setting the Right of a MiddleRight RectTransform is not supported"); break;
                    case UGUIAnchor.BottomLeft: Debug.LogWarning("Setting the Right of a BottomLeft RectTransform is not supported"); break;
                    case UGUIAnchor.BottomCenter: Debug.LogWarning("Setting the Right of a BottomCenter RectTransform is not supported"); break;
                    case UGUIAnchor.BottomRight: Debug.LogWarning("Setting the Right of a BottomRight RectTransform is not supported"); break;
                    case UGUIAnchor.TopStretch: m_rectTransform.offsetMax = new Vector2(-value, m_rectTransform.offsetMax.y); break;
                    case UGUIAnchor.MiddleStretch: m_rectTransform.offsetMax = new Vector2(-value, m_rectTransform.offsetMax.y); break;
                    case UGUIAnchor.BottomStretch: m_rectTransform.offsetMax = new Vector2(-value, m_rectTransform.offsetMax.y); break;
                    case UGUIAnchor.StretchLeft: Debug.LogWarning("Setting the Right of a StretchLeft RectTransform is not supported"); break;
                    case UGUIAnchor.StretchCenter: Debug.LogWarning("Setting the Right of a StretchCenter RectTransform is not supported"); break;
                    case UGUIAnchor.StretchRight: m_rectTransform.offsetMax = new Vector2(-value, m_rectTransform.offsetMax.y); break;
                    case UGUIAnchor.StretchAll: m_rectTransform.offsetMax = new Vector2(-value, m_rectTransform.offsetMax.y); break;
                    default: throw new ArgumentOutOfRangeException($"Unknown Anchor Preset: {Anchor}");
                }
            
            }
        }
        
        /// <summary>
        /// Distance from the bottom edge of the parent RectTransform
        /// </summary>
        public float Bottom
        {
            get
            {
                switch (Anchor)
                {
                    case UGUIAnchor.TopLeft: return ParentRect.rect.height + m_rectTransform.offsetMax.y - m_rectTransform.rect.height;
                    case UGUIAnchor.TopCenter: return ParentRect.rect.height + m_rectTransform.offsetMax.y - m_rectTransform.rect.height;
                    case UGUIAnchor.TopRight: return ParentRect.rect.height + m_rectTransform.offsetMax.y - m_rectTransform.rect.height;
                    case UGUIAnchor.MiddleLeft: return ParentRect.rect.height / 2 + m_rectTransform.offsetMin.y;
                    case UGUIAnchor.MiddleCenter: return ParentRect.rect.height / 2 + m_rectTransform.offsetMin.y;
                    case UGUIAnchor.MiddleRight: return ParentRect.rect.height / 2 + m_rectTransform.offsetMin.y;
                    case UGUIAnchor.BottomLeft: return m_rectTransform.offsetMin.y;
                    case UGUIAnchor.BottomCenter: return m_rectTransform.offsetMin.y;
                    case UGUIAnchor.BottomRight: return m_rectTransform.offsetMin.y;
                    case UGUIAnchor.TopStretch: return ParentRect.rect.height + m_rectTransform.offsetMax.y - m_rectTransform.rect.height;
                    case UGUIAnchor.MiddleStretch: return ParentRect.rect.height / 2 + m_rectTransform.offsetMin.y;
                    case UGUIAnchor.BottomStretch: return m_rectTransform.offsetMin.y;
                    case UGUIAnchor.StretchLeft: return m_rectTransform.offsetMin.y;
                    case UGUIAnchor.StretchCenter: return m_rectTransform.offsetMin.y;
                    case UGUIAnchor.StretchRight: return m_rectTransform.offsetMin.y;
                    case UGUIAnchor.StretchAll: return m_rectTransform.offsetMin.y;
                    default: throw new ArgumentOutOfRangeException($"Unknown Anchor Preset: {Anchor}");
                }
            }
            set
            {
                switch (Anchor)
                {
                    case UGUIAnchor.TopLeft: Debug.LogWarning("Setting the Bottom of a TopLeft RectTransform is not supported"); break;
                    case UGUIAnchor.TopCenter: Debug.LogWarning("Setting the Bottom of a TopCenter RectTransform is not supported"); break;
                    case UGUIAnchor.TopRight: Debug.LogWarning("Setting the Bottom of a TopRight RectTransform is not supported"); break;
                    case UGUIAnchor.MiddleLeft: Debug.LogWarning("Setting the Bottom of a MiddleLeft RectTransform is not supported"); break;
                    case UGUIAnchor.MiddleCenter: Debug.LogWarning("Setting the Bottom of a Center RectTransform is not supported"); break;
                    case UGUIAnchor.MiddleRight: Debug.LogWarning("Setting the Bottom of a MiddleRight RectTransform is not supported"); break;
                    case UGUIAnchor.BottomLeft: Debug.LogWarning("Setting the Bottom of a BottomLeft RectTransform is not supported"); break;
                    case UGUIAnchor.BottomCenter: Debug.LogWarning("Setting the Bottom of a BottomCenter RectTransform is not supported"); break;
                    case UGUIAnchor.BottomRight: Debug.LogWarning("Setting the Bottom of a BottomRight RectTransform is not supported"); break;
                    case UGUIAnchor.TopStretch: Debug.LogWarning("Setting the Bottom of a TopStretch RectTransform is not supported"); break;
                    case UGUIAnchor.MiddleStretch: Debug.LogWarning("Setting the Bottom of a MiddleStretch RectTransform is not supported"); break;
                    case UGUIAnchor.BottomStretch: m_rectTransform.offsetMin = new Vector2(m_rectTransform.offsetMin.x, value); break;
                    case UGUIAnchor.StretchLeft: m_rectTransform.offsetMin = new Vector2(m_rectTransform.offsetMin.x, value); break;
                    case UGUIAnchor.StretchCenter: m_rectTransform.offsetMin = new Vector2(m_rectTransform.offsetMin.x, value); break;
                    case UGUIAnchor.StretchRight: m_rectTransform.offsetMin = new Vector2(m_rectTransform.offsetMin.x, value); break;
                    case UGUIAnchor.StretchAll: m_rectTransform.offsetMin = new Vector2(m_rectTransform.offsetMin.x, value); break;
                    default: throw new ArgumentOutOfRangeException($"Unknown Anchor Preset: {Anchor}");
                }
            
            }
        }
        
        /// <summary>
        /// Distance from the top edge of the parent RectTransform
        /// </summary>
        public float Top
        {
            get
            {
                switch (Anchor)
                {
                    case UGUIAnchor.TopLeft: return -m_rectTransform.offsetMax.y;
                    case UGUIAnchor.TopCenter: return -m_rectTransform.offsetMax.y;
                    case UGUIAnchor.TopRight: return -m_rectTransform.offsetMax.y;
                    case UGUIAnchor.MiddleLeft: return ParentRect.rect.height / 2 - m_rectTransform.offsetMax.y;
                    case UGUIAnchor.MiddleCenter: return ParentRect.rect.height / 2 - m_rectTransform.offsetMax.y;
                    case UGUIAnchor.MiddleRight: return ParentRect.rect.height / 2 - m_rectTransform.offsetMax.y;
                    case UGUIAnchor.BottomLeft: return ParentRect.rect.height - m_rectTransform.offsetMin.y - m_rectTransform.rect.height;
                    case UGUIAnchor.BottomCenter: return ParentRect.rect.height - m_rectTransform.offsetMin.y - m_rectTransform.rect.height;
                    case UGUIAnchor.BottomRight: return ParentRect.rect.height - m_rectTransform.offsetMin.y - m_rectTransform.rect.height;
                    case UGUIAnchor.TopStretch: return -m_rectTransform.offsetMax.y;
                    case UGUIAnchor.MiddleStretch: return ParentRect.rect.height / 2 - m_rectTransform.offsetMax.y;
                    case UGUIAnchor.BottomStretch: return ParentRect.rect.height - m_rectTransform.offsetMin.y - m_rectTransform.rect.height;
                    case UGUIAnchor.StretchLeft: return -m_rectTransform.offsetMax.y;
                    case UGUIAnchor.StretchCenter: return -m_rectTransform.offsetMax.y;
                    case UGUIAnchor.StretchRight: return -m_rectTransform.offsetMax.y;
                    case UGUIAnchor.StretchAll: return -m_rectTransform.offsetMax.y;
                    default: throw new ArgumentOutOfRangeException($"Unknown Anchor Preset: {Anchor}");
                }
            }
            set
            {
                switch (Anchor)
                {
                    case UGUIAnchor.TopLeft: Debug.LogWarning("Setting the Top of a TopLeft RectTransform is not supported"); break;
                    case UGUIAnchor.TopCenter: Debug.LogWarning("Setting the Top of a TopCenter RectTransform is not supported"); break;
                    case UGUIAnchor.TopRight: Debug.LogWarning("Setting the Top of a TopRight RectTransform is not supported"); break;
                    case UGUIAnchor.MiddleLeft: Debug.LogWarning("Setting the Top of a MiddleLeft RectTransform is not supported"); break;
                    case UGUIAnchor.MiddleCenter: Debug.LogWarning("Setting the Top of a Center RectTransform is not supported"); break;
                    case UGUIAnchor.MiddleRight: Debug.LogWarning("Setting the Top of a MiddleRight RectTransform is not supported"); break;
                    case UGUIAnchor.BottomLeft: Debug.LogWarning("Setting the Top of a BottomLeft RectTransform is not supported"); break;
                    case UGUIAnchor.BottomCenter: Debug.LogWarning("Setting the Top of a BottomCenter RectTransform is not supported"); break;
                    case UGUIAnchor.BottomRight: Debug.LogWarning("Setting the Top of a BottomRight RectTransform is not supported"); break;
                    case UGUIAnchor.TopStretch: m_rectTransform.offsetMax = new Vector2(m_rectTransform.offsetMax.x, -value); break;
                    case UGUIAnchor.MiddleStretch: Debug.LogWarning("Setting the Top of a MiddleStretch RectTransform is not supported"); break;
                    case UGUIAnchor.BottomStretch: Debug.LogWarning("Setting the Top of a MiddleStretch RectTransform is not supported"); break;
                    case UGUIAnchor.StretchLeft: m_rectTransform.offsetMax = new Vector2(m_rectTransform.offsetMax.x, -value); break;
                    case UGUIAnchor.StretchCenter: m_rectTransform.offsetMax = new Vector2(m_rectTransform.offsetMax.x, -value); break;
                    case UGUIAnchor.StretchRight: m_rectTransform.offsetMax = new Vector2(m_rectTransform.offsetMax.x, -value); break;
                    case UGUIAnchor.StretchAll: m_rectTransform.offsetMax = new Vector2(m_rectTransform.offsetMax.x, -value); break;
                    default: throw new ArgumentOutOfRangeException($"Unknown Anchor Preset: {Anchor}");
                }
            
            }
        }
        
        /// <summary>
        /// X-Position of the RectTransform
        /// </summary>
        public float PosX
        {
            get
            {
                switch (Anchor)
                {
                    case UGUIAnchor.TopLeft: return m_rectTransform.anchoredPosition.x;
                    case UGUIAnchor.TopCenter: return m_rectTransform.anchoredPosition.x;
                    case UGUIAnchor.TopRight: return m_rectTransform.anchoredPosition.x;
                    case UGUIAnchor.MiddleLeft: return m_rectTransform.anchoredPosition.x;
                    case UGUIAnchor.MiddleCenter: return m_rectTransform.anchoredPosition.x;
                    case UGUIAnchor.MiddleRight: return m_rectTransform.anchoredPosition.x;
                    case UGUIAnchor.BottomLeft: return m_rectTransform.anchoredPosition.x;
                    case UGUIAnchor.BottomCenter: return m_rectTransform.anchoredPosition.x;
                    case UGUIAnchor.BottomRight: return m_rectTransform.anchoredPosition.x;
                    case UGUIAnchor.TopStretch: return Left;
                    case UGUIAnchor.MiddleStretch: return Left;
                    case UGUIAnchor.BottomStretch: return Left;
                    case UGUIAnchor.StretchLeft: return m_rectTransform.anchoredPosition.x;
                    case UGUIAnchor.StretchCenter: return m_rectTransform.anchoredPosition.x;
                    case UGUIAnchor.StretchRight: return m_rectTransform.anchoredPosition.x;
                    case UGUIAnchor.StretchAll: return Left;
                    default: throw new ArgumentOutOfRangeException($"Unknown Anchor Preset: {Anchor}");
                }
            }
            set
            {
                switch (Anchor)
                {
                    case UGUIAnchor.TopLeft: m_rectTransform.anchoredPosition = new Vector2(value, m_rectTransform.anchoredPosition.y); break;
                    case UGUIAnchor.TopCenter: m_rectTransform.anchoredPosition = new Vector2(value, m_rectTransform.anchoredPosition.y); break;
                    case UGUIAnchor.TopRight: m_rectTransform.anchoredPosition = new Vector2(value, m_rectTransform.anchoredPosition.y); break;
                    case UGUIAnchor.MiddleLeft: m_rectTransform.anchoredPosition = new Vector2(value, m_rectTransform.anchoredPosition.y); break;
                    case UGUIAnchor.MiddleCenter: m_rectTransform.anchoredPosition = new Vector2(value, m_rectTransform.anchoredPosition.y); break;
                    case UGUIAnchor.MiddleRight: m_rectTransform.anchoredPosition = new Vector2(value, m_rectTransform.anchoredPosition.y); break;
                    case UGUIAnchor.BottomLeft: m_rectTransform.anchoredPosition = new Vector2(value, m_rectTransform.anchoredPosition.y); break;
                    case UGUIAnchor.BottomCenter: m_rectTransform.anchoredPosition = new Vector2(value, m_rectTransform.anchoredPosition.y); break;
                    case UGUIAnchor.BottomRight: m_rectTransform.anchoredPosition = new Vector2(value, m_rectTransform.anchoredPosition.y); break;
                    case UGUIAnchor.TopStretch: Left = value; break;
                    case UGUIAnchor.MiddleStretch: Left = value; break;
                    case UGUIAnchor.BottomStretch: Left = value; break;
                    case UGUIAnchor.StretchLeft: m_rectTransform.anchoredPosition = new Vector2(value, m_rectTransform.anchoredPosition.y); break;
                    case UGUIAnchor.StretchCenter: m_rectTransform.anchoredPosition = new Vector2(value, m_rectTransform.anchoredPosition.y); break;
                    case UGUIAnchor.StretchRight: m_rectTransform.anchoredPosition = new Vector2(value, m_rectTransform.anchoredPosition.y); break;
                    case UGUIAnchor.StretchAll: Left = value; break;
                    default: throw new ArgumentOutOfRangeException($"Unknown Anchor Preset: {Anchor}");
                }
            
            }
        }
        
        /// <summary>
        /// Y-Position of the RectTransform
        /// </summary>
        public float PosY
        {
            get
            {
                switch (Anchor)
                {
                    case UGUIAnchor.TopLeft: return m_rectTransform.anchoredPosition.y;
                    case UGUIAnchor.TopCenter: return m_rectTransform.anchoredPosition.y;
                    case UGUIAnchor.TopRight: return m_rectTransform.anchoredPosition.y;
                    case UGUIAnchor.MiddleLeft: return m_rectTransform.anchoredPosition.y;
                    case UGUIAnchor.MiddleCenter: return m_rectTransform.anchoredPosition.y;
                    case UGUIAnchor.MiddleRight: return m_rectTransform.anchoredPosition.y;
                    case UGUIAnchor.BottomLeft: return m_rectTransform.anchoredPosition.y;
                    case UGUIAnchor.BottomCenter: return m_rectTransform.anchoredPosition.y;
                    case UGUIAnchor.BottomRight: return m_rectTransform.anchoredPosition.y;
                    case UGUIAnchor.TopStretch: return m_rectTransform.anchoredPosition.y;
                    case UGUIAnchor.MiddleStretch: return m_rectTransform.anchoredPosition.y;
                    case UGUIAnchor.BottomStretch: return m_rectTransform.anchoredPosition.y;
                    case UGUIAnchor.StretchLeft: return Bottom;
                    case UGUIAnchor.StretchCenter: return Bottom;
                    case UGUIAnchor.StretchRight: return Bottom;
                    case UGUIAnchor.StretchAll: return Bottom;
                    default: throw new ArgumentOutOfRangeException($"Unknown Anchor Preset: {Anchor}");
                }
            }
            set
            {
                switch (Anchor)
                {
                    case UGUIAnchor.TopLeft: m_rectTransform.anchoredPosition = new Vector2(m_rectTransform.anchoredPosition.x, value); break;
                    case UGUIAnchor.TopCenter: m_rectTransform.anchoredPosition = new Vector2(m_rectTransform.anchoredPosition.x, value); break;
                    case UGUIAnchor.TopRight: m_rectTransform.anchoredPosition = new Vector2(m_rectTransform.anchoredPosition.x, value); break;
                    case UGUIAnchor.MiddleLeft: m_rectTransform.anchoredPosition = new Vector2(m_rectTransform.anchoredPosition.x, value); break;
                    case UGUIAnchor.MiddleCenter: m_rectTransform.anchoredPosition = new Vector2(m_rectTransform.anchoredPosition.x, value); break;
                    case UGUIAnchor.MiddleRight: m_rectTransform.anchoredPosition = new Vector2(m_rectTransform.anchoredPosition.x, value); break;
                    case UGUIAnchor.BottomLeft: m_rectTransform.anchoredPosition = new Vector2(m_rectTransform.anchoredPosition.x, value); break;
                    case UGUIAnchor.BottomCenter: m_rectTransform.anchoredPosition = new Vector2(m_rectTransform.anchoredPosition.x, value); break;
                    case UGUIAnchor.BottomRight: m_rectTransform.anchoredPosition = new Vector2(m_rectTransform.anchoredPosition.x, value); break;
                    case UGUIAnchor.TopStretch: m_rectTransform.anchoredPosition = new Vector2(m_rectTransform.anchoredPosition.x, value); break;
                    case UGUIAnchor.MiddleStretch: m_rectTransform.anchoredPosition = new Vector2(m_rectTransform.anchoredPosition.x, value); break;
                    case UGUIAnchor.BottomStretch: m_rectTransform.anchoredPosition = new Vector2(m_rectTransform.anchoredPosition.x, value); break;
                    case UGUIAnchor.StretchLeft: Bottom = value; break;
                    case UGUIAnchor.StretchCenter: Bottom = value; break;
                    case UGUIAnchor.StretchRight: Bottom = value; break;
                    case UGUIAnchor.StretchAll: Bottom = value; break;
                    default: throw new ArgumentOutOfRangeException($"Unknown Anchor Preset: {Anchor}");
                }
            
            }
        }
        
        /// <summary>
        /// Z-Position of the RectTransform
        /// </summary>
        public float PosZ
        {
            get => m_rectTransform.position.z;
            set => m_rectTransform.position = new Vector3(m_rectTransform.position.x, m_rectTransform.position.y, value);
        }
        
        /// <summary>
        /// Width of the RectTransform
        /// </summary>
        public float Width
        {
            get
            {
                switch (Anchor)
                {
                    case UGUIAnchor.TopLeft: return m_rectTransform.sizeDelta.x;
                    case UGUIAnchor.TopCenter: return m_rectTransform.sizeDelta.x;
                    case UGUIAnchor.TopRight: return m_rectTransform.sizeDelta.x;
                    case UGUIAnchor.MiddleLeft: return m_rectTransform.sizeDelta.x;
                    case UGUIAnchor.MiddleCenter: return m_rectTransform.sizeDelta.x;
                    case UGUIAnchor.MiddleRight: return m_rectTransform.sizeDelta.x;
                    case UGUIAnchor.BottomLeft: return m_rectTransform.sizeDelta.x;
                    case UGUIAnchor.BottomCenter: return m_rectTransform.sizeDelta.x;
                    case UGUIAnchor.BottomRight: return m_rectTransform.sizeDelta.x;
                    case UGUIAnchor.TopStretch: return m_rectTransform.rect.width;
                    case UGUIAnchor.MiddleStretch: return m_rectTransform.rect.width;
                    case UGUIAnchor.BottomStretch: return m_rectTransform.rect.width;
                    case UGUIAnchor.StretchLeft: return m_rectTransform.sizeDelta.x;
                    case UGUIAnchor.StretchCenter: return m_rectTransform.sizeDelta.x;
                    case UGUIAnchor.StretchRight: return m_rectTransform.sizeDelta.x;
                    case UGUIAnchor.StretchAll: return m_rectTransform.rect.width;
                    default: throw new ArgumentOutOfRangeException($"Unknown Anchor Preset: {Anchor}");
                }
            }
            set
            {
                switch (Anchor)
                {
                    case UGUIAnchor.TopLeft: m_rectTransform.sizeDelta = new Vector2(value, m_rectTransform.sizeDelta.y); break;
                    case UGUIAnchor.TopCenter: m_rectTransform.sizeDelta = new Vector2(value, m_rectTransform.sizeDelta.y); break;
                    case UGUIAnchor.TopRight: m_rectTransform.sizeDelta = new Vector2(value, m_rectTransform.sizeDelta.y); break;
                    case UGUIAnchor.MiddleLeft: m_rectTransform.sizeDelta = new Vector2(value, m_rectTransform.sizeDelta.y); break;
                    case UGUIAnchor.MiddleCenter: m_rectTransform.sizeDelta = new Vector2(value, m_rectTransform.sizeDelta.y); break;
                    case UGUIAnchor.MiddleRight: m_rectTransform.sizeDelta = new Vector2(value, m_rectTransform.sizeDelta.y); break;
                    case UGUIAnchor.BottomLeft: m_rectTransform.sizeDelta = new Vector2(value, m_rectTransform.sizeDelta.y); break;
                    case UGUIAnchor.BottomCenter: m_rectTransform.sizeDelta = new Vector2(value, m_rectTransform.sizeDelta.y); break;
                    case UGUIAnchor.BottomRight: m_rectTransform.sizeDelta = new Vector2(value, m_rectTransform.sizeDelta.y); break;
                    case UGUIAnchor.TopStretch: Debug.LogWarning("Setting the Width of a TopStretch RectTransform is not supported"); break;
                    case UGUIAnchor.MiddleStretch: Debug.LogWarning("Setting the Width of a MiddleStretch RectTransform is not supported"); break;
                    case UGUIAnchor.BottomStretch: Debug.LogWarning("Setting the Width of a MiddleStretch RectTransform is not supported"); break;
                    case UGUIAnchor.StretchLeft: m_rectTransform.sizeDelta = new Vector2(value, m_rectTransform.sizeDelta.y); break;
                    case UGUIAnchor.StretchCenter: m_rectTransform.sizeDelta = new Vector2(value, m_rectTransform.sizeDelta.y); break;
                    case UGUIAnchor.StretchRight: m_rectTransform.sizeDelta = new Vector2(value, m_rectTransform.sizeDelta.y); break;
                    case UGUIAnchor.StretchAll: Debug.LogWarning("Setting the Width of a StretchAll RectTransform is not supported"); break;
                    default: throw new ArgumentOutOfRangeException($"Unknown Anchor Preset: {Anchor}");
                }
            
            }
        }
        
        /// <summary>
        /// Height of the RectTransform
        /// </summary>
        public float Height
        {
            // get => m_rectTransform.rect.height;
            get
            {
                switch (Anchor)
                {
                    case UGUIAnchor.TopLeft: return m_rectTransform.sizeDelta.y;
                    case UGUIAnchor.TopCenter: return m_rectTransform.sizeDelta.y;
                    case UGUIAnchor.TopRight: return m_rectTransform.sizeDelta.y;
                    case UGUIAnchor.MiddleLeft: return m_rectTransform.sizeDelta.y;
                    case UGUIAnchor.MiddleCenter: return m_rectTransform.sizeDelta.y;
                    case UGUIAnchor.MiddleRight: return m_rectTransform.sizeDelta.y;
                    case UGUIAnchor.BottomLeft: return m_rectTransform.sizeDelta.y;
                    case UGUIAnchor.BottomCenter: return m_rectTransform.sizeDelta.y;
                    case UGUIAnchor.BottomRight: return m_rectTransform.sizeDelta.y;
                    case UGUIAnchor.TopStretch: return m_rectTransform.sizeDelta.y;
                    case UGUIAnchor.MiddleStretch: return m_rectTransform.sizeDelta.y;
                    case UGUIAnchor.BottomStretch: return m_rectTransform.sizeDelta.y;
                    case UGUIAnchor.StretchLeft: return m_rectTransform.rect.height;
                    case UGUIAnchor.StretchCenter: return m_rectTransform.rect.height;
                    case UGUIAnchor.StretchRight: return m_rectTransform.rect.height;
                    case UGUIAnchor.StretchAll: return m_rectTransform.rect.height;
                    default: throw new ArgumentOutOfRangeException($"Unknown Anchor Preset: {Anchor}");
                }
            }
            set
            {
                switch (Anchor)
                {
                    case UGUIAnchor.TopLeft: m_rectTransform.sizeDelta = new Vector2(m_rectTransform.sizeDelta.x, value); break;
                    case UGUIAnchor.TopCenter: m_rectTransform.sizeDelta = new Vector2(m_rectTransform.sizeDelta.x, value); break;
                    case UGUIAnchor.TopRight: m_rectTransform.sizeDelta = new Vector2(m_rectTransform.sizeDelta.x, value); break;
                    case UGUIAnchor.MiddleLeft: m_rectTransform.sizeDelta = new Vector2(m_rectTransform.sizeDelta.x, value); break;
                    case UGUIAnchor.MiddleCenter: m_rectTransform.sizeDelta = new Vector2(m_rectTransform.sizeDelta.x, value); break;
                    case UGUIAnchor.MiddleRight: m_rectTransform.sizeDelta = new Vector2(m_rectTransform.sizeDelta.x, value); break;
                    case UGUIAnchor.BottomLeft: m_rectTransform.sizeDelta = new Vector2(m_rectTransform.sizeDelta.x, value); break;
                    case UGUIAnchor.BottomCenter: m_rectTransform.sizeDelta = new Vector2(m_rectTransform.sizeDelta.x, value); break;
                    case UGUIAnchor.BottomRight: m_rectTransform.sizeDelta = new Vector2(m_rectTransform.sizeDelta.x, value); break;
                    case UGUIAnchor.TopStretch: m_rectTransform.sizeDelta = new Vector2(m_rectTransform.sizeDelta.x, value); break;
                    case UGUIAnchor.MiddleStretch: m_rectTransform.sizeDelta = new Vector2(m_rectTransform.sizeDelta.x, value); break;
                    case UGUIAnchor.BottomStretch: m_rectTransform.sizeDelta = new Vector2(m_rectTransform.sizeDelta.x, value); break;
                    case UGUIAnchor.StretchLeft: Debug.LogWarning("Setting the Height of a StretchLeft RectTransform is not supported"); break;
                    case UGUIAnchor.StretchCenter: Debug.LogWarning("Setting the Height of a StretchCenter RectTransform is not supported"); break;
                    case UGUIAnchor.StretchRight: Debug.LogWarning("Setting the Height of a StretchRight RectTransform is not supported"); break;
                    case UGUIAnchor.StretchAll: Debug.LogWarning("Setting the Height of a StretchAll RectTransform is not supported"); break;
                    default: throw new ArgumentOutOfRangeException($"Unknown Anchor Preset: {Anchor}");
                }
            
            }
        }
        
        /// <summary>
        /// Pivot of the RectTransform
        /// </summary>
        public Vector2 Pivot
        {
            get => m_rectTransform.pivot;
            set => m_rectTransform.pivot = value;
        }
        
        /// <summary>
        /// Anchor Preset of the RectTransform
        /// </summary>
        public UGUIAnchor Anchor
        {
            get => m_rectTransform.GetAnchor();
            set => m_rectTransform.SetAnchor(value);
        }
        
        /// <summary>
        /// Parent RectTransform of the RectTransform
        /// </summary>
        public RectTransform ParentRect => (m_rectTransform.parent != null && m_rectTransform.parent.TryGetComponent(out RectTransform t) != false) ? t : null;
        
        /// <summary>
        /// Represents a UGUI RectTransform with StretchAll Anchors
        /// </summary>
        /// <param name="rectTransform">The RectTransform to initialize with</param>
        public SuperRectTransform(RectTransform rectTransform)
        {
            m_rectTransform = rectTransform;
        }
        
        // To String
        public override string ToString()
        {
            switch (Anchor)
            {
                case UGUIAnchor.TopLeft:
                case UGUIAnchor.TopCenter:
                case UGUIAnchor.TopRight:
                case UGUIAnchor.MiddleLeft:
                case UGUIAnchor.MiddleCenter:
                case UGUIAnchor.MiddleRight:
                case UGUIAnchor.BottomLeft:
                case UGUIAnchor.BottomCenter:
                case UGUIAnchor.BottomRight: 
                    return $"{Anchor} | PosX: {PosX} | PosY: {PosY} | Width: {Width} | Height: {Height} | PosZ: {PosZ}";
                case UGUIAnchor.TopStretch: 
                case UGUIAnchor.MiddleStretch: 
                case UGUIAnchor.BottomStretch: 
                    return $"{Anchor} | Left: {Left} | PosY: {PosY} | Right: {Right} | Height: {Height} | PosZ: {PosZ}";
                case UGUIAnchor.StretchLeft:
                case UGUIAnchor.StretchCenter:
                case UGUIAnchor.StretchRight: 
                    return $"{Anchor} | PosX: {PosX} | Top: {Top} | Width: {Width} | Bottom: {Bottom} | PosZ: {PosZ}";
                case UGUIAnchor.StretchAll: 
                    return $"{Anchor} | Left: {Left} | Top: {Top} | Right: {Right} | Bottom: {Bottom} | Width: {Width} | Height: {Height} | PosZ: {PosZ}";
                default: 
                    return "SuperRectTransform: Unknown Anchor Preset";
            }
        }
        
        // Implicit Conversion
        public static implicit operator RectTransform(SuperRectTransform superRectTransform) => superRectTransform.m_rectTransform;
        public static implicit operator SuperRectTransform(RectTransform rectTransform) => new SuperRectTransform(rectTransform);
    }
}