using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using SharedLibs;
using SharedLibs.Score;

namespace GameHaven.RunGame
{ 
    public class GameManager : MonoBehaviour
    {
        public GameObject[] wall;
        [SerializeField] GameObject[] item;
        public Transform spawnPoints;
        public GameObject dust;
        public Transform dustPoints;
        public GameObject cone;
        int coneCount = 1;

        public int score = 0;

        public Text scoreText;
        public Text highScoreText;
        public GameObject re;

        public float wallSpawnDelay;
        public float curSpawnDelay;

        public float coinSpawnDelay;
        public float curCoinSpawnDelay;

        public float gameTime;
        public float scoreTime;
        public float wallSpeed;
        public float timer;

        public bool gameStart;
        public bool gameStop;

        public PlayerControl control;

        public AnimationCounter counter;

        AudioSource bgm;
        public AudioClip[] gameOver;

        private void OnEnable()
        {
            SoundManager.Instance.OnBGMVolumeChanged += SetVolume;
            counter.OnEndCount += () =>
            {
                GameStart();
            };
        }

        private void OnDisable()
        {
            SoundManager.Instance.OnBGMVolumeChanged -= SetVolume;
        }

        void Awake()
        {
            bgm = GameObject.Find("RunGameManager").GetComponent<AudioSource>();
            bgm.Stop();
            gameStart = false;
            gameStop = false;
            wallSpeed = 0;
            highScoreText.text = ScoreManager.Instance.GetGameScore(MinigameType.RunGame).ToString();
            scoreText.text = "0";

        }

        // Update is called once per frame
        void Start()
        {
           
        }

        void Update()
        {
            if (gameStop == false && gameStart==true)
            {
                curSpawnDelay += Time.deltaTime;
                curCoinSpawnDelay += Time.deltaTime;
                gameTime += Time.deltaTime;
                scoreTime += Time.deltaTime;
                timer += Time.deltaTime;

                scoreText.text = string.Format("{0:n0}", score);


                if (scoreTime >= 1)
                {
                    score += 10;
                    scoreTime = 0;
                }

                if (curSpawnDelay > wallSpawnDelay)
                {
                    SpawnWall();
                    SpawnDust();
                    SpawnCone(Random.Range(0, 21));
                    curSpawnDelay = 0;
                    wallSpawnDelay = 2 / wallSpeed;
                }

                if (curCoinSpawnDelay > coinSpawnDelay)
                {
                    SpawnCoin();
                    curCoinSpawnDelay = 0;
                    coinSpawnDelay = Random.Range(4, 7);
                }
            }
        }

        public void SpawnWall()
        {
            Instantiate(wall[Random.Range(0, wall.Length)], spawnPoints.position, spawnPoints.rotation);
        }

        public void SpawnCoin()
        {
            Instantiate(item[Random.Range(0, item.Length)], spawnPoints.position + new Vector3(Random.Range(-3f, 3f),0, 0), spawnPoints.rotation);
        }

        public  void SpawnDust()
        {
            if (control.GetItem == false)
            {
                Instantiate(dust, dustPoints.position + new Vector3(Random.Range(-0.5f, 0.5f), -1, 0), dustPoints.rotation);
            }
        }

        public void SpawnCone(int num)
        {
            if ((num == 0 || num == 11) && coneCount == 1)
            {
                Instantiate(cone, spawnPoints.position + new Vector3(9, 0, 0), spawnPoints.rotation);
                coneCount = 0;
            }
            else if((num == 10 || num == 20) && coneCount == 0)
            {
                Instantiate(cone, spawnPoints.position + new Vector3(-9, 0, 0), spawnPoints.rotation);
                coneCount = 1;
            }
        }

        public void ItemScore(int num)
        {
            score += num;
        }

        public void GameOver()
        {
            bgm.clip = gameOver[Random.Range(0, 2)];
            bgm.loop = false;
            bgm.Play();
            gameStop = true;
            wallSpeed = 0;
            ScoreManager.Instance.SetGameHighScore(MinigameType.RunGame, score);
            GameResultManager.ShowResult(MinigameType.RunGame, score);
        }

        public void GameStart()
        {
            gameTime = 0;
            wallSpeed = 2;
            bgm.Play();
            gameStart = true;
            SetVolume(SoundManager.Instance.BGMVolume);
            control.GameStart();
        }

        public void SetVolume(float volume)
        {
            bgm.volume = volume;
        }
    }
}