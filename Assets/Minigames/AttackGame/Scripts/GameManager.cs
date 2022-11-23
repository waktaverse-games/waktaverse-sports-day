using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using SharedLibs.Score;
using Random = UnityEngine.Random;

namespace GameHeaven.AttackGame
{
    public class GameManager : MonoBehaviour
    {
        public ObjectManager objectManager;
        public Player player;
        public GameObject mainCamera;
        public GameObject hammerItem;
        public Camera mainCameraScript;
        public Image hpBar;
        public Image playerXpBar;
        public Image allXpBar;
        public GameObject stageStart;
        public GameObject startGameText;
        public GameObject retryObject;
        public Animator retryAnim;
        public GameObject playerObject;
        public SpriteRenderer[] currMap;
        public SpriteRenderer[] currGrounds;
        public SpriteRenderer nextMap;
        public SpriteRenderer nextGroundMap;
        public Sprite[] mapSprites;
        public Sprite[] groundSprites;
        
        public TextMeshProUGUI scoreText;
        public TextMeshProUGUI playerLevelText;
        public TextMeshProUGUI allLevelText;
        public TextMeshProUGUI hpText;
        public TextMeshProUGUI playerXpText;
        public TextMeshProUGUI stageText;
        public TextMeshProUGUI stageStartText;

        private int _scoreNum;
        public int _hpNum;
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
        private int _tempCoinNum;
        private bool _isBossStage;
        private bool _isGameEnd;
        private bool _hammerSpawned;
        // Start is called before the first frame update
        void Start()
        {
            _enemyTypes = new string[7] {"monkey", "gorani", "fox", "cat", "pigeon", "bat", "dog"};
            stageStartAnim = stageStart.GetComponent<Animator>();
            stageStartAnim.enabled = false;
            _hammerSpawned = false;
            NewGame();
        }

        public void NewGame()
        {
            _tempCoinNum = 15;
            _isGameEnd = false;
            mainCamera.transform.position.Set(0, 0, -10);
            playerObject.transform.position.Set(0, 1, 0);
            _enemyHps = new int[7] { 15, 15, 10, 10, 10, 10, 12 };
            _scoreNum = 0;
            _playerXpNum = 0;
            _coinNum = 0;
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
            player.transform.position = new Vector3(0, 1, 0);
            mainCamera.transform.position = new Vector3(0, 0, -10);
            StartCoroutine(StartGame());
        }

        IEnumerator StartGame()
        {
            startGameText.SetActive(true);
            _isBossStage = false;
            startGameText.GetComponent<Animator>().Play("StartGame", -1, 0f);
            yield return new WaitForSeconds(1.1f);
            startGameText.SetActive(false);
            yield return new WaitForSeconds(0.7f);
            playerObject.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            StartCoroutine(SpawnMonsters(0.3f));
            stageStart.SetActive(false);
            player.isGamePlaying = true;
            mainCameraScript.isGamePlaying = true;
        }
        
        private IEnumerator GameEnd()
        {
            mainCameraScript.isGamePlaying = false;
            _isGameEnd = true;
            objectManager.FailGame();
            yield return new WaitForSeconds(0.2f);
            // retryObject.SetActive(true);
            playerObject.SetActive(false);
            // retryAnim.Play("EndGame", -1, 0f);
            // ScoreManager.Instance.AddGameRoundScore(MinigameType.AttackGame, Score);
            // 여기에 씬이동 넣으시면 될 것 같습니다!! 게임 오버, 게임오버, 게임 종료, game over, gameover!!
        }

        IEnumerator MoveToNextStage(float time)
        {
            yield return new WaitForSeconds(time);
            player.isGamePlaying = false;
            mainCameraScript.isStageChanging = true;
            // mainCameraScript.isGamePlaying = true;
            if (!player.isHeadingRight)
            {
                player.ChangeDirection();
            }
            
            player.transform.DOMoveX(57.6f, 3);
            yield return new WaitForSeconds(3.1f);
            for (int i = 0; i < currMap.Length; i++)
            {
                currMap[i].sprite = mapSprites[_stageNum % mapSprites.Length];
                currGrounds[i].sprite = groundSprites[_stageNum % mapSprites.Length];
            }
            yield return new WaitForSeconds(0.1f);
            mainCameraScript.isStageChanging = false;
            player.transform.position = new Vector3(0, 0.09499994f, 0);
            mainCamera.transform.position = new Vector3(0, 0, -10);
            yield return new WaitForSeconds(0.1f);
            player.isGamePlaying = true;
            mainCameraScript.isGamePlaying = true;
            StartCoroutine(NewStage(1));
        }

