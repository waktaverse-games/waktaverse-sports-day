using UnityEngine;

namespace GameHeaven.Dialogue
{
    [CreateAssetMenu(fileName = "Dialogue", menuName = "Minigame/Dialogue Data", order = 0)]
    public class DialogueObject : ScriptableObject
    {
        [SerializeField] private string gameName, sceneName;
        [SerializeField] private DialogueData[] dialogue;

        public string GameName => gameName;
        public string SceneName => sceneName;
        public int Count => dialogue.Length;
        
        public DialogueData GetDialogue(int index)
        {
            return dialogue[index];
        }
    }
}