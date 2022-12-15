using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public void playAgain()
    {
        SceneManager.LoadScene("Level1");
    }

    public void mainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
