using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameHeaven.UIUX
{
    public class ModeSelectSceneManager : MonoBehaviour
    {

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