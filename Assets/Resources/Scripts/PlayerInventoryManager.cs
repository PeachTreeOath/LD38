
using UnityEngine;

public class PlayerInventoryManager : Singleton<PlayerInventoryManager> {

    /// <summary>
    /// Maximum available pickups that can be alive at once on the map. 
    /// This should be set within the inspector.
    /// </summary>
    public int MaximumAllowedPickups = 50;

    public float scoreMultiplier = 1;

    [HideInInspector]
    public float PlayerResources { get; set; }

    private int TotalSpawnedPickups = 0;

    protected override void Init()
    {
    }

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

    public void DespawnedPickup()
    {
        TotalSpawnedPickups--;
    }

    public void CollectedPickup()
    {
        if (TotalSpawnedPickups > 0)
            TotalSpawnedPickups--;
        PlayerResources += scoreMultiplier;
    } 

    public void ResetGame()
    {
        TotalSpawnedPickups = 0;
        PlayerResources = 0;
        scoreMultiplier = 1;
    }
}
