using GameHeaven.Root;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameHeaven.UIUX
{
    public class PrologueStoryScene : MonoBehaviour
    {
        private void Awake()
        {
            UIBGM.Instance.OffUIBGM();
        }
        public void OpenStoryMenu()
        {
            UIBGM.Instance.OnUIBGM();
            StoryManager.Instance.ViewPrologue = true;
            SceneManager.LoadScene("StoryMenuScene");
        }
    }
}