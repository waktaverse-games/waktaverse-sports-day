using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SharedLibs;

namespace GameHeaven.SpreadGame
{
    public class ChangeCharacter : MonoBehaviour
    {
        Animator anim;
        SpriteRenderer spriteRenderer;

        [SerializeField] RuntimeAnimatorController[] controllers;
        [SerializeField] Sprite[] sprites;
        [SerializeField] CharacterType currChar;

        private void Awake()
        {
            anim = GetComponent<Animator>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            ChooseCharacter(currChar);
        }

        void ChooseCharacter(CharacterType type)
        {
            anim.runtimeAnimatorController = controllers[(int)type];
            spriteRenderer.sprite = sprites[(int)type];
        }
    }
}
