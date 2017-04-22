

using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryManager : Singleton<PlayerInventoryManager> {

    /// <summary>
    /// Maximum available pickups that can be allive at once on the map
    /// </summary>
    public int MaximumAllowedPickups = 6;

    [HideInInspector]
    public int PlayerResources { get; set; }

    private int TotalSpawnedPickups = 0;

    public bool CanSpawnNewPickup()
    {
        if (TotalSpawnedPickups < MaximumAllowedPickups)
            return true;
        else
            return false;
    }

    public void SpawnedNewPickup()
    {
        TotalSpawnedPickups++;
    }

    public void CollectedPickup()
    {
        if (TotalSpawnedPickups > 0)
            TotalSpawnedPickups--;
        PlayerResources++;
    } 

    public void ResetGame()
    {
        TotalSpawnedPickups = 0;
        PlayerResources = 0;
    }

}
