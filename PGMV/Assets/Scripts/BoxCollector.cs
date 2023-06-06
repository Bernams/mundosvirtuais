using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class BoxCollector : MonoBehaviour
{
    public BoxMessage boxMessage;
    public BoxSpawner boxSpawner;
    public HintsGenerator hintsGenerator;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (boxMessage.HasBox())
            {
                boxMessage.DepositBox();
                boxSpawner.SpawnBox();
            }
        }
    }
}