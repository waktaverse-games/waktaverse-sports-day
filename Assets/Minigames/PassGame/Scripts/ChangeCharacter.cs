using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SharedLibs;
using UnityEditor.Animations;

namespace GameHeaven.PassGame
{
    public class ChangeCharacter : MonoBehaviour
    {
        Animator anim;
        [SerializeField]
        AnimatorController[] controllers;

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
        }
    }
}