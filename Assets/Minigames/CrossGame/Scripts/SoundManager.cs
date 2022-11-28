using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SharedLibs;

namespace GameHeaven.CrossGame
{
    public class SoundManager : MonoBehaviour
    {
        [System.Serializable]
        private class AnimatorDictionary : UnitySerializedDictionary<string, AudioClip> { }

        [SerializeField] private AnimatorDictionary audioClips;
        public AudioSource[] audioSource;

        int audioCursor = 0;

        public void Play(string ClipName)
        {
            audioSource[audioCursor].clip = audioClips[ClipName];
            audioSource[audioCursor].Play();

            if (++audioCursor >= audioSource.Length)
            {
                audioCursor = 0;
            }
        }
    }
}