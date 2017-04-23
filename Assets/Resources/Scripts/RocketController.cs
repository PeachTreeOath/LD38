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

    bool startWobble;
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
    private TextInstructionHandler textHandler;

    // Use this for initialization
    void Start()
    {
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Rocket"));
        rbody = gameObject.GetComponent<Rigidbody2D>();
        textHandler = GameObject.Find("UICanvas").GetComponent<TextInstructionHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        if (flyable)
        {
            if (!launching)
            {
                if (Input.GetAxisRaw("Activate") > 0)
                {
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

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.name.Equals("RocketTrigger"))
        {
            flyable = true;
            if (!launching && !flying)
            {
                textHandler.SetNearShip(true);
            }
        }
        if (col.gameObject.tag.Equals("Meteor"))
        {
            gameObject.GetComponent<Rigidbody2D>().AddForce(col.GetComponent<Rigidbody2D>().velocity, ForceMode2D.Impulse);

            startWobble = false;
            //StartCoroutine(Wobble());
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
}
