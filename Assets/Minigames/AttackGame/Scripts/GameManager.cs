using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
        
        public TextMeshProUGUI scoreText;
        public TextMeshProUGUI playerLevelText;
        public TextMeshProUGUI allLevelText;
        public TextMeshProUGUI hpText;
        public TextMeshProUGUI playerXpText;
        public TextMeshProUGUI stageText;
        public TextMeshProUGUI stageStartText;

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
        // Start is called before the first frame update
        void Start()
        {
            _enemyTypes = new string[7] {"monkey", "gorani", "fox", "cat", "pigeon", "bat", "dog"};
            stageStartAnim = stageStart.GetComponent<Animator>();
            // stageStartAnim.Play("StageNumMove", -1, 0f);
            NewGame();
        }

        void NewGame()
        {
            mainCamera.transform.position.Set(0, 0, -10);
            player.transform.position.Set(0, 1, 0);
            _enemyHps = new int[7] { 15, 15, 10, 10, 10, 10, 12 };
            _scoreNum = 0;
            _playerXpNum = 0;
            _allXpNum = 0;
            _stageNum = 1;
            _playerLevelNum = 1;
            _allLevelNum = 1;
            _defaultHp = 100;
            _defaultPlayerXpSum = 280;
            _allXpSum = 280;
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
        }
        
        void NewStage()
        {
            _stageNum++;
            for (int i = 0; i < 7; i++)
            {
                _enemyHps[i] = (int)(_enemyHps[i] * 1.1f);
            }
            _enemyDamage = (int)(_enemyDamage * 1.1f);
            _enemyXp = (int)(_enemyXp * 1.1f);
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

        // Update is called once per frame
        void Update()
        {

        }
    }
}

