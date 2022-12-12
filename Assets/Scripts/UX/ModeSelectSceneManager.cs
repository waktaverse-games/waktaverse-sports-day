using GameHeaven.Root;
using UnityEngine;

namespace GameHeaven.UIUX
{
    public class ModeSelectSceneManager : MonoBehaviour
    {
        [SerializeField] private GameObject minigameModeLock;
        
        private void Awake()
        {
        }

        private void Start()
        {
            transform.GetChild(1).GetComponent<Animator>().SetTrigger("On");
            UISoundManager.Instance.PlayButtonSFX2();
            
            minigameModeLock.SetActive(!StoryManager.Instance.IsAllUnlock);
        }

        public void SelectStoryMode()
        {
            if (!StoryManager.Instance.ViewPrologue)
            {
                SceneLoader.LoadSceneAsync("PrologueStoryScene");
            }
            else
            {
                SceneLoader.LoadSceneAsync("StoryMenuScene");
            }
        }
        public void SelectNormalMode()
        {
            SceneLoader.LoadSceneAsync("MinigameMenuScene");
        }
    }
}