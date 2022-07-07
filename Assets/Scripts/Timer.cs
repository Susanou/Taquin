using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    [SerializeField] private FloatValue timeRemaining;
    [SerializeField] private Text timeText;
    private bool timerIsRunning = false;
    

    private void Start()
    {
        if(SceneManager.GetActiveScene().name == "Home")
        {
            if (timeRemaining.RuntimeValue < 0)
                DisplayTime(-1);
        }
        else
        {
            // Starts the timer automatically
            timerIsRunning = true;
        }
    }
    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining.RuntimeValue > 0)
            {
                timeRemaining.RuntimeValue -= Time.deltaTime;
                DisplayTime(timeRemaining.RuntimeValue);
            }
            else
            {
                Debug.Log("Time has run out!");
                timeRemaining.RuntimeValue = 0;
                timerIsRunning = false;
                GameBoard.Instance.EndGame();
            }
        }
    }
    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;
        float minutes = Mathf.FloorToInt(timeToDisplay / 60); 
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public float StopClock()
    {
        timerIsRunning = false;
        return timeRemaining.RuntimeValue;
    }

    public void StartClock()
    {
        timeRemaining.RuntimeValue = timeRemaining.initialValue;
        timerIsRunning = true;
    }
}