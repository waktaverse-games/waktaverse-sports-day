using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace GameHeaven.PunctureGame
{
    public class FadeTransition : MonoBehaviour
    {
        [SerializeField] private Sprite[] bgSprites;
        [SerializeField] private Image showImage;
        [SerializeField] private Image fadeImage;

        [SerializeField] private float transTime;

        [SerializeField] private BackgroundScroll bgScroll;

        private void Awake()
        {
            bgScroll.OnBackgroundChanged += FadeTransitionOnBGChanges;
        }

        private void FadeTransitionOnBGChanges(int index)
        {
            StartCoroutine(FadeInOutTransition(showImage, fadeImage, bgSprites[index], transTime));
        }

        private static IEnumerator FadeInOutTransition(Image show, Image fade, Sprite newSp, float time)
        {
            var color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
            fade.color = color;
            fade.sprite = newSp;

            while (color.a < 0.98f)
            {
                yield return null;
                color.a += Time.deltaTime / time;
                fade.color = color;
            }

            show.sprite = newSp;
            fade.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        }
    }
}