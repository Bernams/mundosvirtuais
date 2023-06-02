using UnityEngine;
using UnityEngine.UIElements;

public class BoxSpawner : MonoBehaviour
{
    public GameObject boxPrefab;
    public BoxMessage boxMessage;

    public float minX;
    public float maxX;
    public float minZ;
    public float maxZ;

    public float dropHeight;
    public LayerMask obstacleLayerMask;

    private void Start()
    {
        SpawnBox();
    }

    private void SpawnBox()
    {
        Vector3 spawnPosition;
        bool foundValidPosition = false;
        int count = 0;

        while (!foundValidPosition)
        {
            spawnPosition = GetRandomPosition();

            RaycastHit hit;
            if (Physics.Raycast(spawnPosition + Vector3.up * dropHeight, Vector3.down, out hit, Mathf.Infinity, obstacleLayerMask))
            {
                Debug.Log(count);
                foundValidPosition = true;
                GameObject box = Instantiate(boxPrefab, spawnPosition + Vector3.up * dropHeight, Quaternion.identity);

                if (box.TryGetComponent<BoxController>(out var boxController))
                {
                    boxController.boxMessage = boxMessage;
                }
            }
            count++;
        }
    }

    private Vector3 GetRandomPosition()
    {
        float randomX = Random.Range(minX, maxX);
        float randomZ = Random.Range(minZ, maxZ);
        return new Vector3(randomX, 0f, randomZ);
    }
}