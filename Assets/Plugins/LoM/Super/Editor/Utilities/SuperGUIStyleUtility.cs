using System;
using System.Collections.Concurrent;
using UnityEditor;
using UnityEngine;

namespace LoM.Super.Editor
{
    /// <summary>
    /// Utility class for building cached GUIStyles in one line of code
    /// </summary>
    public static class SuperGUIStyleUtility
    {
        // Static Variables
        private static ConcurrentDictionary<string, GUIStyle> s_styleCache = new();
        
        /// <summary>
        /// Get a GUIStyle with the specified name (Prefer using a prefix to avoid conflicts)
        /// </summary>
        /// <param name="styleName">The name of the GUIStyle</param>
        public static GUIStyle GetStyle(string styleName)
        {
            if (s_styleCache.TryGetValue(styleName, out var style)) return style;
            return null;
        }
        
        /// <summary>
        /// Get a GUIStyle with the specified name & prefix
        /// </summary>
        /// <param name="prefix">The prefix of the GUIStyle</param>
        /// <param name="styleName">The name of the GUIStyle</param>
        public static GUIStyle GetStyle(string prefix, string styleName)
        {
            return GetStyle($"{prefix}_{styleName}");
        }
        
        /// <summary>
        /// Get a GUIStyle with the specified name and create it if it doesn't exist
        /// </summary>
        /// <param name="styleName">The name of the GUIStyle</param>
        /// <param name="createStyle">The function to create the GUIStyle</param>
        public static GUIStyle GetStyle(string styleName, Func<GUIStyle> createStyle)
        {
            if (s_styleCache.TryGetValue(styleName, out var style)) return style;
            style = createStyle();
            s_styleCache.TryAdd(styleName, style);
            return style;
        }
        
        /// <summary>
        /// Get a GUIStyle with the specified name & prefix and create it if it doesn't exist
        /// </summary>
        /// <param name="prefix">The prefix of the GUIStyle</param>
        /// <param name="styleName">The name of the GUIStyle</param>
        /// <param name="createStyle">The function to create the GUIStyle</param>
        public static GUIStyle GetStyle(string prefix, string styleName, Func<GUIStyle> createStyle)
        {
            if (s_styleCache.TryGetValue($"{prefix}_{styleName}", out var style)) return style;
            style = createStyle();
            s_styleCache.TryAdd($"{prefix}_{styleName}", style);
            return style;
        }
        
        /// <summary>
        /// Get a GUIStyle with the specified name and create it if it doesn't exist
        /// </summary>
        /// <param name="styleName">The name of the GUIStyle</param>
        /// <param name="createStyle">The function to create the GUIStyle</param>
        public static GUIStyle Variant(this GUIStyle style, string styleName, Action<GUIStyle> createStyle)
        {
            if (s_styleCache.TryGetValue(styleName, out var cachedStyle)) return cachedStyle;
            GUIStyle newStyle = new GUIStyle(style);
            createStyle(newStyle);
            s_styleCache.TryAdd(styleName, newStyle);
            return newStyle;
        }
        
        /// <summary>
        /// Get a GUIStyle with the specified name & prefix and create it if it doesn't exist
        /// </summary>
        /// <param name="prefix">The prefix of the GUIStyle</param>
        /// <param name="styleName">The name of the GUIStyle</param>
        /// <param name="createStyle">The function to create the GUIStyle</param>
        public static GUIStyle Variant(this GUIStyle style, string prefix, string styleName, Action<GUIStyle> createStyle)
        {
            if (s_styleCache.TryGetValue($"{prefix}_{styleName}", out var cachedStyle)) return cachedStyle;
            GUIStyle newStyle = new GUIStyle(style);
            createStyle(newStyle);
            s_styleCache.TryAdd($"{prefix}_{styleName}", newStyle);
            return newStyle;
        }
        
        // Clear on domain reload
        [InitializeOnLoadMethod]
        private static void ClearCache() => s_styleCache.Clear();
    }
}