using GameHeaven.Root;
using UnityEngine;

namespace GameHeaven.UIUX
{
    public class PrologueStoryScene : MonoBehaviour
    {
        public void OpenStoryMenu()
        {
            StoryManager.Instance.ViewPrologue = true;
            SceneLoader.LoadSceneAsync("StoryMenuScene");
        }
    }
}