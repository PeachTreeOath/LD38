using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This scene manager handles scene loading when there is a persistent scene used.
/// A temporary scene swap is needed because you can't load scenes additively and
/// set active scene on the same frame, so a buffer scene must be used and then
/// merged after frame 1.
/// </summary>
public class SceneTransitionManager : Singleton<SceneTransitionManager>
{
    private Scene tempScene;
    private string tempSceneName = "TempScene";

	protected override void Init()
	{
		
	}

    public void ShowTitleScreen()
    {
        string nextSceneName = "Title";
        SceneManager.LoadScene(nextSceneName, LoadSceneMode.Additive);
    }

    public void StartGame()
    {
        string nextSceneName = "Game";
		Globals.resources = 0;
		Application.LoadLevel(nextSceneName);
    }

    public void ShowVictoryScene()
    {
		Application.LoadLevel("Victory");
    }


    public void ResetGame()
    {
        //Globals.resources = PlayerInventoryManager.Instance.PlayerResources;
        Globals.resources = 0;
        PlayerInventoryManager.Instance.ResetGame();
		Application.LoadLevel(Application.loadedLevel);
    }

    // You only need to use mergeTempScene if you are potentially
    // instantiating objects on the first frame of scene startup
    public IEnumerator SetActive(Scene scene, bool mergeTempScene)
    {
        int i = 0;
        while (i == 0)
        {
            i++;
            yield return null;
        }
        SceneManager.SetActiveScene(scene);
        SceneManager.MergeScenes(tempScene, scene);

        yield break;
    }
}