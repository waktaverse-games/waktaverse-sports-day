using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SharedLibs;
using SharedLibs.Character;

namespace GameHeaven.PassGame
{
    public class ChangeCharacter : MonoBehaviour
    {
        Animator anim;
        [SerializeField]
        RuntimeAnimatorController[] controllers;

        [SerializeField] CharacterType currChar;
        private void Awake()
        {
            anim = GetComponent<Animator>();
            
        }
        private void Start()
        {
            if (CharacterManager.Instance == null)
            {
                ChooseCharacter(CharacterType.Woowakgood);
            }
            else ChooseCharacter(CharacterManager.Instance.CurrentCharacter);
        }

        void ChooseCharacter(CharacterType type)
        {
            anim.runtimeAnimatorController = controllers[(int)type];
        }
    }
}