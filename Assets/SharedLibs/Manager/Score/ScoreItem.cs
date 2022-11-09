using System;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace SharedLibs.Score
{
    [RequireComponent(typeof(Collider2D))]
    public class ScoreItem : MonoBehaviour
    {
        [SerializeField] private int score;
        [SerializeField] private MinigameType type;

        [Header("Tags")]
        [Tooltip("점수 획득 처리 할 태그들")]
        [SerializeField] private string[] scoreEarnTags;
        [Tooltip("아이템 비활성화 처리 할 태그들")]
        [SerializeField] private string[] disableTags;
        [Space]

        [SerializeField, ReadOnly] private bool activated = true;
        public bool IsActivated
        {
            get => activated;
            private set => activated = value;
        }
        [SerializeField] private bool reusable = false;
        [SerializeField] private bool isUsingPool = false;

        /// <summary>
        /// arg: score int variable
        /// </summary>
        public event Action<int> OnUseItem;
        /// <summary>
        /// arg: activate state
        /// </summary>
        public UnityEvent<bool> OnUseItemUnity;

        private void Awake()
        {
            var col = GetComponent<Collider2D>();
            col.isTrigger = true;
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (scoreEarnTags.Contains(col.gameObject.tag)) { UseItem(); }
            if (disableTags.Contains(col.gameObject.tag)) { DisableItem(); }
        }

        public void Init()
        {
            IsActivated = false;
            OnUseItemUnity?.Invoke(true);
        }

        /// <summary>
        /// Use item with events, and disables object or renderer
        /// </summary>
        public void UseItem()
        {
            if (isUsingPool) { IsActivated = true; }
            if (!IsActivated) return;
            ScoreManager.Instance.AddGameRoundScore(type, score);
            OnUseItem?.Invoke(score);
            DisableItem();
        }

        private void DisableItem()
        {
            IsActivated = reusable;
            OnUseItemUnity?.Invoke(reusable);
        }
    }
}