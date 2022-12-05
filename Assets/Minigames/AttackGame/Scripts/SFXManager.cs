using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SharedLibs;

namespace GameHeaven.AttackGame
{
    public class SFXManager : MonoBehaviour
    {
        [SerializeField] public AudioClip[] sounds;

        private AudioSource _audioSource;
        // Start is called before the first frame update
        void Start()
        {
            _audioSource = GetComponent<AudioSource>();
            _audioSource.volume = SharedLibs.SoundManager.Instance.SFXVolume;
        }

        // Update is called once per frame
        public void PlaySfx(int num)
        {
            _audioSource.PlayOneShot(sounds[num]);
        }
    }
}