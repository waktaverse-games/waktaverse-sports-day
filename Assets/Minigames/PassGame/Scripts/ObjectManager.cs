using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GameHeaven.PassGame
{
    public class ObjectManager : MonoBehaviour
    {
        public GameObject player;
        public GameManager gameManager;
        public GameObject deletePos;
        public GameObject egiPrefab;
        public GameObject bidulPrefab;
        public GameObject batPrefab;
        public GameObject dogPrefab;
        public GameObject bugPrefab;
        public GameObject goraniPrefab;
        public GameObject jupokPrefab;
        public GameObject coinPrefab;

        private GameObject[] _egi;
        private GameObject[] _bidul;
        private GameObject[] _bat;
        private GameObject[] _dog;
        private GameObject[] _bug;
        private GameObject[] _gorani;
        private GameObject[] _jupok;
        private GameObject[] _coin;
        private GameObject[] _targetPool;

        // Start is called before the first frame update
        void Awake()
        {
            _egi = new GameObject[10];
            _bidul = new GameObject[10];
            _bat = new GameObject[10];
            _dog = new GameObject[10];
            _bug = new GameObject[10];
            _gorani = new GameObject[10];
            _jupok = new GameObject[10];
            _coin = new GameObject[5];
            Generate();
        }

        void Generate()
        {
            float posX = deletePos.transform.position.x;
            for (int i = 0; i < _egi.Length; i++)
            {
                _egi[i] = Instantiate(egiPrefab);
                _egi[i].GetComponent<EnemyDefaultMove>().player = player;
                _egi[i].GetComponent<EnemyDefaultMove>().gameManager = gameManager;
                _egi[i].GetComponent<EnemyDefaultMove>().deletePosX = posX;
                _egi[i].SetActive(false);
            }

            for (int i = 0; i < _bidul.Length; i++)
            {
                _bidul[i] = Instantiate(bidulPrefab);
                _bidul[i].GetComponent<EnemyDefaultMove>().player = player;
                _bidul[i].GetComponent<EnemyDefaultMove>().gameManager = gameManager;
                _bidul[i].GetComponent<EnemyDefaultMove>().deletePosX = posX;
                _bidul[i].SetActive(false);
            }

            for (int i = 0; i < _bat.Length; i++)
            {
                _bat[i] = Instantiate(batPrefab);
                _bat[i].GetComponent<EnemyDefaultMove>().player = player;
                _bat[i].GetComponent<EnemyDefaultMove>().gameManager = gameManager;
                _bat[i].GetComponent<EnemyDefaultMove>().deletePosX = posX;
                _bat[i].SetActive(false);
            }

            for (int i = 0; i < _dog.Length; i++)
            {
                _dog[i] = Instantiate(dogPrefab);
                _dog[i].GetComponent<EnemyDefaultMove>().player = player;
                _dog[i].GetComponent<EnemyDefaultMove>().gameManager = gameManager;
                _dog[i].GetComponent<EnemyDefaultMove>().deletePosX = posX;
                _dog[i].SetActive(false);
            }

            for (int i = 0; i < _bug.Length; i++)
            {
                _bug[i] = Instantiate(bugPrefab);
                _bug[i].GetComponent<EnemyDefaultMove>().player = player;
                _bug[i].GetComponent<EnemyDefaultMove>().gameManager = gameManager;
                _bug[i].GetComponent<EnemyDefaultMove>().deletePosX = posX;
                _bug[i].SetActive(false);
            }

            for (int i = 0; i < _gorani.Length; i++)
            {
                _gorani[i] = Instantiate(goraniPrefab);
                _gorani[i].GetComponent<EnemyDefaultMove>().player = player;
                _gorani[i].GetComponent<EnemyDefaultMove>().gameManager = gameManager;
                _gorani[i].GetComponent<EnemyDefaultMove>().deletePosX = posX;
                _gorani[i].SetActive(false);
            }

            for (int i = 0; i < _jupok.Length; i++)
            {
                _jupok[i] = Instantiate(jupokPrefab);
                _jupok[i].GetComponent<EnemyDefaultMove>().player = player;
                _jupok[i].GetComponent<EnemyDefaultMove>().gameManager = gameManager;
                _jupok[i].GetComponent<EnemyDefaultMove>().deletePosX = posX;
                _jupok[i].SetActive(false);
            }
            for (int i = 0; i < _coin.Length; i++)
            {
                _coin[i] = Instantiate(coinPrefab);
                _coin[i].GetComponent<Coin>().deletePosX = posX;
                _coin[i].SetActive(false);
            }
        }

        public void MakeObject(string type, Vector3 pos)
        {
            switch (type)
            {
                case "egi":
                    _targetPool = _egi;
                    break;
                case "bidul":
                    _targetPool = _bidul;
                    break;
                case "bat":
                    _targetPool = _bat;
                    break;
                case "dog":
                    _targetPool = _dog;
                    break;
                case "bug":
                    _targetPool = _bug;
                    break;
                case "gorani":
                    _targetPool = _gorani;
                    break;
                case "jupok":
                    _targetPool = _jupok;
                    break;
                case "coin":
                    _targetPool = _coin;
                    break;
            }

            for (int i = 0; i < _targetPool.Length; i++)
            {
                if (!_targetPool[i].activeSelf)
                {
                    _targetPool[i].SetActive(true);
                    _targetPool[i].transform.position = pos;
                    return;
                }
            }

            return;
        }

        public void FailGame()
        {
            for (int i = 0; i < _egi.Length; i++)
            {
                _egi[i].SetActive(false);
            }

            for (int i = 0; i < _bidul.Length; i++)
            {
                _bidul[i].SetActive(false);
            }

            for (int i = 0; i < _bat.Length; i++)
            {
                _bat[i].SetActive(false);
            }

            for (int i = 0; i < _dog.Length; i++)
            {
                _dog[i].SetActive(false);
            }

            for (int i = 0; i < _bug.Length; i++)
            {
                _bug[i].SetActive(false);
            }

            for (int i = 0; i < _gorani.Length; i++)
            {
                _gorani[i].SetActive(false);
            }

            for (int i = 0; i < _jupok.Length; i++)
            {
                _jupok[i].SetActive(false);
            }

            for (int i = 0; i < _coin.Length; i++)
            {
                _coin[i].SetActive(false);
            }
        }
    }
}