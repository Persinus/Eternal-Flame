using System;
using UnityEngine;

namespace LoM.Super.Connect
{
    public class ConnectResponse
    {
        public int StatusCode;
        public string ReturnJSON = "{}";
        
        public ConnectResponse(int statusCode, UnityEngine.Object returnData)
        {
            StatusCode = statusCode;
            ReturnJSON = JsonUtility.ToJson(returnData);
        }
        public ConnectResponse(int statusCode, bool returnData)
        {
            StatusCode = statusCode;
            ReturnJSON = $"{{\"success\": {returnData.ToString().ToLower()}}}";
        }
        public ConnectResponse(int statusCode, string returnData)
        {
            StatusCode = statusCode;
            ReturnJSON = $"{{\"message\": \"{returnData}\"}}";
        }
    }
}