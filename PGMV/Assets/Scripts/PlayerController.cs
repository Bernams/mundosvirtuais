using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;

    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        UnityEngine.Vector3 moveDirection = new UnityEngine.Vector3(horizontalInput, 0f, verticalInput).normalized;
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }
}
