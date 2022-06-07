using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unkillable : MonoBehaviour
{

    private float speed = 2.5f;

    private GameObject player;
    private float rangeX = 70;
    private float rangeY = 30;
    private float rangeZ = 60;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Start()
    {
        StartCoroutine(JumpToRandomPosition());
    }
    // Update is called once per frame
    void Update()
    {
        transform.LookAt(player.transform);
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        //Have this enemy kill anything it touches
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<Enemy>().DestroyEnemy();
        } else if (other.CompareTag("Player"))
        {
            do 
            {
                other.GetComponent<PlayerController>().TakeDamage();
            } while (other.GetComponent<PlayerController>().NoHealth() == false);
        }
    }

    IEnumerator JumpToRandomPosition()
    {
        // Normally, the player is able to simply outrun the unkillable enemy
        // it just because more of a nusiance due to the noise rather than a threat
        // to remedy this, have it randomly relocate to random locations so as to reduce
        // the issue of bunching up that occurs when there are more than one of them.
        // but also make them a more dynamic and present threat, since they are less predictable
        yield return new WaitForSeconds(Random.Range(10, 100));

        float randomX = Random.Range(-rangeX, rangeX);
        float randomY = Random.Range(-rangeY, rangeY);
        float randomZ = Random.Range(-rangeZ, rangeZ);

        transform.Translate(randomX, randomY, randomZ);

        yield return new WaitForSeconds(Random.Range(10, 100));
        StartCoroutine(JumpToRandomPosition());

    }
}
