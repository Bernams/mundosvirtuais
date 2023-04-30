using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateOrchard : MonoBehaviour
{
    public int depth = 5;
    public int width = 50;
    public int height = 50;

    public float scale = 20f;
    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        Terrain orchard = GetComponent<Terrain>();
        orchard.terrainData = OrchardGenerator(orchard.terrainData);
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

        return Mathf.PerlinNoise(xx, yy);
    }
}
