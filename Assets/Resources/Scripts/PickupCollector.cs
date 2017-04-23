using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupCollector : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D col)
	{
		if(col.gameObject.layer == LayerMask.NameToLayer("Pickup"))
		{
			Destroy(col.gameObject);
			//TODO: play sound fx and inc counter
		}
	}
}
