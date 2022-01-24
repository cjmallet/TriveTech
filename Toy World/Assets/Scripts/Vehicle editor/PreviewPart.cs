using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewPart : MonoBehaviour
{
    List<Material> materials = new List<Material>();
    //Color green =

    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Part>().enabled = false;
        Material previewMat = Resources.Load("Materials/Preview material") as Material;
        foreach (Transform transform in gameObject.GetComponentsInChildren<Transform>())
        {
            ReplaceMaterials(transform.gameObject, previewMat);
        }
    }

    private void ReplaceMaterials(GameObject obj, Material mat)
    {
        if (obj.TryGetComponent(out MeshRenderer renderer))
        {
            renderer = obj.GetComponent<MeshRenderer>();
            Material[] mats = new Material[renderer.materials.Length];
            for (int i = 0; i < renderer.materials.Length; i++)
            {
                mats[i] = Instantiate(mat);
                materials.Add(mats[i]);
            }
            renderer.materials = mats;
        }
    }
}
