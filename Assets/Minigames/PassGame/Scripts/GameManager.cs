using System.Collections;
using System.Collections.Generic;
using SharedLibs;
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
        public TestBack testBack;
        public float jumpPower;
        public AudioSource bgm;
        public GameObject mandu;

        private int _score = 0;
        private int _stage = 0;
        private int _coins = 0;
        private Vector3 _spawnPos;
        private Vector3 _coinSpawnPos;
        private TextMeshProUGUI _scoreText;
        private TextMeshProUGUI _stageText;
        private List<string>[] _stageStrings = new List<string>[6];
        private bool _isBat;

        // Start is called before the first frame update
        void Start()
        {
            _spawnPos = spawnPoint.transform.position;
            _coinSpawnPos = coinSpawnPoint.transform.position;
            _coins = 0;
            bgm.volume = SharedLibs.SoundManager.Instance.BGMVolume;
            playerScript.jumpPower = jumpPower;
            _scoreText = scoreT.GetComponent<TextMeshProUGUI>();
            _stageText = stageT.GetComponent<TextMeshProUGUI>();
            _stageStrings = new List<string>[6];
            _stageStrings[0] = new List<string>();
            _stageStrings[1] = new List<string>() { "egi", "bat" };
            // _stageStrings[1] = new List<string>() { "jupok" };
            _stageStrings[2] = new List<string>() { "egi", "bat", "bidul", "dog", "gorani" };
            _stageStrings[3] = new List<string>() { "egi", "bat", "bidul", "dog", "gorani", "bug" };
            _stageStrings[4] = new List<string>() { "egi", "bat", "bidul", "dog", "gorani", "bug", "jupok"  };
            _stageStrings[5] = new List<string>() {  "bat", "bidul", "gorani", "bug", "jupok", "dog" };
            // _stageStrings[6] = new List<string>() { "bat", "bidul", "gorani", "bug", "jupok" };
            GameSet();
        }

        // Update is called once per frame

        public void GameSet()
        {
            endText.SetActive(false);
            toMain.SetActive(false);
            button.SetActive(false);
            // startText.SetActive(true);
            playerScript.reachedJump = false;
            _score = 0;
            _stage = 1;
            Time.timeScale = 1;
            _stageText.SetText("Lv 1");
            _scoreText.SetText(_score.ToString());
            // coinText.SetText(_coins.ToString());
            playerScript.ResetGame();
            Invoke("GameStart", 3.5f);
        }

        public void GameStart()
        {
            startText.SetActive(false);
            StartCoroutine(PlayBgm());
            StartCoroutine(UpgradeStage(40));
            StartCoroutine(StageSpawn(0.1f));
            StartCoroutine(CoinSpawn(0.7f));
            StartCoroutine(ItemSpawn(30.1f));
        }

        IEnumerator PlayBgm()
        {
            yield return new WaitForSeconds(0.8f);
            bgm.Play();
        }

        public void GameOver()
        {
            Time.timeScale = 0;
            StopAllCoroutines();
            StartCoroutine(GameOverEnum());
        }

        IEnumerator GameOverEnum()
        {
            objectManager.FailGame();
            mandu.SetActive(false);
            player.SetActive(false);
            testBack.shouldMove = false;
            Time.timeScale = 1;
            yield return new WaitForSecondsRealtime(0.1f);
            SfxManager.PlaySfx(3);
            bgm.Stop();
            yield return new WaitForSecondsRealtime(0.1f);
            // Debug.Log("Game Over");
            ScoreManager.Instance.SetGameHighScore(MinigameType.PassGame, _score);
            // Debug.Log(_score);
            GameResultManager.ShowResult(MinigameType.PassGame, _score);
        }

        public void AddScore(int addScore)
        {
            // Debug.Log("Add Score: " + addScore);
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
            if (_stage < 5 && _isBat)
            {
                if (_stageStrings[_stage][rnd] == "bidul") rnd += Random.Range(1, 3);
                else if (_stageStrings[_stage][rnd] == "jupok") rnd -= Random.Range(1, 4);
                // Debug.Log(rnd);
            }
            if (_stageStrings[_stage][rnd] == "bat") _isBat = true;
            else _isBat = false;
            objectManager.MakeObject(_stageStrings[_stage][rnd], _spawnPos);
            StartCoroutine(StageSpawn(2.25f));
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
            if (playerScript.jumpItem < 3) 
            {
                playerScript.jumpItem += 1;
                playerScript.ResetJumpText();
                SfxManager.PlaySfx(2);
                doubleJumpState.SetActive(true);
            }
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