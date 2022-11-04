using Sirenix.OdinInspector;
using UnityEngine;

namespace GameHeaven.PunctureGame {
    public class PlayerFloorChecker : MonoBehaviour {
        public delegate void FloorChangeEvent(int floor);

        [SerializeField] private PlayerController player;

        [SerializeField] [ReadOnly] private int currentFloor;

        public int CurrentFloor {
            get => currentFloor;
            set {
                if (currentFloor < value) {
                    currentFloor = value;
                    onFloorChanged?.Invoke(value);
                }
            }
        }

        private void Update() {
            CurrentFloor = Mathf.RoundToInt(-player.currentPos.y) - 1;
        }

        public event FloorChangeEvent onFloorChanged;
    }
}