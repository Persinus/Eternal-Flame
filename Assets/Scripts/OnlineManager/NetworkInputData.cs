using UnityEngine;
using Fusion;

namespace EternalFlame 
{
public struct NetworkInputData : INetworkInput
{
    public Vector2 movement; // Di chuyển
    public bool jump;        // Nhảy
    public bool attack;      // Tấn công
}
}
