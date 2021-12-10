using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private float SpawnDelay = 1f;
    [SerializeField] private float spawnDistanceMin;
    [SerializeField] private float spawnDistanceMax;
    [SerializeField] private float minDistanceBetweenSpawnPoints;

    [SerializeField] private float distanceToActivate;
    public static float distanceToActivateValue;

    public static int amountOfSpawnPoints;

    private List<GameObject> spawnPointList = new List<GameObject>();

    public static List<GameObject> enemyList = new List<GameObject>();

    private int spawnPointIndex = 0;

    private Transform player;
    private GameObject enemyPrefab;
    private GameObject sandTerrain;
    private GameObject enemyParent;
    private GameObject spawnPoint;
    private GameObject spawnPointParent;


    // Start is called before the first frame update
    void Start()
    {
        enemyPrefab = Resources.Load("Enemy") as GameObject;
        enemyParent = GameObject.Find("EnemyParent");
        player = GameObject.Find("CoreBlock").transform;
        sandTerrain = GameObject.Find("SandTerrain");
        spawnPoint = Resources.Load("SpawnPoint") as GameObject;
        spawnPointParent = GameObject.Find("EnemySpawnPoints");

        distanceToActivateValue = distanceToActivate;
    }

    public void SetSpawnPoints(int amountOfSpawnPoints)
    {
        /// Used for potential random spawnpoints inside of a level

        //foreach (Transform spawnPoint in spawnPointParent.transform)
        //{
        //    Destroy(spawnPoint.gameObject);
        //}

        //spawnPointList.Clear();

        //for (int i = 0; i < amountOfSpawnPoints; i++)
        //{ 
        //    Instantiate(spawnPoint, spawnPointParent.transform);
        //    spawnPointList.Add(spawnPoint.gameObject);
        //}

        foreach (Transform spawnPoint in spawnPointParent.transform)
        {
            spawnPointList.Add(spawnPoint.gameObject);
        }
    }

    public void SetSpawnPointsLocation()
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
                        if (spawnPointList[j].gameObject.name == spawnPointList[i].gameObject.name)
                        {
                            goodtogo = true;
                        }
                        else if ((spawnPointList[j - 1].transform.position - spawnPointList[i].transform.position).magnitude < minDistanceBetweenSpawnPoints)
                        {
                            goodtogo = false;
                            break;
                        }
                    }
                }
            }
        }
    }

    public IEnumerator SpawnEnemies(int numberOfEnemiesToSpawn)
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
        enemyList.Add(instantiatedEnemy);
        instantiatedEnemy.GetComponent<NavMeshAgentBehaviour>().agentId++;
    }
}
