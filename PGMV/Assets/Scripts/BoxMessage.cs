using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BoxMessage : MonoBehaviour
{
    private Text messageText;
    private bool hasBox = false;

    public void Start()
    {
        messageText = GetComponent<Text>();
    }

    public bool HasBox()
    {
        return hasBox;
    }

    public void CatchBox()
    {
        messageText.text = "Caixa apanhada";
        hasBox = true;
    }

    public void DepositBox()
    {
        messageText.text = "Depositou uma caixa";
        hasBox = false;
        StartCoroutine(ResetMessage());
    }

    private IEnumerator ResetMessage()
    {
        yield return new WaitForSeconds(10f);
        messageText.text = "";
    }
}