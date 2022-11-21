using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace GameHeaven.SpreadGame
{
    public class ScoreUpdate : MonoBehaviour
    {
        private TextMeshProUGUI tmp;
        public int score;

        private void Awake()
        {
            tmp = GetComponent<TextMeshProUGUI>();
        }

        private void Update()
        {
            tmp.text = score.ToString();
        }
    }
}
