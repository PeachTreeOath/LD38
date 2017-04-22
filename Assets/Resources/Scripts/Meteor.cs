﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour {

	public Vector2 moveDir;
	public float moveSpeed;
	public float torque;

	Rigidbody2D rBody;

	void Start()
	{
		rBody = gameObject.GetComponent<Rigidbody2D>();
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
            Destroy(other.gameObject);
        }
        Destroy(gameObject);
    
    }
}