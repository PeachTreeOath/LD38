using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float runSpeed;
    public float jumpForce;
    public int allowedJumps;
    public int maxHealth;

    private Rigidbody2D body;
    private int usedJumps;
    private int currentHealth;
    private bool invincible;

    private SpriteRenderer sprite;
    private Material origMat;
    private Material flashMat;

    // Use this for initialization
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        origMat = sprite.material;
        flashMat = Resources.Load<Material>("Materials/WhiteFlashMat");

        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        float hSpeed = Input.GetAxisRaw("Horizontal") * runSpeed * Time.deltaTime;
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

    void OnTriggerStay2D(Collider2D col)
    {
        if (!invincible && col.gameObject.tag.Equals("Meteor"))
        {
            TakeDamage();
        }
    }

    private void TakeDamage()
    {
        StartCoroutine(FlashWhite(.05f, .5f));
        invincible = true;
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
