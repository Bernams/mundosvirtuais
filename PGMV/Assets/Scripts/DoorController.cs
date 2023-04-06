using UnityEngine;

public class DoorController : MonoBehaviour
{
    public GameObject pivot;

    private float rotationSpeed = 50f;
    private bool isOpen = false;
    private Quaternion startRotation;
    private Quaternion targetRotation;
    private Quaternion currentRotation;

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
            targetRotation = Quaternion.Euler(-90f, 0f, 0f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && isOpen)
        {
            isOpen = false;
            targetRotation = startRotation;
        }
    }

    private void Update()
    {
        if (isOpen)
        {
            pivot.transform.rotation = Quaternion.RotateTowards(pivot.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            if (pivot.transform.rotation == targetRotation)
            {
                currentRotation = targetRotation;
            }
        }
        else
        {
            pivot.transform.rotation = Quaternion.RotateTowards(pivot.transform.rotation, startRotation, rotationSpeed * Time.deltaTime);

            if (pivot.transform.rotation == startRotation)
            {
                currentRotation = startRotation;
            }
        }
    }
}
