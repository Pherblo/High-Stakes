using UnityEngine;
using UnityEngine.SceneManagement;

public class ManageScenes : MonoBehaviour
{
    [SerializeField] public string _gameSceneName;

    public void LoadDelay(string name)
    {
        Invoke("LoadGameScene", 4);
    }

    public void LoadGameScene()
    {
        SceneManager.LoadScene(_gameSceneName);
    }

    // Loads a single scene and unloads the current
    public void LoadScene(string name)
    {
        Debug.Log("Loading scene: " + name);
        SceneManager.LoadScene(name, LoadSceneMode.Single);
    }

    // Loads a scene ontop the current scene
    public void LoadAdditiveScene(string name)
    {
        Debug.Log("Loading scene additively: " + name);
        SceneManager.LoadScene(name, LoadSceneMode.Additive);
    }

    // Attempt to unload a scene
    public void UnloadAdditiveScene(string name)
    {
        Debug.Log("Unloading scene: " + name);
        SceneManager.UnloadSceneAsync(name);
    }

    // Quit game entirely
    public void QuitGame()
    {
        Debug.Log("Quit game");
        Application.Quit();
    }
}
