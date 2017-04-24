using UnityEngine;

public class ButtonActions : MonoBehaviour {

    public void BeginGame()
    {
        SceneTransitionManager.Instance.StartGame();
    } 

    public void RestartGame()
    {
        SceneTransitionManager.Instance.RestartGame();
    }
}
