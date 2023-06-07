using System;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class GameTimer : MonoBehaviour
{
    public float timeLimit = 60f;
    public bool isTimerRunning;
    public BoxMessage boxMessage;
    public HintsGenerator hintsGenerator;
    public AudioSource ambient;
    public AudioSource car;

    private float currentTime;
    private Text timerText;
    private int rank;
    private string filePath;

    private void Awake()
    {
        filePath = Path.Combine(Application.persistentDataPath, "rank.txt");
    }

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
                EndGame();
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

    private void EndGame()
    {
        int totalBoxCount = boxMessage.GetTotalBoxCount();
        int totalHintsCount = hintsGenerator.GetTotalHintsCount();

        Canvas canvas = FindObjectOfType<Canvas>();
        Image gameOver = canvas.transform.Find("GameOver").GetComponent<Image>();
        Image rankImage = gameOver.transform.Find("Rank").GetComponent<Image>();
        TextMeshProUGUI rankText = rankImage.GetComponentInChildren<TextMeshProUGUI>();
        rankText.text = "Rank: " + hintsGenerator.GetRank();
        gameOver.gameObject.SetActive(true);
        ambient.Stop();
        car.Stop();
        Time.timeScale = 0;
    }
}