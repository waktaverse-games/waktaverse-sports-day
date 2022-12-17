using System;
using System.Collections;
using System.Collections.Generic;
using GameHeaven.Root;
using SharedLibs;
using SharedLibs.Character;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GameHeaven.UIUX
{
    public class StoryMenuSceneManager : MonoBehaviour
    {
        [SerializeField] private Sprite[] minigameSprites, descriptionSprites;
        [SerializeField] private string[] gameNames, engNames;
        [SerializeField] private CharacterType[] charTypes;

        [SerializeField] private GameObject[] lockScreens;

        [SerializeField] private GameObject gameDescObj;
        [SerializeField] private TextMeshProUGUI gameNameText;
        [SerializeField] private Image gameImage, gameDescImage;

        [SerializeField] [ReadOnly] private string selectEngName;

        private int curSeletedStage = 0;
        private bool isDescMenuOpen = false;

        private void Awake()
        {
            GameManager.SetGameMode(GameMode.StoryMode);
        }

        private void Start()
        {
            UISoundManager.Instance.PlayButtonSFX1();

            curSeletedStage = StoryManager.Instance.SelectStoryIndex;
            
            transform.GetChild(3).position -= new Vector3(curSeletedStage * 1610, 0, 0);

            for (int i = 0; i <= StoryManager.Instance.UnlockProgress; i++)
            {
                lockScreens[Mathf.Clamp(i, 0, lockScreens.Length - 1)].SetActive(false);
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (SettingUI.IsSettingScene) return;
                if (isDescMenuOpen)
                {
                    UISoundManager.Instance.PlayButtonSFX1();
                    transform.GetChild(1).gameObject.SetActive(true);
                    transform.GetChild(2).gameObject.SetActive(true);
                    gameDescObj.GetComponent<Animator>().SetTrigger("Off");
                    isDescMenuOpen = false;
                }
                else
                {
                    SceneLoader.LoadSceneAsync("ModeSelectScene");
                }
            }
        }

        private void OnDestroy()
        {
            PlayerPrefs.Save();
        }

        public void OpenDescWindow(int index)
        {
            UISoundManager.Instance.PlayButtonSFX1();
            StoryManager.Instance.SetCurrentIndex(index);
            
            gameNameText.text = gameNames[index];
            gameImage.sprite = minigameSprites[index];
            gameDescImage.sprite = descriptionSprites[index];

            transform.GetChild(1).gameObject.SetActive(false);
            transform.GetChild(2).gameObject.SetActive(false);
            gameDescObj.GetComponent<Animator>().SetTrigger("On");
            isDescMenuOpen = true;
            
            selectEngName = engNames[index];
            CharacterManager.Instance.SetCharacter(charTypes[index]);
        }

        public void ClickArrowButton(int x)
        {
            UISoundManager.Instance.PlayButtonSFX2();
            if (x > 0 && 0 < curSeletedStage)
            {
                curSeletedStage--;
                StartCoroutine(MoveX(transform.GetChild(3), x));
            }
            else if (curSeletedStage < 9 && x < 0)
            {
                curSeletedStage++;
                StartCoroutine(MoveX(transform.GetChild(3), x));
            }
        }

        IEnumerator MoveX(Transform obj, int x)
        {
            WaitForSeconds wait = new WaitForSeconds(0.005f);

            for (int i = 0; i < 10; i++)
            {
                obj.transform.position += new Vector3(x / 10, 0, 0);
                yield return wait;
            }
        }

        public void StartGame()
        {
            UISoundManager.Instance.PlayButtonSFX1();
            var curIdx = StoryManager.Instance.SelectStoryIndex;
            
            if (curIdx > StoryManager.Instance.UnlockProgress) return;

            DialogueSceneManager.LoadDialogue(selectEngName);
        }
    }
}
