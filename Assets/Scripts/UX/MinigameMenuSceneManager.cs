using System;
using System.Collections;
using System.Collections.Generic;
using GameHeaven.Root;
using PlayFab.ClientModels;
using SharedLibs;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using TMPro;
using SharedLibs.Character;
using SharedLibs.Score;

namespace GameHeaven.UIUX
{
    public class MinigameMenuSceneManager : MonoBehaviour
    {
        [SerializeField] RuntimeAnimatorController[] charControllers;
        [SerializeField] Sprite[] minigameSprites;
        [SerializeField] string[] charNames, gameNames, engNames;
        [SerializeField] private MinigameType[] types;
        [SerializeField] private GameObject pieces;
        [SerializeField] Sprite[] descriptionSprites;
        [SerializeField] private string[] characterDescription;
        [SerializeField] private VideoClip[] descriptionVideos;

        CharacterManager characterManager;
        private Stack<int> prevMenues;
        private bool enableClick;
        private int curGame, curChar, curPuzzle;

        [SerializeField] private PuzzleManager puzzleManager;
        [SerializeField] private RankingUI rankingUI;

        private void Awake()
        {
            Time.timeScale = 1.0f;
            GameManager.SetGameMode(GameMode.MinigameMode);
            
            PlayFabManager.Instance.UpdateLeaderBoard();
            
            prevMenues = new Stack<int>();
            prevMenues.Push(1);
            curGame = GameDatabase.Instance.lastSelectedGame;
            curChar = GameDatabase.Instance.lastSelectedCharacter;
            enableClick = true;
            transform.GetChild(1).GetComponent<Animator>().SetTrigger("On");
            characterManager = FindObjectOfType<CharacterManager>();
        }

        private void OnDisable()
        {
            GameDatabase.Instance.lastSelectedGame = curGame;
            GameDatabase.Instance.lastSelectedCharacter = curChar;
        }

        private void Start()
        {
            for (int i = 0; i < 7; i++)
            {
                characterDescription[i] = characterDescription[i].Replace("\\n", "\n");
            }

            transform.GetChild(2).GetChild(1).GetChild(1).GetChild(1).localPosition -= new Vector3(400 * curGame, 0, 0);
            transform.GetChild(3).GetChild(2).GetChild(2).GetChild(0).localPosition -= new Vector3(330 * curChar, 0, 0);

            GameInfoChange();
            CharacterInfoChange();

            UISoundManager.Instance.PlayButtonSFX2();
        }

        private void GameInfoChange() // Change game Info about the curGame
        {
            rankingUI.SetRankingUI(types[curGame]);
            transform.GetChild(2).GetChild(6).GetChild(0).GetChild(0).GetComponent<Image>().sprite = minigameSprites[curGame];

            if (curGame == 0) transform.GetChild(2).GetChild(4).gameObject.SetActive(false);
            else if (curGame == 9) transform.GetChild(2).GetChild(5).gameObject.SetActive(false);
            else
            {
                transform.GetChild(2).GetChild(4).gameObject.SetActive(true);
                transform.GetChild(2).GetChild(5).gameObject.SetActive(true);
            }

            var goals = ScoreManager.Instance.GetRewardGoals(types[curGame]);
            for (int i = 0; i < 3; i++)
            {
                pieces.transform.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = goals[i] + "점 이상";
            }

            for (int i = 0; i < ScoreManager.Instance.GetGameAchievement(types[curGame]); i++)
            {
                pieces.transform.GetChild(i).GetChild(0).GetComponent<Image>().color = new Color(84 / 255f, 204 / 255f, 61 / 255f);
            }
            for (int i = ScoreManager.Instance.GetGameAchievement(types[curGame]); i < 3; i++)
            {
                pieces.transform.GetChild(i).GetChild(0).GetComponent<Image>().color = Color.black;
            }
        }

