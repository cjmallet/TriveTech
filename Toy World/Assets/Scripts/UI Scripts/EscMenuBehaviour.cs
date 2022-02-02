using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using TMPro;

public class EscMenuBehaviour : MonoBehaviour
{
    public GameObject escMenu;
    public PlayerInput playerInput, gameInput;
    public GameObject partSelectorUI;

    public static Vector3 buildCameraPositionStart;
    public static Quaternion buildCameraRotationStart;

    /// <summary>
    /// Method that pauses the game 
    /// </summary>
    public void Pause()
    {
        if (GameManager.Instance.vehicleEditor.coreBlockPlayMode != null)
            playerInput = GameManager.Instance.vehicleEditor.coreBlockPlayMode.GetComponent<PlayerInput>();
        escMenu.SetActive(true);
        playerInput.actions.Disable();
        gameInput.actions.Disable();
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
        AudioListener.pause = true;
    }

    /// <summary>
    /// Method that resumes the game 
    /// </summary>
    public void Resume()
    {
        Time.timeScale = 1;
        AudioListener.pause = false;

        if (!GameManager.Instance.partSelectionManager.buildUIOpen)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }

        playerInput.actions.Enable();
        gameInput.actions.Enable();
        escMenu.SetActive(false);
    }

    /// <summary>
    /// Method that restarts the scene if a player wants to restart in a level
    /// </summary>
    public void Restart()
    {
        if (GameManager.Instance.stateManager.CurrentGameState == GameStateManager.GameState.Playing) //restart the level if playing
        {
            StartCoroutine(LoadScene(SceneManager.GetActiveScene().name));
            GameManager.Instance.stateManager.CurrentGameState = GameStateManager.GameState.Building;
            Resume();
        }
        else  //discard the created vehicle
        {
            GameManager.Instance.vehicleEditor.DeleteAllParts();
            Resume();
        }
    }

    /// <summary>
    /// Method that fixes the preview part not staying in the scene when it is not supposed to be there
    /// </summary>
    public void ResetUnnecesaryParts()
    {
        AudioListener.volume = 0;
        AudioListener.pause = false;
        GameManager.Instance.vehicleEditor.RemovePreviewPart();
    }

    /// <summary>
    /// Enumerator that handles loading of the scene given in the parameter
    /// </summary>
    /// <param name="sceneName"></param>
    /// <returns></returns>
    private IEnumerator LoadScene(string sceneName)
    {
        // Start loading the scene
        AsyncOperation asyncLoadLevel = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);

        if (FPSCameraControllers.canRotate)
        {
            FPSCameraControllers.canRotate = false;
        }
        // Wait until the level finish loading
        while (!asyncLoadLevel.isDone)
            yield return null;
        // Wait a frame so every Awake and Start method is called
        yield return new WaitForEndOfFrame();
    }
}
