using GameHeaven.Root;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameHeaven.UIUX
{
    public class EpilogueStoryScene : MonoBehaviour
    {
        public void ReturnToModeSelect()
        {
            StoryManager.Instance.ViewEpilogue = true;
            SceneManager.LoadScene("ModeSelectScene");
        }
    }
}