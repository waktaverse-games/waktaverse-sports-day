using GameHeaven.Root;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameHeaven.UIUX
{
    public class PrologueStoryScene : MonoBehaviour
    {
        private void Awake()
        {
            FindObjectOfType<UIBGM>().OffUIBGM();
        }
        public void OpenStoryMenu()
        {
            FindObjectOfType<UIBGM>().OnUIBGM();
            StoryManager.Instance.ViewPrologue = true;
            SceneManager.LoadScene("StoryMenuScene");
        }
    }
}