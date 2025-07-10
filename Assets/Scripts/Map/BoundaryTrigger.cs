using UnityEngine;
using System;
using DG.Tweening;

public enum MapDirection
{
    Next,
    Previous
}

public class BoundaryTrigger : MonoBehaviour
{
    public static event Action<MapDirection, int> OnPlayerEnterMap;
    public static event Action OnPlayerExitMap;

    [SerializeField] MapDirection direction;
    [SerializeField] int destinationMapID;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && ShouldTrigger(other))
        {
            OnPlayerEnterMap?.Invoke(direction, destinationMapID);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            OnPlayerExitMap?.Invoke();
        }
    }

    private bool ShouldTrigger(Collider2D other)
    {
        float scaleX = other.transform.localScale.x;

        return direction switch
        {
            MapDirection.Next => scaleX > 0,
            MapDirection.Previous => scaleX < 0,
            _ => false
        };
    }
}
