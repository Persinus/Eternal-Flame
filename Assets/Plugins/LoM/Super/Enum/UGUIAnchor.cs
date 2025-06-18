using System;
using UnityEngine;

namespace LoM.Super
{
    /// <summary>
    /// Represents the anchor of a RectTransform as shown in the Inspector
    /// </summary>
    public enum UGUIAnchor
    {
        TopLeft = 0,
        TopCenter = 1,
        TopRight = 2,
        MiddleLeft = 3,
        MiddleCenter = 4,
        MiddleRight = 5,
        BottomLeft = 6,
        BottomCenter = 7,
        BottomRight = 8,
        TopStretch = 9,
        MiddleStretch = 10,
        BottomStretch = 11,
        StretchLeft = 12,
        StretchCenter = 13,
        StretchRight = 14,
        StretchAll = 15,
    }
    
    // Extension Methods
    public static class UGUIAnchorExtensions
    {
        /// <summary>
        /// Get the anchor of the RectTransform as a UGUIAnchor enum
        /// </summary>
        /// <param name="rectTransform">RectTransform to get the anchor from</param>
        /// <returns>UGUIAnchor enum</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the anchor is a custom value</exception>
        public static UGUIAnchor GetAnchor(this RectTransform rectTransform)
        {
            Vector2 anchorMin = rectTransform.anchorMin;
            Vector2 anchorMax = rectTransform.anchorMax;
            if (anchorMin == new Vector2(0, 1) && anchorMax == new Vector2(0, 1)) return UGUIAnchor.TopLeft;
            if (anchorMin == new Vector2(0.5f, 1) && anchorMax == new Vector2(0.5f, 1)) return UGUIAnchor.TopCenter;
            if (anchorMin == new Vector2(1, 1) && anchorMax == new Vector2(1, 1)) return UGUIAnchor.TopRight;
            if (anchorMin == new Vector2(0, 0.5f) && anchorMax == new Vector2(0, 0.5f)) return UGUIAnchor.MiddleLeft;
            if (anchorMin == new Vector2(0.5f, 0.5f) && anchorMax == new Vector2(0.5f, 0.5f)) return UGUIAnchor.MiddleCenter;
            if (anchorMin == new Vector2(1, 0.5f) && anchorMax == new Vector2(1, 0.5f)) return UGUIAnchor.MiddleRight;
            if (anchorMin == new Vector2(0, 0) && anchorMax == new Vector2(0, 0)) return UGUIAnchor.BottomLeft;
            if (anchorMin == new Vector2(0.5f, 0) && anchorMax == new Vector2(0.5f, 0)) return UGUIAnchor.BottomCenter;
            if (anchorMin == new Vector2(1, 0) && anchorMax == new Vector2(1, 0)) return UGUIAnchor.BottomRight;
            if (anchorMin == new Vector2(0, 1) && anchorMax == new Vector2(1, 1)) return UGUIAnchor.TopStretch;
            if (anchorMin == new Vector2(0, 0.5f) && anchorMax == new Vector2(1, 0.5f)) return UGUIAnchor.MiddleStretch;
            if (anchorMin == new Vector2(0, 0) && anchorMax == new Vector2(1, 0)) return UGUIAnchor.BottomStretch;
            if (anchorMin == new Vector2(0, 0) && anchorMax == new Vector2(0, 1)) return UGUIAnchor.StretchLeft;
            if (anchorMin == new Vector2(0.5f, 0) && anchorMax == new Vector2(0.5f, 1)) return UGUIAnchor.StretchCenter;
            if (anchorMin == new Vector2(1, 0) && anchorMax == new Vector2(1, 1)) return UGUIAnchor.StretchRight;
            if (anchorMin == new Vector2(0, 0) && anchorMax == new Vector2(1, 1)) return UGUIAnchor.StretchAll;
            throw new ArgumentOutOfRangeException();
        }
        
        /// <summary>
        /// Set the anchor of the RectTransform as a UGUIAnchor enum
        /// </summary>
        /// <param name="rectTransform">RectTransform to set the anchor to</param>
        /// <param name="anchor">UGUIAnchor enum</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the anchor is a custom value</exception>
        public static void SetAnchor(this RectTransform rectTransform, UGUIAnchor anchor)
        {
            switch (anchor)
            {
                case UGUIAnchor.TopLeft:
                    rectTransform.anchorMin = new Vector2(0, 1);
                    rectTransform.anchorMax = new Vector2(0, 1);
                    break;
                case UGUIAnchor.TopCenter:
                    rectTransform.anchorMin = new Vector2(0.5f, 1);
                    rectTransform.anchorMax = new Vector2(0.5f, 1);
                    break;
                case UGUIAnchor.TopRight:
                    rectTransform.anchorMin = new Vector2(1, 1);
                    rectTransform.anchorMax = new Vector2(1, 1);
                    break;
                case UGUIAnchor.MiddleLeft:
                    rectTransform.anchorMin = new Vector2(0, 0.5f);
                    rectTransform.anchorMax = new Vector2(0, 0.5f);
                    break;
                case UGUIAnchor.MiddleCenter:
                    rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
                    rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
                    break;
                case UGUIAnchor.MiddleRight:
                    rectTransform.anchorMin = new Vector2(1, 0.5f);
                    rectTransform.anchorMax = new Vector2(1, 0.5f);
                    break;
                case UGUIAnchor.BottomLeft:
                    rectTransform.anchorMin = new Vector2(0, 0);
                    rectTransform.anchorMax = new Vector2(0, 0);
                    break;
                case UGUIAnchor.BottomCenter:
                    rectTransform.anchorMin = new Vector2(0.5f, 0);
                    rectTransform.anchorMax = new Vector2(0.5f, 0);
                    break;
                case UGUIAnchor.BottomRight:
                    rectTransform.anchorMin = new Vector2(1, 0);
                    rectTransform.anchorMax = new Vector2(1, 0);
                    break;
                case UGUIAnchor.TopStretch:
                    rectTransform.anchorMin = new Vector2(0, 1);
                    rectTransform.anchorMax = new Vector2(1, 1);
                    break;
                case UGUIAnchor.MiddleStretch:
                    rectTransform.anchorMin = new Vector2(0, 0.5f);
                    rectTransform.anchorMax = new Vector2(1, 0.5f);
                    break;
                case UGUIAnchor.BottomStretch:
                    rectTransform.anchorMin = new Vector2(0, 0);
                    rectTransform.anchorMax = new Vector2(1, 0);
                    break;
                case UGUIAnchor.StretchLeft:
                    rectTransform.anchorMin = new Vector2(0, 0);
                    rectTransform.anchorMax = new Vector2(0, 1);
                    break;
                case UGUIAnchor.StretchCenter:
                    rectTransform.anchorMin = new Vector2(0.5f, 0);
                    rectTransform.anchorMax = new Vector2(0.5f, 0);
                    break;
                case UGUIAnchor.StretchRight:
                    rectTransform.anchorMin = new Vector2(1, 0);
                    rectTransform.anchorMax = new Vector2(1, 0);
                    break;
                case UGUIAnchor.StretchAll:
                    rectTransform.anchorMin = new Vector2(0, 0);
                    rectTransform.anchorMax = new Vector2(1, 1);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}