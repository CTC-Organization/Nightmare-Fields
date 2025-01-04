using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class GameManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public DayManager dayManager;
    public static GameManager instance;
    public  Volume ppv; // this is the post processing volume
    public TextMeshProUGUI timeDisplay; // Display Time
    public TextMeshProUGUI dayDisplay;

    void Start()
    {
        if (instance == null) instance = this;
        dayManager = Instantiate(dayManager);
        dayManager.Initialize();

    }

    void FixedUpdate()
    {
        dayManager.CalcTime();
        dayManager.DisplayTime();

    }
}
