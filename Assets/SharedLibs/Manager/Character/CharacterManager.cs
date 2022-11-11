using System.Collections.Generic;
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
        
        public CharacterType CurrentCharacter { get; set; }
        public CharacterData CurrentCharacterData => characterDic[CurrentCharacter] == null ? null : characterDic[CurrentCharacter];

        public override void Init() {
            characterDic = new Dictionary<CharacterType, CharacterData>();
            foreach (var data in characterList)
            {
                characterDic.Add(data.type, data.data);
            }
        }
    }
}