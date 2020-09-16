using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIElements : MonoBehaviour
{
    public void restartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void quitGame()
    {
        Application.Quit();
    }

    public void backtoMainMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void startGame()
    {
        SceneManager.LoadScene(1);
    }
}
