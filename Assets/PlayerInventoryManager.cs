

using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryManager : Singleton<PlayerInventoryManager> {

    /// <summary>
    /// Maximum available pickups that can be allive at once on the map
    /// </summary>
    public int MaximumAllowedPickups = 6;

    [HideInInspector]
    public int PlayerResources { get; set; }

    private List<GameObject> AvailablePickups;

	protected override void Init ()
	{
		AvailablePickups = new List<GameObject>();
	}

    public bool CanSpawnNewPickup()
    {
        if (AvailablePickups.Count < MaximumAllowedPickups)
            return true;
        else
            return false;
    }

    public void AddTrackedPickup(GameObject go)
    {
        if (!AvailablePickups.Contains(go))
            AvailablePickups.Add(go);
    }


}
