using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEngine.SceneManagement;

public class SavePrefabPicture : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        GameObject picturePrefab = GameObject.FindGameObjectWithTag("Part");

        if (picturePrefab==null)
        {
            picturePrefab = GameObject.FindGameObjectWithTag("CoreBlock");
        }

        Camera screenshotCam = GetComponent<Camera>();
        screenshotCam.targetTexture = RenderTexture.GetTemporary(Screen.width, Screen.height, 16);
        RenderTexture newRender = screenshotCam.targetTexture;

        Texture2D readableTexture = new Texture2D(newRender.width, newRender.height, TextureFormat.ARGB32, false);
        Rect renderRectangle = new Rect(0,0,newRender.width,newRender.height);
        readableTexture.ReadPixels(renderRectangle,0,0);

        byte[] textureBytes = readableTexture.EncodeToPNG();
        File.WriteAllBytes("Assets/Resources/UI/Images/"+picturePrefab.name+".PNG", textureBytes);
        AssetDatabase.Refresh();
    }
}