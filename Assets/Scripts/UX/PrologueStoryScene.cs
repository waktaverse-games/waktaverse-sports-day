using GameHeaven.Root;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameHeaven.UIUX
{
    public class PrologueStoryScene : MonoBehaviour
    {
        public void OpenStoryMenu()
        {
            StoryManager.Instance.ViewPrologue = true;
            SceneManager.LoadScene("StoryMenuScene");
        }
    }
}