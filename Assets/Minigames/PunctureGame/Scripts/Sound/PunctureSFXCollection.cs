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
        
        private void Start()
        {
            SetVolume(SoundManager.Instance.SFXVolume);
        }

        private void Update()
        {
            transform.position = followTarget.position;
        }

        private void OnEnable()
        {
            SoundManager.Instance.OnSFXVolumeChanged += SetVolume;
        }

        private void OnDisable()
        {
            SoundManager.Instance.OnSFXVolumeChanged -= SetVolume;
        }
    }
}