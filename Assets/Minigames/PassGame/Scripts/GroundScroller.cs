using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.PassGame
{ 
    public class GroundScroller : MonoBehaviour
    {
        public float GroundSpeed = -1.0f;

        public List<SpriteRenderer> Tiles = new List<SpriteRenderer>();
        
        private SpriteRenderer _lastTile;
        private const float DeadLine = -11.5f;
        private const float TileSize = 3.0f;

        void Start()
        {
            FindLastTile();
        }

        void FindLastTile()
        {
            float x = Tiles[0].transform.position.x;
            for (int i = 1; i < Tiles.Count; i++)
            {
                if (Tiles[i].transform.position.x > x)
                {
                    _lastTile = Tiles[i];
                }
            }
        }
        void Update()
        {
            UpdateTiles();
        }
        
        // Ÿ�� �̵�
        void UpdateTiles()
        {
            for (int i = 0; i < Tiles.Count; i++)
            {
                // �� ������ ���� �ڷ� �̵��ҰԿ�
                if (DeadLine >= Tiles[i].transform.position.x)
                {
                    Tiles[i].transform.position = new Vector3(_lastTile.transform.position.x + TileSize, -0.5f, 1f);
                    _lastTile = Tiles[i];
                }
            }

            for (int i = 0; i < Tiles.Count; i++)
            {
                Tiles[i].transform.Translate(new Vector2(GroundSpeed, 0) * Time.deltaTime);
            }
        }
    }
}
