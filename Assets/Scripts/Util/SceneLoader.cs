using System;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

public static class SceneLoader
{
    public static async Task LoadSceneAsync(string sceneName, float waitSec = 0.0f)
    {
        await LoadSceneAsync(sceneName, null, waitSec);
    }

    public static async Task LoadSceneAsync(string sceneName, Action onLoading, float waitSec = 0.0f)
    {
        var op = SceneManager.LoadSceneAsync(sceneName);

        op.allowSceneActivation = false;

        onLoading?.Invoke();
        await Task.Delay((int)(waitSec * 1000));
        while (op.progress < 0.9f) await Task.Yield();

        op.allowSceneActivation = true;
    }

    public static async Task AddSceneAsync(string sceneName)
    {
        await AddSceneAsync(sceneName, null);
    }

    public static async Task AddSceneAsync(string sceneName, Action onLoading)
    {
        var op = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        op.allowSceneActivation = false;

        onLoading?.Invoke();
        while (op.progress < 0.9f) await Task.Yield();

        op.allowSceneActivation = true;
    }

    public static async Task UnloadSceneAsync(string sceneName)
    {
        await UnloadSceneAsync(sceneName, null);
    }

    public static async Task UnloadSceneAsync(string sceneName, Action onLoading)
    {
        var op = SceneManager.UnloadSceneAsync(sceneName);

        // op.allowSceneActivation = false;

        onLoading?.Invoke();
        while (op.progress < 0.9f) await Task.Yield();

        // op.allowSceneActivation = true;
    }
}