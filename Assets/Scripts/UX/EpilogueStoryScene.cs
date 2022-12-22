using GameHeaven.Root;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameHeaven.UIUX
{
    public class EpilogueStoryScene : MonoBehaviour
    {
        private void Awake()
        {
            FindObjectOfType<UIBGM>().OffUIBGM();
        }

        public void ReturnToModeSelect()
        {
            StoryManager.Instance.ViewEpilogue = true;
            FindObjectOfType<UIBGM>().OnUIBGM();
            SceneManager.LoadScene("ModeSelectScene");
            SceneLoader.AddSceneAsync("UnlockMessagesScene");
        }
    }
}