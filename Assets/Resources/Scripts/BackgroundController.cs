using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour {

	public float scrollSpeed;
    public float zIndex;
    public bool lockY;

    private Camera mainCamera;

    private Vector2 offset;

    private void Start() {
    	offset = gameObject.transform.position;
        mainCamera = Camera.main;
    }
    // Update is called once per frame
    void Update () {
		float xPosition, yPosition;
		float camX = mainCamera.gameObject.transform.position.x + offset.x;
		float camY = mainCamera.gameObject.transform.position.y + offset.y;

		float time = Time.time;

		xPosition = camX - Mathf.Repeat(((camX + time) * scrollSpeed), gameObject.GetComponent<SpriteRenderer>().size.x);
		yPosition = camY;
		if(!lockY)
		{
			yPosition *= scrollSpeed;
		}
		
		transform.position = new Vector3(xPosition, yPosition, zIndex);
	}
}
