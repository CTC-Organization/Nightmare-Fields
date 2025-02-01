using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class GameManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public DayManager dayManager;
    public static GameManager instance;
    public Volume ppv; // this is the post processing volume
    public TextMeshProUGUI hourDisplay; // Display Time
    public TextMeshProUGUI dayDisplay;

    public GameObject gameOverPanel;
    public GameObject pausePanel;

    public Health playerHealth;

    public bool isPaused = false;

    void Start()
    {
        if (instance == null) instance = this;
        Time.timeScale = 1;

        playerHealth = GameObject.FindWithTag("Player").GetComponent<Health>();
        dayManager = Instantiate(dayManager);
        dayManager.Initialize();
    }
    void Update()
    {
        if (playerHealth.currentHealth <= 0) GameOver();
    }
    void FixedUpdate()
    {
        dayManager.CalcTime();
        dayManager.DisplayTime();

    }

    public void GameOver()
    {
        Time.timeScale = 0; // pausa jogo
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
    }

    public void Pause()
    {
        if (pausePanel != null)
            pausePanel.SetActive(true);
        Time.timeScale = 0;
        isPaused = true;
    }

    public void ResumeGame()
    {
        if (pausePanel != null)
            pausePanel.SetActive(false);
        Time.timeScale = 1;
        isPaused = false;
    }
}
