using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Angel : MonoBehaviour
{

    GameObject player;
    Animator anim;
    AudioSource audioSource;
    public AudioClip chanting;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        audioSource = GetComponent<AudioSource>();

        anim = gameObject.GetComponent<Animator>();
    }

    private void Update()
    {
        if (player.transform.position.z >= -12)
        {
            anim.SetBool("playerInRange", true);
            audioSource.PlayOneShot(chanting);   
        } else
        {
            anim.SetBool("playerInRange", false);
            audioSource.Stop();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        SceneManager.LoadScene(0);
    }
}
