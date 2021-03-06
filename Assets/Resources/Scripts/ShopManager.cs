﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : NonPersistentSingleton<ShopManager>
{

    private bool isActive;
    private UpgradesPanelController shopPanel;
    private PlayerController player;
    private RocketController rocket;

    public int[] shipCosts = new int[1];
    public int[] speedCosts = { 15, 50, 125 };
    public int[] jumpCosts = { 15, 50, 125 };
    public int[] armorCosts = { 15, 50, 125 };
    public int[] radarCosts = { 15, 50, 125 };
    public int[] magnetCosts = { 15, 50, 125 };
    public int[] resourceCosts = { 15, 50, 125 };
    public int shipPartCost = 100;

    public const string engineString = "Engine";
    public const string shuttleString = "Shuttle";
    public const string boostersString = "Boosters";

    public const string speedString = "Speed";
    public const string jumpString = "Jump";
    public const string armorString = "Armor";
    public const string radarString = "Radar";
    public const string magnetString = "Magnet";
    public const string resourceString = "Resource";

    public Dictionary<string, int[]> costMap;
    public Dictionary<string, int> levelMap;
    public bool hasEngine;
    public bool hasShuttle;
    public bool hasBoosters;

    public bool debugOn;
    private Material defaultMat;

    protected override void Init()
    {
        defaultMat = Resources.Load<Material>("Materials/DefaultMaterial");
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        rocket = GameObject.Find("RocketTest").GetComponent<RocketController>();
        costMap = new Dictionary<string, int[]>();
        costMap.Add(speedString, speedCosts);
        costMap.Add(jumpString, jumpCosts);
        costMap.Add(armorString, armorCosts);
        costMap.Add(radarString, radarCosts);
        costMap.Add(magnetString, magnetCosts);
        costMap.Add(resourceString, resourceCosts);
        costMap.Add(engineString, shipCosts);
        costMap.Add(shuttleString, shipCosts);
        costMap.Add(boostersString, shipCosts);

        levelMap = new Dictionary<string, int>();
        levelMap.Add(engineString, 0);
        levelMap.Add(shuttleString, 0);
        levelMap.Add(boostersString, 0);
        levelMap.Add(speedString, Globals.speedStat);
        levelMap.Add(jumpString, Globals.jumpStat);
        levelMap.Add(armorString, Globals.armorStat);
        levelMap.Add(radarString, Globals.radarStat);
        levelMap.Add(magnetString, Globals.magnetStat);
        levelMap.Add(resourceString, Globals.resourceStat);

        shipCosts[0] = shipPartCost;
        // Init prices
        CraftButton[] buttons = GameObject.FindObjectsOfType<CraftButton>();
        foreach (CraftButton button in buttons)
        {
            SetButtonCost(button, costMap[button.itemName][0]);
        }
    }

    public void SetShipPartCost(int cost)
    {
        shipPartCost = cost;
        shipCosts[0] = shipPartCost;
        // Init prices
        CraftButton[] buttons = GameObject.FindObjectsOfType<CraftButton>();
        foreach (CraftButton button in buttons)
        {
            if (button.itemName.Equals(engineString) || button.itemName.Equals(shuttleString) || button.itemName.Equals(boostersString))
            {
                SetButtonCost(button, cost);
            }
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

    public void SetButtonCost(CraftButton buttonCaller, int cost)
    {
        Text costText = buttonCaller.GetComponentInChildren<Text>();
        costText.text = "x " + cost;
    }

    public void SetButtonBlank(CraftButton buttonCaller)
    {
        Text costText = buttonCaller.GetComponentInChildren<Text>();
        costText.text = "x  -";
    }

    public void PurchaseItem(string itemName, CraftButton buttonCaller, bool hasCost)
    {
        int level = levelMap[itemName];

		if (level > 2 && hasCost)
        {
            return;
        }

        if (itemName.Equals(engineString) )
        {
            if (!hasCost || debugOn || PlayerInventoryManager.Instance.PlayerResources >= shipPartCost && level == 0)
            {
                hasEngine = true;
                rocket.BuildEngine(defaultMat);
                if (!hasCost || !debugOn)
                    PlayerInventoryManager.Instance.PlayerResources -= shipPartCost;
                buttonCaller.SetOrbs(1);
                levelMap[itemName]++;
            }
            return;
        }
        else if (itemName.Equals(shuttleString) )
        {
            if (!hasCost || debugOn || PlayerInventoryManager.Instance.PlayerResources >= shipPartCost && level == 0)
            {
                hasShuttle = true;
                rocket.BuildShuttle(defaultMat);
                if (!hasCost || !debugOn)
                    PlayerInventoryManager.Instance.PlayerResources -= shipPartCost;
                buttonCaller.SetOrbs(1);
                levelMap[itemName]++;
            }
            return;
        }
        else if (itemName.Equals(boostersString) )
        {
            if (!hasCost || debugOn || PlayerInventoryManager.Instance.PlayerResources >= shipPartCost && level == 0)
            {
                hasBoosters = true;
                rocket.BuildBoosters(defaultMat);
                if (!hasCost || !debugOn)
                    PlayerInventoryManager.Instance.PlayerResources -= shipPartCost;
                buttonCaller.SetOrbs(1);
                levelMap[itemName]++;
            }
            return;
        }

		int cost = 0;
		if(hasCost)
		{
			cost = costMap[itemName][level];
		}
        if (!hasCost || debugOn)
        {
            if (hasCost)
                levelMap[itemName]++;
            int newLevel = levelMap[itemName];
            buttonCaller.SetOrbs(newLevel);
            player.SetStat(itemName, newLevel);
            if (newLevel < 3)
            {
                SetButtonCost(buttonCaller, costMap[itemName][newLevel]);
            }
            else
            {
                SetButtonBlank(buttonCaller);
            }
        }
        else
        {
            if (PlayerInventoryManager.Instance.PlayerResources >= cost)
            {
                levelMap[itemName]++;
                int newLevel = levelMap[itemName];
                PlayerInventoryManager.Instance.PlayerResources -= cost;
                buttonCaller.SetOrbs(newLevel);
                player.SetStat(itemName, newLevel);
                if (newLevel < 3)
                {
                    SetButtonCost(buttonCaller, costMap[itemName][newLevel]);
                }
                else
                {
                    SetButtonBlank(buttonCaller);
                }
            }
        }
    }
}
