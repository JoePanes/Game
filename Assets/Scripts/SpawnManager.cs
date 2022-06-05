using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private float spawnRangeX = 70;
    private float spawnRangeZ = 60;

    private float spawnDelay = 2;
    private float spawnInterval = 5;

    private int maxEnemies = 2;
    private int maxTreasure = 1;
    private int currentEnemiesAlive;
    private int currentTreasure;
    public GameObject[] objectPrefabs;
    // Start is called before the first frame update
    void Start()
    {
        currentEnemiesAlive = GameObject.FindGameObjectsWithTag("Enemy").Length;
        currentTreasure = GameObject.FindGameObjectsWithTag("Treasure").Length;

        InvokeRepeating("SpawnObjects", spawnDelay, spawnInterval);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnObjects()
    {
        int index = Random.Range(0, objectPrefabs.Length);

        bool isEnemy = objectPrefabs[index].CompareTag("Enemy");

        //Check if current object should be spawned
        if (isEnemy)
        {
            //If max enemies reached, don't spawn
            if (currentEnemiesAlive >= maxEnemies)
            {
                return;
            } else
            {
                currentEnemiesAlive += 1;
            }
        } else
        {
            //Don't spawn more treasure past a certain limit
            if (currentTreasure >= maxTreasure)
            {
                return;
            } else
            {
                currentTreasure += 1;
            }
        }
        Vector3 spawnLocation = GenerateSpawnPosition();
        
        Instantiate(objectPrefabs[index], spawnLocation, objectPrefabs[index].transform.rotation);

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
