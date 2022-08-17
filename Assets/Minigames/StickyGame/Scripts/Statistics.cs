using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Statistics : MonoBehaviour
{
    public int curRunner, cumulRunner, goldCoin, silverCoin, bronzeCoin, score;
    private Text text;

    private void Awake()
    {
        text = GetComponent<Text>();
    }

    private void Update()
    {
        text.text = "Score : " + score +
            "\nRunner(Current) : " + curRunner + 
            "\nRunner(Cumulative) : " + cumulRunner +
            "\nGold Coin : " + goldCoin +
            "\nSilver Coin : " + silverCoin +
            "\nBronze Coin : " + bronzeCoin;
    }
}
