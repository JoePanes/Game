using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForward : MonoBehaviour
{
    protected static float speed;

    protected bool canDamage;

    protected static AudioSource audiosource;
    // Start is called before the first frame update
    void Start()
    {
        audiosource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        //Allow for the projectile to not impact anything if specified
        if (canDamage)
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                other.GetComponent<Enemy>().DestroyEnemy();
            }
            else if (other.gameObject.CompareTag("Player"))
            {
                other.GetComponent<PlayerController>().TakeDamage();
            }

            Destroy(gameObject);
        }
    }

}
