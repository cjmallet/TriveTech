using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject buttonPrefab;

    private GameObject contentHolder;
    private List<UnityEngine.Object> levels= new List<UnityEngine.Object>();
    private List<int> levelNumbers = new List<int>();
    private char[] charToTrim;
    private string levelPrefix = "Level ";

    /// <summary>
    /// Initialize the level select UI with correct completion
    /// </summary>
    void Start()
    {
        charToTrim = levelPrefix.ToArray();
        contentHolder = GetComponentInChildren<GridLayoutGroup>().gameObject;
        levels = Resources.LoadAll("Levels").ToList();

        foreach (UnityEngine.Object level in levels)
        {
            levelNumbers.Add(GetLevelNumber(level.name));
        }

        levelNumbers.Sort();

        foreach (int levelNumber in levelNumbers)
        {
            string levelName = levelPrefix+levelNumber.ToString();
            GameObject button = Instantiate(buttonPrefab, contentHolder.transform);
            button.transform.localScale = new Vector3(1, 1, 1);
            button.name = levelName;
            button.GetComponentInChildren<TextMeshProUGUI>().text = levelName;
            button.GetComponent<Button>().onClick.AddListener(() => { button.GetComponent<GenericUIChoices>().LoadScene(levelName); });

            //If the level has been completed
            if (PlayerPrefs.GetInt(levelName) == 1)
            {
                button.GetComponent<Image>().color = new Color32(255,255,0,255);
            }
        }
    }

    private int GetLevelNumber(string sceneName)
    {
        string levelString = sceneName.Trim(charToTrim);
        int levelNumber = Int32.Parse(levelString);
        return levelNumber;
    }
}
