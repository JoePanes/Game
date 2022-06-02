using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    private GameObject player;

    private Rigidbody enemyRb;

    // Start is called before the first frame update
    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        //If player gets close enough, chase after them
        if (Vector3.Distance(player.transform.position, transform.position) < 50)
        {
            Vector3 lookDirection = (player.transform.position - transform.position);

            lookDirection.y = 0;

            var rotation = Quaternion.LookRotation(lookDirection);

            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);

            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime);
        }

    }

    public void DestroyEnemy()
    {
        FindObjectOfType<SpawnManager>().DecrementEnemyCount();
        Destroy(gameObject);
    }
}
