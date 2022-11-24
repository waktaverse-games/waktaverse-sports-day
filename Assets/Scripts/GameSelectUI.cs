using System.Collections.Generic;
using DefaultNamespace;
using SharedLibs;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameHeaven.UI
{
    public class GameSelectUI : MonoBehaviour
    {
        [SerializeField] private MinigameSceneData gameSceneData;
        private readonly Dictionary<string, (MinigameType Type, string SceneName)> minigameTypeDic = new();

        private void Awake()
        {
            var scenesList = gameSceneData.GetScenesData();
            foreach (var elem in scenesList) minigameTypeDic.Add(elem.Name, (elem.Type, elem.SceneName));
        }

        public void SelectGame(string game)
        {
            if (!minigameTypeDic.ContainsKey(game))
            {
                Debug.Log("Game name not exists");
                return;
            }

            var sceneData = minigameTypeDic[game];
            SceneManager.LoadScene(sceneData.SceneName, LoadSceneMode.Single);
        }
    }
}