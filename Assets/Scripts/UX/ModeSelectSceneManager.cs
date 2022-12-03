using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameHeaven.UIUX
{
    public class ModeSelectSceneManager : MonoBehaviour
    {
        [SerializeField] AudioClip buttonSound;
        private void Awake()
        {
            transform.GetChild(1).GetComponent<Animator>().SetTrigger("On");
            AudioSource.PlayClipAtPoint(buttonSound, Vector3.zero);
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