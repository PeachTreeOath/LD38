
using UnityEngine;

public class NextLevelPoller : MonoBehaviour {

	bool loadingLevel;

	// Update is called once per frame
	void Update () {
		if(!loadingLevel &&
			Input.GetAxisRaw("Submit") > 0)
		{
			Time.timeScale = 1;
			loadingLevel = true;
            //Debug.Log(Time.time + " go to next level");
            //TODO: Load next level
            Globals.currentLevel += 1;

            if (Globals.currentLevel == 5)
                SceneTransitionManager.Instance.ShowVictoryScene();
            else
                SceneTransitionManager.Instance.ResetGame();
		}
	}
}
