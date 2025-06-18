using System;
using UnityEditor;
using UnityEngine;

namespace LoM.Super.Editor
{
    internal class SuperBehaviourPreferenceConnectProvider : SuperBehaviourPreferenceProvider
    {
        // Getters
        public override string Name => "ConnectServer";
        public override int Order => -1;

        // OnGUI
        public override void OnGUI()
        {
            if (!RenderTitleCollapse("Connect Server", "Enabled")) return;
            RenderIntField("Port", "Port", 20635);
        }
        
        // Register provider on domain reload
        [InitializeOnLoadMethod]
        private static void RegisterProvider() => SuperBehaviourPreferences.RegisterPreferenceProvider<SuperBehaviourPreferenceConnectProvider>();
    }
}