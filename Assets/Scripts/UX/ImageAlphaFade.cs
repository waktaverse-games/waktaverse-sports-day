using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GameHeaven.UIUX
{
    public class ImageAlphaFade : MonoBehaviour
    {
        [SerializeField] private Image targetImage;

        [SerializeField] private float fadeInTime;
        [SerializeField] private float fadeOutTime;
        
        [SerializeField] [Range(0f, 1f)] private float startAlpha;

        [SerializeField] private UnityEvent onFadeInComplete;
        [SerializeField] private UnityEvent onFadeOutComplete;
        
        private void Awake()
        {
            var startColor = targetImage.color;
            startColor.a = startAlpha;
            targetImage.color = startColor;
        }

        public void FadeIn()
        {
            StartCoroutine(FadeImage(targetImage, fadeInTime, 0, 1, () => onFadeInComplete?.Invoke()));
        }
        
        public void FadeOut()
        {
            StartCoroutine(FadeImage(targetImage, fadeOutTime, 1, 0, () => onFadeOutComplete?.Invoke()));
        }
        
        private static IEnumerator FadeImage(Image image, float duration, float startAlpha, float endAlpha, Action onComplete = null)
        {
            Color color = image.color;
            color.a = startAlpha;
            image.color = color;
            float time = 0;
            while (time < duration)
            {
                color.a = Mathf.Lerp(startAlpha, endAlpha, time / duration);
                image.color = color;
                time += Time.deltaTime;
                yield return null;
            }
            color.a = endAlpha;
            image.color = color;
            
            onComplete?.Invoke();
        }
    }
}