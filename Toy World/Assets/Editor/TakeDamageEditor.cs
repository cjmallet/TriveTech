using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(VehicleStats))]
public class TakeDamageEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        VehicleStats statScript = (VehicleStats)target;
        if (GUILayout.Button("Take damage"))
        {
            statScript.TakeDamage(10);
        }
    }
}
