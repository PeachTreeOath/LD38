using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradesPanelController : MonoBehaviour {

	[HideInInspector]
	public CanvasGroup group;

    void Start()
    {
        group = GetComponent<CanvasGroup>();
    }

    public void ToggleShow(bool show)
    {
        if(show)
        {
            group.alpha = 1;
            group.interactable = true;
            group.blocksRaycasts = true;
        }
        else
        {
            group.alpha = 0;
            group.interactable = false;
            group.blocksRaycasts = false;
        }
    }

}
