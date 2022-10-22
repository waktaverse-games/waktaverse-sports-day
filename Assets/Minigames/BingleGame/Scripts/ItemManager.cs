using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.BingleGame
{
    public class ItemManager : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.gameObject.tag == "Player")
            {
                SoundManager.instance.PlayItemSound();
                transform.gameObject.SetActive(false);
            }
        }
    }
}
