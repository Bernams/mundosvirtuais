using UnityEngine;

public class FurnitureGeneratorV2 : MonoBehaviour
{
    public GameObject[] furniturePrefabs;
    public float maxX;
    public float minX;
    public float maxZ;
    public float minZ;

    private GameObject garage;

    void Start()
    {
        garage = GameObject.Find("Garage");

        foreach (GameObject furniturePrefab in furniturePrefabs)
        {
            InstantiateFurniture(furniturePrefab);
        }
    }

    void InstantiateFurniture(GameObject prefab)
    {
        float randomX = Random.Range(minX, maxX);
        float randomZ = Random.Range(minZ, maxZ);

        Vector3 randomPosition = transform.position + new Vector3(randomX, 0, randomZ);
        Quaternion randomRotation = Quaternion.Euler(0, Random.Range(0, 360), 0);

        GameObject furnitureInstance = Instantiate(prefab, randomPosition, randomRotation);
        furnitureInstance.transform.SetParent(garage.transform);
    }
}
