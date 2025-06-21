using UnityEngine;
using System;

public class BoundaryTrigger : MonoBehaviour
{
    public static event Action<string> OnPlayerEnterMap;
    public static event Action OnPlayerExitMap;

    [SerializeField] string mapName;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            OnPlayerEnterMap?.Invoke(mapName);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            OnPlayerExitMap?.Invoke();
        }
    }
}
