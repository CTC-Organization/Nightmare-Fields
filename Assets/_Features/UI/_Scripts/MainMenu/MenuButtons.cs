using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    //Load Scene
    [SerializeField] string tutorialSceneName = "TESTEIntroducao";
    public void Play()
    {
        try
        {
            Destroy(DayManager.dm);
            Destroy(GameManager.instance);
            Time.timeScale = 1f;

            SceneManager.LoadScene(tutorialSceneName);
        }
        catch (System.Exception ex)
        {
            Debug.LogError(ex.Message);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    //Quit Game
    public void Quit()
    {
        Application.Quit();
        Debug.Log("Player Said I quit");
    }
}