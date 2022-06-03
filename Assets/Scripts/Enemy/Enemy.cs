using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    private GameObject player;

    private Rigidbody enemyRb;

    private Animator anim;

    public AudioClip deathSound;
    
    private AudioSource audioSource;

    private bool isDead;

    // Start is called before the first frame update
    void Start()
    {
        isDead = false;
        enemyRb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");

        anim = GetComponent<Animator>();

        anim.SetInteger("walkAnimation", Random.Range(1, 4));

        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //If player gets close enough, chase after them
             ///
        
        if (false && Vector3.Distance(player.transform.position, transform.position) < 50 && isDead == false)
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
        isDead = true;
        FindObjectOfType<SpawnManager>().DecrementEnemyCount();

        audioSource.Stop();
        audioSource.PlayOneShot(deathSound);
        anim.SetBool("isDead", isDead);

        StartCoroutine(DeathAnimation());
    }

    IEnumerator DeathAnimation()
    {
        //Lets the animation play a bit before destroying the object
        yield return new WaitForSeconds(1.5f);

        Destroy(gameObject);
    }

    public bool CheckIfDead()
    {
        //For other classes to access the value of isDead
        return isDead;
    }
}
