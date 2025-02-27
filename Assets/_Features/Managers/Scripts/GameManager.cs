using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int enemyCount;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public DayManager dayManagerOriginal;
    public string arenaSceneName;

    public string farmSceneName;
    public string creditsSceneName;
    public static GameManager instance;
    public Volume ppv; // this is the post processing volume
    public TextMeshProUGUI hourDisplay; // Display Time
    public TextMeshProUGUI dayDisplay;

    public GameObject gameOverPanel;
    public GameObject pausePanel;
    public GameObject wonPanel;

    public Health playerHealth;

    public bool isPaused = false;
    public bool canComeBackToFarm = false;

    // [SerializeField] private InputActionReference pauseResumePressed;
    public TeleportManager tm = new();
    public bool fighting = false;
    public bool fightIsToStart = false;
    public Vector3 spawnPosition;
    public bool isOnFarm = true;
    public bool isNightmareMode = false;



    private Dictionary<Vector2, (FarmTile.PlantState, Vector3)> farmTileStates = new Dictionary<Vector2, (FarmTile.PlantState, Vector3)>();

    void Start()
    {
        if (instance == null)
        {
            Debug.Log("Iniciou GameManager");
            instance = this;
            Time.timeScale = 1;
            DontDestroyOnLoad(instance);
            DayManager.dm = Instantiate(dayManagerOriginal);
            DayManager.dm.Initialize();
            UpdateReferences();
        }
    }
    public void UpdateReferences()
    {

        // enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Count();
        // spawnPosition = GameObject.FindWithTag("SpawnPosition").GetComponent<Vector3>();
        playerHealth = GameObject.FindWithTag("Player").GetComponent<Health>();
        ppv = GameObject.FindWithTag("PostProcessingVolume").GetComponent<Volume>();
        hourDisplay = GameObject.FindWithTag("HourDisplay").GetComponent<TextMeshProUGUI>();
        dayDisplay = GameObject.FindWithTag("DayDisplay").GetComponent<TextMeshProUGUI>();
        gameOverPanel = GameObject.FindWithTag("GameOverPanel");
        pausePanel = GameObject.FindWithTag("PausePanel");
        wonPanel = GameObject.FindWithTag("WonPanel");
        enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Count();

        pausePanel.SetActive(false);
        wonPanel.SetActive(false);
        gameOverPanel.SetActive(false);

        DayManager.dm.ppv = ppv;
        DayManager.dm.hourDisplay = hourDisplay;
        DayManager.dm.dayDisplay = dayDisplay;
    }
    void Update()
    {
        // if (playerHealth.currentHealth <= 0) GameOver();
        enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Count();

        if (canComeBackToFarm && DayManager.dm.days == 5)
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(creditsSceneName);
            Time.timeScale = 1;
        }
        else if (canComeBackToFarm)
        {
            canComeBackToFarm = false;
            InventoryManager.Instance.SaveInventory();
            isNightmareMode = false;
            tm.Teleport(farmSceneName);
        }
        else if (playerHealth == null)
        {
            return;
        }
        else if (playerHealth.currentHealth <= 0) GameOver();
    }
    void FixedUpdate()
    {
        if (fighting == true)
        {
            return;
        }
        DayManager.dm.DayNightSystemUpdate();
    }

    public void SkipToFightTime()
    {
        DayManager.dm.SkipToFightTime();
    }

    public void GameOver()
    {
        Time.timeScale = 0; // pausa jogo
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
    }

    public void CheckCollisionCount(int count)
    {
        if (count >= 10)
        {
            Debug.Log("Colisões atingiram 10! Pausando o jogo.");
            Time.timeScale = 0;
            if (wonPanel != null)
                wonPanel.SetActive(true);
            //isPaused = true;
        }
    }

    public void EnemyCount()
    {
        enemyCount--;
        if (enemyCount <= 0)
        {
            if (wonPanel != null)
                wonPanel.SetActive(true);
            Debug.Log("ENemy count: " + enemyCount);
            StartCoroutine(WaitToWin());
        }
    }
    IEnumerator WaitToWin()
    {
        yield return new WaitForSeconds(3f);

        if (wonPanel != null)
            wonPanel.SetActive(false);
        canComeBackToFarm = true;
    }

    public void SaveFarmTileState(Vector2 position, FarmTile.PlantState state, Vector3 plantPosition)
    {
        farmTileStates[position] = (state, plantPosition);
        Debug.Log($"FarmTile salvo: {position} - {state}");
    }


    public (FarmTile.PlantState, Vector3) GetFarmTileState(Vector2 position)
    {
        Vector3 plantPosition = farmTileStates[position].Item2;
        plantPosition += Vector3.up;
        return (FarmTile.PlantState.Grown, plantPosition);
    }


    // private void OnEnable()
    // {
    //     pauseResumePressed.action.started += PauseResume;
    // }
    // private void OnDisable()
    // {
    //     pauseResumePressed.action.started -= PauseResume;
    // }

    /// <summary>
    /// Função responsável pelo attack
    /// </summary>
    // void PauseResume(InputAction.CallbackContext ctx)
    // {
    //     Debug.Log("apertou");
    //     if (isPaused) // resume
    //     {
    //         Debug.Log("Resume");
    //         if (pausePanel != null)
    //             pausePanel.SetActive(false);
    //         Time.timeScale = 1;
    //         isPaused = false;
    //     }
    //     else if (!isPaused) // pause
    //     {
    //         Debug.Log("Pausou");
    //         if (pausePanel != null)
    //             pausePanel.SetActive(true);
    //         Time.timeScale = 0;
    //         isPaused = true;
    //     }
    // }
}
