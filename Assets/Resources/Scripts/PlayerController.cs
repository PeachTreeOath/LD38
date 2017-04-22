using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float runSpeed;
    public float jumpForce;
    public float hitForce;
    public int allowedJumps;
    public int maxHealth;

    private Rigidbody2D body;
    private int usedJumps;
    private int currentHealth;
    private bool invincible;
    private bool isFacingLeft;

    private SpriteRenderer sprite;
    private Material origMat;
    private Material flashMat;
    private HeartCanvas heartCanvas;

    // Use this for initialization
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        heartCanvas = GameObject.Find("UICanvas").GetComponent<HeartCanvas>();
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

        float hSpeed = Input.GetAxisRaw("Horizontal") * runSpeed * Time.deltaTime;
        if (hSpeed > 0)
        {
            isFacingLeft = false;
            sprite.flipX = false;
        }
        else if (hSpeed < 0)
        {
            isFacingLeft = true;
            sprite.flipX = true;
        }
        transform.Translate(new Vector2(hSpeed, 0));

        if (Input.GetButtonDown("Jump") && allowedJumps > usedJumps)
        {
            usedJumps++;
            body.velocity = Vector2.zero;
            body.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag.Equals("Ground"))
        {
            usedJumps = 0;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (!invincible && col.gameObject.tag.Equals("Meteor"))
        {
            TakeDamage();
        }
    }

    private void TakeDamage()
    {
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

        // Update "listeners"
        heartCanvas.SetHealth(currentHealth);
        GameManager.instance.SetHealth(currentHealth);
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
        sprite.material = origMat;
        invincible = false;
    }

}
