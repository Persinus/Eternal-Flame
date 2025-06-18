using UnityEditor;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace LoM.Super.Connect.Editor
{
    [InitializeOnLoad]
    public class ThreadedEditorUtility
    {
        // Member Variables
        private static Queue<Action> m_actionsToExecuteOnMainThread = new Queue<Action>();

        // Static Constructor
        static ThreadedEditorUtility()
        {
            EditorApplication.update += Update;
        }

        // Update
        private static void Update()
        {
            lock (m_actionsToExecuteOnMainThread)
            {
                while (m_actionsToExecuteOnMainThread.Count > 0)
                {
                    m_actionsToExecuteOnMainThread.Dequeue().Invoke();
                }
            }
        }

        /// <summary>
        /// Execute an action in the main thread
        /// </summary>
        /// <param name="action">Action to execute</param>
        public static void ExecuteInMainThread(Action action)
        {
            lock (m_actionsToExecuteOnMainThread)
            {
                m_actionsToExecuteOnMainThread.Enqueue(action);
            }
        }
        
        /// <summary>
        /// Log in the main thread (using Debug.Log)
        /// </summary>
        /// <param name="message">Message to log</param>
        /// <param name="context">Object to log</param>
        public static void Log(string message, UnityEngine.Object context = null)
        {
            ExecuteInMainThread(() => Debug.Log(message, context));
        }
        
        /// <summary>
        /// Log in the main thread (using Debug.LogWarning)
        /// </summary>
        /// <param name="message">Message to log</param>
        /// <param name="context">Object to log</param>
        public static void LogWarning(string message, UnityEngine.Object context = null)
        {
            ExecuteInMainThread(() => Debug.LogWarning(message, context));
        }
        
        /// <summary>
        /// Log in the main thread (using Debug.LogError)
        /// </summary>
        /// <param name="message">Message to log</param>
        /// <param name="context">Object to log</param>
        public static void LogError(string message, UnityEngine.Object context = null)
        {
            ExecuteInMainThread(() => Debug.LogError(message, context));
        }
    }
}