using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextFader : MonoBehaviour {

    private float elapsedTime;
    private int phase;
    private CanvasGroup group;

    void Start()
    {
        group = GetComponent<CanvasGroup>();
    }
	// Update is called once per frame
	void Update () {

        elapsedTime += Time.deltaTime;
        
        if(phase == 0)
        {
            group.alpha = Mathf.Lerp(0, 1, elapsedTime);
            if(elapsedTime > 2)
            {
                phase = 1;
                elapsedTime = 0;
            }
        }
        else if(phase == 1)
        {
            group.alpha = Mathf.Lerp(1, 0, elapsedTime/2);
        }
        	
	}
}
