using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour {

	public float scrollSpeed;

    // Update is called once per frame
    void Update () {
		gameObject.GetComponent<Renderer>().material.mainTextureOffset += Vector2.right * Time.deltaTime * scrollSpeed;
	}
}
