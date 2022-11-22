using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using SharedLibs.Score;

namespace GameHeaven.PassGame
{
    public class GameManager : MonoBehaviour
    {
        public GameObject player;
        public Player playerScript;
        public ObjectManager objectManager;
        public GameObject spawnPoint;
        public GameObject coinSpawnPoint;
        public GameObject scoreT;
        public GameObject stageT;
        public GameObject startText;
        public GameObject endText;
        public GameObject button;
        public GameObject toMain;
        public GameObject doubleJumpState;
        public GameObject doubleJumpItem;
        public TextMeshProUGUI coinText;
        public SFXManager SfxManager;
        public float jumpPower;

        private int _score = 0;
        private int _stage = 0;
        private int _coins = 0;
        private Vector3 _spawnPos;
        private Vector3 _coinSpawnPos;
        private TextMeshProUGUI _scoreText;
        private TextMeshProUGUI _stageText;
        private List<string>[] _stageStrings = new List<string>[4];

        // Start is called before the first frame update
        void Start()
        {
            _spawnPos = spawnPoint.transform.position;
            _coinSpawnPos = coinSpawnPoint.transform.position;
            _coins = 0;
            playerScript.jumpPower = jumpPower;
            _scoreText = scoreT.GetComponent<TextMeshProUGUI>();
            _stageText = stageT.GetComponent<TextMeshProUGUI>();
            _stageStrings = new List<string>[6];
            _stageStrings[0] = new List<string>();
            // _stageStrings[1] = new List<string>() { "egi", "bat" };
            _stageStrings[1] = new List<string>() { "dog" };
            _stageStrings[2] = new List<string>() { "egi", "bat", "bidul", "dog" };
            _stageStrings[3] = new List<string>() { "egi", "bat", "bidul", "dog", "gorani" };
            _stageStrings[4] = new List<string>() { "egi", "bat", "bidul", "dog", "gorani", "bug" };
            _stageStrings[5] = new List<string>() { "egi", "bat", "bidul", "dog", "gorani", "bug", "jupok" };
            GameSet();
        }

        // Update is called once per frame

        public void GameSet()
        {
            endText.SetActive(false);
            toMain.SetActive(false);
            button.SetActive(false);
            startText.SetActive(true);
            playerScript.reachedJump = false;
            _score = 0;
            _stage = 1;
            Time.timeScale = 1;
            _stageText.SetText("Lv 1");
            _scoreText.SetText(_score.ToString());
            // coinText.SetText(_coins.ToString());
            playerScript.ResetGame();
            Invoke("GameStart", 2f);
        }

        public void GameStart()
        {
            startText.SetActive(false);
            StartCoroutine(UpgradeStage(45));
            StartCoroutine(StageSpawn(0.1f));
            StartCoroutine(CoinSpawn(0.7f));
            StartCoroutine(ItemSpawn(30.1f));
        }

        public void GameOver()
        {
            Time.timeScale = 0;
            StopAllCoroutines();
            objectManager.FailGame();
            player.SetActive(false);
            Debug.Log("Game Over");
            // endText.SetActive(true);
            // button.SetActive(true);
            // toMain.SetActive(true);
            // ScoreManager.Instance.AddGameRoundScore(MinigameType.PassGame, Score);
            // 여기에 씬이동 넣으시면 될 것 같습니다!! 게임 오버, 게임오버, 게임 종료, game over, gameover
        }

        public void AddScore(int addScore)
        {
            _score += addScore;
            _scoreText.text = _score.ToString();
        }

        IEnumerator UpgradeStage(float time)
        {
            yield return new WaitForSeconds(time);
            _stage++;
            _stageText.text = "Lv " + _stage;
            switch (_stage)
            {
                case 2:
                    StartCoroutine(UpgradeStage(40));
                    break;
                case 3:
                    StartCoroutine(UpgradeStage(40));
                    break;
                case 4:
                    StartCoroutine(UpgradeStage(40));
                    break;
            }
        }

        IEnumerator StageSpawn(float time)
        {
            yield return new WaitForSeconds(time);
            int rnd = Random.Range(0, _stageStrings[_stage].Count);
            objectManager.MakeObject(_stageStrings[_stage][rnd], _spawnPos);
            StartCoroutine(StageSpawn(3f));
        }

        IEnumerator CoinSpawn(float time)
        {
            yield return new WaitForSeconds(time);
            objectManager.MakeObject("coin", _coinSpawnPos);
            StartCoroutine(CoinSpawn(6.1f));
        }

        IEnumerator ItemSpawn(float time)
        {
            yield return new WaitForSeconds(time);
            doubleJumpItem.SetActive(true);
            doubleJumpItem.transform.position = _coinSpawnPos;
            StartCoroutine(ItemSpawn(time));
        }

        public void ItemActivate()
        {
            playerScript.jumpItem = true;
            doubleJumpState.SetActive(true);
        }

        public void ItemDeactivate()
        {
            doubleJumpState.SetActive(false);
        }

        public void AddCoin()
        {
            // _coins++;
            SfxManager.PlaySfx(2);
            AddScore(10);
            // coinText.SetText(_coins.ToString());
        }

        public void EndGame()
        {
            Debug.Log("return coin and move to main");
        }
    }
}