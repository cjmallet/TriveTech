using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewPart : MonoBehaviour
{
    List<Material> materials = new List<Material>();
    Color green = new Color(0f, 1f, 0f);
    Color red = new Color(1f, 0f, 0f);

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
                if(renderer.materials[i].GetTexture("_BaseMap") != null)//null check for weird materials
                    mats[i].SetTexture("_AlbedoTexture", renderer.materials[i].GetTexture("_BaseMap"));
                materials.Add(mats[i]);
            }
            renderer.materials = mats;
        }
    }

    /// <summary>
    /// changes the color value of every material in the object to green or red
    /// </summary>
    /// <param name="validity">True if valid (green), false if invalid (red)</param>
    public void SetMaterialColor(bool validity)
    {
        foreach (Material mat in materials)
        {
            if (validity)
                mat.SetColor("_AlbedoColor", green);
            else
                mat.SetColor("_AlbedoColor", red);
        }
    }
}
