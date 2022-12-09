using SharedLibs;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameHeaven.Root
{
    public class StoryManager : MonoSingleton<StoryManager>
    {
        private StoryDB _db;

        public int SelectStoryIndex => _db.lastViewedChapter;
        
        public int UnlockProgress => _db.unlockProgress;
        public bool IsAllUnlock => UnlockProgress == 10;
        public int LastViewedChapter => _db.lastViewedChapter;
        
        public bool ViewPrologue { get => _db.viewPrologue; set => _db.viewPrologue = value; }
        public bool ViewEpilogue { get => _db.viewEpilogue; set => _db.viewEpilogue = value; }

        public override void Init()
        {
            _db = GameDatabase.Instance.DB.storyDB;
        }

        public void SetCurrentIndex(int index)
        {
            _db.lastViewedChapter = index;
        }
        
        public void UnlockNext()
        {
            _db.unlockProgress++;
        }

        public void ResetStoryProgress()
        {
            _db.unlockProgress = 0;
            _db.lastViewedChapter = 0;

            ViewPrologue = false;
            ViewEpilogue = false;

            SceneLoader.LoadSceneAsync("ModeSelectScene");
        }
    }
}