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
        private GameObject gameOverUI;
        [SerializeField]
        private GameObject perfectBonusUI;
        [SerializeField]
        private TextMeshProUGUI countdownUI;
        [SerializeField]
        public AnimationCounter counterPrefab;

        private Coroutine itemEffectCoroutine;
        private Vector3 itemEffectPos = new Vector3(0, 100f, 0);

        private Coroutine countdownFadeCoroutine;


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
            //gameOverUI.SetActive(true);
        }

        public void GameRestart()
        {
            gameOverUI.SetActive(false);
        }

        public IEnumerator CountDownRoutine()
        {
            countdownUI.gameObject.SetActive(true);
            SetCountdownText("3");
            yield return new WaitForSeconds(.5f);
            SetCountdownText("2");
            yield return new WaitForSeconds(.5f);
            SetCountdownText("1");
            yield return new WaitForSeconds(.5f);
            SetCountdownText("Start!");
            GameManager.Instance.GameStart();
            yield return new WaitForSeconds(.5f);
            countdownUI.gameObject.SetActive(false);
        }

        private void SetCountdownText(string text)
        {
            countdownUI.text = text;
            if (countdownFadeCoroutine != null) StopCoroutine(countdownFadeCoroutine);
            countdownFadeCoroutine = StartCoroutine(FadeText(countdownUI, false));
        }

        public IEnumerator PerfectBonus()
        {
            perfectBonusUI.SetActive(true);
            yield return new WaitForSeconds(1.5f);
            perfectBonusUI.SetActive(false);
        }

        public void RestartGame()
        {
            GameManager.Instance.GameReStart();
            gameOverUI.SetActive(false);
        }

        public void ShowItemEffect(string effect)
        {
            if (itemEffectCoroutine != null) StopCoroutine(itemEffectCoroutine);
            itemEffectText.transform.position = Camera.main.WorldToScreenPoint(GameManager.Instance.platform.transform.position) + itemEffectPos;
            itemEffectText.text = effect;
            itemEffectText.gameObject.SetActive(true);
            itemEffectCoroutine = StartCoroutine(FadeText(itemEffectText, false));
        }

        private IEnumerator FadeText(TextMeshProUGUI textUI, bool floatText, float interval = .05f)
        {
            Color textColor = textUI.color;
            textColor.a = 1f;
            itemEffectText.color = textColor;
            while (textColor.a > 0.05f)
            {
                textColor.a *= 0.8f;
                textUI.color = textColor;
                if (floatText) textUI.transform.Translate(0, 5f, 0);
                yield return new WaitForSeconds(interval);
            }
            textUI.gameObject.SetActive(false);
        }
    }
}

