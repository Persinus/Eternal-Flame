using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance { get; private set; }

    public List<MapData> maps = new List<MapData>();
    public int currentMapID = 0;
    public TextAsset Map_Input;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadMapData();
    }

    void LoadMapData()
    {
        if (Map_Input != null)
        {
            string json = Map_Input.text;
            maps = new List<MapData>(JsonHelper.FromJson<MapData>(json));
            Debug.Log($"✅ Loaded {maps.Count} maps từ TextAsset.");
        }
        else
        {
            Debug.LogError("❌ Map_Input (TextAsset) chưa được gán!");
        }
    }

    public MapData? GetNextMap()
    {
        int nextID = currentMapID + 1;
        if (nextID < maps.Count) return maps[nextID];
        return null;
    }

    public MapData? GetPreviousMap()
    {
        int prevID = currentMapID - 1;
        if (prevID >= 0) return maps[prevID];
        return null;
    }

    public void GoToMap(MapDirection direction)
    {
        if (direction == MapDirection.Next && currentMapID < maps.Count - 1)
            currentMapID++;
        else if (direction == MapDirection.Previous && currentMapID > 0)
            currentMapID--;
    }

    public string GetCurrentMapName()
    {
        return maps.Count > currentMapID ? maps[currentMapID].name : "Unknown";
    }
    public string GetMapNameByID(int id)
{
    if (id >= 0 && id < maps.Count)
        return maps[id].name;
    return "Unknown";
}
}
