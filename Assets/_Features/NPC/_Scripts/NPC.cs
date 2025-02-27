using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    public GameObject dialoguePanel;
    public Text dialogueText;
    public string[] dialogueDay1;
    public string[] dialogueDay2;
    public string[] dialogueDay3;
    public string[] dialogueDay4;
    public string[] dialogueDay5;
    private string[] currentDialogue;
    private int index;

    public GameObject contButton;
    public float wordSpeed;
    public bool playerIsClose;

    void Start()
    {
        if (GameManager.instance != null)
        {
            UpdateDialogueBasedOnDay();
        }

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C) && playerIsClose)
        {
            if (dialoguePanel.activeInHierarchy)
            {
                zeroText();
            }
            else
            {
                UpdateDialogueBasedOnDay();
                dialoguePanel.SetActive(true);
                StartCoroutine(Typing());
            }
            if (dialogueText.text == currentDialogue[index])
            {
                contButton.SetActive(true);
            }
        }

    }

    public void UpdateDialogueBasedOnDay()
    {
        int currentDay = DayManager.dm.days;

        if (currentDay == 1)
        {
            currentDialogue = dialogueDay1;
        }
        else if (currentDay == 2)
        {
            currentDialogue = dialogueDay2;
        }
        else if (currentDay == 3)
        {
            currentDialogue = dialogueDay3;
        }
        else if (currentDay == 4)
        {
            currentDialogue = dialogueDay4;
        }
        else
        {
            currentDialogue = dialogueDay5;
        }
        index = 0;
    }

    public void zeroText()
    {
        dialogueText.text = "";
        index = 0;
        dialoguePanel.SetActive(false);
    }

    IEnumerator Typing()
    {
        foreach (char letter in currentDialogue[index].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(wordSpeed);
        }
    }

    public void NextLine()
    {
        contButton.SetActive(false);
        if (index < currentDialogue.Length - 1)
        {
            index++;
            dialogueText.text = "";
            StartCoroutine(Typing());
        }
        else
        {
            zeroText();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = false;
            zeroText();
        }
    }
}
