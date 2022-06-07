using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private float spawnRangeX = 70;
    private float spawnRangeZ = 60;

    private float spawnDelay = 2;
    private float spawnInterval = 5;

    private int currentUnkillableAmount;
    private int maxUnkillable= 5;

    private int maxEnemies = 4;
    private int maxTreasure = 2;
    private int currentEnemiesAlive;
    private int currentTreasure;

    public GameObject[] objectPrefabs;

    public GameObject[] enemyPrefabs;

    private int unkillableSpawnThreshold = 20;

    [SerializeField]
    private GameObject unkillableEnemy;

    private PlayerController playerControl;


    // Start is called before the first frame update
    void Start()
    {
        playerControl = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        
        currentUnkillableAmount = GameObject.FindGameObjectsWithTag("Unkillable").Length;
        currentEnemiesAlive = GameObject.FindGameObjectsWithTag("Enemy").Length;
        currentTreasure = GameObject.FindGameObjectsWithTag("Treasure").Length;

        //Spawners
        InvokeRepeating("SpawnObject", spawnDelay, spawnInterval);

        StartCoroutine(UnkillableSpawner());

        StartCoroutine(EnemySpawner());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnEnemy()
    {
        int index = Random.Range(0, enemyPrefabs.Length);

        SpawnObject(enemyPrefabs[index]);
    }

    void SpawnObject()
    {
        int index = Random.Range(0, objectPrefabs.Length);

        SpawnObject(objectPrefabs[index]);
    }


    void SpawnObject(GameObject currentObject)
    {
        if (CheckIfObjectAllowed(currentObject))
        {
            SpawnCurrentObject(currentObject);
        }
        else
        {
            return;
        }

    }

    bool EnoughUnkillable()
    {
        return currentUnkillableAmount >= maxUnkillable;
    }

    bool EnoughEnemies()
    {
        return currentEnemiesAlive >= maxEnemies;
    }

    bool EnoughTreasure()
    {
        return currentTreasure >= maxTreasure;
    }



    void SpawnCurrentObject(GameObject currentObject)
    {
        Vector3 spawnLocation = GenerateSpawnPosition();

        Instantiate(currentObject, spawnLocation, currentObject.transform.rotation);
    }

    bool CheckIfObjectAllowed(GameObject currentObject)
    {
        if (currentObject.CompareTag("Enemy"))
        {
            //If max enemies reached, don't spawn
            if (EnoughEnemies())
            {
                return false;
            }
            else
            {
                currentEnemiesAlive += 1;
            }
        }
        else if (currentObject.CompareTag("Treasure"))
        {
            //Don't spawn more treasure past a certain limit
            if (EnoughTreasure())
            {
                return false;
            }
            else
            {
                currentTreasure += 1;
            }
        } else if (currentObject.CompareTag("Unkillable"))
        {
            if (EnoughUnkillable())
            {
                return false;
            } else
            {
                currentUnkillableAmount += 1;
            }
        } else
        {
            Debug.Log("You object does not bear a recognised tag that is configured to work with CheckIfObjectAllowed().");
            return false;
        }
        return true;
    }

    Vector3 GenerateSpawnPosition()
    {
        float xPos = Random.Range(-spawnRangeX, spawnRangeX);
        float zPos = Random.Range(-spawnRangeZ, spawnRangeZ);

        //1 for y so it doesn't fall through the floor
        return new Vector3(xPos, 1, zPos);
    }

    public void DecrementEnemyCount()
    {
        currentEnemiesAlive -= 1;
        IncremenetMaxEnemies();
    }

    public void DecrementTreasureCount()
    {
        currentTreasure -= 1;
    }

    public void IncremenetMaxEnemies()
    {
        maxEnemies += 1;

        //Update treasure
        maxTreasure = maxEnemies / 2;
    }

    private int GetNumberOfUnkillableToSpawn()
    {
        int playerTreasure = playerControl.collectedGold - unkillableSpawnThreshold;

        int numberOfUnkillable = 0;
        if (playerTreasure < 0)
        {
            return numberOfUnkillable;
        } else
        {
            return playerTreasure / 10 + 1;
        }
    }

    IEnumerator UnkillableSpawner()
    {
        int numberToSpawn = GetNumberOfUnkillableToSpawn();

        if (numberToSpawn > 0 && numberToSpawn > currentUnkillableAmount)
        {
            SpawnObject(unkillableEnemy);
            currentUnkillableAmount += 1;
        }
        yield return new WaitForSeconds(Random.Range(10, 100));
        StartCoroutine(UnkillableSpawner());
    }

    IEnumerator EnemySpawner()
    {
        yield return new WaitForSeconds(Random.Range(spawnInterval*5, spawnInterval * 10));
        if (currentEnemiesAlive < maxEnemies / 2)
        {
            do
            {
                SpawnEnemy();
            } while (currentEnemiesAlive < maxEnemies);
        }
        StartCoroutine(EnemySpawner());
    }
}
