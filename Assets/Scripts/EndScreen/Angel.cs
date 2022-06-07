using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Angel : MonoBehaviour
{

    GameObject player;
    Animator anim;


    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");



        anim = gameObject.GetComponent<Animator>();
    }

    private void Update()
    {
        if (player.transform.position.z >= -12)
        {
            anim.SetBool("playerInRange", true); 
        } else
        {
            anim.SetBool("playerInRange", false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        SceneManager.LoadScene(0);
    }
}
