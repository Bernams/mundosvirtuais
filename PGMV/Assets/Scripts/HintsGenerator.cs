using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintsGenerator : MonoBehaviour
{
    public float timer = 10f;
    public HintMessage hintMessage;

    private GameObject box;
    private bool wantsHint = true;
    private int hint = 1;

    void Start()
    {
        StartHintTimer();
    }

    private void StartHintTimer()
    {
        Invoke(nameof(AskForHint), timer);
    }

    private void AskForHint()
    {
        hintMessage.setText("Deseja receber uma dica?");

        if (wantsHint)
        {
            switch(hint)
            {
                case 1:
                    //dica 1
                    break;
                case 2:
                    //dica 2
                    break;
                case 3:
                    //dica 3
                    break;
                case 4:
                    //dica 4
                    break;
                case 5:
                    //dica 5
                    break;
                default:
                    break;
            }
        }
    }

    public void setBox(GameObject box)
    {
        this.box = box;
    }
}