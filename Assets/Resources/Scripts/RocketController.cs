using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketController : MonoBehaviour {

	public GameObject launchParticlesFab;
	public GameObject flyingParticlesFab;
	public GameObject nextLevelPromptFab;

	public float launchForce;
	public float accel;
	public float maxAccel;
	public float torqueForce;

	bool flyable;
	bool launching;
	bool flying;
	float launchTimer;
	float launchTime = 2;
	float flytimer;

	float transitionTimer;
	public float transitionTime = 5;

	GameObject launchParticles;
	Rigidbody2D rbody;

	// Use this for initialization
	void Start () {
		Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Rocket"));
		rbody = gameObject.GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		if(flyable)
		{
			if(!launching)
			{
				if(Input.GetAxisRaw("Activate") > 0)
				{
					launching = true;
					launchTimer = Time.time;
					launchParticles = Instantiate(launchParticlesFab) as GameObject;
					launchParticles.transform.position = gameObject.transform.position;
					launchParticles.transform.parent = gameObject.transform;
					Globals.playerObj.GetComponent<PlayerController>().enabled = false;
					Globals.playerObj.GetComponent<Renderer>().enabled = false;
				}
			}
		}

		if(launching && !flying)
		{
			if(Time.time - launchTimer > launchTime)
			{
				if(launchParticles != null)
				{
					Destroy(launchParticles);
				}
				flying = true;
				flytimer = Time.time;
				GameObject flyingParticles = Instantiate(flyingParticlesFab) as GameObject;
				flyingParticles.transform.position = gameObject.transform.position - gameObject.transform.up * (gameObject.GetComponent<Renderer>().bounds.extents.y + .3f);
				flyingParticles.transform.parent = gameObject.transform;
				transitionTimer = Time.time;
			}
		}

		if(flying &&
			Time.time - transitionTimer >= transitionTime)
		{
			GameObject nextLevelPrompt = Instantiate(nextLevelPromptFab) as GameObject;
			nextLevelPrompt.transform.SetParent(GameObject.Find("UICanvas").transform);
			nextLevelPrompt.transform.localPosition = Vector3.zero;
			Time.timeScale = 0;
			Destroy(this);
		}
	}

	void FixedUpdate()
	{
		if(flying)
		{
			rbody.AddForce(gameObject.transform.up * launchForce * .25f * Mathf.Min(maxAccel, Mathf.Max(1, (Time.time - flytimer) * accel)), ForceMode2D.Force);
			rbody.AddForce(Vector3.up * launchForce * .75f * Mathf.Min(maxAccel, Mathf.Max(1, (Time.time - flytimer) * accel)), ForceMode2D.Force);

			if(Input.GetAxis("Horizontal") != 0)
			{
				gameObject.transform.Rotate(gameObject.transform.forward, Time.deltaTime * Input.GetAxis("Horizontal") * -torqueForce);
			}
		}
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if(col.gameObject.name.Equals("RocketTrigger"))
		{
			flyable = true;
		}
	}

	void OnTriggerExit2D(Collider2D col)
	{
		if(col.gameObject.name.Equals("RocketTrigger"))
		{
			flyable = false;
		}
	}
}
