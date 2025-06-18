using System;
using UnityEngine;
using LoM.Super.Connect;
using UnityEditor.Compilation;

namespace LoM.Super.Connect.Editor
{
    /// <summary>
    /// Collection of utility routes for the ConnectServer. <i>(Used by SuperBehaviour VSCode extension)</i>
    /// </summary>
    public class EditorConnectUtils
    {
        // Trigger Compilation
        [EditorConnectRoute("/compile")]
        public static ConnectResponse Compile()
        {
            CompilationPipeline.RequestScriptCompilation();
            return new ConnectResponse(200, "Compilation Requested");
        }
    }
}