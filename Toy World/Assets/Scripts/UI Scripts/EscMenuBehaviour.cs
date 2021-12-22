using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class EscMenuBehaviour : MonoBehaviour
{
    public GameObject escMenu;
    public GameObject coreBlock;
    public PlayerInput playerInput;
    public GameObject partSelectorUI;


    public static Vector3 buildCameraPositionStart;
    public static Quaternion buildCameraRotationStart;


    private Vector3 coreBlockPositionStart;
    private Quaternion coreBlockRotationStart;

    // Start is called before the first frame update
    void Start()
    {
        coreBlockPositionStart = coreBlock.transform.position;
        coreBlockRotationStart = coreBlock.transform.rotation;
    }

    public void Pause()
    {
        escMenu.SetActive(true);
        playerInput.actions.Disable();
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
    }

    public void Resume()
    {
        Time.timeScale = 1;

        if (VehicleEditor._instance.playan)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        playerInput.actions.Enable();
        escMenu.SetActive(false);
    }

    public void Restart()
    {
        Time.timeScale = 1;
        escMenu.SetActive(false);

        LevelManager.Instance.StopTimer();
        LevelManager.Instance.cargoSpawner.CleanCargo();

        if (VehicleEditor._instance.playan)
        {
            VehicleEditor._instance.Play();

            coreBlock.transform.position = coreBlockPositionStart;
            coreBlock.transform.rotation = coreBlockRotationStart;

            Camera.main.transform.position = buildCameraPositionStart;
            Camera.main.transform.rotation = buildCameraRotationStart;

            Camera.main.gameObject.GetComponent<FPSCameraControllers>().m_TargetCameraState.SetFromTransform(Camera.main.transform);
            Camera.main.gameObject.GetComponent<FPSCameraControllers>().m_InterpolatingCameraState.SetFromTransform(Camera.main.transform);            
        }
        else
        {
            StartCoroutine(LoadScene(SceneManager.GetActiveScene().name));

            //playerInput.SwitchCurrentActionMap("UI");

            //foreach (Transform child in coreBlock.transform)
            //{
            //    if (!child.name.Contains("ThirdPersonCam") && 
            //        !child.name.Contains("PlayerUI") &&
            //        !child.name.Contains("Wheels") && 
            //        !child.name.Contains("BoundingBoxWithDirectionArrow") &&
            //        !child.name.Contains("TestPart"))
            //    {
            //        Destroy(child.gameObject);
            //    }
            //}

            //foreach (Transform wheelChild in coreBlock.transform.GetChild(0).transform)
            //{
            //    Destroy(wheelChild.gameObject);
            //}

            //partSelectorUI.SetActive(true);
            //Cursor.lockState = CursorLockMode.None;
        }
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
