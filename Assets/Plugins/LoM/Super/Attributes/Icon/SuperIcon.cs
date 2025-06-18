using System;
using System.Reflection;
using UnityEngine;

namespace LoM.Super
{
    /// <summary>
    /// Attribute to specify an icon for a SuperBehaviour. <br/>
    /// **Note:** Use the SuperBehaviourIcon enum to specify the icon or use a custom path.
    /// <hr/>
    /// <example>
    /// <code>
    /// [SuperIcon(SuperBehaviourIcon.GameManager)]
    /// public class GameManager : SuperBehaviour { }
    /// </code>
    /// </example>
    /// <hr/>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class SuperIcon : IconAttribute
    {        
        // Member Variables
        private SuperBehaviourIcon m_icon;
        private string m_customPath;
        
        /// <summary>
        /// Attribute to specify an icon for a SuperBehaviour.
        /// </summary>
        /// <param name="icon">The icon to use.</param>
        public SuperIcon(SuperBehaviourIcon icon) : base(GetPath(icon))
        {
            m_icon = icon;
            if (icon == SuperBehaviourIcon.Custom)
            {
                throw new ArgumentException("SuperBehaviourIcon.Custom has to specify a custom path.");
            }
        }
        
        /// <summary>
        /// Attribute to specify an icon for a SuperBehaviour.
        /// </summary>
        /// <param name="customPath">The full custom path to the icon (e.g. "Assets/Gizmos/MyIcon.png").</param>
        public SuperIcon(string customPath) : base(customPath) 
        {
            m_icon = SuperBehaviourIcon.Custom;
            m_customPath = customPath;
        }
        
        /// <summary>
        /// Get the path of the icon.
        /// </summary>
        /// <returns>The path of the icon.</returns>
        public string GetPath()
        {
            return m_icon == SuperBehaviourIcon.Custom ? m_customPath : GetPath(m_icon);
        }
        
        // Get Path (internal)
        private static string GetPath(SuperBehaviourIcon icon)
        {
            if (icon == SuperBehaviourIcon.None) icon = SuperBehaviourIcon.Default;
            return $"Assets/Plugins/LoM/Super/Icons/{icon.ToString()}.png";
        }
    }
}