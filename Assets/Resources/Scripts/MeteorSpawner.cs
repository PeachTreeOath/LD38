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
		meteor.transform.position = new Vector3(cam_pos.x, cam_pos.y + 10, 1);
		rb2d = meteor.GetComponent<Rigidbody2D>();
		rb2d.AddForce(new Vector3(0, -1) * UnityEngine.Random.Range(speed_min, speed_max), ForceMode2D.Impulse);
		rb2d.AddTorque(UnityEngine.Random.Range(torque_min, torque_max));			
	}
}
