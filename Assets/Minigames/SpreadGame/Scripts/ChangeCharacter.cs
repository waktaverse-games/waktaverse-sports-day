using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SharedLibs;
using UnityEditor.Animations;

namespace GameHeaven.SpreadGame
{
    public class ChangeCharacter : MonoBehaviour
    {
        Animator anim;
        [SerializeField] AnimatorController[] controllers;
        [SerializeField] Sprite[] sprites;
        [SerializeField] CharacterType currChar;

        private void Awake()
        {
            anim = GetComponent<Animator>();
            ChooseCharacter(currChar);
        }

        void ChooseCharacter(CharacterType type)
        {
            anim.runtimeAnimatorController = controllers[(int)type];
        }
    }
}
