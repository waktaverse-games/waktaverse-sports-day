using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.StickyGame
{
    public class SoundManager : MonoBehaviour
    {
        [SerializeField] AudioSource bgm, acquireSFX, associateSFX, cutSFX, spaceSFX, gameoverSFX;

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
        public void PlayAcquireSound() => acquireSFX.Play();
        public void PlayAssociateSound() => associateSFX.Play();
        public void PlayCutSound() => cutSFX.Play();
        public void PlaySpaceSound() => spaceSFX.Play();
        public void PlayGameOverSound() => gameoverSFX.Play();

        public void SetBGMVolume(float volume)
        {
            bgm.volume = volume;
        }
        public void SetSFXVolume(float volume)
        {
            acquireSFX.volume = volume;
            associateSFX.volume = volume;
            cutSFX.volume = volume;
            spaceSFX.volume = volume;
            gameoverSFX.volume = volume;
        }
    }
}