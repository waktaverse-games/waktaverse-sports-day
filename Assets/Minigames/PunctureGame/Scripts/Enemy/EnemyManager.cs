using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameHaven.PunctureGame
{
    public class EnemyManager : MonoBehaviour
    {
        [SerializeField] private EnemyFactory factory;
        
        [SerializeField, DisableInPlayMode] private Transform[] enemySpawnPoints;
        
        [SerializeField, DisableInPlayMode] private Queue<Enemy> enemyPool;
        [SerializeField, DisableInPlayMode] private List<Enemy> enemies;

        [SerializeField] private float horzSpawnRange = 8.0f;

        private void Awake()
        {
            enemyPool = new Queue<Enemy>();
            for (var i = 0; i < enemySpawnPoints.Length; i++)
            {
                var enemy = factory.CreateRandom();
                enemyPool.Enqueue(enemy);
            }
        }

        private void Start()
        {
            
        }
        
        // TODO: 적들 Pool에 되돌리기 및 리스폰
    }
}