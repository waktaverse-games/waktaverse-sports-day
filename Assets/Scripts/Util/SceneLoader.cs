using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoader
{
    public static async void LoadSceneAsync(string sceneName, float waitSec = 0.0f)
    {
        LoadSceneAsync(sceneName, null, waitSec);
    }
    public static async void LoadSceneAsync(string sceneName, Action onLoading, float waitSec = 0.0f)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        
        op.allowSceneActivation = false;

        onLoading?.Invoke();
        await Task.Delay((int)(waitSec * 1000));
        while (op.progress < 0.9f) await Task.Yield();

        op.allowSceneActivation = true;
    }
    
    public static async void AddSceneAsync(string sceneName)
    {
        AddSceneAsync(sceneName, null);
    }
    public static async void AddSceneAsync(string sceneName, Action onLoading)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        
        op.allowSceneActivation = false;

        onLoading?.Invoke();
        while (op.progress < 0.9f) await Task.Yield();

        op.allowSceneActivation = true;
    }
}