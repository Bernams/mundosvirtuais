using UnityEngine;

public class BoxSpawner : MonoBehaviour
{
    public Vector3 boxPosition;
    public GameObject boxPrefab;
    public BoxMessage boxMessage;

    private void Start()
    {
        GameObject box = Instantiate(boxPrefab, boxPosition, Quaternion.identity) as GameObject;
        if (box.TryGetComponent<BoxController>(out var boxController))
        {
            boxController.boxMessage = boxMessage;
        }
    }
}