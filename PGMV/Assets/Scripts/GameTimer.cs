using System;
using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    public float timeLimit = 60f;
    public bool isTimerRunning;

    private float currentTime;
    private Text timerText;

    void Start()
    {
        timerText = GetComponent<Text>();
        currentTime = timeLimit;
        isTimerRunning = true;
    }

    void FixedUpdate()
    {
        if (isTimerRunning)
        {
            currentTime -= Time.deltaTime;
            UpdateTimerText();

            if (currentTime <= 0f)
            {
                isTimerRunning = false;
            }
        }
    }

    void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(Mathf.Max(0f, currentTime) / 60f);
        int seconds = Mathf.FloorToInt(Mathf.Max(0f, currentTime) % 60f);
        string timeString = string.Format("{0:00}:{1:00}", minutes, seconds);
        timerText.text = timeString;

        if (currentTime <= 5f && currentTime > 0f)
        {
            timerText.color = Color.yellow;
        }
        else if (currentTime <= 0f)
        {
            timerText.color = Color.red;
        }
        else
        {
            timerText.color = Color.white;
        }
    }

}