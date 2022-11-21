using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SharedLibs;

namespace GameHeaven.BingleGame
{
    public class PlayerCharacter : MonoBehaviour
    {
        Animator anim;
        [SerializeField] RuntimeAnimatorController[] animatorControllers;
        [SerializeField] CharacterType currChar;
        private void Awake()
        {
            anim = GetComponent<Animator>();
            //var currChar = SharedLibs.Character.CharacterManager.Instance.CurrentCharacter;
            ChooseCharacter(currChar);
        }

        void ChooseCharacter(CharacterType type)
        {
            anim.runtimeAnimatorController = animatorControllers[(int)type];
        }
    }
}
