using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class MapGenerator : MonoBehaviour
{

    public Transform tilePrefab;
    public Vector2 mapSize;

    [Range(0,1)]
    public float outlinePercent;

    private void Awake()
    {
        Assert.IsNotNull(tilePrefab);
        Assert.AreNotEqual(0f, mapSize.x, "MapSize.x couldn't be 0");
        Assert.AreNotEqual(0f, mapSize.y, "MapSize.y couldn't be 0");
    }
    private void Start()
    {
        GenerateMap();
    }

    public void GenerateMap()
    {
        string holderName = "GeneratedMap";
        if (transform.FindChild(holderName))
        {
            // We will be calling this in the Editor, so we need DestroyImmediate
            DestroyImmediate(transform.FindChild(holderName).gameObject);
        }

        Transform mapHolder = new GameObject(holderName).transform;
        mapHolder.parent = transform;

        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                Vector3 tilePosition = new Vector3(-mapSize.x/2 + 0.5f + x, 0f, -mapSize.y/2 + 0.5f + y);
                
                //Instantiate our tile rotate 90 degrees, in order to be flat on the ground
                Transform newTile = Instantiate(tilePrefab, tilePosition, Quaternion.Euler(Vector3.right * 90)) as Transform;
                newTile.localScale = Vector3.one * (1 - outlinePercent);
                newTile.parent = mapHolder;
            }
        }
    }
}
