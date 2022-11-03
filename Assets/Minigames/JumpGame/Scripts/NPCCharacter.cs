using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SharedLibs;
namespace GameHeaven.JumpGame
{
    
    public class NPCCharacter : MonoBehaviour
    {
        Animator anim; 
        [SerializeField]
        AnimatorOverrideController[] controllers;
        private void Awake()
        {
            anim = GetComponent<Animator>();
        }
        public void SetCharacter(CharacterType type, bool isSegu = false)
        {
            anim.runtimeAnimatorController = controllers[(int)type];
            if (isSegu) { anim.runtimeAnimatorController = controllers[7]; } // �÷��̾ �������϶��� Ư��ó��
        }
    }
}
