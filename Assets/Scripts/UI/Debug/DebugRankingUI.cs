using System;
using SharedLibs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameHeaven.Temp
{
    public class DebugRankingUI : MonoBehaviour
    {
        [SerializeField] private TMP_InputField gameTitleInputFieldPost;
        [SerializeField] private TMP_InputField gameTypeInputFieldPost;
        [SerializeField] private TMP_InputField userNameInputField;
        [SerializeField] private TMP_InputField scoreInputField;
        
        [SerializeField] private TMP_InputField gameTitleInputFieldGet;
        [SerializeField] private TMP_InputField gameTypeInputFieldGet;
        [SerializeField] private TMP_InputField limitInputField;
        [SerializeField] private TextMeshProUGUI resultText;

        public void Submit()
        {
            var gameTitle = gameTitleInputFieldPost.text;
            var gameType = gameTypeInputFieldPost.text;
            var userName = userNameInputField.text;
            var score = Convert.ToInt32(scoreInputField.text);

            ScoreHttpRequest.PostScore(gameTitle, gameType, userName, score);
        }
        
        public async void Get()
        {
            var gameTitle = gameTitleInputFieldGet.text;
            var gameType = gameTypeInputFieldGet.text;
            var limit = Convert.ToInt32(limitInputField.text);

            var res = await ScoreHttpRequest.GetScores(gameTitle, gameType, limit);
            resultText.text = res;
        }
    }
}