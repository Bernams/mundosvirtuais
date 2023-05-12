using UnityEngine;

public class MinimapCameraFollow : MonoBehaviour
{
    public GameObject player;
    public float height = 54f;

    private Transform playerTransform;

    void Start()
    {
        playerTransform = player.transform;
        transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y + height, playerTransform.position.z);
        transform.rotation = Quaternion.Euler(90f, playerTransform.rotation.eulerAngles.y, 0f);
    }

    void Update()
    {
        transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y + height, playerTransform.position.z);
    }
}