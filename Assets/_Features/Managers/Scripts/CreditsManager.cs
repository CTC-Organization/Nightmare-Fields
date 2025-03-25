using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsManager : MonoBehaviour
{
    public float scrollSpeed = 80f;
    public float duration = 80f; // Tempo at� trocar a cena
    public string mainMenuSceneName;

    private RectTransform rectTransform;
    private float elapsedTime = 0f;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        // Faz os cr�ditos rolarem para cima
        rectTransform.anchoredPosition += new Vector2(0, scrollSpeed * Time.deltaTime);

        // Conta o tempo
        elapsedTime += Time.deltaTime;

        // Troca a cena ap�s o tempo especificado
        if (elapsedTime >= duration)
        {
            SceneManager.LoadScene(mainMenuSceneName);
        }
    }
}
