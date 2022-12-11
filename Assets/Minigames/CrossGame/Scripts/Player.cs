using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SharedLibs;
using SharedLibs.Character;

namespace GameHeaven.CrossGame
{
    public class Player : MonoBehaviour
    {
        public CrossGameManager manager;
        Vector3 landPos;

        public CharacterType charactor;
        [System.Serializable]
        private class AnimatorDictionary : UnitySerializedDictionary<CharacterType, RuntimeAnimatorController> { }

        [SerializeField] private AnimatorDictionary animators;
        [HideInInspector] public Animator cntAnimator;
        public bool onBottom;

        private void Awake()
        {
            cntAnimator = GetComponent<Animator>();
            //print(CharacterManager.Instance.CurrentCharacter);
            cntAnimator.runtimeAnimatorController = animators[CharacterManager.Instance.CurrentCharacter];
            //cntAnimator.runtimeAnimatorController = animators[charactor];
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "Bottom")
            {
                onBottom = true;
            }
            else if(collision.tag == "Outline")
            {
                manager.GameOver();
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.tag == "Bottom")
            {
                onBottom = true;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.GetComponent<Platform>() as Platform)
            {
                onBottom = false;
            }
        }

        public void GameOver()
        {
            manager.soundManager.SfxPlay("Drop");
            StartCoroutine(OverRoutine());
        }

        IEnumerator OverRoutine()
        {
            for (int i = 0; i < 1200; i++)
            {
                yield return null;
                transform.position += Vector3.down * 0.01f;
            }
        }
    }
}

