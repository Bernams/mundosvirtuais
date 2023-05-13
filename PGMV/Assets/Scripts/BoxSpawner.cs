using UnityEngine;

public class BoxSpawner : MonoBehaviour
{
    public Vector3 boxPosition;
    public GameObject boxPrefab;

    private void Start()
    {
        GameObject box = Instantiate(boxPrefab, boxPosition, Quaternion.identity) as GameObject;
    }
}
