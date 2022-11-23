using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.PunctureGame
{
    [RequireComponent(typeof(AudioSource))]
    public class BGMCollection<T> : DisposableSingleton<BGMCollection<T>> where T : Enum
    {
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private List<BGMData> bgmList;
        private Dictionary<T, AudioClip> bgmDic;

        protected override void Initialize()
        {
            audioSource.playOnAwake = false;

            bgmDic = new Dictionary<T, AudioClip>();
            foreach (var data in bgmList) bgmDic.Add(data.type, data.clip);
        }

        public void PlayBGM(T type, bool isLoop = false)
        {
            PlayBGM(type, 0L, isLoop);
        }

        public void PlayBGM(T type, ulong delay, bool isLoop = false)
        {
            audioSource.loop = isLoop;
            audioSource.clip = bgmDic[type];
            audioSource.Play(delay);
        }

        public void PauseBGM()
        {
            audioSource.Pause();
        }

        public void StopBGM()
        {
            audioSource.Stop();
        }

        [Serializable]
        public class BGMData
        {
            public T type;
            public AudioClip clip;
        }
    }
}