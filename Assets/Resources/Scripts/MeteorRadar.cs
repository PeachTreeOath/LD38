using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorRadar : MonoBehaviour {

    public GameObject Meteor;
    

    private Camera currentCamera;
    public float yOffset;

	// Use this for initialization
	void Start () {
        currentCamera = Camera.current;
	}
	
	// Update is called once per frame
	void Update () {
        if (Meteor != null && currentCamera != null)
        {
            transform.position = new Vector3(Meteor.transform.position.x,
                currentCamera.transform.position.y + yOffset, -1.0f);
        }
	}
}
