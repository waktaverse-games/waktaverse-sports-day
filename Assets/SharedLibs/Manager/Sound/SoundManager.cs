using System;
using System.Collections;
using System.Collections.Generic;
using SharedLibs;
using UnityEngine;

namespace SharedLibs
{
    public class SoundManager : MonoSingleton<SoundManager>
    {
        [SerializeField] private float sfxVolume;
        [SerializeField] private float bgmVolume;

        private const string SFXVolumeValueKey = "Game.Root.Volume.SFX";
        private const string BGMVolumeValueKey = "Game.Root.Volume.BGM";

        public event Action<float> OnSFXVolumeChanged;
        public event Action<float> OnBGMVolumeChanged;

        public float SFXVolume => sfxVolume;
        public float BGMVolume => bgmVolume;

        public override void Init()
        {
            sfxVolume = SetSFXVolume(PlayerPrefs.HasKey(SFXVolumeValueKey) ? PlayerPrefs.GetFloat(SFXVolumeValueKey) : 1.0f);
            bgmVolume = SetBGMVolume(PlayerPrefs.HasKey(BGMVolumeValueKey) ? PlayerPrefs.GetFloat(BGMVolumeValueKey) : 1.0f);
        }

        protected override void SingletonDestroy()
        {
            PlayerPrefs.Save();
        }

        public float SetSFXVolume(float volume)
        {
            var modVol = Mathf.Clamp(volume, 0.0f, 1.0f);
            sfxVolume = modVol;
            OnSFXVolumeChanged?.Invoke(modVol);
            PlayerPrefs.SetFloat(SFXVolumeValueKey, modVol);
            return modVol;
        }
        public float SetBGMVolume(float volume)
        {
            var modVol = Mathf.Clamp(volume, 0.0f, 1.0f);
            bgmVolume = modVol;
            OnBGMVolumeChanged?.Invoke(modVol);
            PlayerPrefs.SetFloat(BGMVolumeValueKey, modVol);
            return modVol;
        }
    }
}