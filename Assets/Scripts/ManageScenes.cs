using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManageScenes : MonoBehaviour
{
    // Loads a single scene and unloads the current
    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name, LoadSceneMode.Single);
    }

    // Loads a scene ontop the current scene
    public void LoadAdditiveScene(string name)
    {
        SceneManager.LoadScene(name, LoadSceneMode.Additive);
    }

    // Attempt to unload a scene
    public void UnloadAdditiveScene(string name)
    {
        SceneManager.UnloadSceneAsync(name);
    }

    // Quit game entirely
    public void QuitGame()
    {
        Application.Quit();
    }
}
