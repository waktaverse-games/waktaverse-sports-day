using System;
using TMPro;
using UnityEngine;

namespace GameHeaven.PunctureGame.UI
{
    public class ScoreUI : MonoBehaviour
    {
        [SerializeField] private ScoreCollector scoreCollector;
        
        [SerializeField] private TextMeshProUGUI scoreText;
        
        private void Start()
        {
            scoreCollector.OnAddScore += (score, total) =>
            {
                scoreText.text = total.ToString();
            };
        }
    }
}