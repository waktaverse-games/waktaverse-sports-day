using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public GameObject egiPrefab;
    public GameObject bidulPrefab;
    public GameObject batPrefab;
    public GameObject dogPrefab;
    public GameObject bugPrefab;
    public GameObject goraniPrefab;
    public GameObject jupokPrefab;

    private GameObject[] _egi;
    private GameObject[] _bidul;
    private GameObject[] _bat;
    private GameObject[] _dog;
    private GameObject[] _bug;
    private GameObject[] _gorani;
    private GameObject[] _jupok;
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
        Generate();
    }

    void Generate()
    {
        for (int i = 0; i < _egi.Length; i++)
        {
            _egi[i] = Instantiate(egiPrefab);
            _egi[i].SetActive(false);
        }
        for (int i = 0; i < _bidul.Length; i++)
        {
            _bidul[i] = Instantiate(bidulPrefab);
            _bidul[i].SetActive(false);
        }
        for (int i = 0; i < _bat.Length; i++)
        {
            _bat[i] = Instantiate(batPrefab);
            _bat[i].SetActive(false);
        }
        for (int i = 0; i < _dog.Length; i++)
        {
            _dog[i] = Instantiate(dogPrefab);
            _dog[i].SetActive(false);
        }
        for (int i = 0; i < _bug.Length; i++)
        {
            _bug[i] = Instantiate(bugPrefab);
            _bug[i].SetActive(false);
        }
        for (int i = 0; i < _gorani.Length; i++)
        {
            _gorani[i] = Instantiate(goraniPrefab);
            _gorani[i].SetActive(false);
        }
        for (int i = 0; i < _jupok.Length; i++)
        {
            _jupok[i] = Instantiate(jupokPrefab);
            _jupok[i].SetActive(false);
        }
    }

    public GameObject MakeObject(string type)
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
        }

        for (int i = 0; i < _targetPool.Length; i++)
        {
            if (!_targetPool[i].activeSelf)
            {
                _targetPool[i].SetActive(true);
                return _targetPool[i];
            }
        }

        return _targetPool[9];
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
    }
}
