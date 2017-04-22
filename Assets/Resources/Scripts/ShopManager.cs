﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : Singleton<ShopManager>
{

    private bool isActive;
    private UpgradesPanelController shopPanel;
    private PlayerController player;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
    }

	protected override void Init()
	{

	}

    void Update()
    {
        if (!isActive)
        {
            return;
        }

    }

    public void ToggleShop()
    { 
        isActive = !isActive;
		shopPanel = GameObject.Find("UpgradesPanel").GetComponent<UpgradesPanelController>();
        shopPanel.ToggleShow(isActive);
    }

    public bool IsActive()
    {
        return isActive;
    }

}
