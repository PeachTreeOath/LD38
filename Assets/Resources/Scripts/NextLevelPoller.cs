using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevelPoller : MonoBehaviour {

	bool loadingLevel;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(!loadingLevel &&
			Input.GetAxisRaw("Activate") > 0)
		{
			loadingLevel = true;
			Debug.Log(Time.time + " go to next level");
			//TODO: Load next level
		}
	}
}
