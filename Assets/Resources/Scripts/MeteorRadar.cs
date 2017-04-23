using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorRadar : MonoBehaviour {

    public GameObject Meteor;
    

    private Camera currentCamera;
    public float yOffset;
    //Test change

	// Use this for initialization
	void Start () {
        currentCamera = Camera.current;
	}
	
	// Update is called once per frame
	void Update () {
        if (Meteor != null && currentCamera != null)
        {
            /*transform.position = new Vector3(Meteor.transform.position.x,
                currentCamera.transform.position.y + yOffset, -1.0f);*/
			Vector3 screenPos = currentCamera.WorldToScreenPoint(Meteor.transform.position);
			transform.position = currentCamera.ScreenToWorldPoint( new Vector3(screenPos.x, Screen.height - 50, screenPos.z));
        }
	}
}
