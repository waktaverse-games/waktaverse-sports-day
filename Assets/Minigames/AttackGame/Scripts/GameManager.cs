using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace GameHeaven.AttackGame
{
    public class GameManager : MonoBehaviour
    {
        public ObjectManager objectManager;
        public Player player;
        public GameObject mainCamera;
        public Camera mainCameraScript;
        public Image hpBar;
        public Image playerXpBar;
        public Image allXpBar;
        public GameObject stageStart;
        public GameObject startGameText;
        public GameObject retryObject;
        public Animator retryAnim;
        public GameObject playerObject;
        
        public TextMeshProUGUI scoreText;
        public TextMeshProUGUI playerLevelText;
        public TextMeshProUGUI allLevelText;
        public TextMeshProUGUI hpText;
        public TextMeshProUGUI playerXpText;
        public TextMeshProUGUI stageText;
        public TextMeshProUGUI stageStartText;
        public TextMeshProUGUI coinText;

        private int _scoreNum;
        private int _hpNum;
        private int _playerXpNum;
        private int _allXpNum;
        private int _stageNum;
        private int _playerLevelNum;
        private int _allLevelNum;
        private int _defaultHp;
        private int _defaultPlayerXpSum;
        private int _allXpSum;
        private int _enemyDamage;
        private int _enemyXp;
        private int _currentMonsterNum;
        private string[] _enemyTypes;
        private int[] _enemyHps;
        private Animator stageStartAnim;
        private int _coinNum;
        // Start is called before the first frame update
        void Start()
        {
            _enemyTypes = new string[7] {"monkey", "gorani", "fox", "cat", "pigeon", "bat", "dog"};
            stageStartAnim = stageStart.GetComponent<Animator>();
            stageStartAnim.enabled = false;
            NewGame();
        }

        public void NewGame()
        {
            mainCamera.transform.position.Set(0, 0, -10);
            playerObject.transform.position.Set(0, 1, 0);
            _enemyHps = new int[7] { 15, 15, 10, 10, 10, 10, 12 };
            _scoreNum = 0;
            _playerXpNum = 0;
            _allXpNum = 0;
            _stageNum = 1;
            _playerLevelNum = 1;
            _allLevelNum = 1;
            _defaultHp = 90;
            _defaultPlayerXpSum = 280;
            _allXpSum = 300;
            _enemyDamage = 10;
            _enemyXp = 10;
            _hpNum = _defaultHp;
            scoreText.text = _scoreNum.ToString();
            playerLevelText.text = "Lv." + _playerLevelNum;
            allLevelText.text = "Lv " + _allLevelNum;
            stageText.text = "STAGE " + _stageNum;
            hpText.text = _hpNum + " / " + _defaultHp;
            playerXpText.text = _playerXpNum + " / " + _defaultPlayerXpSum;
            hpBar.fillAmount = 1;
            allXpBar.fillAmount = 0;
            playerXpBar.fillAmount = 0;
            retryObject.SetActive(false);
            coinText.text = "x " + _coinNum;
            StartCoroutine(StartGame());
        }

        IEnumerator StartGame()
        {
            stageStartAnim.Play("StageNumMove", -1, 0f);
            yield return new WaitForSeconds(1f);
            playerObject.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            stageStart.SetActive(false);
            player.isGamePlaying = true;
            mainCameraScript.isGamePlaying = true;
        }
        
        private IEnumerator GameEnd()
        {
            mainCameraScript.isGamePlaying = false;
            objectManager.FailGame();
            yield return new WaitForSeconds(0.2f);
            retryObject.SetActive(true);
            playerObject.SetActive(false);
            retryAnim.Play("EndGame", -1, 0f);
        }
        
        IEnumerator NewStage()
        {
            ++_stageNum;
            stageStartText.text = "STAGE " + _stageNum;
            stageStartAnim.enabled = true;
            // stageStartAnim.Play("StageNumMove", -1, 0f);
            yield return new WaitForSeconds(1.1f);
            player.isGamePlaying = true;
            stageText.text = "STAGE " + _stageNum;
            for (int i = 0; i < 7; i++)
            {
                _enemyHps[i] = (int)Math.Truncate(_enemyHps[i] * 1.1f);
            }
            _enemyDamage = (int)Math.Truncate(_enemyDamage * 1.1f);
            _enemyXp = (int)Math.Truncate(_enemyXp * 1.1f);
            StartCoroutine(SpawnMonsters(0.6f));
        }

        IEnumerator SpawnMonsters(float time)
        {
            int enemyType;
            _currentMonsterNum = 25;
            switch (_stageNum)
            {
                case 1:
                    enemyType = 1;
                    break;
                case 2:
                    enemyType = 2;
                    break;
                case 3:
                    enemyType = 4;
                    break;
                case 4:
                    enemyType = 6;
                    break;
                default:
                    enemyType = 7;
                    break;
            }

            yield return new WaitForSeconds(time);
            for (int i = 0; i < _currentMonsterNum; i++)
            {
                int enemyCode = Random.Range(0, enemyType);
                GameObject temp = objectManager.MakeObject(_enemyTypes[enemyCode],
                    new Vector3(Random.Range(11f, 62f), 2, 0));
                temp.GetComponent<Enemy>().SetState(false, _enemyHps[enemyCode], _enemyDamage);
            }
        }

        IEnumerator SpawnBoss(float time)
        {
            int bossNum, enemyNum;
            if (_stageNum < 8)
            {
                bossNum = _stageNum - 1;
                enemyNum = 0;
            }
            else
            {
                bossNum = Random.Range(0, 7);
                enemyNum = (int)Math.Truncate((float)(_stageNum - 5) / 3f);
            }
            _currentMonsterNum = 1 + enemyNum;
            yield return new WaitForSeconds(time);
            for (int i = 0; i < enemyNum; i++)
            {
                int enemyCode = Random.Range(0, 7);
                GameObject tempEnemy = objectManager.MakeObject(_enemyTypes[enemyCode],
                    new Vector3(Random.Range(50f, 64f), 2, 0));
                tempEnemy.GetComponent<Enemy>().SetState(false, _enemyHps[enemyCode], _enemyDamage);
            }
            GameObject tempBoss = objectManager.MakeObject(_enemyTypes[bossNum], new Vector3(63, 6, 0));
            tempBoss.GetComponent<Enemy>().SetState(true, _enemyHps[bossNum] * 3, _enemyDamage * 3);
        }

        public void GetCoin()
        {
            _coinNum++;
            coinText.text = "x " + _coinNum;
            ControlScore(10);
        }

        public void PlayerGetHit(int damage)
        {
            ControlPlayerHp(damage);
        }

        public void EnemyGetHit()
        {
            ControlAllXp((int)(_enemyXp * 0.3));
        }

        public void EnemyDead()
        {
            _currentMonsterNum--;
        }

        public void GetEnemyXp(bool isBoss)
        {
            if (isBoss)
            {
                ControlAllXp(_enemyXp * 2);
                ControlPlayerXp(_enemyXp * 2);
                ControlScore(50);
            }
            else
            {
                ControlAllXp(_enemyXp);
                ControlPlayerXp(_enemyXp);
                ControlScore(20);
            }
        }

        private void ControlScore(int newScore)
        {
            _scoreNum += newScore;
            scoreText.text = _scoreNum.ToString();
        }

        private void ControlAllXp(int newScore)
        {
            _allXpNum += newScore;
            if (_allXpNum >= _allXpSum)
            {
                _allLevelNum++;
                allLevelText.text = "Lv " + _allLevelNum;
                _allXpNum -= _allXpSum;
                _defaultHp = (int)Math.Truncate((float)(_defaultHp) * 1.05f);
                _allXpSum = (int)Math.Truncate((float)(_allXpSum) * 1.1f);
                _hpNum = _defaultHp;
                hpText.text = _hpNum + " / " + _defaultHp;
                hpBar.fillAmount = (float)_hpNum / (float)_defaultHp;
            }
            allXpBar.fillAmount = (float)_allXpNum / (float)_allXpSum;
        }

        private void ControlPlayerXp(int newScore)
        {
            _playerXpNum += newScore;
            if (_playerXpNum >= _defaultPlayerXpSum)
            {
                _playerLevelNum++;
                playerLevelText.text = "Lv." + _playerLevelNum;
                _playerXpNum -= _defaultPlayerXpSum;
                _hpNum = _defaultHp;
                hpText.text = _hpNum + " / " + _defaultHp;
                hpBar.fillAmount = 1f;
                _defaultPlayerXpSum = (int)Math.Truncate((float)(_defaultPlayerXpSum) * 1.1f);
                _enemyDamage = (int)Math.Truncate((float)(_enemyDamage) * 1.1f);
            }
            playerXpBar.fillAmount = (float)_playerXpNum / (float)_defaultPlayerXpSum;
            playerXpText.text = _playerXpNum + " / " + _defaultPlayerXpSum;
        }

        private void ControlPlayerHp(int hp)
        {
            _hpNum -= hp;
            if (_hpNum <= 0)
            {
                _hpNum = 0;
                StartCoroutine(GameEnd());
            }
            hpText.text = _hpNum + " / " + _defaultHp;
            hpBar.fillAmount = (float)_hpNum / (float)_defaultHp;
        }
        
    }
}
