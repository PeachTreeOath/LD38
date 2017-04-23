using UnityEngine;

public class MeteorParticle : MonoBehaviour {

	// Update is called once per frame
	void Update () {
        gameObject.transform.eulerAngles = new Vector3(0f, 0f, 0f);
	}
}
