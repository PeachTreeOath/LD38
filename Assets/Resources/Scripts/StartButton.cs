using UnityEngine;

public class StartButton : MonoBehaviour {

    public void BeginGame()
    {
		Globals.currentLevel = 0;
		Globals.lastLevel = 0;
		Globals.radarStat = 0;
		Globals.speedStat = 0;
		Globals.armorStat = 0;
		Globals.jumpStat = 0;
		Globals.magnetStat = 0;
		Globals.resourceStat = 0;

		Globals.ship1 = false;
		Globals.ship2 = false;
		Globals.ship3 = false;

        SceneTransitionManager.Instance.StartGame();
    }

    public void RestartGame()
    {
        SceneTransitionManager.Instance.RestartGame();
    }
}
