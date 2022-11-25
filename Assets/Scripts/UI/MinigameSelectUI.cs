using System.Collections.Generic;
using SharedLibs;
using UnityEngine;

namespace GameHeaven.Temp
{
    public class MinigameSelectUI : MonoBehaviour
    {
        private static Dictionary<MinigameType, string> gameSceneNameDic;
        [SerializeField] private MinigameSceneData gameSceneData;

        [SerializeField] private Transform buttonsParent;
        [SerializeField] private GameObject gameIconBase;
        private List<GameSceneData> gameSceneList;

        private void Awake()
        {
            gameSceneList = gameSceneData.GetScenesData();
            gameSceneNameDic = new Dictionary<MinigameType, string>();
            foreach (var elem in gameSceneList) gameSceneNameDic.Add(elem.Type, elem.SceneName);
        }

        private void Start()
        {
            foreach (var data in gameSceneList) CreateGameIcon(data);
        }

        public static void SelectGame(MinigameType type)
        {
            if (!gameSceneNameDic.ContainsKey(type))
            {
                Debug.Log("Game name not exists");
                return;
            }

            DebugSceneLoader.Instance.LoadScene(gameSceneNameDic[type]);
        }

        private void CreateGameIcon(GameSceneData data)
        {
            var obj = Instantiate(gameIconBase, buttonsParent);
            var trigger = obj.GetComponent<MinigameSelectTriggerButton>();

            obj.name = data.Name;
            trigger.SetButton(data.Type, data.Icon, data.Name);
        }
    }
}