using System;
using UnityEngine;

namespace GameHeaven.PunctureGame
{
    public class Block : MonoBehaviour
    {
        private SpriteRenderer _renderer;

        [SerializeField] private Material[] mats;

        private void Awake()
        {
            _renderer = GetComponentInChildren<SpriteRenderer>();
        }

        private void OnEnable()
        {
            _renderer.material = mats[UnityEngine.Random.Range(0, mats.Length)];
        }

        public void Crash()
        {
            Debug.Log("Crashed");
            gameObject.SetActive(false);
        }
    }
}