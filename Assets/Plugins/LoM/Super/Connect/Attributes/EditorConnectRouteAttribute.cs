using System;
using UnityEngine;

namespace LoM.Super.Connect
{
    /// <summary>
    /// Attribute is used to mark a method as a route for the editor ConnectServer.<br/>
    /// <br/>
    /// New assemblies can be registered wia <see cref="T:LoM.Super.Connect.Editor.ConnectRouter"/>.RegisterAssembly.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class EditorConnectRouteAttribute : Attribute
    {
        public string Path;
        
        public EditorConnectRouteAttribute(string path)
        {
            Path = path;
        }
    }
}