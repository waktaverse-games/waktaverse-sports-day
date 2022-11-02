using SharedLibs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.JumpGame
{
    public class PlayerCharacter : MonoBehaviour
    {
        Animator anim;
        [SerializeField]
        AnimatorOverrideController[] controllers;
        [SerializeField] NPCCharacter NPC_Left;
        [SerializeField] NPCCharacter NPC_Right;

        [SerializeField] CharacterType currChar;
        private void Awake()
        {
            anim = GetComponent<Animator>();
            
        }
        private void Start()
        {
            //var currChar = SharedLibs.Character.CharacterManager.Instance.CurrentCharacter;
            ChooseCharacter(currChar);
        }

        void ChooseCharacter(CharacterType type)
        {
            anim.runtimeAnimatorController = controllers[(int)type];
            NPC_Left.SetCharacter(type);
            if (type == CharacterType.Gosegu) { NPC_Right.SetCharacter(type, true); }
            else { NPC_Right.SetCharacter(type); }
        }
    }
}
