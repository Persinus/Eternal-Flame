using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace EternalFlame
{
    public class Pos_Map : MonoBehaviour
    {
        
        public List<MapPos> Pos_Go;
        public List<MapPos> Pos_Back;

    }
}
[System.Serializable]
public struct MapPos
{
    public int mapID;
    public Vector2 spawnPos;
}