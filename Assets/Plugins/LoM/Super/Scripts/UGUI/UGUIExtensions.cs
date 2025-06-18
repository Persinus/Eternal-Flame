using System;
using UnityEngine;

namespace LoM.Super
{ 
    // Extension Methods
    public static class UGUIExtensions
    {
        /// <summary>
        /// Returns the SuperRectTransform for the RectTransform
        /// </summary>
        public static SuperRectTransform AsSuperRectTransform(this RectTransform rectTransform) => new SuperRectTransform(rectTransform);
    }
}