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
        [SerializeField] private Sprite[] minigameSprites;
        [SerializeField] private string[] gameNames, engNames;
        [SerializeField] private CharacterType[] charTypes;

        [SerializeField] private GameObject[] lockScreens;

        [SerializeField] private GameObject gameDescObj;
        [SerializeField] private TextMeshProUGUI gameNameText;
        [SerializeField] private Image gameImage;
        [SerializeField] private TextMeshProUGUI gameDescText;

        [SerializeField] [ReadOnly] private string selectEngName;

        private int curSeletedStage = 0;
        private bool isDescMenuOpen = false;

        private void Awake()
        {
            GameManager.SetGameMode(GameMode.StoryMode);
        }

        private void Start()
        {
            for (int i = 0; i <= StoryManager.Instance.UnlockProgress; i++)
            {
                lockScreens[Mathf.Clamp(i, 0, lockScreens.Length - 1)].SetActive(false);
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (isDescMenuOpen)
                {
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
            StoryManager.Instance.SetCurrentIndex(index);
            
            gameNameText.text = gameNames[index];
            gameImage.sprite = minigameSprites[index];

            gameDescObj.GetComponent<Animator>().SetTrigger("On");
            isDescMenuOpen = true;
            
            selectEngName = engNames[index];
            CharacterManager.Instance.SetCharacter(charTypes[index]);
        }

        public void ClickArrowButton(int x)
        {
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
            WaitForSeconds wait = new WaitForSeconds(0.01f);

            for (int i = 0; i < 10; i++)
            {
                obj.transform.position += new Vector3(x / 10, 0, 0);
                yield return wait;
            }
        }

        public void StartGame()
        {
            var curIdx = StoryManager.Instance.SelectStoryIndex;
            
            if (curIdx > StoryManager.Instance.UnlockProgress) return;

            DialogueSceneManager.LoadDialogue(selectEngName);
        }
    }
}
