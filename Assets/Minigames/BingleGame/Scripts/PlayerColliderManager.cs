using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.BingleGame
{
    public class PlayerColliderManager : MonoBehaviour
    {
        bool isBlueCombo = false;
        bool isFirst = true;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "GameOverArea" || collision.gameObject.tag == "Border" || collision.gameObject.tag == "Brick")
            {
                GameManager.instance.GameOver();
            }
            else if(collision.gameObject.tag == "Coin")
            {
                collision.GetComponent<ItemManager>().OnCollision();
                GameManager.instance.IncreaseScore(ScoreType.item);
            }
            else if(collision.gameObject.tag == "PassArea")
            {
                GameManager.instance.ComboReset();
                GameManager.instance.IncreaseScore(ScoreType.pass);
                collision.GetComponent<PassAreaManager>().OnCollision();
            }
            else if (collision.gameObject.tag == "Pole")
            {
                GameManager.instance.IncreaseCombo();
                GameManager.instance.ShowTextEffect();
                GameManager.instance.IncreaseScore(ScoreType.flag);
            }
        }
    }
}