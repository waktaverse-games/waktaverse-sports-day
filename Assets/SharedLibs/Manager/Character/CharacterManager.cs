using UnityEngine;

namespace SharedLibs.Character
{
    public class CharacterManager : MonoSingleton<CharacterManager>
    {
        [System.Serializable]
        private class CharacterDictionary : UnitySerializedDictionary<CharacterType, CharacterData> {}
        
        [SerializeField] private CharacterDictionary characterDic = new CharacterDictionary() {};
        
        public CharacterType CurrentCharacter { get; set; }
        public CharacterData CurrentCharacterData => characterDic[CurrentCharacter] == null ? null : characterDic[CurrentCharacter];

        private void Awake()
        {
            characterDic = new CharacterDictionary();
        }
    }
}