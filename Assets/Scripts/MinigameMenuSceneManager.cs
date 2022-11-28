using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameHeaven.UIUX
{
    public class MinigameMenuSceneManager : MonoBehaviour
    {
        [SerializeField] AudioClip buttonSound;

        private Stack<int> prevMenues;
        private bool enableClick;

        private void Awake()
        {
            prevMenues = new Stack<int>();
            prevMenues.Push(1);
            enableClick = true;
            AudioSource.PlayClipAtPoint(buttonSound, Vector3.zero);
            transform.GetChild(1).GetComponent<Animator>().SetTrigger("On");
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) && enableClick)
            {
                if (prevMenues.Count < 2)
                {

                }
                else
                {
                    enableClick = false;
                    AudioSource.PlayClipAtPoint(buttonSound, Vector3.zero);
                    transform.GetChild(prevMenues.Pop()).GetComponent<Animator>().SetTrigger("Off");
                    transform.GetChild(prevMenues.Peek()).GetComponent<Animator>().SetTrigger("On");
                    Invoke("SetEnableClick", 0.2f);
                }
            }
        }
        private void SetEnableClick()
        {
            enableClick = true;
        }
        IEnumerator ArrowClick()
        {
            yield return 0;
        }
        public void ChooseGameClick()
        {
            if (!enableClick) return;
            enableClick = false;
            AudioSource.PlayClipAtPoint(buttonSound, Vector3.zero);
            transform.GetChild(1).GetComponent<Animator>().SetTrigger("Off");
            transform.GetChild(2).GetComponent<Animator>().SetTrigger("On");
            prevMenues.Push(2);
            Invoke("SetEnableClick", 0.2f);
        }
        public void CheckPuzzleClick()
        {
            if (!enableClick) return;
            enableClick = false;
            AudioSource.PlayClipAtPoint(buttonSound, Vector3.zero);
            transform.GetChild(1).GetComponent<Animator>().SetTrigger("Off");
            transform.GetChild(4).GetComponent<Animator>().SetTrigger("On");
            prevMenues.Push(4);
            Invoke("SetEnableClick", 0.2f);
        }
        public void StartButtonClick()
        {
            if (!enableClick) return;
            enableClick = false;
            // 게임선택
            AudioSource.PlayClipAtPoint(buttonSound, Vector3.zero);
            transform.GetChild(2).GetComponent<Animator>().SetTrigger("Off");
            transform.GetChild(3).GetComponent<Animator>().SetTrigger("On");
            prevMenues.Push(3);
            Invoke("SetEnableClick", 0.2f);
        }
        public void RankingButtonClick()
        {
            if (!enableClick) return;
            enableClick = false;
            AudioSource.PlayClipAtPoint(buttonSound, Vector3.zero);
            transform.GetChild(2).GetComponent<Animator>().SetTrigger("Off");
            transform.GetChild(6).GetComponent<Animator>().SetTrigger("On");
            prevMenues.Push(6);
            Invoke("SetEnableClick", 0.2f);
        }
        public void RightArrowClick()
        {
            if (!enableClick) return;

            Invoke("SetEnableClick", 0.2f);
        }
    }
}