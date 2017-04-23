using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorSpawner : MonoBehaviour {

	public GameObject meteorFab;
    public GameObject radarArrow;

    [Header("Speed")]
	public float speedMin;
	public float speedMax;
	[Space(10)]

	[Range(0, 2)]
	public float secondsPerMeteor;


	[Range(0, 20)]
	public float torqueRange;
	[Range(0, 20)]
    public float spawnerWidth;
    [Range(0, 5)]
    public float spawnerAngleRange;

    public float maxBlastSize;

   
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
			genMeteor();
		}
	}

	void genMeteor()
	{
		Vector3 cam_pos = Camera.main.gameObject.transform.position;

		GameObject meteor = Instantiate<GameObject>(meteorFab);
		float x_offset = UnityEngine.Random.Range(-spawnerWidth, spawnerWidth);
		float x_angle = Random.Range(-spawnerAngleRange, spawnerAngleRange);
		meteor.transform.position = new Vector3(cam_pos.x + x_offset, cam_pos.y + 10, 1);
		Meteor m = meteor.GetComponent<Meteor>();

        

       
        m.blastRadius = Random.Range(0, maxBlastSize);
        m.transform.localScale = new Vector3(Mathf.Clamp(1.0f,3.0f,m.blastRadius),
            Mathf.Clamp(1.0f, 3.0f, m.blastRadius), 1.0f);

        m.moveDir = new Vector3(x_angle, -1).normalized;
		m.moveSpeed = Random.Range(speedMin, speedMax);
		m.torque = Random.Range(-torqueRange, torqueRange);
	}
}
