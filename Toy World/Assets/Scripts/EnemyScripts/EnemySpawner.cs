using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private int numberOfEnemiesToSpawn = 5;
    [SerializeField] private float SpawnDelay = 1f;
    [SerializeField] private float spawnDistanceMin;
    [SerializeField] private float spawnDistanceMax;
    [SerializeField] private float minDistanceBetweenSpawnPoints;

    [SerializeField] private float distanceToActivate;
    public static float distanceToActivateValue;

    [SerializeField] private GameObject enemySpawner;
    private List<GameObject> spawnPointList = new List<GameObject>();

    private int spawnPointIndex = 0;

    private Transform player;
    private GameObject enemyPrefab;
    private GameObject sandTerrain;
    private GameObject enemyParent;


    // Start is called before the first frame update
    void Start()
    {
        enemySpawner = GameObject.Find("EnemySpawnPoints");
        enemyPrefab = Resources.Load("Enemy") as GameObject;
        player = GameObject.Find("CoreBlock").transform;
        sandTerrain = GameObject.Find("SandTerrain");
        enemyParent = GameObject.Find("EnemyParent");

        distanceToActivateValue = distanceToActivate;

        SetSpawnPoints();
        StartCoroutine(SpawnEnemies(numberOfEnemiesToSpawn));
    }

    private void SetSpawnPoints()
    {
        foreach (Transform spawnPoint in enemySpawner.transform)
        {
            spawnPointList.Add(spawnPoint.gameObject);
        }

        SetSpawnPointsLocation();
    }

    private void SetSpawnPointsLocation()
    {
        MeshFilter sandTerrainMesh = sandTerrain.GetComponent<MeshFilter>();

        for (int i = 0; i < spawnPointList.Count; i++)
        {
            bool goodtogo = false;
            while (goodtogo == false)
            {
                int vertIndex = Random.Range(0, sandTerrainMesh.mesh.vertexCount);

                if (Vector3.Distance(sandTerrainMesh.mesh.vertices[vertIndex], player.transform.position) > spawnDistanceMin &&
                    Vector3.Distance(sandTerrainMesh.mesh.vertices[vertIndex], player.transform.position) < spawnDistanceMax)
                {
                    spawnPointList[i].transform.position = sandTerrainMesh.mesh.vertices[vertIndex];
                    goodtogo = true;

                    for (int j = i; j > 0; j--)
                    {
                        if ((spawnPointList[j - 1].transform.position - spawnPointList[i].transform.position).magnitude < minDistanceBetweenSpawnPoints)
                        {
                            goodtogo = false;
                            break;
                        }
                    }
                }
            }
        }
    }

    private IEnumerator SpawnEnemies(int numberOfEnemiesToSpawn)
    {
        WaitForSeconds Wait = new WaitForSeconds(SpawnDelay);
        int spawnedEnemies = 0;

        while (spawnedEnemies < numberOfEnemiesToSpawn)
        {
            spawnPointIndex = Random.Range(0, spawnPointList.Count);

            SpawnEnemy(spawnPointIndex);

            spawnedEnemies++;

            yield return Wait;
        }
    }

    private void SpawnEnemy(int spawnPointIndex)
    {
        GameObject instantiatedEnemy = Instantiate(enemyPrefab, spawnPointList[spawnPointIndex].transform.position, Quaternion.identity);
        instantiatedEnemy.transform.SetParent(enemyParent.transform);
    }
}
