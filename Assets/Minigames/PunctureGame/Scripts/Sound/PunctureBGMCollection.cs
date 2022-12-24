using System;
using SharedLibs;
using UnityEngine;

namespace GameHeaven.PunctureGame
{
    public enum PunctureBGMType
    {
        Default
    }

    public class PunctureBGMCollection : BGMCollection<PunctureBGMType>
    {
        [SerializeField] private Transform followTarget;
        
        private void Start()
        {
            SetVolume(SoundManager.Instance.BGMVolume);
        }

        private void Update()
        {
            transform.position = followTarget.position;
        }

        private void OnEnable()
        {
            SoundManager.Instance.OnBGMVolumeChanged += SetVolume;
        }

        private void OnDisable()
        {
            SoundManager.Instance.OnBGMVolumeChanged -= SetVolume;
        }
    }
}