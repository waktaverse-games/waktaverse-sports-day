using System;
using Redcode.Pools;
using UnityEngine;

namespace GameHeaven.PunctureGame
{
    public class Item : MonoBehaviour, IEntityLogic, IPoolObject
    {
        [SerializeField] private string type;
        
        [SerializeField] private ParticleSystem particle;

        [SerializeField] private Animator animator;
        // private static readonly int Fade = Animator.StringToHash("Fade");
        private static readonly int ItemGet = Animator.StringToHash("ItemGet");

        [SerializeField] private bool isGettable;
        
        public string Type => type;

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (isGettable)
            {
                GetItem();
            }
        }

        public event Action<string> OnGetting; 
        public event Action<Item> OnRelease;

        private void GetItem()
        {
            OnGetting?.Invoke(Type);
            
            particle.Play();
            animator.SetTrigger(ItemGet);
        }

        public void ReleaseItem()
        {
            OnRelease?.Invoke(this);
        }

        public void Ready()
        {
            isGettable = false;
            animator.speed = 0.0f;
        }

        public void Active()
        {
            isGettable = true;
            animator.speed = 1.0f;
        }

        public void Inactive()
        {
            isGettable = false;
            animator.speed = 0.0f;
        }

        public void OnCreatedInPool()
        {
            Inactive();
        }

        public void OnGettingFromPool()
        {
            Active();
        }
    }
}