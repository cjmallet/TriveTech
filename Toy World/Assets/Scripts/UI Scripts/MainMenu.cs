using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject buttonPrefab;

    private GameObject contentHolder;
    private List<SceneAsset> levels= new List<SceneAsset>();

    // Start is called before the first frame update
    void Start()
    {
        contentHolder = GetComponentInChildren<GridLayoutGroup>().gameObject;
        levels = Resources.LoadAll("Levels", typeof(SceneAsset)).Cast<SceneAsset>().ToList();

        foreach (SceneAsset scene in levels)
        {
            GameObject button= Instantiate(buttonPrefab,contentHolder.transform);
            button.transform.localScale = new Vector3(1,1,1);
            button.name = scene.name;
            button.GetComponent<Button>().onClick.AddListener(() => { button.GetComponent<GenericUIChoices>().LoadScene(scene.name);});
        }
    }
}