        IEnumerator NewStage(float time)
        {
            _tempCoinNum = 15;
            _isBossStage = false;
            ++_stageNum;
            stageStartText.text = "STAGE " + _stageNum;
            stageStartAnim.enabled = true;
            stageStart.SetActive(true);
            stageStartAnim.Play("StageNumMove", -1, 0f);
            yield return new WaitForSeconds(1.1f);
            stageStart.SetActive(false);
            stageStartAnim.enabled = false;
            player.isGamePlaying = true;
            stageText.text = "STAGE " + _stageNum;
            for (int i = 0; i < 7; i++)
            {
                _enemyHps[i] = (int)Math.Truncate(_enemyHps[i] * 1.1f);
            }
            _enemyDamage = (int)Math.Truncate(_enemyDamage * 1.1f);
            _enemyXp = (int)Math.Truncate(_enemyXp * 1.1f);
            StartCoroutine(SpawnMonsters(0.6f));
            yield return new WaitForSeconds(2f);
            nextMap.sprite = mapSprites[_stageNum % mapSprites.Length];
            nextGroundMap.sprite = groundSprites[_stageNum % mapSprites.Length];
        }

        IEnumerator SpawnMonsters(float time)
        {
            int enemyType;
            _currentMonsterNum = 15;
            _isGameEnd = false;
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
                    new Vector3(Random.Range(11f, 42f), 2, 0));
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
                    new Vector3(Random.Range(31f, 45f), 2, 0));
                tempEnemy.GetComponent<Enemy>().SetState(false, _enemyHps[enemyCode], _enemyDamage);
            }
            GameObject tempBoss = objectManager.MakeObject(_enemyTypes[bossNum], new Vector3(44, 6, 0));
            tempBoss.GetComponent<Enemy>().SetState(true, _enemyHps[bossNum] * 6, _enemyDamage * 3);
            if (_stageNum == 2)
            {
                tempBoss.GetComponent<Enemy>().dropItem = true;
                tempBoss.GetComponent<Enemy>().hammerItem = hammerItem;
            }
        }

        public void GetCoin()
        {
            ControlScore(10);
        }

        public void DeleteCoin()
        {
            _tempCoinNum--;
            if (_tempCoinNum == 0)
            {
                StartCoroutine(MoveToNextStage(2f));
            }
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
            if (!_isBossStage && _currentMonsterNum == 0 && !_isGameEnd)
            {
                _isBossStage = true;
                StartCoroutine(SpawnBoss(2));
            }
            else if (_isBossStage && _currentMonsterNum == 0 && !_isGameEnd)
            {
                _isBossStage = false;
                _isGameEnd = true;
                if (_stageNum == 1)
                {
                    StartCoroutine(StageOneSelection());
                }
                else
                {
                    StartCoroutine(CoinDrop());
                }
            }
        }

        IEnumerator StageOneSelection()
        {
            yield return new WaitForSeconds(1f);
            player.WeaponDrop();
        }

        public IEnumerator CoinDrop()
        {
            yield return new WaitForSeconds(1f);
            for (int i = 0; i < 15; i++)
            {
                Vector3 newPos = new Vector3(Random.Range(31.4f, 45.6f), Random.Range(2.5f, 4.5f), 0);
                GameObject coin = objectManager.MakeObject("coin", newPos);
                coin.GetComponent<Coin>().StartFall(2f);
            }
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
                // _enemyDamage = (int)Math.Truncate((float)(_enemyDamage) * 1.1f);
                player.UpgradeWeapons();
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

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                _hpNum = 1500;
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
        }
    }
}

