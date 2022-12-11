using System;
using SharedLibs;

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
        private void Start()
        {
            SetVolume(SoundManager.Instance.SFXVolume);
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