using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.JumpGame
{
    public class SoundManager : MonoBehaviour
    {
        [SerializeField] AudioSource bgm;
        [SerializeField] AudioSource itemSFX;
        [SerializeField] AudioSource jumpSFX;
        [SerializeField] AudioSource ropeSFX;
        [SerializeField] AudioSource rusukSFX;
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
        public void PlayBGM() => bgm.Play();
        public void PlayItemSound() => itemSFX.Play();
        public void PlayJumpSound() => jumpSFX.Play();
        public void PlayRopeSound() => ropeSFX.Play();
        public void PlayRusukSound()
        {
            itemSFX.Play();
            rusukSFX.Play();
        }
    }
}