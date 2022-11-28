using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

namespace GameHeaven.UIUX
{
    public class MinigameMenuSceneManager : MonoBehaviour
    {
        [SerializeField] AudioClip[] buttonSounds;
        [SerializeField] RuntimeAnimatorController[] charControllers;
        [SerializeField] SpriteRenderer[] minigameSprites;
        [SerializeField] string[] charNames, gameNames;

        private Stack<int> prevMenues;
        private bool enableClick;
        private int curGame, curChar;

        private void Awake()
        {
            prevMenues = new Stack<int>();
            prevMenues.Push(1);
            enableClick = true;
            curGame = curChar = 0;
            AudioSource.PlayClipAtPoint(buttonSounds[1], Vector3.zero);
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
                    AudioSource.PlayClipAtPoint(buttonSounds[1], Vector3.zero);
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
        IEnumerator ArrowClick(Transform transform, float x)
        {
            WaitForSeconds wait = new WaitForSeconds(0.01f);

            for (int i = 0; i < 10; i++)
            {
                transform.position += new Vector3(x / 10, 0, 0);
                yield return wait;
            }
        }
        public void ChooseGameClick()
        {
            if (!enableClick) return;
            enableClick = false;
            AudioSource.PlayClipAtPoint(buttonSounds[1], Vector3.zero);
            transform.GetChild(1).GetComponent<Animator>().SetTrigger("Off");
            transform.GetChild(2).GetComponent<Animator>().SetTrigger("On");
            prevMenues.Push(2);
            Invoke("SetEnableClick", 0.2f);
        }
        public void CheckPuzzleClick()
        {
            if (!enableClick) return;
            enableClick = false;
            AudioSource.PlayClipAtPoint(buttonSounds[1], Vector3.zero);
            transform.GetChild(1).GetComponent<Animator>().SetTrigger("Off");
            transform.GetChild(4).GetComponent<Animator>().SetTrigger("On");
            prevMenues.Push(4);
            Invoke("SetEnableClick", 0.2f);
        }
        public void ChooseButtonClick()
        {
            if (!enableClick) return;
            enableClick = false;
            transform.GetChild(5).GetChild(4).GetComponent<TextMeshProUGUI>().text = gameNames[curGame];/*
            switch (curGame)
            {
                case 0:
                    transform.GetChild(5).GetChild(4).GetComponent<TextMeshProUGUI>().text = "¿ä¸®Á¶¸®";
                    break;
                case 1:
                    transform.GetChild(5).GetChild(4).GetComponent<TextMeshProUGUI>().text = "¶ì¿ë¶ì¿ë";
                    break;
                case 2:
                    transform.GetChild(5).GetChild(4).GetComponent<TextMeshProUGUI>().text = "µ¹·Áµ¹·Á";
                    break;
                case 3:
                    transform.GetChild(5).GetChild(4).GetComponent<TextMeshProUGUI>().text = "±úºÎ±úºÎ";
                    break;
                case 4:
                    transform.GetChild(5).GetChild(4).GetComponent<TextMeshProUGUI>().text = "½ºÅ°½ºÅ°";
                    break;
                case 5:
                    transform.GetChild(5).GetChild(4).GetComponent<TextMeshProUGUI>().text = "Àâ¾ÆÀâ¾Æ";
                    break;
                case 6:
                    transform.GetChild(5).GetChild(4).GetComponent<TextMeshProUGUI>().text = "ºü»þºü»þ";
                    break;
                case 7:
                    transform.GetChild(5).GetChild(4).GetComponent<TextMeshProUGUI>().text = "ÆúÂ¦ÆúÂ¦";
                    break;
                case 8:
                    transform.GetChild(5).GetChild(4).GetComponent<TextMeshProUGUI>().text = "»§¾ß»§¾ß";
                    break;
                case 9:
                    transform.GetChild(5).GetChild(4).GetComponent<TextMeshProUGUI>().text = "ÈÊÄ¡ÈÊÄ¡";
                    break;
            }*/
            AudioSource.PlayClipAtPoint(buttonSounds[1], Vector3.zero);
            transform.GetChild(2).GetComponent<Animator>().SetTrigger("Off");
            transform.GetChild(5).GetComponent<Animator>().SetTrigger("On");
            prevMenues.Push(5);
            Invoke("SetEnableClick", 0.2f);
        }
        public void RankingButtonClick()
        {
            if (!enableClick) return;
            enableClick = false;
            AudioSource.PlayClipAtPoint(buttonSounds[1], Vector3.zero);
            transform.GetChild(2).GetComponent<Animator>().SetTrigger("Off");
            transform.GetChild(6).GetComponent<Animator>().SetTrigger("On");
            prevMenues.Push(6);
            Invoke("SetEnableClick", 0.2f);
        }
        public void StartButtonClick()
        {
            if (!enableClick) return;
            enableClick = false;
            AudioSource.PlayClipAtPoint(buttonSounds[1], Vector3.zero);
            transform.GetChild(5).GetComponent<Animator>().SetTrigger("Off");
            transform.GetChild(3).GetComponent<Animator>().SetTrigger("On");
            prevMenues.Push(3);
            Invoke("SetEnableClick", 0.2f);
        }
        public void GameRightClick()
        {
            if (!enableClick) return;
            AudioSource.PlayClipAtPoint(buttonSounds[0], Vector3.zero);
            if (curGame < 9)
            {
                StartCoroutine(ArrowClick(transform.GetChild(2).GetChild(1).GetChild(1).GetChild(0), -400));
                curGame++;
            }
            Invoke("SetEnableClick", 0.1f);
        }
        public void GameLeftClick()
        {
            if (!enableClick) return;
            AudioSource.PlayClipAtPoint(buttonSounds[0], Vector3.zero);
            if (curGame > 0)
            {
                StartCoroutine(ArrowClick(transform.GetChild(2).GetChild(1).GetChild(1).GetChild(0), 400));
                curGame--;
            }
            Invoke("SetEnableClick", 0.1f);
        }
        public void CharRightClick()
        {
            if (!enableClick) return;
            AudioSource.PlayClipAtPoint(buttonSounds[0], Vector3.zero);
            if (curChar < 6)
            {
                transform.GetChild(3).GetChild(0).GetChild(1).GetComponent<Animator>().runtimeAnimatorController = charControllers[curChar];
                transform.GetChild(3).GetChild(0).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = charNames[curChar];
                StartCoroutine(ArrowClick(transform.GetChild(3).GetChild(2).GetChild(2).GetChild(0), -330));
                curChar++;
            }
            Invoke("SetEnableClick", 0.1f);
        }
        public void CharLeftClick()
        {
            if (!enableClick) return;
            AudioSource.PlayClipAtPoint(buttonSounds[0], Vector3.zero);
            if (curChar > 0)
            {
                transform.GetChild(3).GetChild(0).GetChild(1).GetComponent<Animator>().runtimeAnimatorController = charControllers[curChar];
                transform.GetChild(3).GetChild(0).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = charNames[curChar];
                StartCoroutine(ArrowClick(transform.GetChild(3).GetChild(2).GetChild(2).GetChild(0), 330));
                curChar--;
            }
            Invoke("SetEnableClick", 0.1f);
        }
        private void CharChange(int idx)
        {
            switch (idx)
            {
                case 0:
                    break;
            }
        }
    }
}