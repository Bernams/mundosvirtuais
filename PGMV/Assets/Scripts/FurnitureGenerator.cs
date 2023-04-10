using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnitureGenerator : MonoBehaviour
{
    public float maxX;
    public float minX;
    public float maxZ;
    public float minZ;
    public float maxYRotation;
    public float minYRotation;

    private GameObject garage = GameObject.Find("Garage");

    void Start()
    {
        float randomX = Random.Range(minX, maxX);
        float randomZ = Random.Range(minZ, maxZ);
        float randomYRotation = Random.Range(minYRotation, maxYRotation);

        Vector3 randomPosition = transform.position + new Vector3(randomX, 0, randomZ);
        Quaternion randomRotation = Quaternion.Euler(garage.transform.rotation.x, randomYRotation, garage.transform.rotation.z);
        transform.SetPositionAndRotation(randomPosition, randomRotation);
    }
}