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

        [SerializeField] private AnimatorDictionary AudioClips;
        public AudioSource[] AudioSource;

        int AudioCursor = 0;

        public void Play(string ClipName)
        {
            AudioSource[AudioCursor].clip = AudioClips[ClipName];
            AudioSource[AudioCursor].Play();

            if (++AudioCursor >= AudioSource.Length)
            {
                AudioCursor = 0;
            }
        }
    }
}