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
        currentCamera = GameObject.Find("Main Camera").GetComponent<Camera>();


        //Used for rotating the arrow to the direction of the meteor
        float angle = Mathf.Atan2(Meteor.GetComponent<Meteor>().moveDir.x,
            Meteor.GetComponent<Meteor>().moveDir.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle + 180.0f, Vector3.back);

        
	}
	
	// Update is called once per frame
	void Update () {
        if (Meteor != null && Camera.current != null)
        {
			Vector3 screenPos = currentCamera.WorldToScreenPoint(Meteor.transform.position);
			transform.position = currentCamera.ScreenToWorldPoint( new Vector3(screenPos.x, Screen.height - 50, 10.0f));
        }
        
	}
}
