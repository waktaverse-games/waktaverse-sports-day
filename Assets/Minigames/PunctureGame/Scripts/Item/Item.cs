using System;
using SharedLibs;
using SharedLibs.Score;
using UnityEngine;

namespace GameHeaven.PunctureGame
{
    public class Item : MonoBehaviour
    {
        [SerializeField] private int score;
        [SerializeField] private MinigameType type;
        
        public Action<Item> OnRelease;

        private void OnTriggerEnter2D(Collider2D col)
        {
            ScoreManager.Instance.AddGameRoundScore(type, score);
            OnRelease.Invoke(this);
        }
    }
}