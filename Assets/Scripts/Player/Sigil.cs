using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sigil : MonoBehaviour
{
    public GameObject SigilLight;
    public ParticleSystem SigilParticles;

    private float speed = -3f;

    private bool hasBeenTriggered;
    private bool animationComplete;


    // Start is called before the first frame update
    void Start()
    {
        animationComplete = false;
        hasBeenTriggered = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (hasBeenTriggered)
        {
            ShowSigilTriggered();
        }
    }

    void ShowSigilTriggered()
    {
        SigilLight.transform.Translate(Vector3.down * speed * Time.deltaTime);

        // After sending the light up, bring it back below the floor
        if (SigilLight.transform.position.y > 1.5f)
        {
            speed = -speed;
            animationComplete = true;
        }

        // Destroy the sigil when the animation for activation is complete 
        if (SigilLight.transform.position.y < 0f && animationComplete)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            GetComponent<AudioSource>().Play();
            SigilLight.SetActive(true);
            hasBeenTriggered = true;
            SigilParticles.Play();

            other.GetComponent<Enemy>().DestroyEnemy();
        }
    }

}
