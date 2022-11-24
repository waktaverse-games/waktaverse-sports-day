using UnityEngine;

namespace SharedLibs.Character
{
    [CreateAssetMenu(fileName = "Character Data", menuName = "Minigame/Character Data", order = 0)]
    public class CharacterData : ScriptableObject
    {
        public string characterName;
        public string characterDescription;
    }
}