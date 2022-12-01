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

        [SerializeField] AudioClip buttonSound;

        private void Awake()
        {
            GameManager.SetGameMode(GameMode.StoryMode);
            
            AudioSource.PlayClipAtPoint(buttonSound, Vector3.zero);
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
                SceneLoader.LoadSceneAsync("ModeSelectScene");
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

            gameDescObj.SetActive(true);
            
            selectEngName = engNames[index];
            CharacterManager.Instance.SetCharacter(charTypes[index]);
        }

        public void StartGame()
        {
            var curIdx = StoryManager.Instance.SelectStoryIndex;
            
            if (curIdx > StoryManager.Instance.UnlockProgress) return;
            
            if (curIdx > StoryManager.Instance.ViewProgress)
            {
                StoryManager.Instance.UpdateViewProgressLatest();
                
                DialogueSceneManager.LoadDialogue(selectEngName);
            }
            else
            {
                LoadingSceneManager.LoadScene(engNames[curIdx], minigameSprites[curIdx]);
            }
        }
    }
}
