using System;
using SharedLibs.Character;
using UnityEngine;

namespace GameHeaven.PunctureGame
{
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField] private Player player;
        
        private void Awake()
        {
            print(CharacterManager.Instance.CurrentCharacter);
        }

        private void OnDisable()
        {
            player.Inactive();
        }
    }
}