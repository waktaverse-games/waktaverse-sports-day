using SharedLibs;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameHeaven.Root
{
    public class StoryManager : MonoSingleton<StoryManager>
    {
        private StoryDB _db;

        [SerializeField] [ReadOnly] private int selectStoryIndex;

        public int SelectStoryIndex => selectStoryIndex;
        
        public int UnlockProgress => _db.unlockProgress;
        public bool IsAllUnlock => UnlockProgress == 10;
        
        public bool ViewPrologue { get => _db.viewPrologue; set => _db.viewPrologue = value; }
        public bool ViewEpilogue { get => _db.viewEpilogue; set => _db.viewEpilogue = value; }

        public override void Init()
        {
            _db = GameDatabase.Instance.DB.storyDB;
        }

        public void SetCurrentIndex(int index)
        {
            selectStoryIndex = index;
        }
        
        public void UnlockNext()
        {
            _db.unlockProgress++;
        }

        public void ResetStoryProgress()
        {
            _db.unlockProgress = 0;
            selectStoryIndex = 0;

            ViewPrologue = false;
            ViewEpilogue = false;

            SceneLoader.LoadSceneAsync("ModeSelectScene");
        }
    }
}