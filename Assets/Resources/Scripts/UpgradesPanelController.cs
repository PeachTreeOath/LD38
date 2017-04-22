using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradesPanelController : MonoBehaviour {

    private CanvasGroup group;

    void Start()
    {
        group = GetComponent<CanvasGroup>();
		Debug.Log(Time.time + " Set group to " + group);
    }

    public void ToggleShow(bool show)
    {
        if(show)
        {
            group.alpha = 1;
        }
        else
        {
            group.alpha = 0;
        }
    }

}
