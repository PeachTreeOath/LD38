using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Tracks the lifecycle of a pickup item, and despawns it if it has been ofscreen for too long. 
/// This is necessary because there are a limited number of pickups that may be active at any given time.
/// </summary>
public class PickupDespawn : MonoBehaviour {

    /// <summary>
    /// The amount of time in seconds this pickup will live before it is despawned.
    /// </summary>
    public float despawnTimer = 10;

    /// <summary>
    /// The amount of time the pickup has been offscreen. 
    /// </summary>
    private float ticker = 0;

    /// <summary>
    /// Flag to track when the pickup is in a state where it will despawn.
    /// </summary>
    private bool isDespawning = false;

    /// <summary>
    /// If the resource is offscreen, count up on a timer before despawinging it.
    /// </summary>
    private void Update()
    {
        if (isDespawning)
        {
            ticker += Time.deltaTime;
            if (ticker >= despawnTimer)
            {
                PlayerInventoryManager.Instance.DespawnedPickup();
                Destroy(gameObject);
            }
        }
    }


    private void OnBecameVisible()
    {
        isDespawning = false;
    }

    private void OnBecameInvisible()
    {
        isDespawning = true;
        ticker = 0;
    }
}
