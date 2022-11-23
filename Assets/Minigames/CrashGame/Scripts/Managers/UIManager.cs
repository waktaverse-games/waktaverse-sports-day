using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace GameHeaven.CrashGame
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI gameScore;
        [System.Obsolete, SerializeField]
        private Text highScore;
        [SerializeField]
        private TextMeshProUGUI itemEffectText;
        //[SerializeField]
        //private Text coin;

        [SerializeField]
        private GameObject GameOverUI;
        [SerializeField]
        private GameObject PerfectBonusUI;

        private Coroutine itemEffectCoroutine;
        private Vector3 itemEffectPos = new Vector3(0, 100f, 0);


        public void SetScoreText(int score)
        {
            gameScore.text = $"점수: {score}";
        }

        public void SetHighScoreText(int score)
        {
            highScore.text = $"최고기록: {score}";
        }

        //public void SetCoinText(int coin)
        //{
        //    this.coin.text = $"코인: {coin}";
        //}

        public void GameOver()
        {
            GameOverUI.SetActive(true);
        }

        public void GameRestart()
        {
            GameOverUI.SetActive(false);
        }

        public IEnumerator PerfectBonus()
        {
            PerfectBonusUI.SetActive(true);
            yield return new WaitForSeconds(1.5f);
            PerfectBonusUI.SetActive(false);
        }

        public void RestartGame()
        {
            GameManager.Instance.GameStart();
            GameOverUI.SetActive(false);
        }

        public void ShowItemEffect(string effect)
        {
            if (itemEffectCoroutine != null) StopCoroutine(itemEffectCoroutine);
            Color textColor = itemEffectText.color;
            textColor.a = 1f;
            itemEffectText.color = textColor;
            itemEffectText.transform.position = Camera.main.WorldToScreenPoint(GameManager.Instance.platform.transform.position) + itemEffectPos;
            itemEffectText.text = effect;
            itemEffectText.gameObject.SetActive(true);
            itemEffectCoroutine = StartCoroutine(FadeText());
        }

        private IEnumerator FadeText()
        {
            Color textColor = itemEffectText.color;
            while (textColor.a > 0.05f)
            {
                textColor.a *= 0.8f;
                itemEffectText.color = textColor;
                itemEffectText.transform.Translate(0, 5f, 0);
                yield return new WaitForSeconds(.05f);
            }
            itemEffectText.gameObject.SetActive(false);
        }
    }
}

