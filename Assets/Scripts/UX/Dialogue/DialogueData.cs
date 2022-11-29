using UnityEngine;

namespace GameHeaven.Dialogue
{
    [System.Serializable]
    public class DialogueData
    {
        [SerializeField] private string name;
        [SerializeField] private string sentence;
        [SerializeField] private Sprite bgSprite;

        public string Name => name;
        public string Sentence => sentence;
        public Sprite BgSprite => bgSprite;
    }
}