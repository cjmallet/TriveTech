using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Allows for some generic UI options to be called.
/// </summary>
public class GenericUIChoices : MonoBehaviour
{
    public void CloseUI(GameObject UI)
    {
        UI.SetActive(false);
    }

    public void OpenUI(GameObject UI)
    {
        UI.SetActive(true);
    }

    /// <summary>
    /// Load a scene dependent on the scene name given and reset neccesary functions
    /// </summary>
    /// <param name="sceneName">The name of the scene to be loaded</param>
    public void LoadScene(string sceneName)
    {
        Time.timeScale = 1;

        FPSCameraControllers.canRotate = false;

        Cursor.lockState = CursorLockMode.None;

        SceneManager.LoadScene(sceneName);
    }
}
