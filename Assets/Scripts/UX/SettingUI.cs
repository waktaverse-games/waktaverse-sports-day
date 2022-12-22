using System;
using GameHeaven.Root;
using SharedLibs;
using UnityEngine;
using UnityEngine.UI;

namespace GameHeaven.UIUX
{
    public class SettingUI : MonoBehaviour
    {
        [SerializeField] private Slider bgmVolumeSlider;
        [SerializeField] private Slider sfxVolumeSlider;
        
        [SerializeField] private Toggle fullScreenToggle;
        
        public static bool IsSettingScene = false;

        private void OnEnable()
        {
            IsSettingScene = true;
            
            fullScreenToggle.isOn = Screen.fullScreen;

            bgmVolumeSlider.value = SoundManager.Instance.BGMVolume;
            sfxVolumeSlider.value = SoundManager.Instance.SFXVolume;
        }

        private void OnDisable()
        {
            IsSettingScene = false;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                CloseSetting();
            }

            if (Input.GetKey(KeyCode.W))
            {
                if (Input.GetKey(KeyCode.A))
                {
                    if (Input.GetKey(KeyCode.K))
                    {
                        if (Input.GetKeyDown(KeyCode.Space))
                        {
                            ResetResolution();
                        }
                    }
                }
            }
        }

        public static void OpenSetting()
        {
            SceneLoader.AddSceneAsync("SettingScene");
        }

        public static void CloseSetting()
        {
            SceneLoader.UnloadSceneAsync("SettingScene");
        }

        public void SaveSettings()
        {
            GameDatabase.Instance.SaveData();
        }
        
        public void ResetSettings()
        {
            GameDatabase.Instance.ResetData();
            
            var volumes = SoundManager.Instance.ResetVolume();
            bgmVolumeSlider.value = volumes.bgmVolume;
            sfxVolumeSlider.value = volumes.sfxVolume;

            SetFullscreen(false);
        }

        public void ResetResolution()
        {
            Screen.SetResolution(1920, 1080, false);
        }
        
        public void SetBGMVolume(float volume)
        {
            SoundManager.Instance.SetBGMVolume(volume);
        }
        
        public void SetSFXVolume(float volume)
        {
            SoundManager.Instance.SetSFXVolume(volume);
        }
        
        public void SetFullscreen(bool isFullscreen)
        {
            Screen.fullScreen = isFullscreen;
        }

        public void ShowCredit()
        {
            
        }

        public void ResetStoryProgress()
        {
            StoryManager.Instance.ResetStoryProgress();
        }
    }
}