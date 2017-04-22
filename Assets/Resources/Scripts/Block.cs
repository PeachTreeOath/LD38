using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour {

    public enum BlockType { Normal, Damage}
    public BlockType Type = BlockType.Normal;
   public int Health = 0;
   public bool isIndestructable = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void TakeDamage(int damage = 1)
    {
        if (!isIndestructable)
        {
            Health -= damage;
            if (Health <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (Type == BlockType.Damage)
        {
            if (collision.collider.tag == "Player")
            {
                collision.collider.GetComponent<PlayerController>().TakeDamage();
            }
        }
    }
}
