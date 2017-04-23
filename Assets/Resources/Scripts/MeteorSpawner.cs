using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorSpawner : MonoBehaviour {

	public GameObject meteorFab;
    public GameObject radarArrow;

    [Header("Speed")]
	public float speedMin;
	public float speedMax;
	public float secondsPerMeteor;
	public float torqueRange;
    public float spawnerWidth;
    public float spawnerAngleRange;
    public float maxBlastSize;
    public int radarLevel;
   
    private float prev_time;

	private Rigidbody2D rb2d;

	// Use this for initialization
	void Start () {
		prev_time = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		float curr_time = Time.time;
		if(curr_time - prev_time > secondsPerMeteor)
		{
			prev_time = curr_time;
			GenerateMeteor();
		}
	}

	void GenerateMeteor()
	{
		//Init the meteor
		GameObject meteor = Instantiate<GameObject>(meteorFab);
		Meteor m = meteor.GetComponent<Meteor>();
        m.radarLevel = radarLevel;

		// Set the meteor launch position
		Vector3 meteorStartPosition = new Vector3();
		Vector3 cameraPosition = Camera.main.gameObject.transform.position;
		meteorStartPosition.x = UnityEngine.Random.Range(-spawnerWidth, spawnerWidth) + cameraPosition.x;
		meteorStartPosition.y = cameraPosition.y + 10;
		meteor.transform.position = meteorStartPosition;
		m.radarArrow = radarArrow;

		// Set the meteor launch angle
		float angle = Random.Range(-spawnerAngleRange, spawnerAngleRange);
		m.moveDir = (Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.down).normalized;
		
		// Set the blast size and radius
        m.blastRadius = Random.Range(0, maxBlastSize);
        m.transform.localScale = new Vector3(Mathf.Clamp(0.5f,3.0f,m.blastRadius),
            Mathf.Clamp(0.5f, 3.0f, m.blastRadius), 1.0f);

        // Set Speed and Torque
		m.moveSpeed = Random.Range(speedMin, speedMax);
		m.torque = Random.Range(-torqueRange, torqueRange);
	}
}
