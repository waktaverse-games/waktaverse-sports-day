using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace SharedLibs.Score {
    [RequireComponent(typeof(Collider2D))]
    public class ScoreItem : MonoBehaviour {
        [SerializeField] private int score;
        [SerializeField] private MinigameType type;

        [SerializeField] private SpriteRenderer spRender;

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
        /// arg: score int variable
        /// </summary>
        public UnityEvent<int> OnUseItemUnity;

        private void Awake() {
            var col = GetComponent<Collider2D>();
            col.isTrigger = true;
        }

        private void OnTriggerEnter2D(Collider2D col) {
            UseItem();
        }

        public void Init() {
            IsActivated = false;
            if (spRender) {
                spRender.enabled = true;
            }
            else {
                gameObject.SetActive(true);
            }
        }

        /// <summary>
        /// Use item with events, and disables object or renderer
        /// </summary>
        /// <param name="check">If check is false, ignoring check item's active state</param>
        public void UseItem() {
            if (!IsActivated) return;
            ScoreManager.Instance.AddGameRoundScore(type, score);
            OnUseItem?.Invoke(score);
            OnUseItemUnity?.Invoke(score);
            
            IsActivated = reusable;
            if (spRender) {
                spRender.enabled = reusable;
            }
            else {
                gameObject.SetActive(reusable);
            }
        }
    }
}