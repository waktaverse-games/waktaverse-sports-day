using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameHeaven.PunctureGame
{
    public class PlayerFloorChecker : MonoBehaviour
    {
        [SerializeField] private PlayerController player;

        [SerializeField, ReadOnly] private int currentFloor = 0;
        public int CurrentFloor
        {
            get => currentFloor;
            set
            {
                if (currentFloor < value)
                {
                    currentFloor = value;
                    onFloorChanged?.Invoke(value);
                }
            }
        }

        public delegate void FloorChangeEvent(int floor);
        public event FloorChangeEvent onFloorChanged; 

        private void Update()
        {
            CurrentFloor = Mathf.RoundToInt(-player.currentPos.y) - 1;
        }
    }
}