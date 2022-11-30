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
        [SerializeField] NPCSpawner npcSpawner;
        private void Awake()
        {
            anim = GetComponent<Animator>();
            
        }
        private void Start()
        {
            CharacterType currChar = SharedLibs.Character.CharacterManager.Instance.CurrentCharacter;
            ChooseCharacter(currChar);
        }

        void ChooseCharacter(CharacterType type)
        {
            npcSpawner.SetFandomCharacter(type);
            anim.runtimeAnimatorController = controllers[(int)type];
        }
    }
}
