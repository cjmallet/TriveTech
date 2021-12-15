using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTestVehicle : MonoBehaviour
{
    private GameObject testVehiclePrefab;
    private GameObject coreBlock;
    private Transform coreBlockTransform;

    public void SpawnTestVehicleMethod()
    {
        coreBlock = GameObject.Find("CoreBlock");
        coreBlockTransform = coreBlock.transform;
        Destroy(coreBlock);

        testVehiclePrefab = Resources.Load("TestVehicle1") as GameObject;
        Instantiate(testVehiclePrefab, coreBlockTransform.position, Quaternion.identity);
    }
}
