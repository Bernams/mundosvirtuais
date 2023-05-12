using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnitureGenerator : MonoBehaviour
{
    public List<GameObject> furniturePositions;
    public List<GameObject> furniturePrefabs;
    public float positionInterval = 1f;
    public float maxYRotation;

    private GameObject garage;

    void Start()
    {
        garage = GameObject.Find("Garage");

        for (int i = 0; i < furniturePositions.Count; i++)
        {
            GameObject positionObject = furniturePositions[i];
            GameObject prefab = furniturePrefabs[i];

            float randomX = Random.Range(-positionInterval, positionInterval);
            float randomZ = Random.Range(-positionInterval, positionInterval);
            float randomYRotation = Random.Range(-maxYRotation, maxYRotation);

            Vector3 randomPosition = positionObject.transform.position + new Vector3(randomX, 0, randomZ);
            Quaternion randomRotation = Quaternion.Euler(
                garage.transform.rotation.x,
                positionObject.transform.rotation.y + randomYRotation,
                garage.transform.rotation.z);

            GameObject instantiatedPrefab = Instantiate(prefab, randomPosition, randomRotation);
            instantiatedPrefab.transform.parent = positionObject.transform;
        }
    }
}
