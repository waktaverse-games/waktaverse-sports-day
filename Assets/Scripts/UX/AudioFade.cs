using System;
using System.Collections;
using SharedLibs;
using UnityEngine;
using UnityEngine.Events;

namespace GameHeaven.UIUX
{
    public class AudioFade : MonoBehaviour
    {
        [SerializeField] private AudioSource targetPlayer;
        
        [SerializeField] private float fadeInTime;
        [SerializeField] private float fadeOutTime;
        
        [SerializeField] private bool muteOnStart;
        private float originalVolume;
        
        [SerializeField] private UnityEvent onFadeInComplete;
        [SerializeField] private UnityEvent onFadeOutComplete;
        
        private void Start()
        {
            originalVolume = targetPlayer.volume;
            if (muteOnStart) targetPlayer.volume = 0;
        }
        
        public void FadeIn()
        {
            StartCoroutine(FadeAudio(targetPlayer, fadeInTime, 0f, originalVolume, () => onFadeInComplete?.Invoke()));
        }
        
        public void FadeOut()
        {
            StartCoroutine(FadeAudio(targetPlayer, fadeOutTime, targetPlayer.volume, 0f, () => onFadeOutComplete?.Invoke()));
        }
        
        private static IEnumerator FadeAudio(AudioSource audioSource, float duration, float startVolume, float endVolume, Action onComplete = null)
        {
            float currentTime = 0;
            while (currentTime < duration)
            {
                currentTime += Time.deltaTime;
                audioSource.volume = Mathf.Lerp(startVolume, endVolume, currentTime / duration);
                yield return null;
            }
            audioSource.volume = endVolume;
            
            onComplete?.Invoke();
        }
    }
}