using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreenMusic : MonoBehaviour {

	// Use this for initialization
	void Start () {
        AudioManager.Instance.PlayMusic("Title_Screen", .50f);
    }
	
	
}
