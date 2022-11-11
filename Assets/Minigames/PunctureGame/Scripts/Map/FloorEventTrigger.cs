using System;
using SharedLibs;
using SharedLibs.Score;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameHeaven.PunctureGame
{
    public class FloorEventTrigger : MonoBehaviour
    {
        [SerializeField] private Player player;

        [SerializeField] [ReadOnly] private int currentFloor;
        [SerializeField] private int floorScore = 5;

        public delegate void FloorChangeEvent(int floor);
        public event FloorChangeEvent onFloorChanged;

        public int CurrentFloor => currentFloor;

        private void OnEnable()
        {
            onFloorChanged += AddFloorScore;
        }

        private void OnDisable()
        {
            onFloorChanged -= AddFloorScore;
        }

        private void Update()
        {
            var floor = Mathf.RoundToInt(-player.currentPos.y) - 1;
            
            if (currentFloor < floor)
            {
                currentFloor = floor;
                onFloorChanged?.Invoke(floor);
            }
        }

        private void AddFloorScore(int floor)
        {
            ScoreManager.Instance.AddGameRoundScore(MinigameType.PunctureGame, floorScore);
        }
    }
}