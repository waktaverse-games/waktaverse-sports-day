using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SharedLibs;


namespace GameHaven.RunGame
{
    public class CharactorSelect : MonoBehaviour
    {

        Animator anim;
        [SerializeField]
        RuntimeAnimatorController[] playerSelectControllers;

        void Awake()
        {
            anim = GetComponent<Animator>();
            var currChar = SharedLibs.Character.CharacterManager.Instance.CurrentCharacter;
            Select(currChar);
            Debug.Log((int)currChar);

        }

        void Select(CharacterType type)
        {
            anim.runtimeAnimatorController = playerSelectControllers[(int)type];
        }
    }
}

