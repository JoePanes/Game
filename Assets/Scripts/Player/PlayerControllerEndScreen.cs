using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControllerEndScreen : MonoBehaviour
{
    Rigidbody playerRb;
    float speed = 0.4f;

    bool canMove = true;
    float forwardInput;

    private void Start()
    {
        playerRb = GetComponent<Rigidbody>();    
    }
    
    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            forwardInput = Input.GetAxis("Vertical");

            transform.Translate(transform.forward * speed * forwardInput * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        canMove = false;
        StartCoroutine(GoToGameScreen());
    }

    IEnumerator GoToGameScreen()
    {
        playerRb.AddTorque(Vector3.right * -8 * Time.deltaTime, ForceMode.Impulse);
        playerRb.AddTorque(Vector3.up * -8 * Time.deltaTime, ForceMode.Impulse);
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene(1);
    }
}
