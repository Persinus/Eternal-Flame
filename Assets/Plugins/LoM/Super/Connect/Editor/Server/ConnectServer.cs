
using System;
using System.Net;
using System.Text;
using System.Threading;
using UnityEditor;
using UnityEngine;

namespace LoM.Super.Connect.Editor
{
    /// <summary>
    /// Internal class which manages the ConnectServer.<br/>
    /// Use ConnectRouter to register routes for the ConnectServer.
    /// </summary>
    [InitializeOnLoad]
    internal class ConnectServer
    {
        // Static Variables
        private static object s_lock = new object();
        private static ConnectServer s_instance;
        private static int s_port;
        private static bool s_enabled;
        
        // Member Variables
        private HttpListener m_listener;
        private Thread m_serverThread;
        private bool m_isRunning;
        
        // Getters
        private string ServerUrl => $"http://localhost:{s_port}/"; 

        // Constructor
        private ConnectServer()
        {
            if (s_instance != null) return;
            s_port = EditorPrefs.GetInt("SuperBehaviour.ConnectServer.Port", 20635);
            s_enabled = EditorPrefs.GetBool("SuperBehaviour.ConnectServer.Enabled", true);
            s_instance = this;
            Restart();
        }

        // Start the server
        private void Start()
        {
            if (m_isRunning) return;
            if (!s_enabled) return;
    
            if (m_listener == null) 
            {
                m_listener = new HttpListener();
                m_listener.Prefixes.Add(ServerUrl);
            }

            m_isRunning = true;
            m_serverThread = new Thread(RunServer) { IsBackground = true };
            m_serverThread.Start();
        }

        // Stop the server
        private void Stop(bool force = false)
        {
            if (!m_isRunning && !force) return;

            m_isRunning = false;
            if (m_listener == null && m_listener.IsListening) m_listener.Stop();
            m_serverThread.Join();
        }
        
        // Restart the server
        private void Restart()
        {
            if (m_isRunning)
            {
                Stop();
            }
            Start();
        }

        // Run the server
        private void RunServer()
        {
            // Start Listener
            try
            {
                m_listener.Start();
            }
            catch 
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ServerUrl + "terminate");
                request.Method = "GET";
                request.Timeout = 5000;
                try
                {
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    response.Close();
                }
                catch {}
                Thread.Sleep(5000);
                Restart();
            }
            
            // Listen for requests
            try
            {
                m_listener.Start();
                while (m_isRunning && m_listener.IsListening)
                {
                    var context = m_listener.GetContext(); // Blocking call
                    HandleRequest(context);
                }
            }
            catch {}
            finally
            {
                m_listener.Close();
                m_listener = null;
            }
        }

        // Handle the request
        private void HandleRequest(HttpListenerContext context)
        {
            HttpListenerResponse response = context.Response;

            // Check for terminate command
            if (context.Request.Url.AbsolutePath == "/terminate")
            {
                string responseMessage = "{\"status\": \"terminated\"}";
                byte[] buffer = Encoding.UTF8.GetBytes(responseMessage);
                response.ContentLength64 = buffer.Length;
                response.OutputStream.Write(buffer, 0, buffer.Length);
                response.OutputStream.Close();
                Stop(force: true);
                return;
            }
            
            // Check for restart command
            if (context.Request.Url.AbsolutePath == "/restart")
            {
                string responseMessage = "{\"status\": \"restarted\"}";
                byte[] buffer = Encoding.UTF8.GetBytes(responseMessage);
                response.ContentLength64 = buffer.Length;
                response.OutputStream.Write(buffer, 0, buffer.Length);
                response.OutputStream.Close();
                ThreadedEditorUtility.ExecuteInMainThread(() => Restart());
                return;
            }
            
            // Return Alive message
            if (context.Request.Url.AbsolutePath == "/")
            {
                string responseString = "{\"status\": \"alive\"}";
                byte[] buffer = Encoding.UTF8.GetBytes(responseString);
                response.ContentLength64 = buffer.Length;
                response.OutputStream.Write(buffer, 0, buffer.Length);
                response.OutputStream.Close();
                return;
            }
            
            // Check if Route exists
            string route = context.Request.Url.AbsolutePath;
            if (ConnectRouter.Exists(route))
            {
                ThreadedEditorUtility.ExecuteInMainThread(() => {
                    ConnectResponse connectResponse = ConnectRouter.Route(route, context.Request);
                    byte[] buffer = Encoding.UTF8.GetBytes(connectResponse.ReturnJSON);
                    response.StatusCode = connectResponse.StatusCode;
                    response.ContentLength64 = buffer.Length;
                    response.OutputStream.Write(buffer, 0, buffer.Length);
                    response.OutputStream.Close();
                });
            }
            else {
                string responseString = "{\"status\": \"not found\"}";
                byte[] buffer = Encoding.UTF8.GetBytes(responseString);
                response.StatusCode = (int)HttpStatusCode.NotFound;
                response.ContentLength64 = buffer.Length;
                response.OutputStream.Write(buffer, 0, buffer.Length);
                response.OutputStream.Close();
            }
        }
        
        // Reload on domain reload
        [InitializeOnLoadMethod]
        private static void ReloadAfterDomainReload()
        {
            s_enabled = EditorPrefs.GetBool("SuperBehaviour.ConnectServer.Enabled", true);
            lock (s_lock)
            {
                if (s_instance == null) 
                {
                    s_instance = new ConnectServer();
                }
                else
                {
                    s_instance.Restart();
                }
            }
        }
    }
}