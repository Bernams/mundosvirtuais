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
        if (File.Exists(filePath))
        {
            using StreamReader reader = new StreamReader(filePath);
            string rankInFile = reader.ReadLine();
            rank = int.Parse(rankInFile);
        }
        else
        {
            rank = 3;
        }
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

        if (totalBoxCount == 0)
        {
            rank = 0;
            Debug.Log("Rank: 0. Você é horroroso.");
        } else
        {
            float boxesPerHint = totalBoxCount / totalHintsCount;
            switch (boxesPerHint)
            {
                case <= 0.2f:
                    rank = 1;
                    Debug.Log("Rank: " + rank);
                    break;
                case (<= 0.4f and > 0.2f):
                    rank = 2;
                    Debug.Log("Rank: " + rank);
                    break;
                case (<= 0.6f and > 0.4f):
                    rank = 3;
                    Debug.Log("Rank: " + rank);
                    break;
                case (<= 0.8f and > 0.6f):
                    rank = 4;
                    Debug.Log("Rank: " + rank);
                    break;
                case > 0.8f:
                    rank = 5;
                    Debug.Log("Rank: " + rank);
                    break;
                default:
                    break;
            }
        }

        Canvas canvas = FindObjectOfType<Canvas>();
        Image gameOver = canvas.transform.Find("GameOver").GetComponent<Image>();
        Image rankImage = gameOver.transform.Find("Rank").GetComponent<Image>();
        TextMeshProUGUI rankText = rankImage.GetComponentInChildren<TextMeshProUGUI>();
        rankText.text = "Rank: " + rank;
        gameOver.gameObject.SetActive(true);

        using StreamWriter writer = new StreamWriter(filePath, false);
        writer.WriteLine(rank);
    }
}