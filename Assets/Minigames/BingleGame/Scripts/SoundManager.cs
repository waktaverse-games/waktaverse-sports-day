using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.BingleGame
{
    public class SoundManager : MonoBehaviour
    {
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

        public void PlayItemSound() => itemSFX.Play();
        public void PlayTurnSound() => turnSFX.Play();
        public void PlayCrashSound() => crashSFX.Play();
    }
}