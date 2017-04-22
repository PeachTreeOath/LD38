using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour {

   public enum BlockType { Normal, Damage}
   public BlockType Type = BlockType.Normal;
	public int Health
	{
		get { return health; }
		set { health = value; maxHealth = value; }
	}
   	public bool isIndestructable = false;

	int maxHealth;
	int health;

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
			float status = ((health-1f)/maxHealth);
			if(status < .5f)
			{
				gameObject.GetComponent<Renderer>().material.SetColor("_Cutoff", Color.black);
			}else if(status < .7f)
			{
				gameObject.GetComponent<Renderer>().material.SetColor("_Cutoff", new Color(.48f, .48f, .48f));
			}

			health -= damage;
			if (health <= 0)
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
