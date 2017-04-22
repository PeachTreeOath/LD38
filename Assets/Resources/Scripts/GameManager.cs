using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager> {

	public void SetHealth(int health)
    {
        if(health <= 0)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        // Show upgrade menu
        if(SceneTransitionManager.instance != null)
        {
            SceneTransitionManager.instance.ResetGame();
        }
        else
        {
            Debug.LogError("You forgot to load the persistence scene");
        }
    }
}
