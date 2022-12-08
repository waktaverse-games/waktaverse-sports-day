using GameHeaven.Root;
using UnityEngine;

namespace GameHeaven.UIUX
{
    public class ModeSelectSceneManager : MonoBehaviour
    {
        [SerializeField] private GameObject minigameModeLock;
     
        [Header("[DEBUG] 체크 시 미니게임 모드가 잠금 해제됩니다")]
        [SerializeField] private bool debugMode;
        
        private void Awake()
        {
            transform.GetChild(1).GetComponent<Animator>().SetTrigger("On");
            UISoundManager.Instance.PlayButtonSFX2();
        }

        private void Start()
        {
            minigameModeLock.SetActive(!debugMode && !StoryManager.Instance.IsAllUnlock);
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