using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : Singleton<ShopManager>
{

    private bool isActive;
    private UpgradesPanelController shopPanel;
    private PlayerController player;

    public int[] speedCosts = { 1, 3, 5 };
    public int[] jumpCosts = { 1, 3, 5 };
    public int[] armorCosts = { 1, 3, 5 };
    public int[] radarCosts = { 1, 3, 5 };
    public int[] magnetCosts = { 1, 3, 5 };
    public int[] resourceCosts = { 1, 3, 5 };

    public const string speedString = "Speed";
    public const string jumpString = "Jump";
    public const string armorString = "Armor";
    public const string radarString = "Radar";
    public const string magnetString = "Magnet";
    public const string resourceString = "Resource";

    public Dictionary<string, int[]> costMap;
    public Dictionary<string, int> levelMap;

    private bool debugOn = true;

    protected override void Init()
    {
		player = GameObject.Find("Player").GetComponent<PlayerController>();
		costMap = new Dictionary<string, int[]>();
		costMap.Add(speedString, speedCosts);
		costMap.Add(jumpString, jumpCosts);
		costMap.Add(armorString, armorCosts);
		costMap.Add(radarString, radarCosts);
		costMap.Add(magnetString, magnetCosts);
		costMap.Add(resourceString, resourceCosts);

		levelMap = new Dictionary<string, int>();
		levelMap.Add(speedString, 0);
		levelMap.Add(jumpString, 0);
		levelMap.Add(armorString, 0);
		levelMap.Add(radarString, 0);
		levelMap.Add(magnetString, 0);
		levelMap.Add(resourceString, 0);
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

    public void PurchaseItem(string itemName, CraftButton buttonCaller)
    {
		Debug.Log("PurchaseItem: " + itemName);
        int level = levelMap[itemName];

        if (level > 2)
        {
            return;
        }

		Debug.Log("PurchaseItem: level " + level);
        int cost = costMap[itemName][level];

        if (debugOn)
        {
            Debug.Log("Resources: " + PlayerInventoryManager.Instance.PlayerResources + " Cost: " + cost);
            levelMap[itemName]++;
            buttonCaller.SetOrbs(levelMap[itemName]);
            player.SetStat(itemName, levelMap[itemName]);
        }
        else
        {
            if (PlayerInventoryManager.Instance.PlayerResources >= cost)
            {
                levelMap[itemName]++;
                PlayerInventoryManager.Instance.PlayerResources -= cost;
                buttonCaller.SetOrbs(levelMap[itemName]);
                player.SetStat(itemName, levelMap[itemName]);
            }
        }
    }
}
