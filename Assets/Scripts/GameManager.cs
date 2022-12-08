using SharedLibs;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameHeaven.Root
{
    public enum GameMode
    {
        MinigameMode, StoryMode
    }
    
    public class GameManager : MonoSingleton<GameManager>
    {
        [SerializeField] [ReadOnly] private GameMode gameMode;

        public static GameMode GameMode => Instance.gameMode;

        public override void Init()
        {
            
        }

        public static void SetGameMode(GameMode mode)
        {
            Instance.gameMode = mode;
        }
        
    }
}