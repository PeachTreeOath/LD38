using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RocketController : MonoBehaviour
{

    public GameObject launchParticlesFab;
    public GameObject flyingParticlesFab;
    public GameObject nextLevelPromptFab;
    public GameObject flamesFab;

    public float launchForce;
    public float accel;
    public float maxAccel;
    public float torqueForce;
	public float minY;

    bool startWobble;
    public bool flyable;
    public bool launching;
    public bool flying;
    float launchTimer;
    float launchTime = 2;
    float flytimer;

    float transitionTimer;
    public float transitionTime = 5;

	GameObject lastTile;
    GameObject launchParticles;
    Rigidbody2D rbody;
    private TextInstructionHandler textHandler;

	WorldGenerator worldGen;

    // Use this for initialization
    void Start()
    {
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Rocket"));
        rbody = gameObject.GetComponent<Rigidbody2D>();
        textHandler = GameObject.Find("UICanvas").GetComponent<TextInstructionHandler>();
		worldGen = GameObject.Find("World").GetComponent<WorldGenerator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (flyable && textHandler.isFlyable()) // Roundabout check against player being too close to crafting table
        {
            if (!launching)
            {
                if (Input.GetAxisRaw("Fire3") > 0 && CheckIfAllPartsBuilt())
                {
                    if(ShopManager.Instance.IsActive())
                    {
                        ShopManager.Instance.ToggleShop();
                    }
					gameObject.transform.rotation = Quaternion.identity;
                    AudioManager.Instance.PlaySound("Door_Sound", 0.5f);
                    launching = true;
                    launchTimer = Time.time;
                    launchParticles = Instantiate(launchParticlesFab) as GameObject;
                    launchParticles.transform.position = gameObject.transform.position;
                    launchParticles.transform.parent = gameObject.transform;
                    Globals.playerObj.GetComponent<PlayerController>().enabled = false;
                    Globals.playerObj.GetComponent<Renderer>().enabled = false;
                    textHandler.SetNearShip(false);
                    Transform backpack = transform.FindChild("Backpack");
                    backpack.SetParent(null);
                    Rigidbody2D backpackBody = backpack.GetComponent<Rigidbody2D>();
                    backpackBody.isKinematic = false;
                    backpackBody.gravityScale = 1f;
                }
            }
        }

        if (launching && !flying)
        {
            if (Time.time - launchTimer > launchTime)
            {
                if (launchParticles != null)
                {
                    Destroy(launchParticles);
                }
                flying = true;
                flytimer = Time.time;
                GameObject flyingParticles = Instantiate(flyingParticlesFab) as GameObject;
                flyingParticles.transform.position = gameObject.transform.position - gameObject.transform.up * (gameObject.GetComponent<Renderer>().bounds.extents.y + .3f);
                flyingParticles.transform.parent = gameObject.transform;
                transitionTimer = Time.time;

                GameObject flames = Instantiate(flamesFab) as GameObject;
                flames.transform.position = gameObject.transform.position + -gameObject.transform.up * (gameObject.GetComponent<Renderer>().bounds.extents.y);
                flames.transform.parent = gameObject.transform;
            }
        }

        if (flying &&
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
        if (flying)
        {
            rbody.AddForce(gameObject.transform.up * launchForce * .25f * Mathf.Min(maxAccel, Mathf.Max(1, (Time.time - flytimer) * accel)), ForceMode2D.Force);
            rbody.AddForce(Vector3.up * launchForce * .75f * Mathf.Min(maxAccel, Mathf.Max(1, (Time.time - flytimer) * accel)), ForceMode2D.Force);

            if (Input.GetAxis("Horizontal") != 0)
            {
                gameObject.transform.Rotate(gameObject.transform.forward, Time.deltaTime * Input.GetAxis("Horizontal") * -torqueForce);
            }
        }
    }
    
	void LateUpdate()
	{
		if(lastTile != null &&
			worldGen.worldTiles.Count > 0 )
		{
            if (Mathf.Abs(gameObject.transform.position.x - Globals.playerObj.transform.position.x) > worldGen.radius * worldGen.scale)
			{
				gameObject.transform.position = new Vector3(Globals.playerObj.transform.position.x + (Globals.playerObj.transform.position.x - gameObject.transform.position.x),
                    gameObject.transform.position.y,
                    gameObject.transform.position.z);
			}
		}
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        // if (col.gameObject.name.Equals("RocketTrigger"))
        // {
        //     flyable = true;
        //     if (!launching && !flying && CheckIfAllPartsBuilt())
        //     {
        //         textHandler.SetNearShip(true);
        //     }
        // }
		if (col.gameObject.tag.Equals("Meteor") && !launching && !flying)
        {
            gameObject.GetComponent<Rigidbody2D>().AddForce(col.GetComponent<Rigidbody2D>().velocity, ForceMode2D.Impulse);

            startWobble = false;
            //StartCoroutine(Wobble());
        }

		if(col.gameObject.tag.Equals("Ground"))
		{
			lastTile = col.gameObject;
		}
    }

    private IEnumerator Wobble()
    {
        startWobble = true;

        Vector2 rotationAxis = new Vector2(gameObject.transform.localPosition.x,
            gameObject.transform.localPosition.y - 2);
        float wobbleTime = .5f;
        float counter = 0;
        while (counter < wobbleTime)
        {
            counter += Time.deltaTime;
            gameObject.transform.RotateAround(rotationAxis, Vector3.forward, .5f);
            if (startWobble == false)
                yield break;
            else
                yield return 0;
        }

        counter = 0;
        while (counter < wobbleTime * 2)
        {
            counter += Time.deltaTime;
            gameObject.transform.RotateAround(rotationAxis, Vector3.forward, -.5f);
            if (startWobble == false)
                yield break;
            else
                yield return 0;
        }

        counter = 0;
        while (counter < wobbleTime)
        {
            counter += Time.deltaTime;
            gameObject.transform.RotateAround(rotationAxis, Vector3.forward, .5f);
            if (startWobble == false)
                yield break;
            else
                yield return 0;
        }
        gameObject.transform.eulerAngles = (new Vector3(0f, 0f, 0f));
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.name.Equals("RocketTrigger"))
        {
            flyable = false;
            textHandler.SetNearShip(false);
        }
    }

	public void BuildEngine(Material defaultMat)
    {
        transform.Find("RocketTop").GetComponent<SpriteRenderer>().enabled = true;
		GameObject.Find("EngineSilhouette").GetComponent<Image>().material = defaultMat;
		Globals.ship1 = true;
    }

	public void BuildShuttle(Material defaultMat)
    {
        transform.Find("RocketMid").GetComponent<SpriteRenderer>().enabled = true;
		GameObject.Find("ShuttleSilhouette").GetComponent<Image>().material = defaultMat;
		Globals.ship2 = true;
    }

	public void BuildBoosters(Material defaultMat)
    {
        transform.Find("RocketBot").GetComponent<SpriteRenderer>().enabled = true;
		GameObject.Find("BoostersSilhouette").GetComponent<Image>().material = defaultMat;
		Globals.ship3 = true;
    }

    public bool CheckIfAllPartsBuilt()
    {
        if (ShopManager.Instance.hasEngine && ShopManager.Instance.hasShuttle && ShopManager.Instance.hasBoosters)
        {
            return true;
        }
        return false;
    }
}
