using System.Numerics;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public GameObject pivot;

    private bool isOpen = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isOpen)
        {
            isOpen = true;
            pivot.transform.rotation = UnityEngine.Quaternion.Euler(-90f, 0f, 0f);
        }
    }
}
