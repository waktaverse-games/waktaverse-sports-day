using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SharedLibs;

namespace GameHeaven.CrossGame
{
    public class CrossGameSoundManager : MonoBehaviour
    {
        [System.Serializable]
        private class AnimatorDictionary : UnitySerializedDictionary<string, AudioClip> { }

        [SerializeField] private AnimatorDictionary audioClips;
        public AudioSource[] sfxPlayers;
        public AudioSource bgmPlayer;

        int audioCursor = 0;

        private void Start()
        {
            foreach (AudioSource tmp in sfxPlayers)
            {
                tmp.volume = SoundManager.Instance.SFXVolume;
            }
            bgmPlayer.volume = SoundManager.Instance.BGMVolume;
        }

        public void Play(string ClipName)
        {
            sfxPlayers[audioCursor].clip = audioClips[ClipName];
            sfxPlayers[audioCursor].Play();

            if (++audioCursor >= sfxPlayers.Length)
            {
                audioCursor = 0;
            }
        }
    }
}