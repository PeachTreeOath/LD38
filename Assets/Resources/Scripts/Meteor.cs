﻿
using UnityEngine;

public class Meteor : MonoBehaviour {

	public Vector2 moveDir;
	public float moveSpeed;
	public float torque;
    public float blastRadius;
    public GameObject Explosion;

    /// <summary>
    /// Prefab for the resource pickup item dropped by the meteor.
    /// </summary>
    public GameObject PickupItem;

	Rigidbody2D rBody;
    CameraFollow mainCameraScript;

    void Start()
	{
		rBody = gameObject.GetComponent<Rigidbody2D>();
        mainCameraScript = GameObject.FindWithTag("MainCamera").GetComponent<CameraFollow>();
	}

	void FixedUpdate()
	{
		rBody.AddForce(moveDir * moveSpeed, ForceMode2D.Force);
        rBody.AddTorque(torque);
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("Ground"))
        {
            other.GetComponent<Block>().TakeDamage();
            //Destroy(other.gameObject);
        }
        RaycastHit2D[] hit2dList = Physics2D.CircleCastAll(transform.position, blastRadius, new Vector2(0.0f, 0.0f));
        for(int i = 0; i < hit2dList.Length; i++)
        {
            if (hit2dList[i].collider.tag.Equals("Ground"))
            {
                hit2dList[i].collider.GetComponent<Block>().TakeDamage();
                //Destroy(other.gameObject);
            }
        }
        
        GameObject go = Instantiate<GameObject>(Explosion, transform.position, transform.rotation);
        AudioManager.Instance.PlaySound("Explosion", .075f);
        if (blastRadius > 1.0f)
        {
            go.transform.localScale = new Vector3(blastRadius, blastRadius, 1.0f);
        }
        SpawnResources(1);
        Destroy(gameObject);

        mainCameraScript.shakeCycles = 5;
        mainCameraScript.shakeIntensity = 0.2f;
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
                GameObject go = Instantiate(PickupItem);
                go.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                go.transform.position = this.transform.position;
                Rigidbody2D rb = go.GetComponent<Rigidbody2D>();
                PlayerInventoryManager.Instance.SpawnedNewPickup();

                Vector2 randomDirection = new Vector2(Random.Range(0, 2), Random.Range(0, 2));
                rb.AddForce(randomDirection * moveSpeed, ForceMode2D.Impulse);
            }
        }
    }
}
