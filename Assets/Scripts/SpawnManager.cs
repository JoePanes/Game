using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private float spawnRangeX = 20;
    private float spawnRangeZ = 20;

    private float spawnDelay = 2;
    private float spawnInterval = 5;

    private int maxEnemies = 7;
    private int currentEnemiesAlive;
    public GameObject[] objectPrefabs;
    // Start is called before the first frame update
    void Start()
    {
        currentEnemiesAlive = GameObject.FindGameObjectsWithTag("Enemy").Length;
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
        if (isEnemy && currentEnemiesAlive >= maxEnemies)
        {
            // If too many enemies present, try again
            SpawnObjects();
            return;
        } else if (isEnemy)
        {
            currentEnemiesAlive += 1;
        }
        Vector3 spawnLocation = GenerateSpawnPosition();
        
        Instantiate(objectPrefabs[index], spawnLocation, objectPrefabs[index].transform.rotation);

    }

    Vector3 GenerateSpawnPosition()
    {
        float xPos = Random.Range(-spawnRangeX, spawnRangeX);
        float zPos = Random.Range(-spawnRangeZ, spawnRangeZ);
        return new Vector3(xPos, 0, zPos);
    }

    public void DecrementEnemyCount()
    {
        currentEnemiesAlive -= 1;
        Debug.Log(currentEnemiesAlive);
    }
}
