using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    //Load Scene
    [SerializeField] string tutorialSceneName;
    [SerializeField] string creditsSceneName;

    public void StartGame()
    {
        SceneManager.LoadScene(tutorialSceneName);
    }

    public void ShowCredits()
    {
        SceneManager.LoadScene(creditsSceneName);

    }

    //Quit Game
    public void Quit()
    {
        Application.Quit();
        Debug.Log("Player Said I quit");
    }
}