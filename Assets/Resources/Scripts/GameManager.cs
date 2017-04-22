﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager> {

	protected override void Init()
	{

	}

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
        SceneTransitionManager.Instance.ResetGame();
    }
}
