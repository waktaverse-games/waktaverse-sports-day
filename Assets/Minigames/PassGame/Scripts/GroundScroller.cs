using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.PassGame
{ 
    public class GroundScroller : MonoBehaviour
    {
        public GameObject TilePrefab = null;
        public float GroundSpeed = -1.0f;

        private List<SpriteRenderer> _tiles = new List<SpriteRenderer>();
        private SpriteRenderer _lastTile;
        private const float DeadLine = -11.5f;
        private const float TileSize = 1.0f;

        void Start()
        {
            if (TilePrefab)
            {
                InitTiles();
            }
        }

        void Update()
        {
            if (TilePrefab)
            {
                UpdateTiles();
            }
        }

        void InitTiles()
        {
            for (int i = 0; i < 23; i++)
            {
                SpriteRenderer addTile = Instantiate(TilePrefab.GetComponent<SpriteRenderer>(),
                    new Vector3(-11f + i * DeadLine, -0.5f, 1f), Quaternion.identity);
                    
                addTile.transform.SetParent(this.transform);
                _tiles.Add(addTile);
            }

            _lastTile = _tiles[_tiles.Count - 1];
        }
        
        // Ÿ�� �̵�
        void UpdateTiles()
        {
            for (int i = 0; i < _tiles.Count; i++)
            {
                // �� ������ ���� �ڷ� �̵��ҰԿ�
                if (DeadLine >= _tiles[i].transform.position.x)
                {
                    _tiles[i].transform.position = new Vector3(_lastTile.transform.position.x + TileSize, -0.5f, 1f);
                    _lastTile = _tiles[i];
                }
            }

            for (int i = 0; i < _tiles.Count; i++)
            {
                _tiles[i].transform.Translate(new Vector2(GroundSpeed, 0) * Time.deltaTime);
            }
        }
    }
}
