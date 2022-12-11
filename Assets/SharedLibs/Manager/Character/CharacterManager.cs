using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SharedLibs.Character
{
    public class CharacterManager : MonoSingleton<CharacterManager>
    {
        [System.Serializable]
        public class CharacterDicValue
        {
            public CharacterType type;
            public CharacterData data;
        }

        [SerializeField] private List<CharacterDicValue> characterList;
        private Dictionary<CharacterType, CharacterData> characterDic;

        [SerializeField] [ReadOnly] private CharacterType currentCharacter;

        public CharacterType CurrentCharacter => currentCharacter;
        public CharacterData CurrentCharacterData => characterDic[CurrentCharacter] == null ? null : characterDic[CurrentCharacter];

        public override void Init() {
            characterDic = new Dictionary<CharacterType, CharacterData>();
            foreach (var data in characterList)
            {
                characterDic.Add(data.type, data.data);
            }
        }

        public void SetCharacter(CharacterType character) => currentCharacter = character;
    }
}