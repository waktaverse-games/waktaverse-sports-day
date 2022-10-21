using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SharedLibs;
using SharedLibs.Character;

namespace GameHeaven.CrossGame
{
    public class Player : MonoBehaviour
    {
        public CrossGameManager Manager;
        Vector3 LandPos;

        BoxCollider2D Collider;
        public CharacterType Charactor;
        [System.Serializable]
        private class AnimatorDictionary : UnitySerializedDictionary<CharacterType, RuntimeAnimatorController> { }

        [SerializeField] private AnimatorDictionary Animators;
        [HideInInspector] public Animator CntAnimator;
        public bool OnBottom;

        private void Awake()
        {
            Collider = GetComponent<BoxCollider2D>();
            CntAnimator = GetComponent<Animator>();
            print(CharacterManager.Instance.CurrentCharacter);
            CntAnimator.runtimeAnimatorController = Animators[CharacterManager.Instance.CurrentCharacter];
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "Bottom")
            {
                OnBottom = true;
            }
            else if(collision.tag == "Outline")
            {
                Manager.GameOver();
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.tag == "Bottom")
            {
                OnBottom = true;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.GetComponent<Platform>() as Platform)
            {
                OnBottom = false;
            }
        }
    }
}

