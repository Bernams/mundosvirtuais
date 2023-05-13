using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxController : MonoBehaviour
{
    public BoxMessage boxMessage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            boxMessage.CatchBox();
            Destroy(gameObject);
        }
    }
}