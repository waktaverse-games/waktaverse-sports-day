using SharedLibs;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameHeaven.Root
{
    public class StoryManager : MonoSingleton<StoryManager>
    {
        private StoryDB _db;

        [SerializeField] [ReadOnly] private int selectStoryIndex;

        public int UnlockProgress => _db.unlockProgress;
        public int ViewProgress => _db.viewProgress;

        public int SelectStoryIndex => selectStoryIndex;

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

        public void UpdateViewProgressLatest()
        {
            _db.viewProgress = SelectStoryIndex;
        }
    }
}