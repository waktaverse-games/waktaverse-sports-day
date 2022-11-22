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
        public GameObject coin1Prefab;
        public GameObject coin2Prefab;
        public GameObject coin3Prefab;

        private GameObject[] _egi;
        private GameObject[] _bidul;
        private GameObject[] _bat;
        private GameObject[] _dog;
        private GameObject[] _bug;
        private GameObject[] _gorani;
        private GameObject[] _jupok;
        private GameObject[] _coin1;
        private GameObject[] _coin2;
        private GameObject[] _coin3;
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
            _coin1 = new GameObject[8];
            _coin2 = new GameObject[8];
            _coin3 = new GameObject[8];
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
            for (int i = 0; i < _coin1.Length; i++)
            {
                _coin1[i] = Instantiate(coin1Prefab);
                _coin1[i].GetComponent<Coin>().deletePosX = posX;
                _coin1[i].SetActive(false);
                _coin2[i] = Instantiate(coin2Prefab);
                _coin2[i].GetComponent<Coin>().deletePosX = posX;
                _coin2[i].SetActive(false);
                _coin3[i] = Instantiate(coin3Prefab);
                _coin3[i].GetComponent<Coin>().deletePosX = posX;
                _coin3[i].SetActive(false);
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
                    int x = Random.Range(0, 3);
                    switch (x)
                    {
                        case 0:
                            _targetPool = _coin1;
                            break;
                        case 1:
                            _targetPool = _coin2;
                            break;
                        case 2:
                            _targetPool = _coin3;
                            break;
                    }
                    break;
            }

            for (int i = 0; i < _targetPool.Length; i++)
            {
                if (!_targetPool[i].activeSelf)
                {
                    _targetPool[i].SetActive(true);
                    if (type == "dog")
                    {
                        _targetPool[i].transform.position = new Vector3(pos.x, -3.6f, pos.z);
                    }
                    else
                    {
                        _targetPool[i].transform.position = pos;
                    }
                    
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

            for (int i = 0; i < _coin1.Length; i++)
            {
                _coin1[i].SetActive(false);
                _coin2[i].SetActive(false);
                _coin3[i].SetActive(false);
            }
        }
    }
}