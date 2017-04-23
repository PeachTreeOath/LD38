using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupCollector : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D col)
	{
		if(col.gameObject.layer == LayerMask.NameToLayer("Pickup"))
		{
			AudioManager.Instance.PlaySound("Item_Pickup_Sound", 4.0f);
			PlayerInventoryManager.Instance.CollectedPickup();
			Destroy(col.gameObject);
		}
	}
}
