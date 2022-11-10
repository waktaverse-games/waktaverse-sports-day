using UnityEngine;

namespace GameHeaven.PunctureGame.Utils
{
    public class SpriteColorize : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spRenderer;
        [SerializeField] private Color baseColor = Color.white;

        [SerializeField] private bool randomizeOnEnable;
        [SerializeField] private bool useColorPresets;

        [SerializeField] private Color[] colorPresets;

        private Color RandomColor =>
            useColorPresets
                ? colorPresets[Random.Range(0, colorPresets.Length)]
                : new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));

        private void OnEnable()
        {
            SetRandomColor();
        }

        public Color SetRandomColor()
        {
            var randCol = randomizeOnEnable ? RandomColor : baseColor;
            spRenderer!.color = randCol;
            return randCol;
        }
    }
}