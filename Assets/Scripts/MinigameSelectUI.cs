using System.Collections.Generic;
using SharedLibs;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DefaultNamespace
{
    [System.Serializable]
    public struct MinigameSceneData
    {
        private string name;
        private MinigameType type;
        private Scene scene;
        
        public string Name => name;
        public MinigameType Type => type;
        public Scene Scene => scene;
    }
    
    public class MinigameSelectUI : MonoBehaviour
    {
        [SerializeField] private List<MinigameSceneData> minigameList;
        private readonly Dictionary<string, (MinigameType Type, Scene Scene)> minigameTypeDic = new();

        private void Awake()
        {
            foreach (var elem in minigameList)
            {
                minigameTypeDic.Add(elem.Name, (elem.Type, elem.Scene));
            }
        }

        public void SelectGame(string game)
        {
            if (!minigameTypeDic.ContainsKey(game))
            {
                Debug.Log("Game name not exists");
                return;
            }

            var sceneData = minigameTypeDic[game];
            SceneManager.LoadScene(sceneData.Scene.buildIndex, LoadSceneMode.Single);
        }
    }
}