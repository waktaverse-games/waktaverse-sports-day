using System;
using System.Collections.Generic;
using SharedLibs;
using UnityEngine;

namespace DefaultNamespace
{
    [Serializable]
    public struct GameSceneData
    {
        [SerializeField] private string name;
        [SerializeField] private MinigameType type;
        [SerializeField] [Scene] private string sceneName;

        public string Name => name;
        public MinigameType Type => type;
        public string SceneName => sceneName;
    }

    [CreateAssetMenu(fileName = "Minigame Scenes Data", menuName = "Minigame/Scenes Data", order = 0)]
    public class MinigameSceneData : ScriptableObject
    {
        [SerializeField] private List<GameSceneData> scenesData;

        public List<GameSceneData> GetScenesData()
        {
            return scenesData;
        }
    }
}