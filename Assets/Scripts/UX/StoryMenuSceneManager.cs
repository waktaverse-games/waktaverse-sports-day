using System;
using System.Collections;
using System.Collections.Generic;
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

        [SerializeField] [ReadOnly] private int curIndex;
        [SerializeField] [ReadOnly] private int storyProgressIndex = -1;
        private const string StoryProgressPrefsKey = "Game.Root.Story.Progress.Index";

        [SerializeField] private GameObject gameDescObj;
        [SerializeField] private TextMeshProUGUI gameNameText;
        [SerializeField] private Image gameImage;
        [SerializeField] private TextMeshProUGUI gameDescText;

        [SerializeField] [ReadOnly] private string selectEngName;

        [SerializeField] AudioClip buttonSound;
        
        private void Awake()
        {
            // DEBUG
            storyProgressIndex = -1;
            
            // storyProgressIndex = PlayerPrefs.HasKey(StoryProgressPrefsKey)
            //     ? PlayerPrefs.GetInt(StoryProgressPrefsKey)
            //     : -1;

            ResultSceneManager.SetResultType(true);
            
            AudioSource.PlayClipAtPoint(buttonSound, Vector3.zero);
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
            curIndex = index;
            
            gameNameText.text = gameNames[index];
            gameImage.sprite = minigameSprites[index];

            gameDescObj.SetActive(true);
            
            selectEngName = engNames[index];
            CharacterManager.Instance.SetCharacter(charTypes[index]);
        }

        public void StartGame()
        {
            if (curIndex > storyProgressIndex)
            {
                UpdateStoryProgress();
                DialogueSceneManager.LoadDialogue(selectEngName);
            }
            else
            {
                LoadingSceneManager.LoadScene(engNames[curIndex], minigameSprites[curIndex]);
            }
        }

        private void UpdateStoryProgress()
        {
            storyProgressIndex = curIndex > storyProgressIndex ? curIndex : storyProgressIndex;
            PlayerPrefs.SetInt(StoryProgressPrefsKey, storyProgressIndex);
        }
    }
}
