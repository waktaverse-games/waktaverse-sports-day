using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.JumpGame
{
    public class SoundManager : MonoBehaviour
    {
        [SerializeField] AudioSource bgm;
        [SerializeField] AudioSource gameOverSFX;
        [SerializeField] AudioSource itemSFX;
        [SerializeField] AudioSource jumpSFX;
        [SerializeField] AudioSource ropeSFX;
        [SerializeField] AudioSource rusukSFX;
        [SerializeField] AudioSource hitSFX;
        #region Singleton
        public static SoundManager Instance = null;
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                if (Instance != this)
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
        public void PlayGameOverSound() => gameOverSFX.Play();
        public void PlayItemSound() => itemSFX.Play();
        public void PlayJumpSound() => jumpSFX.Play();
        public void PlayRopeSound() => ropeSFX.Play();
        public void PlayHitSound() => hitSFX.Play();
        public void TurnOffBGM() => bgm.Stop();
        public void PlayRusukSound()
        {
            itemSFX.Play();
            rusukSFX.Play();
        }
        public void SetBGMVolume(float volume)
        {
            bgm.volume = volume;
        }
        public void SetSFXVolume(float volume)
        {
            itemSFX.volume = volume;
            jumpSFX.volume = volume;
            ropeSFX.volume = volume;
            rusukSFX.volume = volume;
            hitSFX.volume = volume;
            gameOverSFX.volume = volume;
        }
    }
}