using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour {

	public float scrollSpeed;
    public float zIndex;
	// Update is called once per frame
	void Update () {
		float xPosition;
		float camX = Camera.main.gameObject.transform.position.x;
		float camY = Camera.main.gameObject.transform.position.y;

		xPosition = camX - Mathf.Repeat((camX * scrollSpeed), gameObject.GetComponent<SpriteRenderer>().size.x / 2);
		transform.position = new Vector3(xPosition, camY, zIndex);
	}
}