        private void CharacterInfoChange()
        {
            if (curChar == 0) transform.GetChild(3).GetChild(3).gameObject.SetActive(false);
            else if (curChar == 6) transform.GetChild(3).GetChild(4).gameObject.SetActive(false);
            else
            {
                transform.GetChild(3).GetChild(3).gameObject.SetActive(true);
                transform.GetChild(3).GetChild(4).gameObject.SetActive(true);
            }

            transform.GetChild(3).GetChild(0).GetChild(1).GetComponent<Animator>().runtimeAnimatorController = charControllers[curChar];
            transform.GetChild(3).GetChild(0).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = charNames[curChar];
            transform.GetChild(3).GetChild(1).GetChild(2).GetComponent<TextMeshProUGUI>().text = characterDescription[curChar];
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) && enableClick)
            {
                if (SettingUI.IsSettingScene) return;
                if (prevMenues.Count < 2)
                {
                    SceneManager.LoadScene("ModeSelectScene");
                }
                else
                {
                    enableClick = false;
                    UISoundManager.Instance.PlayButtonSFX2();
                    transform.GetChild(prevMenues.Pop()).GetComponent<Animator>().SetTrigger("Off");
                    transform.GetChild(prevMenues.Peek()).GetComponent<Animator>().SetTrigger("On");
                    Invoke("SetEnableClick", 0.2f);
                }
            }
        }
        
        private void SetEnableClick()
        {
            enableClick = true;
        }
        IEnumerator ArrowClick(Transform transform, float x)
        {
            WaitForSeconds wait = new WaitForSeconds(0.01f);

            for (int i = 0; i < 10; i++)
            {
                transform.position += new Vector3(x / 10, 0, 0);
                yield return wait;
            }
        }
        public void ChooseGameClick()
        {
            if (!enableClick) return;
            enableClick = false;
            UISoundManager.Instance.PlayButtonSFX2();
            transform.GetChild(1).GetComponent<Animator>().SetTrigger("Off");
            transform.GetChild(2).GetComponent<Animator>().SetTrigger("On");
            if (prevMenues.Peek() != 2) prevMenues.Push(2);
            Invoke("SetEnableClick", 0.2f);
        }
        public void CheckPuzzleClick()
        {
            if (!enableClick) return;
            enableClick = false;
            UISoundManager.Instance.PlayButtonSFX2();
            transform.GetChild(1).GetComponent<Animator>().SetTrigger("Off");
            transform.GetChild(4).GetComponent<Animator>().SetTrigger("On");
            if (prevMenues.Peek() != 4) prevMenues.Push(4);
            Invoke("SetEnableClick", 0.2f);
        }
        public void ChooseButtonClick()
        {
            if (!enableClick) return;
            enableClick = false;
            transform.GetChild(5).GetChild(0).GetChild(1).GetChild(0).GetComponent<Image>().sprite = minigameSprites[curGame];
            transform.GetChild(5).GetChild(0).GetChild(2).GetComponent<Image>().sprite = descriptionSprites[curGame];
            transform.GetChild(5).GetChild(0).GetChild(5).GetComponent<TextMeshProUGUI>().text = gameNames[curGame];
            transform.GetChild(5).GetChild(1).GetComponent<VideoPlayer>().clip = descriptionVideos[curGame];
            // 게임 영상
            UISoundManager.Instance.PlayButtonSFX2();
            transform.GetChild(2).GetComponent<Animator>().SetTrigger("Off");
            transform.GetChild(5).GetComponent<Animator>().SetTrigger("On");
            if (prevMenues.Peek() != 5) prevMenues.Push(5);
            Invoke("SetEnableClick", 0.2f);
        }
        public void RankingButtonClick()
        {
            if (!enableClick) return;
            enableClick = false;
            UISoundManager.Instance.PlayButtonSFX2();
            transform.GetChild(2).GetComponent<Animator>().SetTrigger("Off");
            transform.GetChild(6).GetComponent<Animator>().SetTrigger("On");
            var tr = transform.GetChild(6).GetChild(1).GetChild(0).GetChild(0).transform;
            tr.position = new Vector3(tr.position.x, -1000, tr.position.z);
            rankingUI.SetRankingBoard(types[curGame]);
            if (prevMenues.Peek() != 6) prevMenues.Push(6);
            Invoke("SetEnableClick", 0.2f);
        }
        public void StartButtonClick()
        {
            if (!enableClick) return;
            enableClick = false;
            UISoundManager.Instance.PlayButtonSFX2();
            transform.GetChild(5).GetComponent<Animator>().SetTrigger("Off");
            transform.GetChild(3).GetComponent<Animator>().SetTrigger("On");
            if (prevMenues.Peek() != 3) prevMenues.Push(3);
            Invoke("SetEnableClick", 0.2f);
        }
        public void GameRightClick()
        {
            if (!enableClick) return;
            enableClick = false;
            UISoundManager.Instance.PlayButtonSFX1();
            if (curGame < 9)
            {
                StartCoroutine(ArrowClick(transform.GetChild(2).GetChild(1).GetChild(1).GetChild(1), -400));
                curGame++;
                GameInfoChange();
            }
            Invoke("SetEnableClick", 0.1f);
        }

        public void GameLeftClick()
        {
            if (!enableClick) return;
            enableClick = false;
            UISoundManager.Instance.PlayButtonSFX1();
            if (curGame > 0)
            {
                StartCoroutine(ArrowClick(transform.GetChild(2).GetChild(1).GetChild(1).GetChild(1), 400));
                curGame--;
                GameInfoChange();
            }
            Invoke("SetEnableClick", 0.1f);
        }
        public void CharRightClick()
        {
            if (!enableClick) return;
            enableClick = false;
            UISoundManager.Instance.PlayButtonSFX1();
            if (curChar < 6)
            {
                StartCoroutine(ArrowClick(transform.GetChild(3).GetChild(2).GetChild(2).GetChild(0), -330));
                curChar++;
                CharacterInfoChange();
            }
            Invoke("SetEnableClick", 0.1f);
        }
        public void CharLeftClick()
        {
            if (!enableClick) return;
            enableClick = false;
            UISoundManager.Instance.PlayButtonSFX1();
            if (curChar > 0)
            {
                StartCoroutine(ArrowClick(transform.GetChild(3).GetChild(2).GetChild(2).GetChild(0), 330));
                curChar--;
                CharacterInfoChange();
            }
            Invoke("SetEnableClick", 0.1f);
        }
        public void PuzzleRightClick()
        {
            if (!enableClick) return;
            enableClick = false;
            UISoundManager.Instance.PlayButtonSFX1();
            if (curPuzzle < 4)
            {
                curPuzzle++;
                transform.GetChild(4).GetChild(2).position += new Vector3(105, 0, 0);
                StartCoroutine(ArrowClick(transform.GetChild(4).GetChild(0).GetChild(0), -2000));
            }
            Invoke("SetEnableClick", 0.1f);
        }
        public void PuzzleLeftClick()
        {
            if (!enableClick) return;
            enableClick = false;
            UISoundManager.Instance.PlayButtonSFX1();
            if (curPuzzle > 0)
            {
                curPuzzle--;
                transform.GetChild(4).GetChild(2).position += new Vector3(-105, 0, 0);
                StartCoroutine(ArrowClick(transform.GetChild(4).GetChild(0).GetChild(0), 2000));
            }
            Invoke("SetEnableClick", 0.1f);
        }
        public void GameStartButtonClick()
        {
            if (!enableClick) return;
            enableClick = false;
            characterManager.SetCharacter((SharedLibs.CharacterType)curChar);
            FindObjectOfType<UIBGM>().OffUIBGM();
            LoadingSceneManager.LoadScene(engNames[curGame], minigameSprites[curGame]);
            Invoke("SetEnableClick", 0.2f);
        }
        public void PiecePuzzle(int puzzleIndex)
        {
            if (!puzzleManager.PiecePuzzle(puzzleIndex))
            {
                Debug.Log("퍼즐이 꽉 찼거나 놓을 퍼즐이 없습니다");
            }
        }
    }
}