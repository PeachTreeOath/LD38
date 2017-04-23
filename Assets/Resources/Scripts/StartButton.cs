using UnityEngine;

public class StartButton : MonoBehaviour {

    public void BeginGame()
    {
        SceneTransitionManager.Instance.StartGame();
    } 
}
