using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private int health;
    private int collectedGold;

    private float speed = 2750;
    private float turnSpeed = 75;

    private float forwardInput;
    private float rotationInput;

    private bool canPowerAttack;
    public static bool sprinting;
    public static bool gameOver;

    public TextMeshProUGUI healthDisplay;
    public TextMeshProUGUI goldCounter;

    public float gravityModifer;

    public GameObject sigil;

    private Animator anim;

    [SerializeField]
    private ParticleSystem powerAttack;

    private AudioManager audioMana;

    Rigidbody playerRb;
    

    // Start is called before the first frame update
    void Start()
    {
        //Initialise variables
        Physics.gravity *= gravityModifer;
        gameOver = false;
        collectedGold = 0;
        sprinting = false;
        health = 5;
        canPowerAttack = true;

        //Connect to attached components
        anim = GetComponent<Animator>();
        playerRb = GetComponent<Rigidbody>();

        //Setup player specific UI
        DisplayGold();
        DisplayHealth();

        audioMana = FindObjectOfType<AudioManager>();

    }

    // Update is called once per frame
    void Update()
    {

        if (gameOver != true)
        {
            MovePlayer();

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Instantiate(sigil, transform.position, transform.rotation);
            }

            if (Input.GetKeyDown(KeyCode.LeftAlt))
            {
                StartCoroutine("PowerAttack");
            }
        }
    }

    void MovePlayer() 
    {

        //Allow the player to move forward and backward
        forwardInput = Input.GetAxis("Vertical");

        if (Input.GetKeyDown(KeyCode.LeftShift) && sprinting == false)
        {
            sprinting = true;
            speed = speed * 2;
            StartCoroutine("Sprint");
        }
        // if the player is moving backwards, reduce speed
        if (forwardInput < 0)
        {
            playerRb.AddForce(transform.forward * (speed - (speed * 0.5f))  * forwardInput);
        }
        else
        {
            playerRb.AddForce(transform.forward * speed * forwardInput);

        }

        // Allow player to rotate
        rotationInput = Input.GetAxis("Horizontal");

        transform.Rotate(Vector3.up, Time.deltaTime * turnSpeed * rotationInput);

        // Share input values with animator
        anim.SetFloat("inputForward_f", forwardInput);
        

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage();
            
            // If player has no health, then end game
            if (NoHealth() && !gameOver)
            {
                GameOver();
            }
            
        } else if (collision.gameObject.CompareTag("Treasure"))
        {
            UpdateGold();

            Destroy(collision.gameObject);
        }

        
    }

    void UpdateGold()
    {
        collectedGold += 1;

        DisplayGold();
    }

    void TakeDamage()
    {
        if (health > 0)
        {
            health -= 1;

            DisplayHealth();
        }
    }

    void DisplayGold()
    {
        goldCounter.text = "" + collectedGold;
    }

    void DisplayHealth()
    {
        healthDisplay.text = "Health: " + health;
    }

    bool NoHealth()
    {
        return health <= 0;
    }

    IEnumerator Sprint ()
    {
        anim.SetBool("isSprinting", true);
        yield return new WaitForSeconds(2);

        speed = speed / 2;

        anim.SetBool("isSprinting", false);
        sprinting = false;

    }

    IEnumerator PowerAttack()
    {
        if (canPowerAttack)
        {
            canPowerAttack = !canPowerAttack;

            audioMana.Play("PlayerPowerAttack");
            yield return new WaitForSeconds(1.5f);
            powerAttack.Play();
        } else
        {
            yield return new WaitForSeconds(0);
        }
    }

    void GameOver()
    {
        Debug.Log("Game Over");
        gameOver = true;

        anim.SetBool("isGameOver", true);
        audioMana.Play("PlayerDeath");
    }

}
