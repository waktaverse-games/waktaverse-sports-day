using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SharedLibs;

namespace GameHeaven.CrashGame
{
    public class SoundManager : MonoBehaviour
    {
        private float sfxVolume, bgmVolume;

        // sfx audio: 0, 1¹ø, BGM: 2¹ø
        [SerializeField]
        private List<AudioSource> audioSources = new List<AudioSource>();
        [SerializeField]
        private List<AudioClip> audioEffectClips = new List<AudioClip> ();
        private Dictionary<string, AudioClip> audioEffectDictionary = new Dictionary<string, AudioClip> ();
        [SerializeField]
        private List<AudioClip> audioSoundClips = new List<AudioClip>();
        private Dictionary<string, AudioClip> audioSoundDictionary = new Dictionary<string, AudioClip>();
        [SerializeField]
        private List<AudioClip> audioBGMClips = new List<AudioClip>();
        private Dictionary<string, AudioClip> audioBGMDictionary = new Dictionary<string, AudioClip>();

        private void Awake()
        {
            sfxVolume = SharedLibs.SoundManager.Instance.SFXVolume;
            bgmVolume = SharedLibs.SoundManager.Instance.BGMVolume;

            foreach (var clip in audioEffectClips)
            {
                audioEffectDictionary.Add(clip.name, clip);
            }
            foreach (var clip in audioSoundClips)
            {
                audioSoundDictionary.Add(clip.name, clip);
            }
            foreach (var clip in audioBGMClips)
            {
                audioBGMDictionary.Add(clip.name, clip);
            }
        }

        private void OnEnable()
        {
            SharedLibs.SoundManager.Instance.OnSFXVolumeChanged += SetSFXVolume;
            SharedLibs.SoundManager.Instance.OnBGMVolumeChanged += SetBGMVolume;
        }

        private void OnDisable()
        {
            SharedLibs.SoundManager.Instance.OnSFXVolumeChanged -= SetSFXVolume;
            SharedLibs.SoundManager.Instance.OnBGMVolumeChanged -= SetBGMVolume;
        }

        private void SetSFXVolume(float sfxVolume)
        {
            this.sfxVolume = sfxVolume;
        }

        private void SetBGMVolume(float bgmVolume)
        {
            this.bgmVolume = bgmVolume;
        }

        public void StopBGM()
        {
            audioSources[2].Stop();
        }

        public void StopSound()
        {
            audioSources[1].Stop();
        }

        public void PlayBGM(string audioName, float pitch = 1f)
        {
            audioSources[2].volume = bgmVolume;
            audioSources[2].pitch = pitch;
            audioSources[2].Stop();
            audioSources[2].clip = audioBGMDictionary[audioName];
            audioSources[2].Play();
        }

        public void PlaySound(string audioName, float pitch = 1f, float volume = 1f)
        {
            audioSources[1].volume = sfxVolume * volume;
            audioSources[1].pitch = pitch;
            audioSources[1].Stop();
            audioSources[1].clip = audioSoundDictionary[audioName];
            audioSources[1].Play();
        }

        public void PlayEffect(string audioName, float pitch = 1f, float volume = 1f)
        {
            audioSources[0].volume = sfxVolume * volume;
            audioSources[0].pitch = pitch;
            audioSources[0].PlayOneShot(audioEffectDictionary[audioName]);
        }
    }
}

