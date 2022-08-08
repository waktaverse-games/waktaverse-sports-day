using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundScroller : MonoBehaviour
{
    public GameObject tilePrefab = null;
    private List<SpriteRenderer> tiles = new List<SpriteRenderer>();
    public float groundSpeed = -1.0f;

    private SpriteRenderer lastTile;
    private const float deadLine = -11.5f;
    private const float tileSize = 1.0f;
    void Start()
    {
        if (tilePrefab)
        {
            for (int i = 0; i < 23; i++)
            {
                SpriteRenderer addTile = Instantiate(tilePrefab.GetComponent<SpriteRenderer>(), 
                    new Vector2(-11f + i * tileSize, -0.5f), Quaternion.identity);
                tiles.Add(addTile);
            }
        
            lastTile = tiles[tiles.Count - 1];
        }
        
    }

    void Update()
    {
        if (tilePrefab is null)
        {
            return;
        }
        
        for (int i = 0; i < tiles.Count; i++)
        {
            if (deadLine >= tiles[i].transform.position.x)
            {
                tiles[i].transform.position = new Vector2(lastTile.transform.position.x + tileSize, -0.5f);
                lastTile = tiles[i];
            }
        }
        
        for (int i = 0; i < tiles.Count; i++)
        {
            tiles[i].transform.Translate(new Vector2(groundSpeed, 0) * Time.deltaTime);
        }
    }
}
