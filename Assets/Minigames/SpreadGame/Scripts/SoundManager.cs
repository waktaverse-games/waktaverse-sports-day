using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.SpreadGame
{
    public class SoundManager : MonoBehaviour
    {
        [SerializeField] AudioSource bgm, bulletSFX, slashSFX, scoreItemSFX, upgradeItemSFX, bombSFX, GgangSFX, MonkeySFX, RaNiSFX, PokJuSFX;

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
        public void PlaybulletSound() => bulletSFX.Play();
        public void PlaySlashSound() => slashSFX.Play();
        public void PlayScoreItemSound() => scoreItemSFX.Play();
        public void PlayUpgradeItemSound() => upgradeItemSFX.Play();
        public void PlayBomgSound() => bombSFX.Play();
        public void PlayGgangSound() => GgangSFX.Play();
        public void PlayMonkeySound() => MonkeySFX.Play();
        public void PlayRaNiSound() => RaNiSFX.Play();
        public void PlayPokJuSound() => PokJuSFX.Play();

        public void SetBGMVolume(float volume)
        {
            bgm.volume = volume;
        }
        public void SetSFXVolume(float volume)
        {
            bulletSFX.volume = volume;
            slashSFX.volume = volume;
            scoreItemSFX.volume = volume;
            upgradeItemSFX.volume = volume;
            bombSFX.volume = volume;
            GgangSFX.volume = volume;
            MonkeySFX.volume = volume;
            RaNiSFX.volume = volume;
            PokJuSFX.volume = volume;
        }
    }
}