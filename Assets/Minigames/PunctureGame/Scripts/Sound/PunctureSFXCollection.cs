using System;
using SharedLibs;
using UnityEngine;

namespace GameHeaven.PunctureGame
{
    public enum PunctureSFXType
    {
        GameOver,
        ItemGet,
        Bounce,
        BlockBreak
    }

    public class PunctureSFXCollection : SFXCollection<PunctureSFXType>
    {
        [SerializeField] private Transform followTarget;

        [SerializeField] private float multiplier;
        
        private void Start()
        {
            SetPunctureVolume(SoundManager.Instance.SFXVolume);
        }

        private void Update()
        {
            transform.position = followTarget.position;
        }

        private void OnEnable()
        {
            SoundManager.Instance.OnSFXVolumeChanged += SetPunctureVolume;
        }

        private void OnDisable()
        {
            SoundManager.Instance.OnSFXVolumeChanged -= SetPunctureVolume;
        }

        private void SetPunctureVolume(float volume) => SetVolume(volume * multiplier);
    }
}