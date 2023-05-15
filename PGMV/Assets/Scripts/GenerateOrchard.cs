using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateOrchard : MonoBehaviour
{
    public float depth = 5f;
    public int width = 50;
    public int height = 50;
    public float treeScaleMin = 0.5f;
    public float treeScaleMax = 1.5f;
    public float treeOffsetY = 0.1f;
    public int numberOfRowsInGrid = 10;
    public int numberOfColumnsInGrid = 10;

    public GameObject tree;

    public float scale = 20f;

    private Terrain orchard;
    // Start is called before the first frame update
    void Start()
    {
        orchard = GetComponent<Terrain>();
        orchard.terrainData = OrchardGenerator(orchard.terrainData);
        GenerateTrees();
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    private void GenerateTrees()
    {
        float treePlacementIntervalX = width / (numberOfColumnsInGrid - 1);
        float treePlacementIntervalY = height / (numberOfRowsInGrid - 1);

        for (int row = 0; row < numberOfRowsInGrid; row++)
        {
            for (int col = 0; col < numberOfColumnsInGrid; col++)
            {
                float randomRotation = Random.Range(0f, 360f);
                float randomScale = Random.Range(treeScaleMin, treeScaleMax);
                Vector3 randomPosition = new Vector3(col * treePlacementIntervalX + orchard.transform.position.x, orchard.transform.position.y, row * treePlacementIntervalY + orchard.transform.position.z);

                Debug.Log(randomPosition);

                // Adjust tree position to prevent clipping with the terrain
                float sampleHeight = orchard.SampleHeight(randomPosition);
                randomPosition.y = sampleHeight + treeOffsetY;
                Vector3 raycastOrigin = new Vector3(randomPosition.x, sampleHeight + 1f, randomPosition.z);
                RaycastHit hit;
                if (Physics.Raycast(raycastOrigin, Vector3.down, out hit, 10f))
                {
                    float distanceToGround = hit.distance;
                    randomPosition.y += distanceToGround;
                }

                Debug.Log(randomPosition);

                GameObject instantiatedTree = Instantiate(tree, randomPosition, Quaternion.Euler(0f, randomRotation, 0f));
                instantiatedTree.transform.localScale = new Vector3(randomScale, randomScale, randomScale);
                instantiatedTree.transform.parent = transform;
            }
        }
    }

    TerrainData OrchardGenerator(TerrainData terrainData)
    {
        terrainData.heightmapResolution = width + 1;

        terrainData.size = new Vector3(width, depth, height);

        terrainData.SetHeights(0, 0, PerlinNoiseHeights());

        return terrainData;
    }

    float[,] PerlinNoiseHeights()
    {
        float[,] heights = new float[width, height];
        for (int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                heights[x, y] = GeneratePerlinNoise(x, y);
            }
        }
        return heights;
    }

    float GeneratePerlinNoise(int x, int y)
    {
        float xx = (float) x / width * scale;
        float yy = (float) y / height * scale;

        return Mathf.PerlinNoise(xx, yy) + 0.4f * Mathf.PerlinNoise(10 * xx, 10 * yy);
    }
}
