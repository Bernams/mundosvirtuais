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
        startRotation = pivot.transform.localRotation;
        Debug.Log(pivot.transform.localRotation);
        targetRotation = startRotation;
        currentRotation = startRotation;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isOpen)
        {
            isOpen = true;
            targetRotation = Quaternion.Euler(pivot.transform.localRotation.x - 90f, pivot.transform.localRotation.y, pivot.transform.localRotation.z);
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
            pivot.transform.localRotation = Quaternion.RotateTowards(pivot.transform.localRotation, targetRotation, rotationSpeed * Time.deltaTime);

            if (pivot.transform.localRotation == targetRotation)
            {
                currentRotation = targetRotation;
            }
        }
        else
        {
            pivot.transform.localRotation = Quaternion.RotateTowards(pivot.transform.localRotation, startRotation, rotationSpeed * Time.deltaTime);

            Debug.Log(pivot.transform.localRotation);


            if (pivot.transform.localRotation == startRotation)
            {
                currentRotation = startRotation;
            }
        }
    }
}