using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    public void GoToMainMenu()
    {
        SceneManager.LoadScene(0);        
    }

    public void GoToGame()
    {
        //Load the main game scene
        SceneManager.LoadScene(1);
    }

    public void GoToEndScreen()
    {
        SceneManager.LoadScene(2);
    }

    public void GoToHowTo()
    {
        SceneManager.LoadScene(3);
    }
}
