using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.CrashGame
{
    public class SoundManager : MonoBehaviour
    {
        // effect audio: 0¹ø, BGM: 1¹ø
        [SerializeField]
        private List<AudioSource> audioSources = new List<AudioSource>();
        [SerializeField]
        private List<AudioClip> audioEffectClips = new List<AudioClip> ();
        private Dictionary<string, AudioClip> audioEffectDictionary = new Dictionary<string, AudioClip> ();
        [SerializeField]
        private List<AudioClip> audioSoundClips = new List<AudioClip>();
        private Dictionary<string, AudioClip> audioSoundDictionary = new Dictionary<string, AudioClip>();

        private void Awake()
        {
            foreach (var clip in audioEffectClips)
            {
                audioEffectDictionary.Add(clip.name, clip);
            }
            foreach (var clip in audioSoundClips)
            {
                audioSoundDictionary.Add(clip.name, clip);
            }
        }

        public void PlaySound(string audioName, float pitch = 1f, float volume = .5f)
        {
            audioSources[1].volume = volume;
            audioSources[1].pitch = pitch;
            audioSources[1].Stop();
            audioSources[1].clip = audioSoundDictionary[audioName];
            audioSources[1].Play();
        }

        public void PlayEffect(string audioName, float pitch = 1f, float volume = .5f)
        {
            audioSources[0].volume = volume;
            audioSources[0].pitch = pitch;
            audioSources[0].PlayOneShot(audioEffectDictionary[audioName]);
        }
    }
}

