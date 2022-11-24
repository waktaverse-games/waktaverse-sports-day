using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameHeaven.PunctureGame
{
    public class FloorManager : MonoBehaviour
    {
        [SerializeField] private PlayerController controller;

        [SerializeField] [ReadOnly] private int currentFloor;

        public int CurrentFloor => currentFloor;

        public event Action<int> OnFloorChanged;
        
        private void Update()
        {
            var floor = Mathf.RoundToInt(-controller.Position.y);

            if (currentFloor < floor)
            {
                currentFloor = floor;
                OnFloorChanged?.Invoke(floor);
            }
        }
    }
}