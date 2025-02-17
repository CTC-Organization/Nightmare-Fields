using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IntroductionDialogue : MonoBehaviour
{
    public GameObject dialoguePanel;
    public Text dialogueText;
    public string[] dialogue;
    private int index;

    public GameObject contButton;
    public float wordSpeed;
    public string nextSceneName;

    void Start()
    {
        dialoguePanel.SetActive(true);
        contButton.SetActive(false);
        StartCoroutine(Typing());
    }

    void Update()
    {
        if (dialogueText.text == dialogue[index])
        {
            contButton.SetActive(true);
        }
    }

    public void zeroText()
    {
        dialogueText.text = "";
        index = 0;
        dialoguePanel.SetActive(false);
        contButton.SetActive(false);
    }

    IEnumerator Typing()
    {
        dialogueText.text = "";
        contButton.SetActive(false);
        foreach (char letter in dialogue[index].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(wordSpeed);
        }
        contButton.SetActive(true);
    }

    public void NextLine()
    {
        contButton.SetActive(false);
        if (index < dialogue.Length - 1)
        {
            index++;
            dialogueText.text = "";
            StartCoroutine(Typing());
        }
        else
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
