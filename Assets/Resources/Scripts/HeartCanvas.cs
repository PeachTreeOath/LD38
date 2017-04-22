using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartCanvas : MonoBehaviour {

    public GameObject heartFab;
    public float heartSpacing;

    private Transform firstHeartLocation;
    private List<GameObject> heartList;

	// Use this for initialization
	void Start () {
        firstHeartLocation = transform.Find("FirstHeartLocation");
        heartList = new List<GameObject>();
    }
	
    public void SetMaxHealth(int maxHealth)
    {
        GameObject heart = Instantiate(heartFab);
        for (int i = 0; i < maxHealth; i++)
        {
           // heart.transform.position =
        }
    }

    public void SetHealth(int currentHealth)
    {
        if(currentHealth > 0)
        {

        }
    }
}
