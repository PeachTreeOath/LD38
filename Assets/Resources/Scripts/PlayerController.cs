using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float runSpeed;
    public float jumpForce;

    private Rigidbody2D body;
    private bool isJumping;

	// Use this for initialization
	void Start () {
        body = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        float hSpeed = Input.GetAxisRaw("Horizontal") * runSpeed * Time.deltaTime;
        transform.Translate(new Vector2(hSpeed, 0));

        if(Input.GetButtonDown("Jump"))
        {
            isJumping = true;
            body.AddForce(new Vector2(0, jumpForce),ForceMode2D.Impulse);
        }
	}
}
