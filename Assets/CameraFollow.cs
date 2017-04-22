using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    private GameObject player;

	// Use this for initialization
	void Start () {
        player = GameObject.Find("Player");	
	}

    void LateUpdate()
    {
        Vector2 playerPos = player.transform.position;
        transform.position = new Vector3(playerPos.x, playerPos.y, transform.position.z);
    }
}
