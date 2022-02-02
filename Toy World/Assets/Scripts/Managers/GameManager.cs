using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Manager for all other manager scripts.
/// </summary>
public class GameManager : MonoBehaviour
{
    /// <summary>
    /// Singleton gamemanager allows us to acces all managers everywhere.
    /// </summary>
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            return instance;
        }
    }

    void Awake()
    {
        //set the static instance
        if (instance == null) { instance = this; }
        else { Destroy(this); }

        Cursor.lockState = CursorLockMode.None; // Always start the game with the mouse unlocked.
    }

    public VehicleEditor vehicleEditor;
    public GameStateManager stateManager;
    public PartSelectionManager partSelectionManager;
    public EscMenuBehaviour menuManager;
    public LevelManager levelManager;
    public PlayerInput inputManager;
}
