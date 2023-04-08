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

    void Start()
    {
        float randomX = Random.Range(minX, maxX);
        float randomZ = Random.Range(minZ, maxZ);
        float randomYRotation = Random.Range(minYRotation, maxYRotation);
        Vector3 randomPosition = new Vector3(randomX, transform.position.y, randomZ);
        Quaternion randomRotation = Quaternion.Euler(0f, randomYRotation, 0f);
        transform.position = randomPosition;
        transform.rotation = randomRotation;
    }
}
