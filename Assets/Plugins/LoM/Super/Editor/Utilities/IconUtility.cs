using System;
using System.Collections.Concurrent;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace LoM.Super.Internal
{
    /// <summary>
    /// Utility class for getting icons based on class name (internal use only)
    /// </summary>
    internal static class IconUtility
    {   
        // Static Methods
        private static ConcurrentDictionary<SuperBehaviourIcon, Texture2D> s_textureCache = new();
        
        /// <summary>
        /// Get icon based on class name
        /// </summary>
        public static SuperBehaviourIcon GetIconByClass(UnityEngine.Object obj)
        {
            return GetIconByClassName(obj.GetType().Name);
        }
        
        /// <summary>
        /// Get icon based on class name
        /// </summary>
        public static SuperBehaviourIcon GetIconByClassName(string className)
        {            
            if (className.Contains("Test"))
            {
                return SuperBehaviourIcon.Test;
            }
            
            if (className.Contains("GameManager"))
            {
                return SuperBehaviourIcon.GameManager;
            }
            
            if (className.Contains("SceneManager"))
            {
                return SuperBehaviourIcon.SceneManager;
            }
            
            if (className.Contains("PlayerController"))
            {
                return SuperBehaviourIcon.PlayerController;
            }
            
            if (className.Contains("PlayerMove"))
            {
                return SuperBehaviourIcon.PlayerMovement;
            }
            
            if (className.Contains("Sound") || className.Contains("Audio") || className.Contains("Music"))
            {
                return SuperBehaviourIcon.Sound;
            }
            
            if (className.Contains("Manager"))
            {
                return SuperBehaviourIcon.Manager;
            }
            
            if (className.Contains("Controller"))
            {
                return SuperBehaviourIcon.Controller;
            }
            
            if (className.Contains("Generator"))
            {
                return SuperBehaviourIcon.Generator;
            }
            
            if (className.Contains("GameState"))
            {
                return SuperBehaviourIcon.GameState;
            }
            
            if (className.Contains("Spawn"))
            {
                return SuperBehaviourIcon.Spawn;
            }
            
            if (className.Contains("Settings"))
            {
                return SuperBehaviourIcon.Settings;
            }
            
            if (className.Contains("Animator") || className.Contains("Animation"))
            {
                return SuperBehaviourIcon.Animation;
            }
            
            if (className.Contains("Trigger"))
            {
                return SuperBehaviourIcon.Trigger;
            }
            
            if (className.Contains("Loader"))
            {
                return SuperBehaviourIcon.Loader;
            }
            
            if (className.EndsWith("Data") || className.EndsWith("Store"))
            {
                return SuperBehaviourIcon.Data;
            }
            
            if (className.EndsWith("State"))
            {
                return SuperBehaviourIcon.State;
            }
            
            if (className.Contains("Menu"))
            {
                return SuperBehaviourIcon.Menu;
            }
            
            if (className.EndsWith("Control"))
            {
                return SuperBehaviourIcon.Control;
            }
            
            if (className.Contains("Input"))
            {
                return SuperBehaviourIcon.Input;
            }
            
            if (className.Contains("Debug"))
            {
                return SuperBehaviourIcon.Debug;
            }
            
            if (className.Contains("Gizmo"))
            {
                return SuperBehaviourIcon.Gizmo;
            }
            
            return SuperBehaviourIcon.Default;
        }
    
        /// <summary>
        /// Get icon texture based on class name
        /// </summary>
        public static Texture2D GetIconTextureByClass(UnityEngine.Object obj)
        {
            return GetIconTextureByClassName(obj.GetType().Name);
        }
        
        /// <summary>
        /// Get icon texture based on class name
        /// </summary>
        public static Texture2D GetIconTextureByClassName(string className)
        {
            return GetIconTexture(GetIconByClassName(className));
        }
        
        /// <summary>
        /// Get icon texture from icon
        /// </summary>
        public static Texture2D GetIconTexture(SuperBehaviourIcon icon)
        {
            if (s_textureCache.TryGetValue(icon, out Texture2D texture))
            {
                return texture;
            }
            
            texture = AssetDatabase.LoadAssetAtPath<Texture2D>($"Assets/Plugins/LoM/Super/Icons/{icon}.png");
            s_textureCache.TryAdd(icon, texture);
            return texture;
        }
        
        // Clear on domain reload
        [InitializeOnLoadMethod]
        private static void ClearCache() => s_textureCache.Clear();
    }
}