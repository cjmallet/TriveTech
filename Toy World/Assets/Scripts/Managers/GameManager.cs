using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
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
    }

    public VehicleEditor vehicleEditor;
    public GameStateManager stateManager;
    public PartSelectionManager partSelectionManager;
    public EscMenuBehaviour menuManager;
    public LevelManager levelManager;
}
