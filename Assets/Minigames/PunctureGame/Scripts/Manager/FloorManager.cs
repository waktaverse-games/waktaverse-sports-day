using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameHeaven.PunctureGame
{
    public class FloorManager : MonoBehaviour
    {
        [SerializeField] private PlayerController controller;
        [SerializeField] private ScoreCollector scoreCollector;

        [SerializeField] [ReadOnly] private int currentFloor;

        public int CurrentFloor => currentFloor;

        private void Awake()
        {
            controller = FindObjectOfType<PlayerController>();
        }

        private void Update()
        {
            var floor = Mathf.RoundToInt(-controller.Position.y);

            if (currentFloor < floor)
            {
                currentFloor = floor;
                OnFloorChanged?.Invoke(floor);
            }
        }

        private void OnEnable()
        {
            OnFloorChanged += AddFloorScore;
        }

        private void OnDisable()
        {
            OnFloorChanged -= AddFloorScore;
        }

        public event Action<int> OnFloorChanged;

        private void AddFloorScore(int floor)
        {
            scoreCollector.AddFloorScore();
        }
    }
}