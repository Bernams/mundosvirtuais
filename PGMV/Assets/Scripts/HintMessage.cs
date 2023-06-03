using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HintMessage : MonoBehaviour
{
    private Text messageText;

    void Start()
    {
        messageText = GetComponent<Text>();
    }

    public void setText(string text)
    {
        messageText.text = text;
    }
}