using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace SharedLibs.Score {
    [RequireComponent(typeof(Collider2D))]
    public class ScoreItem : MonoBehaviour {
        [SerializeField] private int score;
        [SerializeField] private MinigameType type;

        [SerializeField, ReadOnly] private bool activated = true;
        public bool IsActivated {
            get => activated;
            private set => activated = value;
        }
        [SerializeField] private bool reusable = false;

        /// <summary>
        /// arg: score int variable
        /// </summary>
        public event Action<int> OnUseItem;
        /// <summary>
        /// arg: activate state
        /// </summary>
        public UnityEvent<bool> OnUseItemUnity;

        private void Awake() {
            var col = GetComponent<Collider2D>();
            col.isTrigger = true;
        }

        private void OnTriggerEnter2D(Collider2D col) {
            UseItem();
        }

        public void Init() {
            IsActivated = false;
            OnUseItemUnity?.Invoke(true);
        }

        /// <summary>
        /// Use item with events, and disables object or renderer
        /// </summary>
        public void UseItem() {
            if (!IsActivated) return;
            ScoreManager.Instance.AddGameRoundScore(type, score);
            OnUseItem?.Invoke(score);
            
            IsActivated = reusable;
            OnUseItemUnity?.Invoke(reusable);
        }
    }
}