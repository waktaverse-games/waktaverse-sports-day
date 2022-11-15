using UnityEngine;

namespace SharedLibs.Character
{
    [CreateAssetMenu(fileName = "CharacterData", menuName = "Minigame/Character", order = 0)]
    public class CharacterData : ScriptableObject
    {
        public string characterName;
        public string characterDescription;
    }
}