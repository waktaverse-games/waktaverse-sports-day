using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using SharedLibs;

namespace GameHaven.RunGame
{ 
    public class GameManager : MonoBehaviour
    {
        #region Singleton
        public static GameManager instance = null;
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                if (instance != this)
                    Destroy(this.gameObject);
            }
        }
        #endregion

        public GameObject[] wall;
        [SerializeField] GameObject[] item;
        public Transform spawnPoints;
        public GameObject dust;
        public Transform dustPoints;
        public GameObject cone;
        int coneCount = 1;

        public int score = 0;
        public int highScore;

        public Text scoreText;
        public Text highScoreText;
        public GameObject re;

        public float wallSpawnDelay;
        public float curSpawnDelay;

        public float coinSpawnDelay;
        public float curCoinSpawnDelay;

        public static float gameTime;
        public static float scoreTime;
        public static float wallSpeed;
        public float timer;


        // Update is called once per frame
        void Start()
        {
            gameTime = 0;
            wallSpeed = 2;
            highScoreText.text = "0";
            re.SetActive(false);

        }

        void Update()
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
            if (PlayerControl.GetItem == false)
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
            //re.SetActive(true);
            Time.timeScale = 0;
        }

        public void Retry()
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

    }
}