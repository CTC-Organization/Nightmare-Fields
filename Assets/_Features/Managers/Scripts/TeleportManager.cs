
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportManager
{

    public void Teleport(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        SceneManager.sceneLoaded += OnSceneLoaded;

        if (sceneName == GameManager.instance.farmSceneName)
        {
            GameManager.instance.fightIsToStart = false;
            GameManager.instance.fighting = false;
        }
        else if (sceneName == GameManager.instance.arenaSceneName)
        {
            GameManager.instance.dayDisplay.text = "";
            GameManager.instance.hourDisplay.text = "";
            GameManager.instance.fightIsToStart = false;
            GameManager.instance.fighting = true;
        }



    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameManager.instance.UpdateReferences();
        if (scene.name == GameManager.instance.farmSceneName)
        {
            Debug.Log("Farm Scene Loaded (name):"+ scene.name);
            DayManager.dm.UpdateReferencesToFarm();
        }
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


}
