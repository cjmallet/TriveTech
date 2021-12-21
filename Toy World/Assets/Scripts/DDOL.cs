using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Use this to keep references of stuff along different scenes
/// </summary>
public class DDOL : MonoBehaviour
{
    private static DDOL _instance;
    public static DDOL Instance
    {
        get { return _instance; }
    }

    private GameObject _p1Coreblock;//, _p2Coreblock;
    public GameObject P1Coreblock
    {
        get { return _p1Coreblock; }
    }
    /*
    public GameObject P2Coreblock
    {
        get { return _p2Coreblock; }
    }
    */

    void Awake()
    {
        //set the static instance
        if (_instance == null) { _instance = this; }
        else { Destroy(this); }

        DontDestroyOnLoad(this);
        InstantiatePersistentObjects();
    }

    private void InstantiatePersistentObjects()
    {
        if (_p1Coreblock == null)
        {
            _p1Coreblock = Instantiate(Resources.Load("Parts/CoreBlock") as GameObject);
            //_p1Coreblock.SetActive(false);
        }
        /*
        if (_p2Coreblock == null)
        {
            _p2Coreblock = Instantiate(Resources.Load("Parts/CoreBlock") as GameObject);
            _p2Coreblock.SetActive(false);
        }
        */
    }
}
