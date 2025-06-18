using System;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

namespace LoM.Super.Editor
{
    public static class SuperBehaviourPreferences
    {
        // Static Variables
        private static Color s_bgColor = new Color(48f / 255f, 48f / 255f, 48f / 255f); 
        private static Texture2D s_bgTexture = null;
        private static List<SuperBehaviourPreferenceProvider> s_providers = new();
        
        // Static Getters
        public static Color BackgroundColor => s_bgColor;
        public static Texture2D BackgroundTexture => s_bgTexture;
        
        // Register the SettingsProvider
        [SettingsProvider]
        public static SettingsProvider SuperBehaviourSettingsProvider()
        {
            // Set Static Variables
            s_bgTexture = new Texture2D(1, 1);
            s_bgTexture.SetPixel(0, 0, s_bgColor);
            s_bgTexture.Apply();
            
            // Register the SettingsProvider
            SettingsProvider provider = new("Preferences/SuperBehaviour", SettingsScope.User)
            {
                // Define the GUI for your preferences here
                guiHandler = (searchContext) =>
                {
                    EditorGUILayout.Space();
                    s_providers = s_providers.OrderBy(p => p.Order).ThenBy(p => p.Name).ToList();
                    foreach (var provider in s_providers)
                    {
                        try {
                            provider.OnGUI();
                        }
                        catch
                        {
                            EditorGUILayout.HelpBox($"An error occurred while rendering the preference provider {provider.Name}. Please contact support.", MessageType.Error);
                        }
                    }
                },

                // Keywords to support search functionality in the Preferences window
                keywords = new HashSet<string>(new[] { "Super", "Behaviour", "Connect", "Server", "Cache", "PreCache" })
            };

            return provider;
        }
        
        /// <summary>
        /// Register a preference provider
        /// </summary>
        public static void RegisterPreferenceProvider<T>() where T : SuperBehaviourPreferenceProvider, new()
        {
            if (s_providers.Exists(p => p.GetType() == typeof(T))) return;
            s_providers.Add(new T());
        }
    }
}