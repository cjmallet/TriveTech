using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using TMPro;

public class EscMenuBehaviour : MonoBehaviour
{
    public GameObject escMenu;
    public PlayerInput playerInput;
    public GameObject partSelectorUI;

    public static Vector3 buildCameraPositionStart;
    public static Quaternion buildCameraRotationStart;

    public void Pause()
    {
        if (GameManager.Instance.vehicleEditor.coreBlockPlayMode != null)
            playerInput = GameManager.Instance.vehicleEditor.coreBlockPlayMode.GetComponent<PlayerInput>();
        escMenu.SetActive(true);
        playerInput.actions.Disable();
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
        AudioListener.pause = true;
    }

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
        escMenu.SetActive(false);
    }

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

    public void ResetUnnecesaryParts()
    {
        AudioListener.volume = 0;
        AudioListener.pause = false;
        GameManager.Instance.vehicleEditor.RemovePreviewPart();
    }

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
