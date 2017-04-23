using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Tracks the lifecycle of a pickup item, and despawns it if it has fallen off the map.
public class PickupDespawn : MonoBehaviour {

    private float spawnTime;
    private bool isVacuuming;
    private Transform player;
    private float vacuumSpeed = 10;
    

    // Use this for initialization
    void Start()
    {
        spawnTime = Time.time;
        GameObject.Find("World").GetComponent<WorldGenerator>().worldTiles.Add(gameObject);
    }

    /// <summary>
    /// If the resource has fallen off the map, despawn it.
    /// </summary>
    private void Update()
    {
        if (isVacuuming)
        {
            Vector2 newPos = Vector2.MoveTowards(transform.position, player.position, vacuumSpeed * Time.deltaTime);
            transform.position = newPos;
            return;
        }

        if (gameObject.transform.localPosition.y < -50 || 
           Time.time - spawnTime > 30)
        {
            PlayerInventoryManager.Instance.DespawnedPickup();
            Destroy(this.gameObject);
        }
    }

    public void StartVacuum(Transform player)
    {
        this.player = player;
        isVacuuming = true;
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<Rigidbody2D>().gravityScale = 0f;
    }
}
