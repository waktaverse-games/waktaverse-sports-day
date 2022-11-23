using System;
using TMPro;
using UnityEngine;

namespace GameHeaven.PunctureGame.UI
{
    public class DebugUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI debugScoreUI;
        [SerializeField] private TextMeshProUGUI debugAddScoreUI;
        [SerializeField] private TextMeshProUGUI debugPlayerUI;

        [SerializeField] private PlayerController controller;
        [SerializeField] private ScoreCollector scoreCollector;

        private void Awake()
        {
            controller = FindObjectOfType<PlayerController>();
        }

        private void Start()
        {
            scoreCollector.OnAddScore += (score, total) =>
            {
                debugScoreUI.text = "Score: " + (total);
                debugAddScoreUI.text = "Added! (+" + score + ")";
            };
        }

        private void Update()
        {
            debugPlayerUI.text = controller.MoveVector.ToString();
        }
    }
}