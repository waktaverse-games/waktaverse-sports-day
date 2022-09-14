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
        public Image hpBar;
        public Image playerXpBar;
        public Image allXpBar;
        
        public TextMeshProUGUI scoreText;
        public TextMeshProUGUI playerLevelText;
        public TextMeshProUGUI allLevelText;
        public TextMeshProUGUI hpText;
        public TextMeshProUGUI playerXpText;
        public TextMeshProUGUI stageText;

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
        private int _damage;
        private int _enemyXp;
        // Start is called before the first frame update
        void Start()
        {

        }

        void NewGame()
        {
            mainCamera.transform.position.Set(0, 0, -10);
            player.transform.position.Set(0, 1, 0);
            _scoreNum = 0;
            _playerXpNum = 0;
            _allXpNum = 0;
            _stageNum = 1;
            _playerLevelNum = 1;
            _allLevelNum = 1;
            _defaultHp = 100;
            _defaultPlayerXpSum = 280;
            _allXpSum = 280;
            _damage = 10;
            _enemyXp = 10;
            scoreText.text = _scoreNum.ToString();
            playerLevelText.text = "Lv." + _playerLevelNum;
            allLevelText.text = "Lv " + _allLevelNum;
            stageText.text = "STAGE " + _stageNum;
            hpText.text = _hpNum + "/" + _defaultHp;
            playerXpText.text = _playerXpNum + "/" + _defaultPlayerXpSum;
            hpBar.fillAmount = 1;
            allXpBar.fillAmount = 1;
            playerXpBar.fillAmount = 1;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

