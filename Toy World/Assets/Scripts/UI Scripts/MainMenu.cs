using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Main menu controller and manager.
/// </summary>
public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject buttonPrefab;

    private GameObject contentHolder;
    private List<UnityEngine.Object> levels= new List<UnityEngine.Object>();
    private List<int> levelNumbers = new List<int>();
    private char[]  charToTrimEnd;
    private string levelPrefix = "Level ";
    private static bool start=true;

    /// <summary>
    /// Initialize the level select UI with correct completion
    /// </summary>
    void Start()
    {
        if (start)
        { 
            start = false;
        }
        else
        {
            Camera.main.GetComponent<AudioListener>().enabled = false;
        }
        charToTrimEnd = ".unity".ToCharArray();
        contentHolder = GetComponentInChildren<GridLayoutGroup>().gameObject;
        levels = Resources.LoadAll("Levels").ToList();

        //Load all the scenes from the build index in build settings and save their number
        for (int x=0; x<SceneManager.sceneCountInBuildSettings;x++)
        {
            if (SceneUtility.GetScenePathByBuildIndex(x).Contains("Level"))
            {
                string sceneName = SceneUtility.GetScenePathByBuildIndex(x);
                sceneName = sceneName.TrimEnd(charToTrimEnd);
                sceneName = sceneName.Remove(0,30);
                levelNumbers.Add(Int32.Parse(sceneName));
            }
        }

        levelNumbers.Sort();

        //Initialize a button for each level number found
        foreach (int levelNumber in levelNumbers)
        {
            string levelName = levelPrefix+levelNumber.ToString();
            GameObject button = Instantiate(buttonPrefab, contentHolder.transform);
            button.transform.localScale = new Vector3(1, 1, 1);
            button.name = levelName;
            button.GetComponentInChildren<TextMeshProUGUI>().text = levelName;
            button.transform.GetChild(0).GetComponent<Image>().sprite=(Sprite)Resources.Load("UI/LevelPreview/"+levelName,typeof(Sprite));
            button.GetComponent<Button>().onClick.AddListener(() => { AudioManager.Instance.MenuButtonClickSound(); button.GetComponent<GenericUIChoices>().LoadScene(levelName); });

            if (PlayerPrefs.GetInt(levelName+ "_Cleared")==1)
            {
                button.transform.GetChild(2).gameObject.SetActive(true);
            }
            else
            {
                button.transform.GetChild(2).gameObject.SetActive(false);

            }

            //If the level has been completed
            if (PlayerPrefs.GetInt(levelName) == 1)
            {
                button.GetComponent<Image>().color = new Color32(255,255,0,255);
            }
        }
    }
}
