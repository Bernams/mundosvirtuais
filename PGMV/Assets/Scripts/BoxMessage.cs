using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BoxMessage : MonoBehaviour
{
    private Text messageText;
    private bool hasBox = false;
    private int totalBoxCount = 0;

    public void Start()
    {
        messageText = GetComponent<Text>();
        messageText.text = "Caixas depositadas: " + totalBoxCount;
    }

    public bool HasBox()
    {
        return hasBox;
    }

    public void CatchBox()
    {
        messageText.text = "Caixa apanhada\n\n" + messageText.text;
        hasBox = true;
    }

    public void DepositBox()
    {
        totalBoxCount += 1;
        messageText.text = "Caixa depositada!\n\nCaixas depositadas: " + totalBoxCount;
        hasBox = false;
        StartCoroutine(ResetMessage());
    }

    private IEnumerator ResetMessage()
    {
        yield return new WaitForSeconds(10f);
        messageText.text = "Caixas depositadas: " + totalBoxCount;
    }

    public int GetTotalBoxCount()
    {
        return totalBoxCount;
    }
}