using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    public float runSpeed;
    public float carryingSpeedFactor;
    public float jumpForce;
    public float carryingJumpForce;
    public float hitForce;
    public int maxHealth;
    public int metal;

    public GameObject shopText;
    public GameObject shipText;
    public GameObject deployText;
    public GameObject backpack;
    public GameObject rocket;
    public RocketController rocketController;

    private Rigidbody2D body;
    private int allowedJumps;
    private int usedJumps;
    private int currentHealth;
    private float origRunSpeed;
    private bool inHitStun;
    private bool invincible;
    private bool isFacingLeft;

    private SpriteRenderer sprite;
    private Animator animator;
    private Material origMat;
    private Material flashMat;
    private HeartCanvas heartCanvas;
    private MetalCanvas metalCanvas;
    private CircleCollider2D vacuumCollider;
    private TextInstructionHandler textHandler;
    private MeteorSpawner meteorSpawner;

    public bool nearBackpack;
    private bool wearingBackpack;

    public enum FacingEnum { LEFT, RIGHT };

    private ShopManager shop;

    public FacingEnum GetFacing()
    {
        if (isFacingLeft)
        {
            return FacingEnum.LEFT;
        }
        else
        {
            return FacingEnum.RIGHT;
        }
    }

    void Awake()
    {
        textHandler = GameObject.Find("UICanvas").GetComponent<TextInstructionHandler>();
    }

    // Use this for initialization
    void Start()
    {
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Pickup"));
        Globals.playerObj = gameObject;
        Globals.startY = gameObject.transform.position.y;
        shop = ShopManager.Instance;

        NearBackpack(false);

        body = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        heartCanvas = GameObject.Find("UICanvas").GetComponent<HeartCanvas>();
        metalCanvas = GameObject.Find("UICanvas").transform.FindChild("MetalImage").GetComponent<MetalCanvas>();
        origMat = sprite.material;
        flashMat = Resources.Load<Material>("Materials/WhiteFlashMat");
        vacuumCollider = transform.FindChild("VacuumTrigger").GetComponent<CircleCollider2D>();
        meteorSpawner = GameObject.Find("MeteorSpawner").GetComponent<MeteorSpawner>();

        origRunSpeed = runSpeed;
        currentHealth = maxHealth;
        heartCanvas.SetHealth(currentHealth);
        allowedJumps = 1;
    }

    // Update is called once per frame
    void Update()
    {
        // Player in hitstun
        if (inHitStun)
        {
            return;
        }

        float hSpeed;

        float curRunSpeed = runSpeed + (1.5f * (Globals.speedStat * .5f));
        if (wearingBackpack)
        {
            hSpeed = Input.GetAxisRaw("Horizontal") * curRunSpeed * carryingSpeedFactor * Time.deltaTime;
        }
        else
        {
            hSpeed = Input.GetAxisRaw("Horizontal") * curRunSpeed * Time.deltaTime;
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

            if (wearingBackpack)
            {
                body.AddForce(new Vector2(0, carryingJumpForce), ForceMode2D.Impulse);
            }
            else
            {
                body.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            }

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

        //Refresh the text on every frame
        NearBackpack(nearBackpack);

    }

    void OnDisable()
    {
        if (shipText != null)
            shipText.SetActive(false);
        if (shopText != null)
            shopText.SetActive(false);
    }

    public void SetStat(string itemName, int newLevel)
    {
        switch (itemName)
        {
            case ShopManager.speedString:
                Globals.speedStat = newLevel;
                break;
            case ShopManager.jumpString:
                Globals.jumpStat = newLevel;
                allowedJumps = Globals.jumpStat + 1;
                break;
            case ShopManager.armorString:
                Globals.armorStat = newLevel;
                currentHealth += 2;
                heartCanvas.SetHealth(currentHealth);
                break;
            case ShopManager.radarString:
                Globals.radarStat = newLevel;
                meteorSpawner.radarLevel = Globals.radarStat;
                break;
            case ShopManager.magnetString:
                Globals.magnetStat = newLevel;
                vacuumCollider.radius = newLevel * 1f;
                break;
            case ShopManager.resourceString:
                Globals.resourceStat = newLevel;
                switch (Globals.resourceStat)
                {
                    case 1:
                        PlayerInventoryManager.Instance.scoreMultiplier = 1.25f;
                        break;
                    case 2:
                        PlayerInventoryManager.Instance.scoreMultiplier = 1.75f;
                        break;
                    case 3:
                        PlayerInventoryManager.Instance.scoreMultiplier = 2.5f;
                        break;
                }
                break;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (!invincible && col.gameObject.tag.Equals("Meteor"))
        {
            TakeDamage();
        }

        if (col.gameObject.tag.Equals("Backpack") &&
           col.GetType() == typeof(CapsuleCollider2D))
        {
            NearBackpack(true);
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag.Equals("Backpack") &&
           col.GetType() == typeof(CapsuleCollider2D))
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
        if (invincible)
        {
            return;
        }

        AudioManager.Instance.PlaySound("Damage", 0.5f);
        StartCoroutine(FlashWhite(.05f, 1f));
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
        inHitStun = true;
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
            if (inHitStun && elapsedTime > duration / 2)
            {
                inHitStun = false;
            }
        }
        animator.SetBool("isJumping", false);
        sprite.material = origMat;
        invincible = false;
    }

    private void NearBackpack(bool near)
    {
        shopText.SetActive(near);
        textHandler.SetNearCraft(near);

        nearBackpack = near;

        if (near && !rocketController.launching && !rocketController.flying && rocketController.CheckIfAllPartsBuilt())
        {
            textHandler.SetNearShip(true);
            shipText.SetActive(true);
            rocketController.flyable = true;
        }
        else
        {
            textHandler.SetNearShip(false);
            shipText.SetActive(false);
            rocketController.flyable = false;
        }

        if (!near && shop.IsActive())
        {
            shop.ToggleShop();
        }
    }

    private void WearBackpack(bool wear)
    {
        wearingBackpack = wear;
        if (wear)
        {
            rocket.transform.rotation = Quaternion.identity;
            deployText.SetActive(true);
        }
        if (!wear)
        {
            deployText.SetActive(false);
            if (GetFacing() == FacingEnum.LEFT)
            {
                rocket.transform.position = new Vector3(transform.position.x + .5f, transform.position.y + 1f);
                // backpack.transform.position = new Vector3(transform.position.x + .5f, transform.position.y);
                //rocket.transform.position = new Vector3(transform.position.x + 3f, transform.position.y + 2f);
            }
            else
            {
                rocket.transform.position = new Vector3(transform.position.x - 1.5f, transform.position.y + 1f);
                //backpack.transform.position = new Vector3(transform.position.x - .5f, transform.position.y);
                //rocket.transform.position = new Vector3(transform.position.x - 3f, transform.position.y + 2f);
            }
        }

        if (shop.IsActive())
        {
            shop.ToggleShop();
        }

        //backpack.SetActive(!wear);
        rocket.SetActive(!wear);
    }
}
