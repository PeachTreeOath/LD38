using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : Singleton<ShopManager>
{

    private bool isActive;
    private UpgradesPanelController shopPanel;

    void Start()
    {
        shopPanel = GameObject.Find("UpgradesPanel").GetComponent<UpgradesPanelController>();
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
        shopPanel.ToggleShow(isActive);
    }

}
