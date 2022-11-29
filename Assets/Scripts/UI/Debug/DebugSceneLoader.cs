using System.Threading.Tasks;
using SharedLibs;
using UnityEngine.SceneManagement;

namespace GameHeaven.Temp
{
    public class DebugSceneLoader : MonoSingleton<DebugSceneLoader>
    {
        public override void Init()
        {
        }

        public async void LoadScene(string sceneName)
        {
            SceneManager.LoadScene("MinigameMenuScene");
        }
    }
}