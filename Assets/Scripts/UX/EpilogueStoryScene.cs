using GameHeaven.Root;
using UnityEngine;

namespace GameHeaven.UIUX
{
    public class EpilogueStoryScene : MonoBehaviour
    {
        public void ReturnToModeSelect()
        {
            StoryManager.Instance.ViewEpilogue = true;
            SceneLoader.LoadSceneAsync("ModeSelectScene");
        }
    }
}