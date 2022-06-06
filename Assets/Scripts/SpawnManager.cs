using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private float spawnRangeX = 70;
    private float spawnRangeZ = 60;

    private float spawnDelay = 2;
    private float spawnInterval = 1;

    private int currentUnkillableAmount;
    private int maxUnkillable= 5;

    private int maxEnemies = 20;
    private int maxTreasure = 10;
    private int currentEnemiesAlive;
    private int currentTreasure;
    public GameObject[] objectPrefabs;

    [SerializeField]
    private GameObject UnkillableEnemy;

    // Start is called before the first frame update
    void Start()
    {
        currentUnkillableAmount = GameObject.FindGameObjectsWithTag("Unkillable").Length;
        currentEnemiesAlive = GameObject.FindGameObjectsWithTag("Enemy").Length;
        currentTreasure = GameObject.FindGameObjectsWithTag("Treasure").Length;

        InvokeRepeating("SpawnObject", spawnDelay, spawnInterval);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnObject()
    {
        int index = Random.Range(0, objectPrefabs.Length);

        spawnObject(objectPrefabs[index]);


    }

    void spawnObject(GameObject currentObject)
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
}
