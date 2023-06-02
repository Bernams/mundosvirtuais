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
                Vector3 randomPosition = new Vector3(col * treePlacementIntervalX + orchard.transform.position.x, 0f, row * treePlacementIntervalY + orchard.transform.position.z);

                GameObject instantiatedTree = Instantiate(tree, randomPosition, Quaternion.Euler(0f, randomRotation, 0f));
                instantiatedTree.transform.localScale = new Vector3(randomScale, randomScale, randomScale);
                instantiatedTree.transform.parent = transform;

                float targetHeight = orchard.SampleHeight(randomPosition);

                instantiatedTree.transform.localPosition = new Vector3(instantiatedTree.transform.localPosition.x, targetHeight, instantiatedTree.transform.localPosition.z);
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
            for (int y = 0; y < height; y++)
            {
                float randomHeight = GeneratePerlinNoise(x, y);
                heights[x, y] = randomHeight;
                //if (x % (width / numberOfColumnsInGrid) == 0 && y % (height / numberOfRowsInGrid) == 0)
                //{
                  //  float randomRotation = Random.Range(0f, 360f);
                    //float randomScale = Random.Range(treeScaleMin, treeScaleMax);
                    //GameObject instantiatedTree = Instantiate(tree, new Vector3(orchard.transform.position.x + x, orchard.transform.position.y + randomHeight, orchard.transform.position.z + y), Quaternion.Euler(0f, randomRotation, 0f));
                    //instantiatedTree.transform.localScale = new Vector3(randomScale, randomScale, randomScale);
                    //instantiatedTree.transform.parent = transform;
                //}
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
