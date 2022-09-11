using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        public GameObject lmonkeyPrefab;
        public GameObject lpigeonPrefab;
        public GameObject lbatPrefab;
        public GameObject ldogPrefab;
        public GameObject lcatPrefab;
        public GameObject lgoraniPrefab;
        public GameObject lfoxPrefab;

        private GameObject[] _monkey;
        private GameObject[] _pigeon;
        private GameObject[] _bat;
        private GameObject[] _dog;
        private GameObject[] _cat;
        private GameObject[] _gorani;
        private GameObject[] _fox;
        private GameObject _lmonkey;
        private GameObject _lpigeon;
        private GameObject _lbat;
        private GameObject _ldog;
        private GameObject _lcat;
        private GameObject _lgorani;
        private GameObject _lfox;
        private GameObject[] _pyochang;
        private GameObject[] _arrow;
        private GameObject[] _poop;
        private GameObject[] _targetPool;
        private GameObject _emptyObject;
        
        void Awake()
        {
            _monkey = new GameObject[50];
            _pigeon = new GameObject[50];
            _bat = new GameObject[50];
            _dog = new GameObject[50];
            _fox = new GameObject[50];
            _gorani = new GameObject[50];
            _fox = new GameObject[50];
            _arrow = new GameObject[50];
            _pyochang = new GameObject[50];
            _poop = new GameObject[50];
            Generate();
        }
        
        void Generate()
        {
            for (int i = 0; i < _monkey.Length; i++)
            {
                _monkey[i] = Instantiate(monkeyPrefab);
                _monkey[i].GetComponent<Enemy>().player = player;
                _monkey[i].GetComponent<Enemy>().gameManager = gameManager;
                _monkey[i].SetActive(false);
            }
            _lmonkey = Instantiate(lmonkeyPrefab);
            _lmonkey.GetComponent<Enemy>().player = player;
            _lmonkey.GetComponent<Enemy>().gameManager = gameManager;
            _lmonkey.SetActive(false);
            for (int i = 0; i < _pigeon.Length; i++)
            {
                _pigeon[i] = Instantiate(pigeonPrefab);
                _pigeon[i].GetComponent<Enemy>().player = player;
                _pigeon[i].GetComponent<Enemy>().gameManager = gameManager;
                _pigeon[i].SetActive(false);
            }
            _lpigeon = Instantiate(lpigeonPrefab);
            _lpigeon.GetComponent<Enemy>().player = player;
            _lpigeon.GetComponent<Enemy>().gameManager = gameManager;
            _lpigeon.SetActive(false);
            for (int i = 0; i < _bat.Length; i++)
            {
                _bat[i] = Instantiate(batPrefab);
                _bat[i].GetComponent<Enemy>().player = player;
                _bat[i].GetComponent<Enemy>().gameManager = gameManager;
                _bat[i].SetActive(false);
            }
            _lbat = Instantiate(lbatPrefab);
            _lbat.GetComponent<Enemy>().player = player;
            _lbat.GetComponent<Enemy>().gameManager = gameManager;
            _lbat.SetActive(false);
            for (int i = 0; i < _dog.Length; i++)
            {
                _dog[i] = Instantiate(dogPrefab);
                _dog[i].GetComponent<Enemy>().player = player;
                _dog[i].GetComponent<Enemy>().gameManager = gameManager;
                _dog[i].SetActive(false);
            }
            _ldog = Instantiate(ldogPrefab);
            _ldog.GetComponent<Enemy>().player = player;
            _ldog.GetComponent<Enemy>().gameManager = gameManager;
            _ldog.SetActive(false);
            for (int i = 0; i < _cat.Length; i++)
            {
                _cat[i] = Instantiate(catPrefab);
                _cat[i].GetComponent<Enemy>().player = player;
                _cat[i].GetComponent<Enemy>().gameManager = gameManager;
                _cat[i].SetActive(false);
            }
            _lcat = Instantiate(lcatPrefab);
            _lcat.GetComponent<Enemy>().player = player;
            _lcat.GetComponent<Enemy>().gameManager = gameManager;
            _lcat.SetActive(false);
            for (int i = 0; i < _gorani.Length; i++)
            {
                _gorani[i] = Instantiate(goraniPrefab);
                _gorani[i].GetComponent<Enemy>().player = player;
                _gorani[i].GetComponent<Enemy>().gameManager = gameManager;
                _gorani[i].SetActive(false);
            }
            _lgorani = Instantiate(lgoraniPrefab);
            _lgorani.GetComponent<Enemy>().player = player;
            _lgorani.GetComponent<Enemy>().gameManager = gameManager;
            _lgorani.SetActive(false);
            for (int i = 0; i < _fox.Length; i++)
            {
                _fox[i] = Instantiate(foxPrefab);
                _fox[i].GetComponent<Enemy>().player = player;
                _fox[i].GetComponent<Enemy>().gameManager = gameManager;
                _fox[i].SetActive(false);
            }
            _lfox = Instantiate(lfoxPrefab);
            _lfox.GetComponent<Enemy>().player = player;
            _lfox.GetComponent<Enemy>().gameManager = gameManager;
            _lfox.SetActive(false);
            for (int i = 0; i < _arrow.Length; i++)
            {
                _arrow[i] = Instantiate(arrowPrefab);
                _arrow[i].GetComponent<Projectile>().gameManager = gameManager;
                _arrow[i].SetActive(false);
            }
            for (int i = 0; i < _pyochang.Length; i++)
            {
                _pyochang[i] = Instantiate(pyochangPrefab);
                _pyochang[i].GetComponent<Projectile>().gameManager = gameManager;
                _pyochang[i].SetActive(false);
            }
            for (int i = 0; i < _poop.Length; i++)
            {
                _poop[i] = Instantiate(poopPrefab);
                _poop[i].GetComponent<Projectile>().gameManager = gameManager;
                _poop[i].SetActive(false);
            }
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
                case "lmonkey":
                    _lmonkey.SetActive(true);
                    _lmonkey.transform.position = pos;
                    return _lmonkey;
                case "lpigeon":
                    _lpigeon.SetActive(true);
                    _lpigeon.transform.position = pos;
                    return _lpigeon;
                case "lbat":
                    _lbat.SetActive(true);
                    _lbat.transform.position = pos;
                    return _lbat;
                case "ldog":
                    _ldog.SetActive(true);
                    _ldog.transform.position = pos;
                    return _ldog;
                case "lcat":
                    _lcat.SetActive(true);
                    _lcat.transform.position = pos;
                    return _lcat;
                case "lgorani":
                    _lgorani.SetActive(true);
                    _lgorani.transform.position = pos;
                    return _lgorani;
                case "lfox":
                    _lfox.SetActive(true);
                    _lfox.transform.position = pos;
                    return _lfox;
                case "arrow":
                    _targetPool = _fox;
                    break;
                case "pyochang":
                    _targetPool = _pyochang;
                    break;
                case "poop":
                    _targetPool = _poop;
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
            _lmonkey.SetActive(false);
            _lbat.SetActive(false);
            _lcat.SetActive(false);
            _ldog.SetActive(false);
            _lfox.SetActive(false);
            _lgorani.SetActive(false);
            _lpigeon.SetActive(false);
        }
    }
}

