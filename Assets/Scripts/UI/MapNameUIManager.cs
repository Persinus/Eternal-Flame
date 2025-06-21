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

    void ShowMapName(string mapName)
    {
        mapNameText.gameObject.SetActive(true);
        mapNameTextComponent.text = mapName;
    }

    void HideMapName()
    {
        mapNameText.gameObject.SetActive(false);
    }
}