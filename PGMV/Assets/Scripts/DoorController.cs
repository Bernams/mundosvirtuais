using System.Numerics;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public GameObject pivot;
    public float rotationSpeed = 50f;

    private bool isOpen = false;
    private UnityEngine.Quaternion startRotation;
    private UnityEngine.Quaternion targetRotation;
    private UnityEngine.Quaternion currentRotation;

    private void Start()
    {
        startRotation = pivot.transform.rotation;
        targetRotation = startRotation;
        currentRotation = startRotation;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isOpen)
        {
            isOpen = true;
            targetRotation = UnityEngine.Quaternion.Euler(-90f, 0f, 0f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && isOpen)
        {
            if (currentRotation == targetRotation)
            {
                isOpen = false;
                targetRotation = startRotation;
            }
        }
    }

    private void Update()
    {
        if (isOpen)
        {
            pivot.transform.rotation = UnityEngine.Quaternion.RotateTowards(pivot.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            if (pivot.transform.rotation == targetRotation)
            {
                currentRotation = targetRotation;
            }
        }
    }
}
