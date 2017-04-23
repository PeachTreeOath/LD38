using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VacuumTrigger : MonoBehaviour {
    
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag.Equals("Pickup"))
        {
            col.GetComponent<PickupDespawn>().StartVacuum(transform);
        }
    }
}
