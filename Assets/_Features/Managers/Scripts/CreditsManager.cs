using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsManager : MonoBehaviour
{
    public float scrollSpeed = 80f;
    public float duration = 80f; // Tempo até trocar a cena
    public string nextScene = "MainMenu"; // Nome da cena para onde irá após os créditos

    private RectTransform rectTransform;
    private float elapsedTime = 0f;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        // Faz os créditos rolarem para cima
        rectTransform.anchoredPosition += new Vector2(0, scrollSpeed * Time.deltaTime);

        // Conta o tempo
        elapsedTime += Time.deltaTime;

        // Troca a cena após o tempo especificado
        if (elapsedTime >= duration)
        {
            SceneManager.LoadScene(nextScene);
        }
    }
}
