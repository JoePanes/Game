using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MoveForward
{
    private GameObject player;

    Rigidbody rigidBdy;
    private void Awake()
    {
        speed = 3000;

        player = GameObject.Find("Player");
        rigidBdy = GetComponent<Rigidbody>();

        StartCoroutine(EnableDamage());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 playerPos = player.transform.position;

        transform.LookAt(playerPos);

        rigidBdy.AddRelativeForce(Vector3.forward * speed * Time.deltaTime);


    }

    private void Update()
    {
        
    }

    IEnumerator EnableDamage()
    {
        yield return new WaitForSeconds(0.5f);

        canDamage = true;
    }

}
