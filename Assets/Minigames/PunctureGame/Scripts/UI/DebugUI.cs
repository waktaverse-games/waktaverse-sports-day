using System;
using SharedLibs.Score;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameHeaven.PunctureGame.UI
{
    public class DebugUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI debugScoreUI;
        [SerializeField] private TextMeshProUGUI debugAddScoreUI;

        private void Awake()
        {
            ScoreManager.Instance.OnAddScore += (add, orig) =>
            {
                debugScoreUI.text = "Score: " + (add + orig);
                debugAddScoreUI.text = "Added! (+" + add + ")";
            };
        }
    }
}