using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameHeaven.UIUX
{
    public class ModeSelectSceneManager : MonoBehaviour
    {
        private void Awake()
        {
            transform.GetChild(1).GetComponent<Animator>().SetTrigger("On");
            UISoundManager.Instance.PlayButtonSFX2();
        }
        public void SelectStoryMode()
        {
            SceneManager.LoadScene("StoryMenuScene");
        }
        public void SelectNormalMode()
        {
            SceneManager.LoadScene("MinigameMenuScene");
        }
    }
}