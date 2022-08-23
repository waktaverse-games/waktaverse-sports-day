using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public Player playerScript;
    public ObjectManager objectManager;
    public GameObject spawnPoint;
    public GameObject scoreT;
    public GameObject stageT;
    public GameObject startText;
    public GameObject endText;
    public GameObject button;
    public float jumpPower;

    private int _score = 0;
    private int _stage = 0;
    private Vector3 _spawnPos;
    private TextMeshProUGUI _scoreText;
    private TextMeshProUGUI _stageText;
    private List<string>[] _stageStrings = new List<string>[4];

    // Start is called before the first frame update
    void Start()
    {
        _spawnPos = spawnPoint.transform.position;
        playerScript.jumpPower = jumpPower;
        _scoreText = scoreT.GetComponent<TextMeshProUGUI>();
        _stageText = stageT.GetComponent<TextMeshProUGUI>();
        _stageStrings = new List<string>[4];
        _stageStrings[0] = new List<string>();
        _stageStrings[1] = new List<string>(){"egi", "bat"};
        _stageStrings[2] = new List<string>(){"egi", "bat", "bidul", "dog", "jupok"};
        _stageStrings[3] = new List<string>(){"egi", "bat", "bidul", "dog", "gorani", "bug", "jupok"};
        GameSet();
    }

    // Update is called once per frame

    public void GameSet()
    {
        endText.SetActive(false);
        button.SetActive(false);
        startText.SetActive(true);
        _score = 0;
        _stage = 1;
        Time.timeScale = 1;
        _stageText.SetText("Lv 1");
        _scoreText.SetText(_score.ToString());
        Invoke("GameStart", 2f);
    }

    public void GameStart()
    {
        startText.SetActive(false);
        StartCoroutine(UpgradeStage(45));
        StartCoroutine(StageSpawn(0.1f));
    }

    public void GameOver()
    {
        Time.timeScale = 0;
        StopAllCoroutines();
        objectManager.FailGame();
        endText.SetActive(true);
        button.SetActive(true);
    }
    
    public void AddScore(int addScore)
    {
        _score += addScore;
        _scoreText.text = _score.ToString();
    }

    IEnumerator UpgradeStage(float time)
    {
        yield return new WaitForSeconds(time);
        _stage++;
        _stageText.text = "Lv " + _stage;
        switch (_stage)
        {
            case 2:
                StartCoroutine(UpgradeStage(45));
                break;
        }
    }

    IEnumerator StageSpawn(float time)
    {
        yield return new WaitForSeconds(time);
        int rnd = Random.Range(0, _stageStrings[_stage].Count);
        objectManager.MakeObject(_stageStrings[_stage][rnd], _spawnPos);
        StartCoroutine(StageSpawn(6f));
    }
}
