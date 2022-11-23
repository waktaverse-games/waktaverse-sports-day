using System.Collections.Generic;
using Cinemachine;
using SharedLibs;
using SharedLibs.Character;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameHeaven.PunctureGame
{
    [System.Serializable]
    public class PlayerData
    {
        [SerializeField] private CharacterType type;
        [SerializeField] private GameObject charObj;

        public CharacterType Type => type;
        public GameObject CharObj => charObj;
    }
    
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField] private List<PlayerData> objList;

        [SerializeField] [ReadOnly] private Player player;
        [SerializeField] [ReadOnly] private PlayerController controller;

        [SerializeField] private CinemachineVirtualCamera cmCam;

        private void Awake()
        {
            var curType = CharacterManager.Instance.CurrentCharacter;
            var baseObj = objList.Find(obj => obj.Type.Equals(curType)).CharObj;
            var playerObj = Instantiate(baseObj, Vector3.zero, Quaternion.identity);
            
            player = playerObj.GetComponent<Player>();
            controller = playerObj.GetComponent<PlayerController>();

            cmCam.Follow = player.transform;
        }
    }
}