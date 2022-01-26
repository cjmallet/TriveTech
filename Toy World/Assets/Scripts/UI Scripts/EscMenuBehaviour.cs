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
        if(VehicleEditor._instance.coreBlockPlayMode != null)
            playerInput = VehicleEditor._instance.coreBlockPlayMode.GetComponent<PlayerInput>();
        escMenu.SetActive(true);
        playerInput.actions.Disable();
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
    }

    public void Resume()
    {
        Time.timeScale = 1;

        if (VehicleEditor._instance.playan || !VehicleEditor._instance.buildUIOpen)
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
        if (VehicleEditor._instance.playan) //restart the level if playing
        {            
            StartCoroutine(LoadScene(SceneManager.GetActiveScene().name));
            Resume();
            Cursor.lockState = CursorLockMode.None;
        }
        else  //discard the created vehicle
        {
            VehicleEditor._instance.DeleteAllParts();
            Resume();
        }
    }

    public void RemoveButton()
    {
        VehicleEditor._instance.RemovePreviewPart();
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
