using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorSpawner : MonoBehaviour {

	public GameObject meteorFab;
	public float seconds_per_meteor;
	public float speed_min;
	public float speed_max;
	public float torque_min;
	public float torque_max;
    public int max_blastSize;

	private float prev_time;

	private Rigidbody2D rb2d;

	// Use this for initialization
	void Start () {
		prev_time = 0;
	}
	
	// Update is called once per frame
	void Update () {
		float curr_time = Time.time;
		if(curr_time - prev_time > seconds_per_meteor)
		{
			prev_time = curr_time;
			genMeteor();
		}
	}

	void genMeteor()
	{
		Vector3 cam_pos = Camera.main.gameObject.transform.position;

		GameObject meteor = Instantiate<GameObject>(meteorFab);
		float x_offset = UnityEngine.Random.Range(-2f, 2f);
		float x_angle = -x_offset;
		meteor.transform.position = new Vector3(cam_pos.x + x_offset, cam_pos.y + 10, 1);
		Meteor m = meteor.GetComponent<Meteor>();
        m.blastRadius = Random.Range(0, max_blastSize);
		m.moveDir = new Vector3(x_angle, -1).normalized;
		m.moveSpeed = Random.Range(speed_min, speed_max);
		m.torque = Random.Range(torque_min, torque_max);
	}
}
