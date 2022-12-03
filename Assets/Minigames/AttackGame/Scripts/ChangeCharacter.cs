using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SharedLibs;
using SharedLibs.Character;
using UnityEngine.UI;

namespace GameHeaven.AttackGame
{
    public class ChangeCharacter : MonoBehaviour
    {
        Animator anim;
        [SerializeField]
        RuntimeAnimatorController[] controllers;
        [SerializeField]
        Sprite[] sprites;

        [SerializeField] Image image;
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
            image.sprite = sprites[(int)currChar];
            var tempColor = image.color;
            tempColor.a = 255f;
            image.color = tempColor;
        }

        void ChooseCharacter(CharacterType type)
        {
            anim.runtimeAnimatorController = controllers[(int)type];
        }
    }
}