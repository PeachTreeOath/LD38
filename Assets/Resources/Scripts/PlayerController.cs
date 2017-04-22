﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float runSpeed;
    public float carryingRunSpeed;
    public float jumpForce;
    public float hitForce;
    public int allowedJumps;
    public int maxHealth;
    public int metal;

    public GameObject shopText;
    public GameObject backpack;

    public int radarStat;
    public int speedStat;
    public int armorStat;
    public int jumpStat;
    public int magnetStat;
    public int resourceStat;

    private Rigidbody2D body;
    private int usedJumps;
    private int currentHealth;
    private bool invincible;
    private bool isFacingLeft;

    private SpriteRenderer sprite;
    private Animator animator;
    private Material origMat;
    private Material flashMat;
    private HeartCanvas heartCanvas;
    private MetalCanvas metalCanvas;
    
    private bool nearBackpack;
    private bool wearingBackpack;

	public enum FacingEnum { LEFT, RIGHT };

    private ShopManager shop;

	public FacingEnum GetFacing()
	{
		if(isFacingLeft)
		{
			return FacingEnum.LEFT;
		}else
		{
			return FacingEnum.RIGHT;
		}
	}

    // Use this for initialization
    void Start()
    {
		Globals.playerObj = gameObject;
        shop = ShopManager.Instance;

        NearBackpack(false);

        body = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        heartCanvas = GameObject.Find("UICanvas").GetComponent<HeartCanvas>();
        metalCanvas = GameObject.Find("UICanvas").transform.FindChild("MetalImage").GetComponent<MetalCanvas>();
        origMat = sprite.material;
        flashMat = Resources.Load<Material>("Materials/WhiteFlashMat");

        currentHealth = maxHealth;
        heartCanvas.SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        // Player in hitstun
        if(invincible)
        {
            return;
        }

        float hSpeed;

        if (wearingBackpack)
        {
            hSpeed = Input.GetAxisRaw("Horizontal") * carryingRunSpeed * Time.deltaTime;
        }
        else
        {
            hSpeed = Input.GetAxisRaw("Horizontal") * runSpeed * Time.deltaTime;
        }

        if (hSpeed > 0)
        {
            isFacingLeft = false;
            sprite.flipX = false;
            animator.SetBool("isMoving", true);
        }
        else if (hSpeed < 0)
        {
            isFacingLeft = true;
            sprite.flipX = true;
            animator.SetBool("isMoving", true);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }
        transform.Translate(new Vector2(hSpeed, 0));

        if (Input.GetButtonDown("Jump") && allowedJumps > usedJumps)
        {
            AudioManager.Instance.PlaySound("Jump", 0.3f);
            usedJumps++;
            body.velocity = Vector2.zero;
            body.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            animator.SetBool("isJumping", true);
        }

        if (nearBackpack && Input.GetButtonDown("Activate"))
        {
            shop.ToggleShop();
        }

        if (nearBackpack && !wearingBackpack && Input.GetButtonDown("Fire2"))
        {
            WearBackpack(true);
        }
        else if (wearingBackpack && Input.GetButtonDown("Fire2"))
        {
            WearBackpack(false);
        }

    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag.Equals("Ground"))
        {
           
        }

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (!invincible && col.gameObject.tag.Equals("Meteor"))
        {
            
            TakeDamage();
        }
        
        if(col.gameObject.tag.Equals("Backpack") &&
           col.GetType() == typeof(CircleCollider2D))
        {
            
            NearBackpack(true);
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if(col.gameObject.tag.Equals("Backpack") &&
           col.GetType() == typeof(CircleCollider2D))
        {
            NearBackpack(false);
        }        
    }

    public void ResetJump()
    {
        animator.SetBool("isJumping", false);
        usedJumps = 0;
    }

    public void TakeDamage()
    {
        AudioManager.Instance.PlaySound("Damage",0.5f);
        StartCoroutine(FlashWhite(.05f, .5f));
        Vector2 hitDir = Vector2.zero;
        if (isFacingLeft)
        {
            hitDir = new Vector2(hitForce, hitForce);
        }
        else
        {
            hitDir = new Vector2(-hitForce, hitForce);
        }
        body.velocity = Vector2.zero;
        body.AddForce(hitDir, ForceMode2D.Impulse);
        usedJumps = allowedJumps;
        invincible = true;
        currentHealth--;
        animator.SetBool("isJumping", true);

        // Update "listeners"
        heartCanvas.SetHealth(currentHealth);
        GameManager.Instance.SetHealth(currentHealth);
    }

    private IEnumerator FlashWhite(float flashSpeed, float duration)
    {
        float elapsedTime = 0;
        bool toggleFlash = false;
        while (elapsedTime < duration)
        {
            toggleFlash = !toggleFlash;
            if (toggleFlash)
            {
                sprite.material = flashMat;
            }
            else
            {
                sprite.material = origMat;
            }
            yield return new WaitForSeconds(flashSpeed);
            elapsedTime += flashSpeed;
        }
        animator.SetBool("isJumping", false);
        sprite.material = origMat;
        invincible = false;
    }

    private void NearBackpack(bool near)
    {
        shopText.SetActive(near);
        nearBackpack = near;

        if(!near && shop.IsActive())
        {
            shop.ToggleShop();
        }
    }

    private void WearBackpack(bool wear)
    {
        wearingBackpack = wear;

        if(!wear)
        {
            if(GetFacing() == FacingEnum.LEFT)
            {
                backpack.transform.position = new Vector3(transform.position.x + .5f, transform.position.y);
            }
            else
            {
                backpack.transform.position = new Vector3(transform.position.x - .5f, transform.position.y);
            }
        }

        if(shop.IsActive())
        {
            shop.ToggleShop();
        }

        backpack.SetActive(!wear);
    }

    

}
