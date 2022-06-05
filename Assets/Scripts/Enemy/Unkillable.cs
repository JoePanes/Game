using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unkillable : MonoBehaviour
{

    private float speed = 2;

    public GameObject player;

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
}
