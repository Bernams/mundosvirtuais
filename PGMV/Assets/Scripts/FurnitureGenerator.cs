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
        garage = this.gameObject;
        Bounds garageBounds = garage.transform.Find("SpawnLimits").gameObject.GetComponent<BoxCollider>().bounds;

        for (int i = 0; i < furniturePositions.Count; i++)
        {
            GameObject positionObject = furniturePositions[i];
            GameObject prefab = furniturePrefabs[i];

            Bounds prefabBounds = GetPrefabBounds(prefab);

            while (true)
            {
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

                if (garageBounds.Contains(prefabBounds.min + randomPosition - prefab.transform.position) &&
                    garageBounds.Contains(prefabBounds.max + randomPosition - prefab.transform.position))
                {
                    break;
                }
                Destroy(instantiatedPrefab);
            }
        }
    }

    private Bounds GetPrefabBounds(GameObject prefab)
    {
        Bounds bounds = new Bounds(prefab.transform.position, Vector3.zero);

        Renderer[] renderers = prefab.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            bounds.Encapsulate(renderer.bounds);
        }

        return bounds;
    }

}
