using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour {

	public float scrollSpeed;
    public float zIndex;
    public bool lockY;

    private Camera mainCamera;

    private void Start() {
        mainCamera = Camera.main;
    }
    // Update is called once per frame
    void Update () {
		float xPosition, yPosition;
		float camX = mainCamera.gameObject.transform.position.x;
		float camY = mainCamera.gameObject.transform.position.y;

		xPosition = camX - Mathf.Repeat((camX * scrollSpeed), gameObject.GetComponent<SpriteRenderer>().size.x);
		yPosition = camY * scrollSpeed;
		if(lockY)
		{
			transform.position = new Vector3(xPosition, 0, zIndex);
		}
		else
		{
			transform.position = new Vector3(xPosition, yPosition, zIndex);
		}
		
	}
}
