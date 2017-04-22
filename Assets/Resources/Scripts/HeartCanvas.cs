using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartCanvas : MonoBehaviour
{

    public GameObject heartFab;
    public float heartSpacing;

    private Transform firstHeartLocation;
    private List<GameObject> heartList;

    // Use this for initialization
    void Start()
    {
		firstHeartLocation = GameObject.Find("FirstHeartLocation").transform;
        heartList = new List<GameObject>();
    }

    public void SetMaxHealth(int maxHealth)
    {
        for (int i = 0; i < maxHealth; i++)
        {
            GameObject heart = Instantiate(heartFab);
            heart.transform.position = firstHeartLocation.position + new Vector3(i * heartSpacing, 0, 0);
            heart.transform.SetParent(transform);
            heartList.Add(heart);
        }
    }

    public void SetHealth(int currentHealth)
    {
        if (currentHealth > 0)
        {
            int i = 0;
            foreach (GameObject heart in heartList)
            {
                if (i >= currentHealth)
                {
                    heart.GetComponent<Image>().enabled = false;
                }
                i++;
            }
        }
    }
}
