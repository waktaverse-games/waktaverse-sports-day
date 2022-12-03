using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.BingleGame
{
    public class PlayerColliderManager : MonoBehaviour
    {
        bool isBlueCombo = false;
        bool isFirst = true;
        int flagScore = 30;
        int comboStack = 0;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "GameOverArea" || collision.gameObject.tag == "Border" || collision.gameObject.tag == "Brick")
            {
                GameManager.instance.GameOver();
            }
            else if (collision.gameObject.tag == "Pole")
            {
                if (collision.gameObject.transform.name == "Tree Left")
                {
                    if(isFirst || !isBlueCombo)
                    {
                        isFirst = false;
                        isBlueCombo = true;
                        comboStack = 0;
                    }
                    else
                    {
                        comboStack++;
                    }
                }
                else
                {
                    if(isFirst || isBlueCombo)
                    {
                        isFirst = false;
                        isBlueCombo = false;
                        comboStack = 0;
                    }
                    else
                    {
                        comboStack++;
                    }
                }
                GameManager.instance.IncreaseScore(flagScore + comboStack);
                GameManager.instance.ShowTextEffect("ÄÞº¸ +" + comboStack, isBlueCombo);
            }
        }
    }
}