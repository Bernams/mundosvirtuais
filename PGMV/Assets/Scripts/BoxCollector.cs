using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class BoxCollector : MonoBehaviour
{
    public BoxMessage boxMessage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (boxMessage.HasBox())
            {
                boxMessage.DepositBox();
            }
        }
    }
}