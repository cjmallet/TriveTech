using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GenericUIChoices : MonoBehaviour
{
    public void CloseUI(GameObject UI)
    {
        UI.SetActive(false);
    }

    public void OpenUI(GameObject UI)
    {
        UI.SetActive(true);
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
