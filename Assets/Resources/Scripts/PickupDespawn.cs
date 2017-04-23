using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Tracks the lifecycle of a pickup item, and despawns it if it has fallen off the map.
public class PickupDespawn : MonoBehaviour {


    /// <summary>
    /// If the resource has fallen off the map, despawn it.
    /// </summary>
    private void Update()
    {
       if (gameObject.transform.localPosition.y < -50)
        {
            PlayerInventoryManager.Instance.DespawnedPickup();
            Destroy(this.gameObject);
        }
    }
}
