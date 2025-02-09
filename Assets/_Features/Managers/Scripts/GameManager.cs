using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
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

    [SerializeField] private InputActionReference pauseResumePressed;

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

    public void CheckCollisionCount(int count)
    {
        if (count >= 5)
        {
            Debug.Log("Colisões atingiram 5! Pausando o jogo.");
            Time.timeScale = 0;
            if (pausePanel != null)
                pausePanel.SetActive(true);
            isPaused = true;
        }
    }



    private void OnEnable()
    {
        pauseResumePressed.action.started += PauseResume;
    }
    private void OnDisable()
    {
        pauseResumePressed.action.started -= PauseResume;
    }

    /// <summary>
    /// Função responsável pelo attack
    /// </summary>
    void PauseResume(InputAction.CallbackContext ctx)
    {
        Debug.Log("apertou");
        if (isPaused) // resume
        {
            Debug.Log("Resume");
            if (pausePanel != null)
                pausePanel.SetActive(false);
            Time.timeScale = 1;
            isPaused = false;
        }
        else if (!isPaused) // pause
        {
            Debug.Log("Pausou");
            if (pausePanel != null)
                pausePanel.SetActive(true);
            Time.timeScale = 0;
            isPaused = true;
        }
    }
}
