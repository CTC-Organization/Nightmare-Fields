using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro; // using text mesh for the clock display

using UnityEngine.Rendering;
using System.Linq; // used to access the volume component


[CreateAssetMenu(fileName = "DayManager", menuName = "ScriptableObjects/DayManager", order = 1)]
public class DayManager : ScriptableObject
{
    private GameManager gm;
    private TextMeshProUGUI hourDisplay; // Display Time
    private TextMeshProUGUI dayDisplay; // Display Day
    private Volume ppv; // this is the post processing volume
    private GameObject[] lights;

    public float tick; // Increasing the tick, increases second rate
    public float seconds;
    public int mins;
    public int hours;
    public int days = 1;

    public bool activateLights; // checks if lights are on

    public void Initialize()
    {
        ppv = GameManager.instance.ppv;
        hourDisplay = GameManager.instance.hourDisplay; // Display Time
        dayDisplay = GameManager.instance.dayDisplay;

        lights = GameObject.FindGameObjectsWithTag("Light");

        //Debug.Log("Lights: "+ lights.Length);
    }

    public void CalcTime() // Used to calculate sec, min and hours
    {
        seconds += Time.fixedDeltaTime * tick; // multiply time between fixed update by tick

        if (seconds >= 60) // 60 sec = 1 min
        {
            seconds = 0;
            mins += 1;
        }

        if (mins >= 60) //60 min = 1 hr
        {
            mins = 0;
            hours += 1;
        }

        if (hours >= 24) //24 hr = 1 day
        {
            hours = 0;
            days += 1;
        }
        ControlPPV(); // changes post processing volume after calculation
    }

    public void ControlPPV() // used to adjust the post processing slider.
    {
        //ppv.weight = 0;
        if (hours >= 18 && hours < 19) // dusk at 21:00 / 9pm    -   until 22:00 / 10pm
        {
            ppv.weight = (float)mins / 60; // since dusk is 1 hr, we just divide the mins by 60 which will slowly increase from 0 - 1 
            //for (int i = 0; i < stars.Length; i++)
            //{
            //    stars[i].color = new Color(stars[i].color.r, stars[i].color.g, stars[i].color.b, (float)mins / 60); // change the alpha value of the stars so they become visible
            //}

            if (activateLights == false) // if lights havent been turned on
            {
                if (mins > 45) // wait until pretty dark
                {
                    for (int i = 0; i < lights.Length; i++)
                    {
                        lights[i].SetActive(true); // turn them all on
                    }
                    activateLights = true;
                }
            }
        }


        if (hours >= 5 && hours < 6) // Dawn at 6:00 / 6am    -   until 7:00 / 7am
        {
            ppv.weight = 1 - (float)mins / 60; // we minus 1 because we want it to go from 1 - 0
            //for (int i = 0; i < stars.Length; i++)
            //{
            //    stars[i].color = new Color(stars[i].color.r, stars[i].color.g, stars[i].color.b, 1 - (float)mins / 60); // make stars invisible
            //}
            if (activateLights == true) // if lights are on
            {
                if (mins > 45) // wait until pretty bright
                {
                    for (int i = 0; i < lights.Length; i++)
                    {
                        lights[i].SetActive(false); // shut them off
                    }
                    activateLights = false;
                }
            }
        }
    }

    public void DisplayTime() // Shows time and day in ui
    {

        hourDisplay.text = string.Format("{0:00}:{1:00}", hours, mins); // The formatting ensures that there will always be 0's in empty spaces
        dayDisplay.text = "Day: " + days; // display day counter
    }
}
