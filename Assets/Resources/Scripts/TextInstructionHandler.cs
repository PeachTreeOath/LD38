using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextInstructionHandler : MonoBehaviour {

    public Text craftText;
    public Text shipText;

    private bool isNearCraft;
    private bool isNearShip;

    public void SetNearCraft(bool near)
    {
        isNearCraft = near;
        RecalculateText();
    }

    public void SetNearShip(bool near)
    {
        isNearShip = near;
        RecalculateText();
    }

    private void RecalculateText()
    {
        if(isNearCraft)
        {
            craftText.enabled = true;
            shipText.enabled = false;
        }
        else if (isNearShip)
        {
            shipText.enabled = true;
            craftText.enabled = false;
        }
        else
        {
            craftText.enabled = false;
            shipText.enabled = false;
        }
    }
}
