using GameHeaven.Root;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameHeaven.UIUX
{
    public class EpilogueStoryScene : MonoBehaviour
    {
        private void Awake()
        {
            UIBGM.Instance.OffUIBGM();
        }

        public void ReturnToModeSelect()
        {
            StoryManager.Instance.ViewEpilogue = true;
            UIBGM.Instance.OnUIBGM();
            SceneManager.LoadScene("ModeSelectScene");
            SceneLoader.AddSceneAsync("UnlockMessagesScene");
        }
    }
}