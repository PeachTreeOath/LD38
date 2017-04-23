using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartCanvas : MonoBehaviour
{

    public GameObject heartFab;
    public float heartSpacing;

    private Image[] heartList;

    private void Start()
    {
        heartList = transform.Find("Hearts").GetComponentsInChildren<Image>();
    }

    public void SetHealth(int currentHealth)
    {
        if (currentHealth > 0)
        {
            for (int i = 0; i < heartList.Length; i++)
            {
                if (i >= currentHealth)
                {
                    heartList[i].enabled = false;
                }
                else
                {
                    heartList[i].enabled = true;
                }
            }
        }
    }
}
