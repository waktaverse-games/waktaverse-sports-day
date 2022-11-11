using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.JumpGame
{
    public class ItemManager : MonoBehaviour
    {
        [SerializeField] int score;

        ItemSpawner spawner;
        Animator animator;
        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        public void InitializeItem(ItemSpawner _spawner, int itemNum)
        {
            spawner = _spawner;
            animator.SetInteger("itemNum", itemNum);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                SoundManager.Instance.PlayItemSound();
                GameManager.Instance.IncreaseScore(score);
                spawner.DeactiavteItem(gameObject);
            }
            if(collision.gameObject.tag == "Border")
            {
                spawner.DeactiavteItem(gameObject);
            }
        }
    }
}
