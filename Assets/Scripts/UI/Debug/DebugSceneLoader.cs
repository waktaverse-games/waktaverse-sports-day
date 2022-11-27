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
            var oper = SceneManager.LoadSceneAsync(sceneName);
            oper.allowSceneActivation = true;
            await Task.Delay(100);
        }
    }
}