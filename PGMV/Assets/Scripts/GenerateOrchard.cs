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
    public int numberOfRowsInGrid = 5;
    public int numberOfColumnsInGrid = 5;
    public int numberOfRocks = 30;
    public int numberOfBushes = 30;
    public LayerMask collisionLayer;

    public GameObject tree;
    public GameObject bush;
    public GameObject rock;

    public float scale = 20f;

    private Terrain orchard;

    void Start()
    {
        orchard = GetComponent<Terrain>();
        orchard.terrainData = OrchardGenerator(orchard.terrainData);
        GenerateTrees();
        GenerateRocksAndBushes();
    }

    private void GenerateRocksAndBushes()
    {
        for (int i = 0; i < numberOfRocks; i++)
        {
            bool isInstantiated = false;
            while (!isInstantiated)
            {
                float randomRotation = Random.Range(0f, 360f);
                Vector3 randomPosition = new Vector3(Random.Range(0f, width) + orchard.transform.position.x, 0f, Random.Range(0f, height) + orchard.transform.position.z);

                Collider[] colliders = Physics.OverlapBox(randomPosition, rock.transform.localScale / 2f, Quaternion.identity, collisionLayer);
                if (colliders.Length == 0)
                {
                    GameObject instantiatedRock = Instantiate(rock, randomPosition, Quaternion.Euler(0f, randomRotation, 0f));
                    instantiatedRock.transform.parent = transform;
                    float targetHeight = orchard.SampleHeight(randomPosition);

                    instantiatedRock.transform.localPosition = new Vector3(instantiatedRock.transform.localPosition.x, targetHeight, instantiatedRock.transform.localPosition.z);
                    isInstantiated = true;
                }
            }
        }
        for (int i = 0; i < numberOfBushes; i++)
        {
            bool isInstantiated = false;
            while (!isInstantiated)
            {
                float randomRotation = Random.Range(0f, 360f);
                Vector3 randomPosition = new Vector3(Random.Range(0f, width) + orchard.transform.position.x, 0f, Random.Range(0f, height) + orchard.transform.position.z);

                Collider[] colliders = Physics.OverlapBox(randomPosition, rock.transform.localScale / 2f, Quaternion.identity, collisionLayer);
                if (colliders.Length == 0)
                {
                    GameObject instantiatedBush = Instantiate(bush, randomPosition, Quaternion.Euler(0f, randomRotation, 0f));
                    instantiatedBush.transform.parent = transform;
                    float targetHeight = orchard.SampleHeight(randomPosition);

                    instantiatedBush.transform.localPosition = new Vector3(instantiatedBush.transform.localPosition.x, targetHeight, instantiatedBush.transform.localPosition.z);
                    isInstantiated = true;
                }
            }
        }
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
               
            }
        }
        return heights;
    }

    float GeneratePerlinNoise(int x, int y)
    {
        float xx = (float)x / width * scale;
        float yy = (float)y / height * scale;

        return Mathf.PerlinNoise(xx, yy) + 0.4f * Mathf.PerlinNoise(10 * xx, 10 * yy);
    }
}
