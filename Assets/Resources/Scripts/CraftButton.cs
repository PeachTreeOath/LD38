using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftButton : MonoBehaviour {

    public static Sprite blueOrb;

    public string itemName;

	KeyCode numKeyCode;

	UpgradesPanelController shopPanel;

    private void Awake()
    {
        blueOrb = Resources.Load<Sprite>("Textures/OrbBlue");
		numKeyCode = (KeyCode) System.Enum.Parse(typeof(KeyCode), "Alpha"+transform.Find("KeyNumberText").GetComponent<Text>().text);
		shopPanel = GameObject.Find("UpgradesPanel").GetComponent<UpgradesPanelController>();
    }

	void Update()
	{
		if(shopPanel.group.alpha == 1 &&
			Input.GetKeyDown(numKeyCode))
		{
			Debug.Log("Key down " + itemName);
		}
	}

    public void OnClick()
    {
        ShopManager.Instance.PurchaseItem(itemName, this);
    }

    public void SetOrbs(int numOrbs)
    {
        Image[] images = GetComponentsInChildren<Image>();
        int orbsPassed = 0;

        for (int i = 0; i < images.Length; i++)
        {
            if (images[i].gameObject.GetInstanceID() != gameObject.GetInstanceID())
            {
                images[i].sprite = blueOrb;
                orbsPassed++;
            }
            if(orbsPassed >= numOrbs)
            {
                break;
            }
        }
    }
}
