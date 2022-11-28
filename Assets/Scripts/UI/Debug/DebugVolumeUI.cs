using System;
using SharedLibs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameHeaven.Temp
{
    public class DebugVolumeUI : MonoBehaviour
    {
        [SerializeField] private Slider sfxSlider;
        [SerializeField] private Slider bgmSlider;
        
        [SerializeField] private TextMeshProUGUI sfxVolumeText;
        [SerializeField] private TextMeshProUGUI bgmVolumeText;

        private void Awake()
        {
            sfxSlider.onValueChanged.AddListener((vol => sfxVolumeText.text = Math.Round(vol, 1).ToString()));
            bgmSlider.onValueChanged.AddListener((vol => bgmVolumeText.text = Math.Round(vol, 1).ToString()));
        }

        private void Start()
        {
            sfxSlider.value = SoundManager.Instance.SFXVolume;
            bgmSlider.value = SoundManager.Instance.BGMVolume;
        }

        public void SetSFXSliderValue(float val)
        {
            SoundManager.Instance.SetSFXVolume(val);
        }
        public void SetBGMSliderValue(float val)
        {
            SoundManager.Instance.SetBGMVolume(val);
        }
    }
}