using UnityEngine;

namespace GameHeaven.PunctureGame.Test
{
    public class SceneChanger : MonoBehaviour
    {
        public void ChangeScene(string sceneName)
        {
            LevelManager.Instance.LoadScene(sceneName);
        }
    }
}