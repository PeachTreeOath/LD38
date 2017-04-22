using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public int shakeCycles;
    public float shakeIntensity;
    private GameObject player;

	// Use this for initialization
	void Start ()
    {
        player = GameObject.Find("Player");
        shakeCycles = 0;
    }

    void LateUpdate()
    {
        Vector2 playerPos = player.transform.position;
        transform.position = new Vector3(playerPos.x, playerPos.y, transform.position.z);

        // add random jitter to camera position except z axis
        if (shakeCycles > 0) {
            Vector2 cameraShake = Random.insideUnitCircle * shakeIntensity;
            transform.localPosition = transform.position + new Vector3(cameraShake.x, cameraShake.y, 0);
            shakeCycles--;
        }
    }
}
