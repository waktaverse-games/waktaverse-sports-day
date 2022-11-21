using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameHeaven.AttackGame
{
    public class ObjectManager : MonoBehaviour
    {
        public GameObject player;
        public GameManager gameManager;
        public GameObject monkeyPrefab;
        public GameObject pigeonPrefab;
        public GameObject batPrefab;
        public GameObject dogPrefab;
        public GameObject catPrefab;
        public GameObject goraniPrefab;
        public GameObject foxPrefab;
        public GameObject arrowPrefab;
        public GameObject pyochangPrefab;
        public GameObject poopPrefab;
        public GameObject coin1Prefab;
        public GameObject coin2Prefab;
        public GameObject coin3Prefab;
        public SFXManager sfxManager;
        public GameObject damagePrefab;

        private GameObject[] _monkey;
        private GameObject[] _pigeon;
        private GameObject[] _bat;
        private GameObject[] _dog;
        private GameObject[] _cat;
        private GameObject[] _gorani;
        private GameObject[] _fox;
        private GameObject[] _pyochang;
        private GameObject[] _arrow;
        private GameObject[] _poop;
        private GameObject[] _targetPool;
        private GameObject[] _coin1;
        private GameObject[] _coin2;
        private GameObject[] _coin3;
        private GameObject[] _damageText;
        private GameObject _emptyObject;
        private ObjectManager _objectManager;

        void Awake()
        {
            _monkey = new GameObject[25];
            _pigeon = new GameObject[25];
            _bat = new GameObject[25];
            _dog = new GameObject[25];
            _cat = new GameObject[25];
            _gorani = new GameObject[25];
            _fox = new GameObject[25];
            _arrow = new GameObject[50];
            _pyochang = new GameObject[50];
            _poop = new GameObject[50];
            _coin1 = new GameObject[15];
            _coin2 = new GameObject[15];
            _coin3 = new GameObject[15];
            _damageText = new GameObject[15];
            _objectManager = GetComponent<ObjectManager>();
            Generate();
            player.SetActive(false);
        }
        
        void Generate()
        {
            for (int i = 0; i < _monkey.Length; i++)
            {
                _monkey[i] = Instantiate(monkeyPrefab);
                _monkey[i].GetComponent<Enemy>().player = player;
                _monkey[i].GetComponent<Enemy>().gameManager = gameManager;
                _monkey[i].GetComponent<Enemy>().objectManager = _objectManager;
                _monkey[i].GetComponent<Enemy>().tweenId = i;
                _monkey[i].GetComponent<Enemy>().sfxManager = sfxManager;
                _monkey[i].SetActive(false);
            }
            for (int i = 0; i < _pigeon.Length; i++)
            {
                _pigeon[i] = Instantiate(pigeonPrefab);
                _pigeon[i].GetComponent<Enemy>().player = player;
                _pigeon[i].GetComponent<Enemy>().gameManager = gameManager;
                _pigeon[i].GetComponent<Enemy>().objectManager = _objectManager;
                _pigeon[i].GetComponent<Enemy>().tweenId = i + 60;
                _pigeon[i].GetComponent<Enemy>().sfxManager = sfxManager;
                _pigeon[i].SetActive(false);
            }
            for (int i = 0; i < _bat.Length; i++)
            {
                _bat[i] = Instantiate(batPrefab);
                _bat[i].GetComponent<Enemy>().player = player;
                _bat[i].GetComponent<Enemy>().gameManager = gameManager;
                _bat[i].GetComponent<Enemy>().objectManager = _objectManager;
                _bat[i].GetComponent<Enemy>().sfxManager = sfxManager;
                _bat[i].GetComponent<Enemy>().tweenId = i + 120;
                _bat[i].SetActive(false);
            }
            for (int i = 0; i < _dog.Length; i++)
            {
                _dog[i] = Instantiate(dogPrefab);
                _dog[i].GetComponent<Enemy>().player = player;
                _dog[i].GetComponent<Enemy>().gameManager = gameManager;
                _dog[i].GetComponent<Enemy>().objectManager = _objectManager;
                _dog[i].GetComponent<Enemy>().tweenId = i + 180;
                _dog[i].GetComponent<Enemy>().sfxManager = sfxManager;
                _dog[i].SetActive(false);
            }
            for (int i = 0; i < _cat.Length; i++)
            {
                _cat[i] = Instantiate(catPrefab);
                _cat[i].GetComponent<Enemy>().player = player;
                _cat[i].GetComponent<Enemy>().gameManager = gameManager;
                _cat[i].GetComponent<Enemy>().objectManager = _objectManager;
                _cat[i].GetComponent<Enemy>().tweenId = i + 240;
                _cat[i].GetComponent<Enemy>().sfxManager = sfxManager;
                _cat[i].SetActive(false);
            }
            for (int i = 0; i < _gorani.Length; i++)
            {
                _gorani[i] = Instantiate(goraniPrefab);
                _gorani[i].GetComponent<Enemy>().player = player;
                _gorani[i].GetComponent<Enemy>().gameManager = gameManager;
                _gorani[i].GetComponent<Enemy>().objectManager = _objectManager;
                _gorani[i].GetComponent<Enemy>().tweenId = i + 300;
                _gorani[i].GetComponent<Enemy>().sfxManager = sfxManager;
                _gorani[i].SetActive(false);
            }
            for (int i = 0; i < _fox.Length; i++)
            {
                _fox[i] = Instantiate(foxPrefab);
                _fox[i].GetComponent<Enemy>().player = player;
                _fox[i].GetComponent<Enemy>().gameManager = gameManager;
                _fox[i].GetComponent<Enemy>().objectManager = _objectManager;
                _fox[i].GetComponent<Enemy>().tweenId = i + 360;
                _fox[i].GetComponent<Enemy>().sfxManager = sfxManager;
                _fox[i].SetActive(false);
            }
            
            for (int i = 0; i < _arrow.Length; i++)
            {
                _arrow[i] = Instantiate(arrowPrefab);
                _arrow[i].GetComponent<Projectile>().gameManager = gameManager;
                _arrow[i].GetComponent<Projectile>().tweenId = i + 420;
                _arrow[i].SetActive(false);
            }
            for (int i = 0; i < _pyochang.Length; i++)
            {
                _pyochang[i] = Instantiate(pyochangPrefab);
                _pyochang[i].GetComponent<Projectile>().gameManager = gameManager;
                _pyochang[i].GetComponent<Projectile>().tweenId = i + 480;
                _pyochang[i].SetActive(false);
            }
            for (int i = 0; i < _poop.Length; i++)
            {
                _poop[i] = Instantiate(poopPrefab);
                _poop[i].GetComponent<Projectile>().gameManager = gameManager;
                _poop[i].GetComponent<Projectile>().tweenId = i + 540;
                _poop[i].SetActive(false);
            }
            for (int i = 0; i < _damageText.Length; i++)
            {
                _damageText[i] = Instantiate(damagePrefab);
                _damageText[i].SetActive(false);
            }

            for (int i = 0; i < _coin1.Length; i++)
            {
                _coin1[i] = Instantiate(coin1Prefab);
                _coin1[i].GetComponent<Coin>().tweenId = i + 600;
                _coin1[i].GetComponent<Coin>().gameManager = gameManager;
                _coin1[i].SetActive(false);
                _coin2[i] = Instantiate(coin2Prefab);
                _coin2[i].GetComponent<Coin>().tweenId = i + 605;
                _coin2[i].GetComponent<Coin>().gameManager = gameManager;
                _coin2[i].SetActive(false);
                _coin3[i] = Instantiate(coin3Prefab);
                _coin3[i].GetComponent<Coin>().tweenId = i + 610;
                _coin3[i].GetComponent<Coin>().gameManager = gameManager;
                _coin3[i].SetActive(false);
            }
        }

        void Test()
        {
            Debug.Log("VAR");
        }
        
        public GameObject MakeObject(string type, Vector3 pos)
        {
            switch (type)
            {
                case "monkey":
                    _targetPool = _monkey;
                    break;
                case "pigeon":
                    _targetPool = _pigeon;
                    break;
                case "bat":
                    _targetPool = _bat;
                    break;
                case "dog":
                    _targetPool = _dog;
                    break;
                case "cat":
                    _targetPool = _cat;
                    break;
                case "gorani":
                    _targetPool = _gorani;
                    break;
                case "fox":
                    _targetPool = _fox;
                    break;
                case "arrow":
                    _targetPool = _arrow;
                    break;
                case "pyochang":
                    _targetPool = _pyochang;
                    break;
                case "poop":
                    _targetPool = _poop;
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
                case "damage":
                    _targetPool = _damageText;
                    break;
            }
            for (int i = 0; i < _targetPool.Length; i++)
            {
                if (!_targetPool[i].activeSelf)
                {
                    _targetPool[i].SetActive(true);
                    _targetPool[i].transform.position = pos;
                    return _targetPool[i];
                }
            }
            return _emptyObject;
        }

        public void FailGame()
        {
            for (int i = 0; i < _monkey.Length; i++)
            {
                _monkey[i].SetActive(false);
            }
            for (int i = 0; i < _pigeon.Length; i++)
            {
                _pigeon[i].SetActive(false);
            }
            for (int i = 0; i < _bat.Length; i++)
            {
                _bat[i].SetActive(false);
            }
            for (int i = 0; i < _dog.Length; i++)
            {
                _dog[i].SetActive(false);
            }
            for (int i = 0; i < _cat.Length; i++)
            {
                _cat[i].SetActive(false);
            }
            for (int i = 0; i < _gorani.Length; i++)
            {
                _gorani[i].SetActive(false);
            }
            for (int i = 0; i < _fox.Length; i++)
            {
                _fox[i].SetActive(false);
            }
            for (int i = 0; i < _arrow.Length; i++)
            {
                _arrow[i].SetActive(false);
            }
            for (int i = 0; i < _pyochang.Length; i++)
            {
                _pyochang[i].SetActive(false);
            }
            for (int i = 0; i < _poop.Length; i++)
            {
                _poop[i].SetActive(false);
            }

            for (int i = 0; i < _damageText.Length; i++)
            {
                _damageText[i].SetActive(false);
            }
        }
    }
}

