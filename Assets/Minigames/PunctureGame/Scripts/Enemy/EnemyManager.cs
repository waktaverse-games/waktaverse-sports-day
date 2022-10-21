using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameHeaven.PunctureGame
{
    public class EnemyManager : MonoBehaviour
    {
        [SerializeField] private EnemyFactory factory;
        
        private Queue<Enemy> enemyPool;
        [SerializeField] private int poolSize;
        
        [SerializeField, DisableInPlayMode] private List<Enemy> enemies;

        [SerializeField] private PlayerFloorChecker floorChecker;

        private void Awake()
        {
            enemyPool = new Queue<Enemy>();
            for (var i = 0; i < poolSize; i++)
            {
                var enemy = factory.CreateRandom();
                enemyPool.Enqueue(enemy);
            }
            
            floorChecker.onFloorChanged += OnPlayerFloorChanged;
        }

        private void OnPlayerFloorChanged(int floor)
        {
            Debug.Log("Player entered on " + floor + " floor");
        }

        private void Start()
        {
            
        }
        
        // TODO: 적들 Pool에 되돌리기 및 리스폰
    }
}