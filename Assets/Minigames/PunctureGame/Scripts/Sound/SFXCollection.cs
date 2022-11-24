using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameHeaven.PunctureGame
{
    [RequireComponent(typeof(AudioSource))]
    public class SFXCollection<T> : MonoBehaviour where T : Enum
    {
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private List<SFXData> sfxList;
        private Dictionary<T, AudioData[]> sfxDic;

        private void Awake()
        {
            audioSource.playOnAwake = false;

            sfxDic = new Dictionary<T, AudioData[]>();
            foreach (var data in sfxList) sfxDic.Add(data.type, data.audios);
        }

        public void PlaySFX(T type, bool isRand = true)
        {
            var audios = sfxDic[type];
            var audio = audios[isRand ? Random.Range(0, audios.Length) : 0];
            audioSource.PlayOneShot(audio.clip, audio.volume);
        }
        public void PlaySFX(T type, float volume, bool isRand = true)
        {
            var audios = sfxDic[type];
            var audio = audios[isRand ? Random.Range(0, audios.Length) : 0];
            audioSource.PlayOneShot(audio.clip, volume);
        }

        [Serializable]
        public class SFXData
        {
            public T type;
            public AudioData[] audios;
        }
    }
}