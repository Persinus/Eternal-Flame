using UnityEngine;
using TMPro;

public class MapNameUIManager : MonoBehaviour
{
    [SerializeField] Canvas mapNameText;
    [SerializeField] TextMeshProUGUI mapNameTextComponent;

    private void OnEnable()
    {
        BoundaryTrigger.OnPlayerEnterMap += ShowMapName;
        BoundaryTrigger.OnPlayerExitMap += HideMapName;
    }

    private void OnDisable()
    {
        BoundaryTrigger.OnPlayerEnterMap -= ShowMapName;
        BoundaryTrigger.OnPlayerExitMap -= HideMapName;
    }

void ShowMapName(MapDirection mapDirection, int index)
{
    // Cập nhật map hiện tại
    MapManager.Instance.currentMapID = index;

    // Lấy tên map từ index
    string mapName = MapManager.Instance.GetMapNameByID(index);

    mapNameText.gameObject.SetActive(true);
    mapNameTextComponent.text = mapName;
}

    void HideMapName()
    {
        mapNameText.gameObject.SetActive(false);
    }
}