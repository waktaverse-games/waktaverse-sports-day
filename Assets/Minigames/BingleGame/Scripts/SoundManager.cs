using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.BingleGame
{
    public class SoundManager : MonoBehaviour
    {
        [SerializeField] AudioSource bgm;
        [SerializeField] AudioSource itemSFX;
        [SerializeField] AudioSource turnSFX;
        [SerializeField] AudioSource crashSFX;

        #region Singleton
        public static SoundManager instance = null;
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                if (instance != this)
                    Destroy(this.gameObject);
            }
        }
        #endregion
        void Start()
        {
            SetBGMVolume(SharedLibs.SoundManager.Instance.BGMVolume);
            SetSFXVolume(SharedLibs.SoundManager.Instance.SFXVolume);
        }
        private void OnEnable()
        {
            SharedLibs.SoundManager.Instance.OnBGMVolumeChanged += SetBGMVolume;
            SharedLibs.SoundManager.Instance.OnSFXVolumeChanged += SetSFXVolume;
        }
        private void OnDisable()
        {
            SharedLibs.SoundManager.Instance.OnBGMVolumeChanged -= SetBGMVolume;
            SharedLibs.SoundManager.Instance.OnSFXVolumeChanged -= SetSFXVolume;
        }
        public void PlayBGM() => bgm.Play();
        public void PlayItemSound() => itemSFX.Play();
        public void PlayTurnSound() => turnSFX.Play();
        public void PlayCrashSound() => crashSFX.Play();
        public void SetBGMVolume(float volume)
        {
            bgm.volume = volume;
        }
        public void SetSFXVolume(float volume)
        {
            itemSFX.volume = volume;
            turnSFX.volume = volume;
            crashSFX.volume = volume;
        }
    }
}