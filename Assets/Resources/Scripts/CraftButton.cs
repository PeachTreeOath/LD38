using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftButton : MonoBehaviour {

    public static Sprite blueOrb;

    public string itemName;

    private void Awake()
    {
        blueOrb = Resources.Load<Sprite>("Textures/OrbBlue");
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
