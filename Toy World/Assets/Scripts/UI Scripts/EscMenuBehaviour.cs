using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscMenuBehaviour : MonoBehaviour
{
    public GameObject escMenu;

    public GameObject coreBlock;
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
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
    }

    public void Resume()
    {
        Time.timeScale = 1;

        if (!VehicleEditor._instance.buildUIOpen)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        escMenu.SetActive(false);
    }

    public void Restart()
    {
        Time.timeScale = 1;
        escMenu.SetActive(false);

        LevelManager.Instance.StopTimer();

        if (!VehicleEditor._instance.buildUIOpen)
        {
            VehicleEditor._instance.Play();

            coreBlock.transform.position = coreBlockPositionStart;
            coreBlock.transform.rotation = coreBlockRotationStart;
        }
    }
}
