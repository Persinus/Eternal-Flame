using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using LoM.Super.Connect;

namespace LoM.Super.Connect.Editor
{
    /// <summary>
    /// The ConnectRouter allows you to register methods as routes for the ConnectServer.<br/>
    /// This server can be accessed by external applications to communicate with the Unity Editor.<br/>
    /// For example, the SuperBehaviour VSCode extension uses the ConnectServer to trigger compilation and other editor actions.
    /// <hr/>
    /// <example>
    /// https://localhost:20635/compile
    /// </example>
    /// <hr/>
    /// </summary>
    public static class ConnectRouter 
    {
        // Member Variables
        private static readonly Dictionary<string, MethodInfo> _routes = new Dictionary<string, MethodInfo>();

        // Static Constructor
        static ConnectRouter()
        {
            LoadRoutes(Assembly.GetExecutingAssembly());
        }
        
        /// <summary>
        /// Register an assembly to be searched for ConnectRoutes.<br/>
        /// <i>This method should best be called on editor startup.</i>
        /// <hr/>
        /// <example>
        /// <code>
        /// ConnectRouter.RegisterAssembly(typeof(YourClass).Assembly);
        /// </code>
        /// </example>
        /// <hr/>
        /// </summary>
        /// <param name="assembly">The assembly to search for ConnectRoutes.</param>
        public static void RegisterAssembly(Assembly assembly)
        {
            LoadRoutes(assembly);
        }

        // Check if Route exists [internal]
        internal static bool Exists(string path)
        {
            return _routes.ContainsKey(path);
        }

        // Route [internal]
        internal static ConnectResponse Route(string path, HttpListenerRequest request)
        {
            if (_routes.TryGetValue(path, out MethodInfo routeMethod))
            {
                string requestBody = new StreamReader(request.InputStream).ReadToEnd();
                var parameters = routeMethod.GetParameters();
                if (parameters.Length == 1 && parameters[0].ParameterType == typeof(string))
                {
                    return (ConnectResponse)routeMethod.Invoke(null, new object[] { requestBody });
                }
                else if (parameters.Length == 0)
                {
                    return (ConnectResponse)routeMethod.Invoke(null, null);
                }
                else
                {
                    Debug.LogError($"ConnectRoute method {routeMethod.Name} has invalid parameters. Must have 1 string parameter or no parameters.");
                }
            }
            return null;
        }

        // Load Routes
        private static void LoadRoutes(Assembly assembly)
        {
            foreach (var type in assembly.GetTypes())
            {
                foreach (var method in type.GetMethods())
                {
                    var editorConnectRouteAttribute = method.GetCustomAttribute<EditorConnectRouteAttribute>();
                    if (editorConnectRouteAttribute != null)
                    {
                        var routeKey = editorConnectRouteAttribute.Path;
                        _routes[routeKey] = method;
                    }
                }
            }
        }
    }
}