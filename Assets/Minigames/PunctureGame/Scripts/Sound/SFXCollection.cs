using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameHeaven.PunctureGame
{
    [RequireComponent(typeof(AudioSource))]
    public class SFXCollection<T> : DisposableSingleton<SFXCollection<T>> where T : Enum
    {
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private List<SFXData> sfxList;
        private Dictionary<T, AudioClip[]> sfxDic;

        protected override void Initialize()
        {
            audioSource.playOnAwake = false;

            sfxDic = new Dictionary<T, AudioClip[]>();
            foreach (var data in sfxList) sfxDic.Add(data.type, data.clips);
        }

        public void PlaySFX(T type, bool isRand = true)
        {
            var clips = sfxDic[type];
            audioSource.PlayOneShot(clips[isRand ? Random.Range(0, clips.Length) : 0]);
        }

        public void PlaySFX(T type, float volume, bool isRand = true)
        {
            var clips = sfxDic[type];
            audioSource.PlayOneShot(clips[isRand ? Random.Range(0, clips.Length) : 0], volume);
        }

        [Serializable]
        public class SFXData
        {
            public T type;
            public AudioClip[] clips;
        }
    }
}