using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GameHeaven.CrashGame
{
    public class GameOverZone : MonoBehaviour
    {
        //protected void OnCollisionEnter2D(Collision2D collision)
        //{
        //    if (collision.collider.CompareTag("Brick"))
        //    {
        //        Debug.Log("Brick!");
        //        GameManager.Instance.GameOver();
        //    }
        //}

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.CompareTag("Brick"))
            {
                Debug.Log("Brick!");
                MiniGameManager.Instance.GameOver();
            }
        }
    }
}
