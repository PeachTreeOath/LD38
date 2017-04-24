using System;
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
    public int[] speedCosts = { 20, 40, 100 };
    public int[] jumpCosts = { 10, 30, 70 };
    public int[] armorCosts = { 75, 125, 250 };
    public int[] radarCosts = { 30, 50, 150 };
    public int[] magnetCosts = { 50, 300, 750 };
    public int[] resourceCosts = { 50, 300, 1000 };
    public int shipPartCost = 100; // TODO: Set this differently per level

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
        levelMap.Add(speedString, 0);
        levelMap.Add(jumpString, 0);
        levelMap.Add(armorString, 0);
        levelMap.Add(radarString, 0);
        levelMap.Add(magnetString, 0);
        levelMap.Add(resourceString, 0);

        // Kick off initial theme here since there are no other scene singletons..
        

        shipCosts[0] = shipPartCost;
        // Init prices
        CraftButton[] buttons = GameObject.FindObjectsOfType<CraftButton>();
        foreach(CraftButton button in buttons)
        {
            SetButtonCost(button, costMap[button.itemName][0]);
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

    public void PurchaseItem(string itemName, CraftButton buttonCaller)
    {
        int level = levelMap[itemName];

        if (level > 2)
        {
            return;
        }

        if (itemName.Equals(engineString))
        {
            if (debugOn || PlayerInventoryManager.Instance.PlayerResources >= shipPartCost)
            {
                hasEngine = true;
                rocket.BuildEngine();
                if (!debugOn)
                    PlayerInventoryManager.Instance.PlayerResources -= shipPartCost;
                buttonCaller.SetOrbs(1);
                GameObject.Find("EngineSilhouette").GetComponent<Image>().material = defaultMat;
            }
            return;
        }
        else if (itemName.Equals(shuttleString))
        {
            if (debugOn || PlayerInventoryManager.Instance.PlayerResources >= shipPartCost)
            {
                hasShuttle = true;
                rocket.BuildShuttle();
                if (!debugOn)
                    PlayerInventoryManager.Instance.PlayerResources -= shipPartCost;
                buttonCaller.SetOrbs(1);
                GameObject.Find("ShuttleSilhouette").GetComponent<Image>().material = defaultMat;
            }
            return;
        }
        else if (itemName.Equals(boostersString))
        {
            if (debugOn || PlayerInventoryManager.Instance.PlayerResources >= shipPartCost)
            {
                hasBoosters = true;
                rocket.BuildBoosters();
                if (!debugOn)
                    PlayerInventoryManager.Instance.PlayerResources -= shipPartCost;
                buttonCaller.SetOrbs(1);
                GameObject.Find("BoostersSilhouette").GetComponent<Image>().material = defaultMat;
            }
            return;
        }

        int cost = costMap[itemName][level];
        if (debugOn)
        {
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
