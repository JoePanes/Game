using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private int health;
    private int collectedGold;

    private float speed = 2500;
    private float normalSpeed;
    private float sprintSpeed = 4500;
    private float turnSpeed = 75;

    private float forwardInput;
    private float rotationInput;

    private bool canPowerAttack;
    private bool canPlaceRune;

    //To keep track of how much gold is
    ///needed to replenish the power attack
    private int powerAttackGoldCount;

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

    public SpawnManager spawner;
    

    private bool canMove;

    private void Awake()
    {
        canMove = false;
        StartCoroutine(WaitForStartingAnimation());
    }

    IEnumerator WaitForStartingAnimation()
    {
        yield return new WaitForSeconds(15);
        canMove = true;
    }

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
        canPlaceRune = true;
        normalSpeed = speed;
        powerAttackGoldCount = 0;

        //Connect to attached components
        anim = GetComponent<Animator>();
        playerRb = GetComponent<Rigidbody>();

        //Setup player specific UI
        DisplayGold();
        DisplayHealth();

        audioMana = FindObjectOfType<AudioManager>();

        spawner = FindObjectOfType<SpawnManager>();
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();

        if (gameOver != true)
        {

            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartCoroutine("RunePlacement");
            }

            if (Input.GetKeyDown(KeyCode.LeftAlt))
            {
                StartCoroutine("PowerAttack");
            }
        }
    }

    void MovePlayer() 
    {
        if (!canMove) return;

        //Allow the player to move forward and backward
        forwardInput = Input.GetAxis("Vertical");

        if (Input.GetKeyDown(KeyCode.LeftShift) && sprinting == false)
        {
            sprinting = true;
            speed = sprintSpeed;
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
        //Prevent dying enemies from harming player
        if (collision.gameObject.CompareTag("Enemy") && collision.gameObject.GetComponent<Enemy>().CheckIfDead() == false) 
        {
            TakeDamage();
                       
        } else if (collision.gameObject.CompareTag("Treasure"))
        {
            UpdateGold();

            spawner.DecrementTreasureCount();
            
            spawner.IncremenetMaxEnemies();
            Destroy(collision.gameObject);
        }

        
    }

    void UpdateGold()
    {
        collectedGold += 1;

        DisplayGold();
        
        if (canPowerAttack == false)
        {
            powerAttackGoldCount += 1;
        }

        if (canPowerAttack == false && powerAttackGoldCount >= 3)
        {
            canPowerAttack = true;
            powerAttackGoldCount = 0;

        }
        

        Debug.Log(powerAttackGoldCount);
    }

    public void TakeDamage()
    {
        if (health > 0)
        {
            audioMana.Play("PlayerDamaged" + Random.Range(1, 11));
            health -= 1;

            DisplayHealth();

            // If player has no health, then end game
            if (NoHealth() && !gameOver)
            {
                GameOver();
            }
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

    public bool NoHealth()
    {
        return health <= 0;
    }

    IEnumerator Sprint ()
    {
        anim.SetBool("isSprinting", true);
        yield return new WaitForSeconds(2);

        speed = normalSpeed;

        anim.SetBool("isSprinting", false);
        sprinting = false;

    }

    IEnumerator PowerAttack()
    {
        if (canPowerAttack)
        {
            canPowerAttack = false;
            powerAttackGoldCount = 0;

            audioMana.Play("PlayerPowerAttackStart");
            yield return new WaitForSeconds(1.5f);
            powerAttack.Play();
            audioMana.Play("PlayerPowerAttackSoundEffects");

        } else
        {
            yield return new WaitForSeconds(0);
        }
    }

    IEnumerator RunePlacement()
    {
        if (canPlaceRune)
        {

            canPlaceRune = false;

            //Play a random version of the word for the spell
            int soundClip = Random.Range(1, 5);
            audioMana.Play("PlayerRuneSpellWord" + soundClip);
            
            yield return new WaitForSeconds(0.75f);
            
            audioMana.Play("PlayerRuneSpellSoundEffect");

            yield return new WaitForSeconds(0.75f);


            //Spawn rune
            Instantiate(sigil, transform.position, transform.rotation);
            canPlaceRune = true;

        } else
        {
            yield return new WaitForSeconds(0);
        }

    }

    void GameOver()
    {
        canMove = false;
        Debug.Log("Game Over");
        gameOver = true;

        anim.SetBool("isGameOver", true);
        audioMana.Play("PlayerDeath");
    }

    public bool isGameOver()
    {
        return gameOver;
    }
}
