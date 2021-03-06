﻿
using UnityEngine;

public class Meteor : MonoBehaviour
{

    public Vector2 moveDir;
    public float moveSpeed;
    public float torque;
    public float blastRadius;
    public GameObject Explosion;
    public int radarLevel;

    /// <summary>
    /// Prefab for the resource pickup item dropped by the meteor.
    /// </summary>
    public GameObject PickupItem;
    public GameObject radarArrow;

    private GameObject World;
    private GameObject arrow;
    private WorldGenerator worldGen;
    Rigidbody2D rBody;
    CameraFollow mainCameraScript;
    void Start()
    {
        World = GameObject.Find("World");
        rBody = gameObject.GetComponent<Rigidbody2D>();
        mainCameraScript = GameObject.FindWithTag("MainCamera").GetComponent<CameraFollow>();
        worldGen = GameObject.Find("World").GetComponent<WorldGenerator>();

        //Create radar arrow
        if (radarLevel > 0)
        {
            arrow = Instantiate<GameObject>(radarArrow);
            MeteorRadar radar = arrow.GetComponent<MeteorRadar>();
            radar.Meteor = gameObject;
            radar.radarLevel = radarLevel;
        }

        //transform.localScale = new Vector3(blastRadius, blastRadius, 1.0f);
        transform.localScale = new Vector3(Mathf.Clamp(blastRadius,1.0f, 3.0f),
            Mathf.Clamp(blastRadius, 1.0f, 3.0f), 1.0f);
    }

    void FixedUpdate()
    {
        rBody.AddForce(moveDir * moveSpeed, ForceMode2D.Force);
        //rBody.AddForce(moveDir * moveSpeed * 20, ForceMode2D.Force);
        rBody.AddTorque(torque);


        if (transform.position.y <= -30.0f)
        {
            if (arrow != null)
            {
                arrow.GetComponent<MeteorRadar>().DestroyRadar();
            }
            Destroy(gameObject);
        }
    }

    private void Update() {
        if (worldGen.worldTiles.Count > 0) {
            GameObject leftMost = worldGen.GetWorldLeftMost();
            GameObject rightMost = worldGen.GetWorldRightMost();
            if (gameObject.transform.position.x < leftMost.transform.position.x ||
                gameObject.transform.position.x > rightMost.transform.position.x) {
                if (gameObject.transform.position.x < leftMost.transform.position.x) {
                    gameObject.transform.position = new Vector3(rightMost.transform.position.x, 
                        gameObject.transform.position.y, 
                        gameObject.transform.position.z);
                } else {
                    gameObject.transform.position = new Vector3(leftMost.transform.position.x, 
                        gameObject.transform.position.y, 
                        gameObject.transform.position.z);
                }
                //gameObject.transform.position = new Vector3(gameObject.transform.position.x, Globals.startY + 1, gameObject.transform.position.z);
                //gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                //gameObject.GetComponent<Rigidbody2D>().angularVelocity = 0;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("Meteor"))
        {
            if (blastRadius > other.GetComponent<Meteor>().blastRadius) // Bigger meteors dont explode on impact
            {
                return;
            }
        }
        if (other.tag.Equals("Ground"))
        {
            other.GetComponent<Block>().TakeDamage();
            //Destroy(other.gameObject);
        }
        RaycastHit2D[] hit2dList = Physics2D.CircleCastAll(transform.position, blastRadius * .75f, new Vector2(0.0f, 0.0f));
        for (int i = 0; i < hit2dList.Length; i++)
        {
            if (hit2dList[i].collider.tag.Equals("Ground"))
            {
                hit2dList[i].collider.GetComponent<Block>().TakeDamage();
                //Destroy(other.gameObject);
            }
        }
        
        GameObject go = Instantiate<GameObject>(Explosion, transform.position, transform.rotation);
        AudioManager.Instance.PlaySound("Explosion", .03f);
        if (blastRadius > 1.0f)
        {
            go.transform.localScale = new Vector3(blastRadius, blastRadius, 1.0f);
        }
        SpawnResources(1);
        if (arrow != null)
        {
            arrow.GetComponent<MeteorRadar>().DestroyRadar();
        }
        Destroy(gameObject);

        mainCameraScript.shakeCycles = 5;
        mainCameraScript.shakeIntensity = 0.2f * (blastRadius / 5.0f);
    }

    /// <summary>
    /// Handles creation of meteor resouce pickup items.
    /// </summary>
    /// <param name="amountSpawned"></param>
    private void SpawnResources(int amountSpawned)
    {
        for (int i = 0; i < amountSpawned; i++)
        {
            if (PlayerInventoryManager.Instance.CanSpawnNewPickup())
            {
                GameObject go = Instantiate(PickupItem, World.transform);
                go.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                go.transform.position = this.transform.position;
                Rigidbody2D rb = go.GetComponent<Rigidbody2D>();
                PlayerInventoryManager.Instance.SpawnedNewPickup();

                Vector2 randomDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
                rb.AddForce(randomDirection * 4, ForceMode2D.Impulse);
            }
        }
    }
}
