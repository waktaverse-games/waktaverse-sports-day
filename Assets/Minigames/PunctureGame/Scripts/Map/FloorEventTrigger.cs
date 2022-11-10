using Sirenix.OdinInspector;
using UnityEngine;

namespace GameHeaven.PunctureGame
{
    public class FloorEventTrigger : MonoBehaviour
    {
        [SerializeField] private Player player;

        [SerializeField] [ReadOnly] private int currentFloor;

        public delegate void FloorChangeEvent(int floor);
        public event FloorChangeEvent onFloorChanged;

        public int CurrentFloor
        {
            get => currentFloor;
            private set
            {
                if (currentFloor < value)
                {
                    currentFloor = value;
                    onFloorChanged?.Invoke(value);
                }
            }
        }

        private void Update()
        {
            CurrentFloor = Mathf.RoundToInt(-player.currentPos.y) - 1;
        }
    }
}