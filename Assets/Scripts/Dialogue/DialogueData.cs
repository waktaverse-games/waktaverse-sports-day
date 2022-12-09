using UnityEngine;

namespace GameHeaven.Dialogue
{
    [System.Serializable]
    public class DialogueData
    {
        [SerializeField] private string name;
        [SerializeField] private string sentence;
        [SerializeField] private Sprite bgSprite;
        [SerializeField] private AudioClip bgm;
        [SerializeField] private AudioClip sfx;

        public string Name => name;
        public string Sentence => sentence;
        public Sprite BgSprite => bgSprite;
        public AudioClip BGM => bgm;
        public AudioClip Sfx => sfx;
    }
}